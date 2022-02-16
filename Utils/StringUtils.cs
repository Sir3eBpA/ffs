using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Utils
{
    public static class StringUtils
    {
        public static string StripToCharactersOnly(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            char[] arr = input.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                                  || char.IsWhiteSpace(c)
                                                  || c == '-')));
            return new string(arr);
        }
    }
}
