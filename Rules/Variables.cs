using System;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public class Variables : IRules
    {
        public Variables()
        {
        }

        public void Verify(string filename, string[] content)
        {

            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (line.IsComment()) continue;
                if (new Regex("\\s*\\w+\\s+\\*?\\w+\\s*\\=").Match(line).Success){
                    Console.WriteLine($"[{filename}]:[{i}] Forbidden creation and assignation variable at the same time.");
                }

                if (new Regex("([^(\t ]+_t|[a-zA-Z0-9_]|int|signed|unsigned|char|long|short|float|double|void|const|struct [^ ]+)\\*").Match(line).Success)
                {
                    if (!line.StartsWith("//") && !line.StartsWith("/*"))
                        Console.WriteLine($"[{filename}]:[{i}]  Misplaced pointer symbol.");
                }


            }
        }
    }
}
