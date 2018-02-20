using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using appRegex.Rules;

namespace appRegex
{
    class Program
    {
        struct hello
        {
            
        }
        private  static void Main(string[] args)
        {
            var str =
                File.ReadAllLines(@"app.c");
            //"(\s|\t)*(void|char|string|int|double|float)(\s|\t)+(\w+)(\s|\t)*(\(.*\))" function

          

            List<IRules> rules = new List<IRules>();


            rules.Add(new StructureDefinition());
            rules.Add(new TypedefUsage());

            foreach (var rule in rules)
            {
                rule.Verify("app.c", str);
            }
            //[ \t]*(typedef[ \t]*)?(struct|union|enum)([ \t]+[_a-zA-Z0-9]+)?$
      
      
          
        
            Console.ReadKey();
        }
        public string stdr(string plop)
        {
            return "plop";
        }
        public  void     Func ()
        {
          Console.WriteLine("plop");
        }
    }
}
