using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.CommandReader
{
    internal class AllCommandReader
    {
        private int index;
        private string[] parts;

        internal AllCommandReader()
        {
            index = 0;
            parts = new string[2];
        }
        internal string[] ReadAllCommand(string input)
        {
            int partsIndex = 0;

            parts[partsIndex++] = GetSubstring(input, index, 3); 
            index += 3; 

            while (index < input.Length && IsWhiteSpace(input[index]))
            {
                index++;
            }

            int startIndex = index;
            while (index < input.Length)
            {
                index++;
            }
            parts[partsIndex] = GetSubstring(input, startIndex);

            return parts;
        }
        private string GetSubstring(string str, int startIndex, int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = str[startIndex + i];
            }

            return new string(result);
        }
        static string GetSubstring(string str, int startIndex)
        {
            int length = str.Length - startIndex;
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = str[startIndex + i];
            }

            return new string(result);

        }
        private bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
        }
    }
}
