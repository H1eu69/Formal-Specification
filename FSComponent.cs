using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification
{
    public class FSComponent
    {
        public string func_name { get; set; }
        public List<DataType> parameters { get; set; }

        public DataType output { get; set; }
        public Pre pre { get; set; }
        public Post post { get; set; }

        public FSComponent(string input)
        {
            string[] lines = input
                .Split(Environment.NewLine.ToCharArray())
                .Skip(2)
                .ToArray();

            string firstLine = GetLine(input, 1);
            string secLine = GetLine(input, 2);

            parameters = new List<DataType>();
            output = new DataType();
            pre = new Pre(secLine);

            while (firstLine.Contains(" "))
            {
                firstLine = firstLine.Replace(" ", "");
            }
            func_name = firstLine.Substring(0, firstLine.IndexOf("("));

            output.var_name = firstLine.Substring(firstLine.IndexOf(")") + 1, 
                firstLine.LastIndexOf(":") - firstLine.IndexOf(")") - 1);

            output.var_type = firstLine.Substring(firstLine.LastIndexOf(":") + 1);

            string parametersList = firstLine.Substring(firstLine.IndexOf("(") + 1,
                firstLine.LastIndexOf(")") - firstLine.IndexOf("(") - 1);

            int count = parametersList.Count(x => x == ','); 

            for (int i = 0; i <= count; i++)
            {
                DataType parameter = new DataType();
                parameter.var_name = parametersList.Substring(0, parametersList.IndexOf(":"));

                if (count == 0)
                {
                    parameter.var_type = parametersList.Substring(parametersList.IndexOf(":") + 1);
                             
                }
                else
                    parameter.var_type = parametersList.Substring(parametersList.IndexOf(":") + 1,
                            parametersList.IndexOf(",") - parametersList.IndexOf(":") - 1);

                parameters.Add(parameter);
                parametersList = parametersList.Substring(parametersList.IndexOf(",") + 1);
                parametersList = parametersList + ",";
            }

            if (hasArray())
            {
                string thirdLine = string.Join(Environment.NewLine, lines);
                post = new Post(thirdLine, true);
            }
            else
            {
                string thirdLine = string.Join(Environment.NewLine, lines);
                post = new Post(thirdLine, false);
            }
        }

        string GetLine(string text, int lineNo)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            return lines.Length >= lineNo ? lines[lineNo - 1] : null;
        }

        public bool hasArray()
        {
            foreach (var paras in parameters)
            {
                if (paras.GetTypeFormat() == "System.Double[]" || (paras.GetTypeFormat() == "System.Int32[]"))
                    return true;
            }
            return false;
        }


    }
}
