using System;
using System.Text.RegularExpressions;

namespace appRegex.Rules
{
    public static class Tools
    {
        public static int GetColumLenght(this string line , int to)
        {
            int index = 0;
            int lenght = 0;
            while (index < to)
            {
                while (index < to && line[index] != '\t')
                {
                    index++;
                    lenght++;
                }
                if (index < to && line[index] == '\t')
                {
                    int tabSize = getTabSize(lenght);
                    lenght = lenght + tabSize;
                    index++;
                }
            }
            return lenght;
        }

        private static int getTabSize(int size)
        {
            int count = 1;
            int ret = 0;

            while (size >= count * 8)
            {
                count++;
            }
            ret = (count * 8) - size;
            return ret;
        }
        public static bool IsComment(this string str)
        {
            Regex reg = new Regex(@"^((\s)*\/\/)\s*");

            if (reg.Match(str).Success)
            {
                return true;
            }
            reg = new Regex(@"^((\s)*\/\*)\s*");
            if (reg.Match(str).Success)
            {
                return true;
            }
            return false;

        }
    }
}
