using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification
{
    public class Pre
    {
        public string condition { get; set; }
        public Pre(string input)
        {
            condition = input.Remove(0, 3);

            while(condition.Contains(" "))
            {
                condition = condition.Replace(" ", "");
            }
        }


        public bool isEmptyOrWhiteSpace()
        {
            return string.IsNullOrEmpty(condition) || string.IsNullOrWhiteSpace(condition);
        }
    }
}
