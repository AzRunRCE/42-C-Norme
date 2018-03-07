using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Misc : IRules
    {
        public Misc()
        {
        }

        public void Verify(string filename, string[] content)
        {
            ForbiddenStatement(filename, content);
            OwnLineBrace(filename, content);
        }

        private void ForbiddenStatement(string filename, string[] content)
        {
            Regex reg = new Regex(@"(for|case|switch|goto)\s*");
            Regex regDo = new Regex(@"do\s*\{\s*.*\s*\}.*\s*while");
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                foreach (Match match in reg.Matches(line))
                {
                    Console.WriteLine($"[{filename}]:[{i}] Forbidden statement ({match.Captures[0].Value}).");
                }

            }
            foreach (Match match in regDo.Matches(String.Join(null,content)))
            {
                Console.WriteLine($"[{filename}] Forbidden statement (do while).");
            }

        }

        private void OwnLineBrace(string filename, string[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                if (line.Contains("{") && !(line.Trim().Length == 1))
                {
                    Console.WriteLine($"[{filename}]:[{i}] Opening brace must be on their own line.");
                }
                else if (line.Contains("{") && !(line.Trim().Length == 1))
                {
                    if (!line.EndsWith(";"))
                    {
                        Console.WriteLine($"[{filename}]:[{i}] CLosing brace must be on their own line.");
                    }
                }
            }
        }

     
    }
}
