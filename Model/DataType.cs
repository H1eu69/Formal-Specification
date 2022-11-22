using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification
{
    public class DataType
    {
        public string var_name { get; set; }
        public string var_type { get; set; }

        public string GetTypeFormat()
        {
            //Non-Array
            if (var_type == "R")
                return "System.Double";
            if (var_type == "Z")
                return "System.Int32";
            if (var_type == "N")
                return "System.Int32";
            if (var_type == "B")
                return "System.Boolean";
            if (var_type == "char*")
                return "System.String";
            //Array
            if (var_type == "R*")
                return "System.Double[]";
            if (var_type == "Z*")
                return "System.Int32[]";
            if (var_type == "N*")
                return "System.Int32[]";

            return "";
        }

    }
}
