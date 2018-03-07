using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Indentation : IRules
    {
        public Indentation()
        {

        }

        public void Verify(string filename, string[] content)
        {
            CheckIndentation(filename, content, 0, 0);
        }

    


        private void CheckIndentation(string filename, string[] content, int index, int indentation)
        {
            int countIndent = 0;
            if (index >= content.Length)
                return;
            string line = content[index];
            if (!line.IsComment() && line.Contains("{"))
            {
                indentation++;
                CheckIndentation(filename, content, index + 1, indentation);
                return;
            }

            if (!line.IsComment() && line.Contains("}"))
            {
                indentation--;
                CheckIndentation(filename, content, index + 1, indentation);
                return;
            }
            else
            {
                while (countIndent < line.Length && (line[countIndent] == ' ' || line[countIndent] == '\t'))
                {
                    countIndent++;
                }
                Regex regCond = new Regex(@"^( |\t)+(if|else|else if|while)( |\t)*\(.*\)$");
                if (countIndent != indentation && line.Length > 0 && !regCond.Match(content[index - 1]).Success)
                {
                    if (!line.IsComment())
                        Console.WriteLine($"[{filename}]:[{index}] Wrong indentation.");
                }
                CheckIndentation(filename, content, index + 1, indentation);
            }



        }
    }
}
