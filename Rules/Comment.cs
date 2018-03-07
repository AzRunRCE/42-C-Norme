using System;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Comment : IRules
    {
        public Comment()
        {
        }
        public void Verify(string filename, string[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];

                Regex reg = new Regex(@"^((\s)*\/\/)\s*");

                if (reg.Match(line).Success)
                {
                    Console.WriteLine($"[{filename}]:[{i}] Wrong type comment.");
                }
                reg = new Regex(@"^((\s)*\/\*)\s*");
                if (reg.Match(line).Success)
                {
                    //Console.WriteLine($"[{filename}]:[{i}] Comment in function forbiden.");
                }
               
            }

        }
    }
}
