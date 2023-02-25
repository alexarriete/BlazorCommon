using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public static class Extensor
    {
        public static string RemoveDiacritics(this string text, bool toLower = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            if (toLower)
            {
                return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
