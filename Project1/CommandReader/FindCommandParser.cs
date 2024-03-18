using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.CommandReader
{
    internal class FindCommandParser
    {
        public bool TryParse(string input, out TruthTable truthTable)
        {
            truthTable = new TruthTable();
            string[] lines = input.Split(';');
            int startIndex = 0;

            for (int i = startIndex; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }
                string line = lines[i].Trim();

                if (line.StartsWith("FIND"))
                {
                    line = line.Substring("FIND".Length).Trim();
                }
                
                if (TryParseLine(line, out var row, out var result))
                { 
                    truthTable.AddRow(row, result);
                }
                else
                {
                    return false;
                }               
            }
                return true; 
        }

        private bool TryParseLine(string line, out List<int> row, out int result)
        {
            row = new List<int>();
            int colonIndex = line.IndexOf(':');

            if (colonIndex != -1)
            {
                string arguments = line.Substring(0, colonIndex);
                string[] argumentValues = arguments.Split(',');

                foreach (var values in argumentValues)
                {
                    row.Add(int.Parse(values));
                }   
                if (int.TryParse(line.Substring(colonIndex + 1), out result))
                {
                    return true;
                }
            }
            result = 0;
            return false; 
        }
    }
}

