using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
namespace appRegex.Rules
{
    class FunctionsDefinition : IRules
    {

        public void Verify(string filename, string[] content)
        {
            CheckDefinition(filename,content);
            CheckFunction(filename, content);
          
        }
        public void CheckFunction(string filename, string[] content)
        {
            int level = 0;
            int line_func_start = 0;
            int count = 0;
            for (int i = 0; i < content.Length; i++)
            {
               
                string line = content[i];
                if (line.Contains("{"))
                {
                    if (level == 0)
                    {
                        line_func_start = i;
                        count = -1;
                    }
                    level++;
                }
                else if (line.Contains("}"))
                {
                    level--;
                    if (level == 0){
                        if (count > 25){
                            Console.WriteLine($"[{filename}]:[{line_func_start}] Too many lines for this function ({count}).");
                        }
                        CheckVariableLineUp(filename, content, line_func_start, i);
                        CheckHavingComment(filename, content, line_func_start, i);
                        CheckVariablePosition(filename, content, line_func_start, i);
                        CheckVariableCount(filename, content, line_func_start, i);
                    }

                }
                count++;

            }
        }

        private void CheckVariablePosition(string filename, string[] content, int from, int to)
        {
            List<int> lines = new List<int>();
            Regex reg = new Regex(@"^( |\t)*((struct|unsigned|long|static)( |\t))*[a-zA-Z0-9_]+( |\t)+(?<variablename>[a-zA-Z0-9_\*\[\]]+)( = [a-zA-Z0-9_\(\)\+\-\/%* \""\',{}\?\:\>\[\]]+)*;", RegexOptions.IgnoreCase);
            for (int i = from; i < to; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                Match match = reg.Match(line);
                if (match.Success)
                {
                    lines.Add(i);
                }
            }
            if (lines.Count() == 0)
                return;
            bool not_up = false;
            int firstVariable = lines.First();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (firstVariable != lines[i])
                {
                    not_up = true;
                    break;
                }
                firstVariable++;
            }
            if (not_up)
            {
                Console.WriteLine($"[{filename}]:[{from}] Variable must be on the top from the function.");
            }
            else{
                if (!string.IsNullOrEmpty(content[lines.Last() + 1]))
                    Console.WriteLine($"[{filename}]:[{lines.Last() + 1}] A blank line must be used after variables definitions.");
            }

             
        }

        private void CheckHavingComment(string filename, string[] content, int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                string line = content[i];
                if (line.IsComment())
                {
                    Console.WriteLine($"[{filename}]:[{i}] Comment it's forbidden in function.");
                }
            }
        }

        private void CheckVariableLineUp(string filename, string[] content, int from, int to)
        {
            List<int> tab = new List<int>();
            List<int> lines = new List<int>();

            Regex reg = new Regex(@"^( |\t)*((struct|unsigned|long|static)( |\t))*[a-zA-Z0-9_]+( |\t)+(?<variablename>[a-zA-Z0-9_\*\[\]]+)( = [a-zA-Z0-9_\(\)\+\-\/%* \""\',{}\?\:\>\[\]]+)*;", RegexOptions.IgnoreCase);
            for (int i = from; i < to; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                Match match = reg.Match(line);
                if (match.Success)
                {
                    int lenght = line.GetColumLenght(match.Groups["variablename"].Index);
                    tab.Add(lenght);
                    lines.Add(i);
                }

            }
            if (tab.Distinct().Skip(1).Any())
            {
                Console.WriteLine($"[{filename}]:[{lines.First()}] Variables name has to be lined up.");
            }

        }

        private void CheckVariableCount(string filename, string[] content, int from, int to)
        {
            List<int> lines = new List<int>();
            Regex reg = new Regex(@"^( |\t)*((struct|unsigned|long|static)( |\t))*[a-zA-Z0-9_]+( |\t)+(?<variablename>[a-zA-Z0-9_\*\[\]]+)( = [a-zA-Z0-9_\(\)\+\-\/%* \""\',{}\?\:\>\[\]]+)*;", RegexOptions.IgnoreCase);
            for (int i = from; i < to; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                Match match = reg.Match(line);
                if (match.Success)
                {
                    lines.Add(i);
                }

            }
            if (lines.Count() > 5)
            {
                Console.WriteLine($"[{filename}]:[{lines.First()}] Too many variables in this function.");
            }

        }

        public void CheckDefinition(string filename, string[] content){
            Regex reg = new Regex("(static )?(inline )?(const )?[a-z0-9_]+[ \t]+(\\*)*[a-zA-Z0-9_]+\\(([a-zA-Z0-9_*,]*[ \t]+[a-zA-Z0-9_*,]+,?|[a-zA-Z0-9_*,])*\\)[ \t]*;?",RegexOptions.IgnoreCase);

            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;

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

                    if (new Regex("[A-Z]+").Match(func_name).Success &&  !line.Contains("#define")){
                        Console.WriteLine($"[{filename}]:[{i}] Forbidden uppercase in function name.");
                    }

                    if (line.EndsWith(";") && !filename.StartsWith("static"))
                    {
                        if (filename.EndsWith("c"))
                        {
                            Console.WriteLine($"[{filename}]:[{i}] Forbidden function declaration in c file.");
                        }
                    }
                    else  
                    {
                        if (filename.EndsWith("h")  && !line.EndsWith(";") && !line.Contains("#define")){
                            Console.WriteLine($"[{filename}]:[{i}] Forbidden function implementation in h file.");
                        }

                    }
                
                }

            }
            
        }
    }
}
