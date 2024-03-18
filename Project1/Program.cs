using Project1;
using Project1.CommandReader;
using Project1.ReadCommands;
using System.IO;
using System;
using System.Text;
using System.Reflection;
using System.Xml.Linq;

namespace Project1;
public class TreeNode
{
    public string Name;
    public TreeNode OperandA;
    public TreeNode OperandB;
    private bool _value;

    public bool Value
    {
        get
        {
            if (Name == "&")
                return OperandA.Value & OperandB.Value;
            else if (Name == "|")
                return OperandA.Value | OperandB.Value;
            else if (Name == "!")
                return !_value;
            else 
                return _value;
        }
        set
        {
            if (Name != "&" && Name != "|" && Name != "!") 
                _value = value;
            else
                throw new Exception("Cannot set value for operators");
        }
    }
    public TreeNode()
    {
    }
    public TreeNode(string name)
    {
        Name = name;
    }
}
struct FunctionRootKVP
{
    public string functionName;
    public TreeNode root;
}
struct FunctionArgumentOrderKVP
{
    public string functionName;
    public List<string> argumentOrder;
}
class KeyValueListItem
{
    public FunctionRootKVP functionRootItem;
    public FunctionArgumentOrderKVP argumentOrderItem;
    public KeyValueListItem next;
}
class HashTable
{
    private const int TableSize = 100;
    private KeyValueListItem[] _keyValueLists;
    private KeyValueListItem[] _argumentOrderLists;

    public HashTable()
    {
        _keyValueLists = new KeyValueListItem[TableSize];
        _argumentOrderLists = new KeyValueListItem[TableSize];
    }

    public void AddFunctionAndRoot(string functionName, TreeNode rootNode)
    {
        int hash = HashFunc(functionName);
        var newItem = new KeyValueListItem { functionRootItem = new FunctionRootKVP { functionName = functionName, root = rootNode } };

        if (_keyValueLists[hash] == null)
        {
            _keyValueLists[hash] = newItem;
        }
        else
        {
            newItem.next = _keyValueLists[hash];
            _keyValueLists[hash] = newItem;
        }
    }
    public void AddFunctionAndArgumentsOrder(string functionName, List<string> argumentOrder)
    {
        int hash = HashFunc(functionName);
        var argumentOrderItem = new KeyValueListItem { argumentOrderItem = new FunctionArgumentOrderKVP { functionName = functionName, argumentOrder = argumentOrder } };

        if (_argumentOrderLists[hash] == null)
        {
            _argumentOrderLists[hash] = argumentOrderItem;
        }
        else
        {
            argumentOrderItem.next = _argumentOrderLists[hash];
            _argumentOrderLists[hash] = argumentOrderItem;
        }
    }
    public TreeNode GetRoot(string functionName)
    {
        int hash = HashFunc(functionName);

        var current = _keyValueLists[hash];
        while (current != null)
        {
            if (current.functionRootItem.functionName == functionName)
            {
                return current.functionRootItem.root;
            }
            current = current.next;
        }
        throw new KeyNotFoundException($"Function '{functionName}' not found in the 'FunctionAndRoot' hashtable.");
    }
    public List<string> GetArgumentOrder(string functionName)
    {
        int hash = HashFunc(functionName);

        var current = _argumentOrderLists[hash];
        while (current != null)
        {
            if (current.argumentOrderItem.functionName == functionName)
            {
                return current.argumentOrderItem.argumentOrder;
            }

            current = current.next;
        }

        throw new KeyNotFoundException($"Function '{functionName}' not found in the 'FunctionAndArgumentOrder' hashtable.");
    }
    private int HashFunc(string key)
    {
        var v = 0;
        for (int i = 0; i < key.Length; i++)
        {
            v += key[i];
        }
        return v % TableSize;
    }
}
class SimpleDictionary
{
    private List<string> keys;
    private List<int> values;

