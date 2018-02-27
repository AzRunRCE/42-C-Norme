using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    class FunctionsDefinition : IRules
    {

        public void Verify(string filename, string[] content)
        {
             CheckDefinition(filename,content);
           
        }

        public void CheckDefinition(string filename, string[] content){
            Regex reg = new Regex("(static )?(inline )?(const )?[a-z0-9_]+[ \t]+(\\*)*[a-zA-Z0-9_]+\\(([a-zA-Z0-9_*,]*[ \t]+[a-zA-Z0-9_*,]+,?)*\\)[ \t]*;?",RegexOptions.IgnoreCase);

            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];


                var res = reg.Match(line);
                if (res.Success)
                {
                    var m = new Regex("[a-zA-Z0-9_]+\\(", RegexOptions.IgnoreCase).Match(res.Captures[0].Value);
                    string func_name = m.Captures[0].Value.Split('(')[0];
                    var func_param = res.Captures[0].Value.Substring(m.Captures[0].Index + m.Captures[0].Length).Split(')')[0];
                    if (func_param.Split(',').Length > 4)
                    {
                        Console.WriteLine($"[{filename}]:[{i}] Too many arguments for this function.");
                    }
                    if (String.IsNullOrEmpty(func_param))
                    {
                        Console.WriteLine($"[{filename}]:[{i}] Missing void word for this function.");
                    }

                    if (new Regex("[A-Z]+").Match(line).Success){
                        Console.WriteLine($"[{filename}]:[{i}] Forbidden uppercase in function name.");
                    }

                    if (line.EndsWith(";") && !filename.EndsWith("h") && !filename.StartsWith("static")){
                        Console.WriteLine($"[{filename}]:[{i}] Forbidden function declaration in c file.");
                    }
                   
                    if (!line.EndsWith(";") && filename.EndsWith("c") && !line.Contains("#define"))
                    {
                        Console.WriteLine($"[{filename}]:[{i}] Forbidden function implementation in h file.");
                    }
                
                }

            }
            
        }
    }
}
