using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    class StructureDefinition : IRules
    {

        public void Verify(string filename, string[] content)
        {
            if (!filename.EndsWith(".c"))
                return;
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                Regex regStructDefinition = new Regex(@"(\s|\t)*typedef(\s|\t)+(struct)(\s|\t)+([_a-zA-Z0-9]+)(\s|\t)+([_a-zA-Z0-9]+)", RegexOptions.IgnoreCase);

                foreach (var match in regStructDefinition.Matches(line))
                {
                   Console.WriteLine($"[{filename}]:[{i}] Forbiden definition of struct in c file.");
                }
            }
        }
    }
}
