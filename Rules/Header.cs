using System;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Header : IRules
    {
        public Header()
        {
        }

        public void Verify(string filename, string[] content)
        {

            var str = string.Concat(content);
            var match = new Regex(@"(\/\*([\s\w_\.\>@\<#\/\:\+]|\*{74})+\*\/)", RegexOptions.Multiline).Matches(str);
            if (match.Count <= 10){
                Console.WriteLine($"[{filename}]:[1] Missing or corrupted header.");    
            } else{
                if (!String.IsNullOrEmpty(content[match.Count]))
                {
                    Console.WriteLine($"[{filename}]:[{match.Count}] Missing an empty line after header.");
                }
            }
           

        }
    }
}
