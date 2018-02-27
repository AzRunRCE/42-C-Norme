using System;
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
            Regex reg = new Regex(@"( \t)*(for|do|case|switch|goto)( \t)*", RegexOptions.IgnoreCase);
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                foreach (Match match in reg.Matches(line))
                {
                    Console.WriteLine($"[{filename}]:[{i}] {match.Captures[0].Value} Forbidden statement.");
                }
            }
        }

        private void OwnLineBrace(string filename, string[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
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
