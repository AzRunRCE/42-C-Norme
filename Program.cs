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
           string[] str =
                File.ReadAllLines(@"My_app.c");
           

            List<IRules> rules = new List<IRules>();


            rules.Add(new StructureDefinition());
            rules.Add(new TypedefUsage());
            rules.Add(new NamingConvention("union","u_"));
            rules.Add(new NamingConvention("struct", "s_"));
            rules.Add(new NamingConvention("enum", "e_"));
            rules.Add(new NamingConvention("global", "g_"));
            rules.Add(new FunctionsDefinition());
            //rules.Add(new NamingConvention("typedef struct", "s_"));
            rules.Add(new Formating());
            rules.Add(new Misc());
            foreach (var rule in rules)
            {
                rule.Verify("ft_app.c", str);
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
