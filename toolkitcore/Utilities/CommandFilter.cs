using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Utilities
{
    public class CommandFilter
    {
        public static IEnumerable<string> Parse(string input)
        {
            List<string> cache = new List<string>();
            string segment = "";
            bool quoted = false;
            bool escaped = false;

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
    }
}
