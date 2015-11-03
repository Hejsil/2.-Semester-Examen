using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ExtensionMethods
{
    public static class IntExtensionMethods
    {
        //Converst the int to the formatted string x,xx kr
        public static string ToKr(this int cash)
        {
            string cashString = cash.ToString();
            int length = cashString.Count();

            //Inserts zeros until the string is of length 3
            for (int i = length; i < 3; i++)
                cashString = '0' + cashString;

            //Inserts the decimal seperator
            cashString = cashString.Insert(cashString.Count() - 2, NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator.ToString());

            return cashString + " kr";
        }
    }
}
