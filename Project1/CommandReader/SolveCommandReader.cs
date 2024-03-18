using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.CommandReader
{
    internal class SolveCommandReader
    {
        private int index;
        private string[] parts;

        internal SolveCommandReader()
        {
            index = 0;
            parts = new string[2];
        }
        internal string[] ReadSolveCommand(string input)
        {
            int partsIndex = 0;

            parts[partsIndex++] = GetSubstring(input, index, 5); 
            index += 5; 

            while (index < input.Length && IsWhiteSpace(input[index]))
            {
                index++;
            }

            int startIndex = index;
            while (index < input.Length && input[index] != ')')
            {
                index++;
            }

            parts[partsIndex] = GetSubstring(input, startIndex, index - startIndex + 1); 
            return parts;
        }

        private bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
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
    }
}

