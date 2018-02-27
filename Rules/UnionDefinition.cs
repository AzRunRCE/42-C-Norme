using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    class NamingConvention : IRules
    {

        private string _type;
        private string _prefix;
        public  NamingConvention(string type, string prefix){
            _type = type;
            _prefix = prefix;   
        }
       
        public void Verify(string filename, string[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                Regex regex = new Regex($"(\\s|\\t)*{_type}(\\s|\\t)+(?<variable_name>[_a-zA-Z0-9]+)(\\s|\\t)*([^\\;])$", RegexOptions.IgnoreCase);
                Match match = regex.Match(line);
                var variableName = match.Groups["variable_name"].Value;
                if(match.Success && !variableName.StartsWith($"{_prefix}", StringComparison.CurrentCulture))    {
                    
                    Console.WriteLine($"[{filename}]:[{i}] {_type} name must started by {_prefix}.");
                }

            }
        }
    }
}