    public SimpleDictionary()
    {
        keys = new List<string>();
        values = new List<int>();
    }
    public void Add(string key, int value)
    {
        int index = FindKeyIndex(key);
        if (index == -1)
        {
            keys.Add(key);
            values.Add(value);
        }
        else
        {
            values[index] = value;
        }
    }
    public int GetValue(string key)
    {
        int index = FindKeyIndex(key);
        if (index != -1)
        {
            return values[index];
        }
        throw new KeyNotFoundException($"Key '{key}' not found in the dictionary.");
    }
    private int FindKeyIndex(string key)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == key)
            {
                return i;
            }
        }
        return -1;
    }
}
public class SimpleStack<T>
{
    private T[] items;
    private int topIdx;

    public SimpleStack(int capacity)
    {
        items = new T[capacity];
        topIdx = -1;
    }

    public void Push(T value)
    {
        if (topIdx >= items.Length - 1)
            throw new InvalidOperationException("Stack overflow");

        items[++topIdx] = value;
    }

    public T Pop()
    {
        if (topIdx < 0)
            throw new InvalidOperationException("Stack is empty.");

        return items[topIdx--];
    }
    public int Count()
    {
        return topIdx + 1;
    }
}
internal class VariationGenerator
{
    public List<List<int>> GenerateVariations(int numberOfArguments)
    {
        List<List<int>> result = new List<List<int>>();
        int[] currentValues = new int[numberOfArguments];

        do
        {
            List<int> variation = new List<int>();
            for (int i = 0; i < numberOfArguments; i++)
            {
                variation.Add(currentValues[i]);
            }
            result.Add(variation);
        } while (Next(2, currentValues)); 

        return result;
    }

    private bool Next(int n, int[] currentValues)
    {
        int k = currentValues.Length;
        currentValues[k - 1]++;

        for (int i = k - 1; i > 0; i--)
        {
            if (currentValues[i] >= n)
            {
                currentValues[i] = 0;
                currentValues[i - 1]++;
            }
        }

        return currentValues[0] < n;
    }
}
public class TruthTable
{
    public List<List<int>> rows;
    public List<int> results; 

