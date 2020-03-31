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
                if (escaped && !c.Equals('"'))
                {
                    escaped = false;
                    segment += '\\';
                }

                switch (c)
                {
                    case ' ' when !quoted:
                        cache.Add(segment);
                        segment = "";
                        break;
                    case '"' when !escaped:
                        quoted = !quoted;
                        break;
                    case '"':
                        segment += c;
                        escaped = false;
                        break;
                    case '\\':
                        escaped = true;
                        break;
                    default:
                        segment += c;
                        break;
                }
            }

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
                        case '=' when !escaped:
                            sep = true;
                            break;
                        case '\\':
                            escaped = true;
                            break;
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
                
                cache.Add(new KeyValuePair<string, string>(key, value));
            }

            return cache;
        }
    }
}
