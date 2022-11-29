using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification
{
    public class Post
    {
        public string description { get; set; }

        public string type { get; set; }

        public List<string> cases { get; set; }
        public List<LoopCondition> loops { get; set; }
        public string conclude { get; set; }

        public Post(string input, bool isArray)
        {
            if (!isArray)
            {
                cases = new List<string>();
                description = input.Remove(0, 4);
                description = description.Replace("\r\n", string.Empty);

                while (description.Contains(" "))
                {
                    description = description.Replace(" ", "");
                }

                string temp = description;
                while (temp.Contains("|"))
                {
                    cases.Add(temp.Substring(0, temp.IndexOf("|")));
                    temp = temp.Remove(0, temp.IndexOf("|") + 2);
                }
                cases.Add(temp);
            }
            else
            {
                loops = new List<LoopCondition>();  
                description = input.Replace("\r\n", string.Empty);

                while (description.Contains(" "))
                {
                    description = description.Replace(" ", "");
                }
                description = description.Remove(0, 8);
                string temp = description.Remove(0, 1);
                temp = temp.Remove(temp.LastIndexOf(')'), 1);

                string tempCount = temp;
                List<int> dotIndex = new List<int>();
                int i = 0;
                while (tempCount != "")
                {
                    if (tempCount[0] == '.')
                    {
                        if (tempCount[1] != '.')
                        {
                            dotIndex.Add(i);
                            i++;
                            tempCount = tempCount.Remove(0, 1);
                        }
                        else
                        {
                            i += 2;
                            tempCount = tempCount.Remove(0, 2);
                        }
                    }
                    else
                    {
                        i++;
                        tempCount = tempCount.Remove(0, 1);
                    }

                }
                int count = dotIndex.Count;
                string removeTemp = temp;
                for ( i = 0; i < count; i++)
                {
                    LoopCondition loopCondition = new LoopCondition();
                    loopCondition.type = temp.Substring(0, 2);
                    loopCondition.var_name = temp.Substring(2, temp.IndexOf("TH") - 2);
                    loopCondition.condition = temp.Substring(temp.IndexOf('{'), temp.IndexOf('}') - temp.IndexOf('{') + 1);

                    string condition_temp = loopCondition.condition;
                    loopCondition.start = condition_temp.Substring( 1, condition_temp.IndexOf('.') - 1);
                    loopCondition.end = condition_temp.Substring(condition_temp.LastIndexOf('.') + 1, condition_temp.IndexOf('}') - condition_temp.LastIndexOf('.') - 1);
                    loops.Add(loopCondition);

                    temp = removeTemp.Remove(0, dotIndex.First() + 1);
                    dotIndex.RemoveAt(0);
                }
                conclude = temp;
            }
        }

        public string GetStringInit(string input)
        {
            string result = "";
            int count = 0;
            if (input.Contains("&&"))
            {
                for (int i = 0; i <= input.Length; i++)
                {
                    if (input[i].Equals('('))
                        count += 1;
                    else
                    {
                        result = input.Substring(i, input.IndexOf(')') - count).ToString();
                        if (result.Contains("TRUE"))
                            result = result.Replace("TRUE", "true");
                        if (result.Contains("FALSE"))
                            result = result.Replace("FALSE", "false");
                        return result;
                    }
                }
                return result;
            }
            if (input.Contains('('))
                return input.Substring(1, input.IndexOf(')') - 1);
            return input;
        }

        public string GetCondition(string input)
        {
            if (input.Contains("&&"))
            {
                string output = input.Remove(1, GetStringInit(input).Length + 4);
                output = output.Remove(0, 1);
                output = output.Remove(output.LastIndexOf(')'), 1);
                int loopCount = output.Length;
                for(int i = 0;i < loopCount; i++)
                {
                    if (output[i] == '='
                        && output[i - 1] != '!'
                        && output[i - 1] != '<'
                        && output[i - 1] != '>'
                        && output[i - 1] != '=')
                    {
                        output = output.Substring(0, i + 1) + '=' + output.Substring(i + 1);
                    }
                }
                return output;
            }
            return input;
        }


        public bool isEmptyOrWhiteSpace()
        {
            return string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description);
        }
    }
}
