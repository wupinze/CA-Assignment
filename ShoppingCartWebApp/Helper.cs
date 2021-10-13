using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ShoppingCartWebApp
{
    public class Helper
    {

        public static string Highlight(string inString, string searchStr)
        {
            if (String.IsNullOrEmpty(searchStr))
                return inString;

            string lowString = inString.ToLower();
            string lowSearchStr = searchStr.ToLower();
            int replaceLength = searchStr.Length;
            int replacePosition = lowString.IndexOf(lowSearchStr);

            if (replacePosition == -1)
                return inString;
            else
            {
                string replaceString = inString.Substring(replacePosition, replaceLength);
                Debug.WriteLine("replaceString = " + replaceString);
                return inString.Replace(replaceString, "<span class = \"highlight\">" + replaceString + "</span>");
            }
        }
    }
}
