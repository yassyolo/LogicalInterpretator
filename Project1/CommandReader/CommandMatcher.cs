using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.CommandReader
{
    public class CommandMatcher
    {
        public static bool CommandStartsWith(string input, string commandName)
        {
            bool isMatched = true;

            if (commandName.Length > input.Length)
            {
                return !isMatched;
            }

            for (int i = 0; i < commandName.Length; i++)
            {
                if (input[i] != commandName[i])
                {
                    return !isMatched;
                }
            }

            return isMatched;
        }
    }

}
