using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttrition.Common
{
    public static class ExtensionMethods //Return all of the properities in a class(objType) except the label
    {
        public static string[] ToPropertyList<T>(this Type objType, string labelName)//
        {
            return objType.GetProperties().Where(a => a.Name != labelName).Select(a => a.Name).ToArray();
        }
           
    }
}
