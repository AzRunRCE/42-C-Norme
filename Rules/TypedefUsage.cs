using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace appRegex.Rules
{
    class TypedefUsage : IRules
    {
        public void Verify(string filename, string[] content)
        {
         
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                Regex regStructDefinition = new Regex(@"(\s|\t)*typedef(\s|\t)+([_a-zA-Z0-9]+)(\s|\t)+([_a-zA-Z0-9]+)", RegexOptions.IgnoreCase);
                Regex typeDefTab = new Regex(@"(\s|\t)*typedef\t([_a-zA-Z0-9]+)(\t)([_a-zA-Z0-9]+)", RegexOptions.IgnoreCase);
                foreach (Match match in regStructDefinition.Matches(line))
                {
                    if (typeDefTab.Match(match.Value).Success != true)
                        Console.WriteLine($"[{filename}]:[{i}] Tabulation must used in typedef definition.");
                }
            }
        }
    }
}