    public TruthTable()
    {
        rows = new List<List<int>>();
        results = new List<int>();
    }
    public void AddRow(List<int> row, int result)
    {
        rows.Add(row);
        results.Add(result);
    }
    public bool Matches(TreeNode root)
    {
        foreach (var (row, result) in CombineRowsAndResults())
        {
            int expressionResult = EvaluateExpression(root, row); 
            if (expressionResult != result)
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerable<(List<int> row, int result)> CombineRowsAndResults()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            yield return (rows[i], results[i]);
        }
    }
    private static int EvaluateExpression(TreeNode root, List<int> row)
    {
        if (root == null)
            throw new ArgumentNullException(root.Name);

        if (IsOperand(root.Name[0]))
            return row[root.Name[0] - 'a'];

        if (root.Name == "&")
            return (EvaluateExpression(root.OperandA, row) & EvaluateExpression(root.OperandB, row));

        if (root.Name == "|")
            return (EvaluateExpression(root.OperandA, row) | EvaluateExpression(root.OperandB, row));

        if (root.Name == "!")
            return (EvaluateExpression(root.OperandA, row) == 0) ? 1 : 0;

        throw new InvalidOperationException($"Invalid operator: {root.Name}");
    }
    private static bool IsOperand(char c)
    {
        return (c >= 'a' && c <= 'z');
    }
}
public class LogicFunctionFinder
{
    private static List<TreeNode> GenerateAllTrees(int numberOfArguments)
    {
        List<TreeNode> result = new List<TreeNode>();
        GenerateTreesRecursive(result, new TreeNode("a"), numberOfArguments - 1, numberOfArguments);
        GenerateTreesRecursive(result, new TreeNode("b"), numberOfArguments - 1, numberOfArguments);
        GenerateTreesRecursive(result, new TreeNode("c"), numberOfArguments - 1, numberOfArguments);

        return result;
    }
    private static void GenerateTreesRecursive(List<TreeNode> result, TreeNode current, int remainingLevels, int numberOfArguments)
    {
        if (remainingLevels == 0)
        {
            result.Add(current);
            return;
        }
        TreeNode andNode = new TreeNode("&");
        andNode.OperandA = current;
        andNode.OperandB = new TreeNode("a"); 
        GenerateTreesRecursive(result, andNode, remainingLevels - 1, numberOfArguments);

        TreeNode orNode = new TreeNode("|");
        orNode.OperandA = current;
        orNode.OperandB = new TreeNode("a");
        GenerateTreesRecursive(result, orNode, remainingLevels - 1, numberOfArguments);

        for (char operand = 'a'; operand < 'a' + numberOfArguments; operand++)
        {
            andNode = new TreeNode("&");
            andNode.OperandA = current;
            andNode.OperandB = new TreeNode(operand.ToString());
            GenerateTreesRecursive(result, andNode, remainingLevels - 1, numberOfArguments);

            orNode = new TreeNode("|");
            orNode.OperandA = current;
            orNode.OperandB = new TreeNode(operand.ToString());
            GenerateTreesRecursive(result, orNode, remainingLevels - 1, numberOfArguments);
        }
    }
    public string FindFunction(TruthTable truthTable)
    {
        int numberOfArguments = truthTable.rows[0].Count;
        List<TreeNode> allTrees = GenerateAllTrees(numberOfArguments);

        foreach (var tree in allTrees)
        {
            if (truthTable.Matches(tree))
            {
                Console.WriteLine("Match found!");
                return ConvertTreeToString(tree);
            }
        }
        return "No matching function found";
    }
    public static string ConvertTreeToString(TreeNode root)
    {
        if (root == null)
            throw new ArgumentNullException(root.Name);

        if (IsOperand(root.Name[0]))
            return root.Name.ToString();

        string left = ConvertTreeToString(root.OperandA);
        string right = ConvertTreeToString(root.OperandB);

        if (root.Name == "&")
            return $"({left} & {right})";

        if (root.Name == "|")
            return $"({left} | {right})";

        if (root.Name == "!")
            return $"(!{left})";

        throw new InvalidOperationException($"Invalid operator: {root.Name}");
    }
    private static bool IsOperand(char c)
    {
        return (c >= 'a' && c <= 'z');
    }
}
public class StartUp
{
    static HashTable functionRootTable = new HashTable();
    static HashTable functionArgumentOrderTable = new HashTable();
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Input command: ");
            string input = Console.ReadLine();
            if (input == null || input == " ")
            {
                break;
            }
            if (CommandMatcher.CommandStartsWith(input, "DEFINE"))
            {
                DefineCommandReader defineCommandReader = new DefineCommandReader();
                string[] parts = defineCommandReader.ReadDefineCommand(input);
                List<string> argumentsOrder = defineCommandReader.ArgumentsOrder;
                string functionName = FunctionNameExtractor.GetFunctionName(parts[1]);
                functionArgumentOrderTable.AddFunctionAndArgumentsOrder(functionName, argumentsOrder);
                try
                {
                    string expression = GetExpression(parts[2]);
                    if(ArgumentsInExpression(expression) != argumentsOrder.Count) 
                    {
                        throw new ArgumentException("There is undefined operand in the expression.");
                    }
                    TreeNode root = BuildTree(expression);
                    functionRootTable.AddFunctionAndRoot(functionName, root);

                    TreeNode retrievedNode = functionRootTable.GetRoot(functionName);
                    List<string> arguments = functionArgumentOrderTable.GetArgumentOrder(functionName);
                    Console.WriteLine($"Retrieved Node for {functionName}: {retrievedNode.Name}");
                    Console.WriteLine($"Retrieved arguments {functionName}: {string.Join(" ", arguments)}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }             
            }
            //DEFINE A(a, b, c, d): "ab&c|d!|"
            //SOLVE A(0, 0, 1, 1)
            else if (CommandMatcher.CommandStartsWith(input, "SOLVE"))
            {
                SolveCommandReader solveCommandReader = new SolveCommandReader();
                string[] solveParts = solveCommandReader.ReadSolveCommand(input);
                string functionName = FunctionNameExtractor.GetFunctionName(solveParts[1]);
                List<string> arguments = GetArguments(solveParts[1]);
                TreeNode retrievedNode = functionRootTable.GetRoot(functionName);

                int result = Solve(retrievedNode, functionName, arguments);
                Console.WriteLine($"Result: {result}");
            }
            else if (CommandMatcher.CommandStartsWith(input, "ALL"))
            {
                AllCommandReader allCommandReader = new AllCommandReader();
                string[] allParts = allCommandReader.ReadAllCommand(input);
                string functionName = allParts[1];
                TreeNode root = functionRootTable.GetRoot(functionName);
                List<string> argumentOrder = functionArgumentOrderTable.GetArgumentOrder(functionName);
                VariationGenerator variationGenerator = new VariationGenerator();
                List<List<int>> variations = variationGenerator.GenerateVariations(argumentOrder.Count); 

                Console.WriteLine($"ALL {functionName} -> {string.Join(" , ", argumentOrder)} : {functionName}");
                foreach (var variation in variations)
                {
                    SimpleDictionary argumentValues = new SimpleDictionary();
                    for (int i = 0; i < argumentOrder.Count; i++)
                    {
                        argumentValues.Add(argumentOrder[i], variation[i]);
                    }

                    UpdateArguments(root, argumentValues);
                    bool result = DFS(root);

                    Console.WriteLine($"{string.Join(" , ", variation)} : {(result ? 1 : 0)}");
                }
            }
            //FIND 	0,0:0;0,1:0;1,0:0;1,1:1;
            else if (CommandMatcher.CommandStartsWith(input, "FIND"))
            {
                var parser = new FindCommandParser();
                if (parser.TryParse(input, out var truthTable))
                {
                    var functionFinder = new LogicFunctionFinder();
                    string logicalFunction = functionFinder.FindFunction(truthTable);
                    Console.WriteLine($"Found logical function: {logicalFunction}");
                }
                else
                {
                    Console.WriteLine("Failed to parse the input.");
                }            
            }           
        }    
        static string GetExpression(string expression)
        {
            int index = 0;
            while (index < expression.Length)
            {
                char currentChar = expression[index];
                if (IsUpperCaseLetter(currentChar))
                {
                    string functionName = currentChar + "";
                    int cnt = functionArgumentOrderTable.GetArgumentOrder(functionName).Count;
                    int argumentsCnt = 1;

                    index++;

                    while (index < expression.Length && expression[index] != ')')
                    {

                        if (expression[index] == ',')
                        {
                            argumentsCnt++;
                        }
                        index++;
                    }
                    if (cnt != argumentsCnt)
                    {
                        throw new ArgumentException($"{functionName} needs {cnt} arguments, but has {argumentsCnt}");
                    }
                }
                index++;
            }
            return expression;
        }
        static int ArgumentsInExpression(string expression) 
        {
            int index = 0;
            int argumentsCnt = 0;
            while (index < expression.Length)
            {
                char currentChar = expression[index];
                if(IsOperand(currentChar))
                {
                    argumentsCnt++;
                }
                index++;
            }
            return argumentsCnt;    
        }
        static TreeNode BuildTree(string expression)
        {
            SimpleStack<TreeNode> stack = new SimpleStack<TreeNode>(expression.Length);
            int index = 0;

            while (index < expression.Length)
            {
                char currentChar = expression[index];

                if (currentChar == ' ' || currentChar == '\"' || currentChar == ')' || currentChar == '(')
                {
                    index++;
                    continue;
                }
                else if (IsOperator(currentChar))
                {
                    TreeNode newOperator = new TreeNode { Name = CharToString(currentChar) };

                    if (newOperator.Name == "!" && stack.Count() > 0)
                    {
                        newOperator.OperandA = stack.Pop();
                        stack.Push(newOperator);
                    }
                    else if (stack.Count() >= 2)
                    {
                        newOperator.OperandB = stack.Pop();
                        newOperator.OperandA = stack.Pop();
                        stack.Push(newOperator);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid expression format");
                    }
                }
                else if (IsOperand(currentChar))
                {
                    TreeNode newOperand = new TreeNode { Name = CharToString(currentChar) };
                    stack.Push(newOperand);
                }
                else if (IsUpperCaseLetter(currentChar))
                {
                    string functionName = CharToString(currentChar);
                    TreeNode functionRoot = functionRootTable.GetRoot(functionName);
                    stack.Push(functionRoot);

                    while (index < expression.Length && IsUpperCaseLetter(expression[index]))
                    {
                        index++;
                    }
                    while (index < expression.Length && expression[index] != ')')
                    {
                        index++;
                    }
                    continue;
                }
                index++;
            }

            Stack<TreeNode> finalStack = new Stack<TreeNode>(stack.Count());

            while (stack.Count() > 0)
            {
                finalStack.Push(stack.Pop());
            }

            TreeNode finalRoot = finalStack.Pop();

            while (finalStack.Count > 0)
            {
                TreeNode operatorNode = finalStack.Pop();
                operatorNode.OperandA = finalRoot;
                finalRoot = operatorNode;
            }

            return finalRoot;
        }
        static List<string> GetArguments(string input)
        {
            int index = 0; 
            List<string> argumentsOrder = new List<string>();

            while (index < input.Length && input[index] != '(')
            {
                index++;
            }
            index++;
            while (index < input.Length && input[index] != ')')
            {
                int startIndex = index;
                while (index < input.Length && input[index] != ',' && input[index] != ')')
                {
                    index++;
                }

                argumentsOrder.Add(GetSubstring(input, startIndex, index - startIndex));
                if (index < input.Length && input[index] == ',')
                {
                    index++;
                }
            }
            return argumentsOrder;
        }
        static int Solve(TreeNode root, string functionName, List<string> arguments)
        {
            List<string> argumentOrder = functionArgumentOrderTable.GetArgumentOrder(functionName);
            SimpleDictionary argumentValues = new SimpleDictionary();

            for (int i = 0; i < argumentOrder.Count; i++)
            {
                argumentValues.Add(argumentOrder[i], int.Parse(arguments[i]));
            }
            UpdateArguments(root, argumentValues);

            return DFS(root) ? 1 : 0;
        }
        static void UpdateArguments(TreeNode node, SimpleDictionary argumentValues)
        {
            if (node == null)
            {
                return;
            }
            if (IsOperand(node.Name[0]))
            {
                int argumentValue = argumentValues.GetValue(node.Name);
                node.Value = argumentValue != 0;
                return;
            }

            UpdateArguments(node.OperandA, argumentValues);
            UpdateArguments(node.OperandB, argumentValues);
        }
        static bool DFS(TreeNode root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (IsOperand(root.Name[0]))
            {
                return root.Value;
            }

            if (root.Name == "&")
            {
                return DFS(root.OperandA) && DFS(root.OperandB);
            }

            if (root.Name == "|")
            {
                return DFS(root.OperandA) || DFS(root.OperandB);
            }

            if (root.Name == "!")
            {
                return !DFS(root.OperandA);
            }
            throw new InvalidOperationException($"Invalid operator: {root.Name}");
        }
        static bool IsOperator(char c)
        {
            return c == '&' || c == '|' || c == '!';
        }
        static bool IsOperand(char c)
        {
            return (c >= 'a' && c <= 'z');
        }
        static bool IsUpperCaseLetter(char c)
        {
            return (c >= 'A' && c <= 'Z');
        }
        static string GetSubstring(string str, int startIndex, int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = str[startIndex + i];
            }

            return new string(result);
        }
        static string CharToString(char character)
        {
            return character + "";
        }
    }
}


 
