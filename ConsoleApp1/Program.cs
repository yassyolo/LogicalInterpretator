using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Dictionary<string, TreeNode> functions = new Dictionary<string, TreeNode>();

        Console.WriteLine("Въведете команда:");
        string command = Console.ReadLine();

        if (command.StartsWith("DEFINE"))
        {
            string[] parts = ParseDefineCommand(command);
            string functionName = GetFunctionName(parts[1]);
            string expression = GetExpression(parts[3]);

            TreeNode root = BuildTree(expression);
            functions.Add(functionName, root);

            Console.WriteLine("Функцията е дефинирана успешно.");
        }
        else if (command.StartsWith("SOLVE"))
        {
            string[] parts = ParseSolveCommand(command);
            string functionName = parts[1];

            if (functions.ContainsKey(functionName))
            {
                for (int i = 2; i < parts.Length; i += 2)
                {
                    string variableName = parts[i];
                    bool variableValue = bool.Parse(parts[i + 1]);

                    SetVariableValue(functions[functionName], variableName, variableValue);
                }

                bool result = functions[functionName].Value;
                Console.WriteLine($"Резултат: {result}");
            }
            else
            {
                Console.WriteLine($"Грешка: Функция {functionName} не е дефинирана.");
            }
        }
    }
    static string[] ParseSolveCommand(string command)
    {
        List<string> parts = new List<string>();
        int index = 0;

        while (index < command.Length && !IsLetter(command[index]))
        {
            index++;
        }

        parts.Add(GetSubstring(command, index, 5)); // "SOLVE"

        while (index < command.Length && IsWhiteSpace(command[index]))
        {
            index++;
        }

        while (index < command.Length)
        {
            int startIndex = index;
            while (index < command.Length && !IsWhiteSpace(command[index]))
            {
                index++;
            }

            parts.Add(GetSubstring(command, startIndex, index - startIndex)); // Variable name

            while (index < command.Length && IsWhiteSpace(command[index]))
            {
                index++;
            }

            if (index < command.Length)
            {
                parts.Add(command[index].ToString()); // Boolean value
                index++;
            }

            while (index < command.Length && IsWhiteSpace(command[index]))
            {
                index++;
            }
        }

        return parts.ToArray();
    }

    static string[] ParseDefineCommand(string command)
    {
        List<string> parts = new List<string>();
        int index = 0;

        while (index < command.Length && !IsLetter(command[index]))
        {
            index++;
        }

        parts.Add(GetSubstring(command, index, 6)); // "DEFINE"

        while (index < command.Length && IsWhiteSpace(command[index]))
        {
            index++;
        }

        int startIndex = index;
        while (index < command.Length && !IsWhiteSpace(command[index]))
        {
            index++;
        }

        parts.Add(GetSubstring(command, startIndex, index - startIndex)); // Function declaration

        while (index < command.Length && IsWhiteSpace(command[index]))
        {
            index++;
        }

        parts.Add(GetSubstring(command, index)); // Expression

        return parts.ToArray();
    }

    static string GetFunctionName(string functionDeclaration)
    {
        int index = 0;

        while (index < functionDeclaration.Length && functionDeclaration[index] != '(')
        {
            index++;
        }

        return GetSubstring(functionDeclaration, 0, index);
    }

    static string GetExpression(string expression)
    {
        return expression;
    }

    static TreeNode BuildTree(string expression)
    {
        Stack<TreeNode> stack = new Stack<TreeNode>();
        char[] delimiters = new char[] { ' ', '\t' };
        string[] tokens = Tokenize(expression, delimiters);

        foreach (string token in tokens)
        {
            if (bool.TryParse(token, out bool variableValue))
            {
                // If the token is a boolean value, push it onto the stack
                stack.Push(new TreeNode { IsOperand = true, Value = variableValue });
            }
            else
            {
                // If the token is an operator, create a node and push it onto the stack
                TreeNode node = new TreeNode { Name = token };
                node.OperandB = stack.Pop();
                node.OperandA = stack.Pop();
                stack.Push(node);
            }
        }

        // The root of the tree should be at the top of the stack
        return stack.Pop();
    }

    static string[] Tokenize(string expression, char[] delimiters)
    {
        List<string> tokens = new List<string>();
        int index = 0;

        while (index < expression.Length)
        {
            if (IsWhiteSpace(expression[index]))
            {
                index++;
            }
            else if (expression[index] == '&' || expression[index] == '|' || expression[index] == '!')
            {
                tokens.Add(expression[index].ToString());
                index++;
            }
            else
            {
                int startIndex = index;
                while (index < expression.Length && !IsWhiteSpace(expression[index]) &&
                       expression[index] != '&' && expression[index] != '|' && expression[index] != '!')
                {
                    index++;
                }

                tokens.Add(GetSubstring(expression, startIndex, index - startIndex));
            }
        }

        return tokens.ToArray();
    }

    static void SetVariableValue(TreeNode node, string variableName, bool variableValue)
    {
        if (node.IsOperand && node.Name == variableName)
        {
            node.Value = variableValue;
        }

        if (node.OperandA != null)
        {
            SetVariableValue(node.OperandA, variableName, variableValue);
        }

        if (node.OperandB != null)
        {
            SetVariableValue(node.OperandB, variableName, variableValue);
        }
    }

    static bool IsWhiteSpace(char c)
    {
        return c == ' ' || c == '\t';
    }

    static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    static string GetSubstring(string str, int startIndex)
    {
        return str.Substring(startIndex);
    }

    static string GetSubstring(string str, int startIndex, int length)
    {
        return str.Substring(startIndex, length);
    }
}

class TreeNode
{
    public string Name;
    public TreeNode OperandA;
    public TreeNode OperandB;
    public bool IsOperand;
    public bool Value;

    public TreeNode()
    {
        // Initialize any necessary fields or properties
    }
}
