using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ExtensionMethods
{
    public static class StringExtensionMethods
    {
        //Tries to parse the string to a credit amount. Accepts formats: "x", "x,x", "x,xx"
        public static bool TryToCredit(this string str, out int credit)
        {
            //Splittes the string at the decimal separator
            var splittedString = str.Split(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator.ToCharArray());
            var count = splittedString.Count();
            int res;

            //When the string array only has one element, then there were no decimal seperator
            if (count == 1)
            {
                if (int.TryParse(splittedString[0], out res))
                {
                    credit = res * 100;
                    return true;
                }
            }
            else if (count == 2)
            {
                if (int.TryParse(splittedString[0], out res))
                {
                    credit = res * 100;

                    //If format "x,x"
                    if (splittedString[1].Length == 1)
                    {
                        if (int.TryParse(splittedString[1], out res))
                        {
                            credit += res * 10;
                            return true;
                        }
                    }
                    //If format "x,xx"
                    else if (splittedString[1].Length == 2)
                    {
                        if (int.TryParse(splittedString[1], out res))
                        {
                            credit += res;
                            return true;
                        }
                    }
                }
            }

            credit = 0;
            return false;
        }
    }
}
