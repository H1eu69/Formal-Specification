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

        public List<string> cases { get; set; }

        public Post(string input)
        {
            cases = new List<string>();
            description = input.Remove(0, 4);
            description = description.Replace("\r\n", string.Empty);

            while (description.Contains(" "))
            {
                description = description.Replace(" ", "");
            }

            string temp = description;
            while (temp.Contains("|")){
                cases.Add(temp.Substring(0, temp.IndexOf("|")));
                temp = temp.Remove(0, temp.IndexOf("|") + 2);
            }
            cases.Add(temp);
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
                string temp = input.Remove(1, GetStringInit(input).Length + 4);
                temp = temp.Remove(0, 1);
                temp = temp.Remove(temp.LastIndexOf(')'), 1);
                return temp;
            }
            return input;
        }


        public bool isEmptyOrWhiteSpace()
        {
            return string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description);
        }
    }
}
