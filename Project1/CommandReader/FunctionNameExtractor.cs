using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.CommandReader
{
    internal class FunctionNameExtractor
    {
        public static string GetFunctionName(string functionDeclaration)
        {
            int index = 0;

            while (index < functionDeclaration.Length && functionDeclaration[index] != '(')
            {
                index++;
            }
            return GetSubstring(functionDeclaration, 0, index);
        }

        private static string GetSubstring(string str, int startIndex, int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = str[startIndex + i];
            }

            return new string(result);
        }

    }
}
