using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.ReadCommands
{
    internal class DefineCommandReader
    {
        private int index;
        private string[] parts;
        public List<string> ArgumentsOrder { get; private set; } 
        internal DefineCommandReader()
        {
            index = 0;
            parts = new string[3];
            ArgumentsOrder = new List<string>();
        }

        internal string[] ReadDefineCommand(string input)
        {
            int partsIndex = 0;

            parts[partsIndex++] = GetSubstring(input, index, 6); 
            index += 6; 

            while (index < input.Length && IsWhiteSpace(input[index]))
            {
                index++;
            }
            int startIndex = index;
            while (index < input.Length && input[index] != ':')
            {
                if (IsLowerCaseLetter(input[index]))
                {
                    string argumentName = "";
                    while (index < input.Length && (IsLowerCaseLetter(input[index])))
                    {
                        argumentName += input[index];
                        index++;
                    }
                    ArgumentsOrder.Add(argumentName);
                }
                else
                {
                    index++;
                }
            }

            parts[partsIndex++] = GetSubstring(input, startIndex, index - startIndex); 

            while (index < input.Length && IsWhiteSpace(input[index]))
            {
                index++;
            }

            parts[partsIndex] = GetSubstring(input, index + 1);

            return parts;
        }

        private bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
        }
        private bool IsLowerCaseLetter(char c)
        {
            return (c >= 'a' && c <= 'z');
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
    }
}
