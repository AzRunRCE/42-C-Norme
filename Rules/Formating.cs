using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Formating : IRules
    {
        public Formating()
        {
        }

        public void Verify(string filename, string[] content)
        {
            if (!new Regex("^[a-z0-9]+([a-z0-9_]+[a-z0-9]+)*[.][ch]$").Match(filename).Success)
            {
                Console.WriteLine($"[{filename}]:[{0}] Filenames should respect the snake_case naming convention.");
            }
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                CheckSpaceVirgule(filename,i, line);
                CheckOperatorSpace(filename, i, line);
                CheckOperandSpace(filename, i, line);
                CheckSpaceAfterKeyword(filename, i, line);
                int count = 0;
                foreach (var c in line){
                    if (c == '\t')
                        count = count + 4;
                    else
                        count++;
                }
                if (count > 80)
                    Console.WriteLine($"[{filename}]:[{i}] Too many columns");
                
                if (line.EndsWith(" ") || line.EndsWith("\t") || line.EndsWith(" ;") || line.EndsWith("\t;"))
                {
                    Console.WriteLine($"[{filename}]:[{i}] Trailing space(s) at the end of the line.");
                }

                Regex reg = new Regex("(if.*[^&|=^><+\\-*%\\/!]=[^=].*==.*)|(if.*==.*[^&|=^><+\\-*%\\/!]=[^=].*)", RegexOptions.IgnoreCase);
                if (reg.Match(line).Success)
                {
                    Console.WriteLine($"[{filename}]:[{i}] Condition and assignment on the same line.");
                }
                if (new Regex("([^(\t ]+_t|int|signed|unsigned|char|long|short|float|double|void|const|struct [^ ]+)\\*").Match(line).Success)
                {
                    Console.WriteLine($"[{filename}]:[{i}]  Misplaced pointer symbol.");
                }
            }
        }

        private void CheckSpaceVirgule(string filename, int position,  string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ';' || str[i] == ',')
                {
                    if (i + 1 != str.Length && str[i + 1] != ' ')
                    {
                        Console.WriteLine($"[{filename}]:[{position}] The ',' must followed by a space");
                    }

                }
            }
        }


        private void CheckOperatorSpace(string filename, int position, string str)
        {
            List<string> list = new List<string>() { "+", "-", "/", "%", "|", "&", "^"};
            foreach (var item in list)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i].ToString() == item )
                    {
                        if ((i + 1 != str.Length && !(str[i + 1] == ' ' || str[i + 1] == ';' || str[i + 1] == item[0])))
                        {
                            Console.WriteLine($"[{filename}]:[{position}] The '{item}' must splited by a space");
                        }

                        if ((i - 1 > -1 && !(str[i - 1] == ' ' || str[i - 1] == item[0])))
                        {
                            if (i + 1 != str.Length && str[i + 1] != item[0])
                                Console.WriteLine($"[{filename}]:[{position}] The '{item}' must splited by a space");
                        }
                    }
                }
            }
          
        }

        private void CheckOperandSpace(string filename, int position, string str)
        {
            List<string> list = new List<string>() {  "\\=\\=", "\\!\\=", "\\<\\=", "\\>\\=", "\\&\\&", "\\|\\|", "\\+\\=", "\\-\\=", "\\*\\=", "\\/\\=", "\\%\\=", "\\&\\=", "\\^\\=", "\\|\\=", "\\>\\>", "\\<\\<", "\\>\\>\\=", "\\<\\<\\=" };
            foreach (var item in list)
            {
                Regex regStructDefinition = new Regex(@"(?<before>.*)(" + item + ")(?<after>.*)", RegexOptions.IgnoreCase);

                Match match = regStructDefinition.Match(str);
                string outputOperand = item.Replace("\\", "");
                if (match.Success)
                {
                    if (!match.Groups["before"].Value.EndsWith(" ") || !match.Groups["after"].Value.StartsWith(" "))
                        Console.WriteLine($"[{filename}]:[{position}] The '{outputOperand}' operand must followed by a space.");
                }
            }
        }

        private void CheckSpaceAfterKeyword(string filename, int position, string str)
        {
      
            Regex regStructDefinition = new Regex("(return|if|else if|else|while|for)\\(", RegexOptions.IgnoreCase);
            Match match = regStructDefinition.Match(str);
            if (match.Success)
            {
                Console.WriteLine($"[{filename}]:[{position}] Missing space after keyword {match.Captures[0]}.");
            }
        }

    }
}
