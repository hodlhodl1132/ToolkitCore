using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using Verse;

namespace ToolkitCore.Controllers
{
    public static class ChatCommandController
    {
        public static ToolkitChatCommand GetChatCommand(string commandText)
        {
            var baseCommand = Parse(commandText).FirstOrDefault();

            if (baseCommand == null)
            {
                return null;
            }

            return DefDatabase<ToolkitChatCommand>.AllDefsListForReading.FirstOrDefault(
                c => c.commandText.EqualsIgnoreCase(baseCommand)
            );
        }

        public static IEnumerable<string> Parse(string input, string prefix = "!")
        {
            // Strip the prefix from the input (if it exists)
            if (input.ToLowerInvariant().StartsWith(prefix))
            {
                input = input.Substring(prefix.Length);
            }
            
            var cache = new List<string>();
            var segment = "";
            var quoted = false;
            var escaped = false;

            foreach (var c in input)
            {
                // If the last character was a backslash, but the current character
                // isn't a quote, we'll leave "escape mode" and append a backslash
                // to the current segment. The parser itself should only handle 
                // escape quotes.
                if (escaped && !c.Equals('"'))
                {
                    escaped = false;
                    segment += '\\';
                }

                switch (c)
                {
                    // If we've encountered a space and aren't in "quoted mode",
                    // we'll add the current segment to the cache and clear the
                    // buffer.
                    case ' ' when !quoted:
                        cache.Add(segment);
                        segment = "";
                        break;
                    
                    // If we've encountered a quote while not in "escape mode",
                    // we'll toggle "quoted mode". Quoted mode signals to the parser
                    // to disable the above behavior until we've exited quote mode.
                    case '"' when !escaped:
                        quoted = !quoted;
                        break;
                    
                    // If we've encountered a quote while in "escape mode",
                    // we'll add the quote to the buffer and exit "escape mode".
                    // This is to allow users to include quotes in quoted text.
                    case '"':
                        segment += c;
                        escaped = false;
                        break;
                    
                    // If we've encountered a backslash, we'll enter "escape mode".
                    // Escape mode signals to the parser to disable default handling
                    // for special characters, like quotes, in favor of append them
                    // to the current buffer.
                    case '\\':
                        escaped = true;
                        break;
                    
                    // If no special cases exist for the current character, we'll
                    // simply add it to the buffer.
                    default:
                        segment += c;
                        break;
                }
            }

            // If any data is still in the buffer, we'll add it to the cache.
            if (segment.Length > 0)
            {
                cache.Add(segment);
            }

            return cache.ToArray();
        }

        public static List<KeyValuePair<string, string>> ParseKeyed(string input, string prefix = "!")
        {
            return ParseKeyed(Parse(input, prefix));
        }

        public static List<KeyValuePair<string, string>> ParseKeyed(IEnumerable<string> input)
        {
            var cache = new List<KeyValuePair<string, string>>();

            foreach (var segment in input)
            {
                if (!segment.Contains('='))
                {
                    continue;
                }

                var key = "";
                var value = "";
                var sep = false;
                var escaped = false;

                foreach (var c in segment)
                {
                    switch (c)
                    {
                        // If we've encountered an equals sign while we're not in
                        // "escaped mode", we'll signal to the parser to swap to
                        // writing to the value buffer.
                        case '=' when !escaped:
                            sep = true;
                            break;
                        
                        // If we've encountered an equals sign while in "escaped mode",
                        // we'll add the equals sign to the current buffer.
                        case '=':
                        {
                            if (!sep)
                            {
                                key += c;
                            }
                            else
                            {
                                value += c;
                            }
                            escaped = false;
                            break;
                        }
                        // If we've encountered a backslash, we'll signal to the
                        // parser to enter "escaped mode".
                        case '\\':
                            escaped = true;
                            break;
                        
                        // If there's no special handling for the current character,
                        // we'll add the character to the current buffer. If "sep"
                        // is true, the current buffer will be the value buffer, else
                        // it'll be the key buffer.
                        default:
                        {
                            if (!sep)
                            {
                                key += c;
                            }
                            else
                            {
                                value += c;
                            }

                            break;
                        }
                    }
                }
                
                // We'll add the pair to the cache as a KeyValuePair to facilitate
                // duplicate keys.
                cache.Add(new KeyValuePair<string, string>(key, value));
            }

            return cache;
        }
    }
}
