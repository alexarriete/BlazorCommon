using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class MyLanguages
    {
        public static List<LanguageItem> Get()
        {
            var colors = new System.Drawing.Color().GetType().GetProperties().ToList();
            var items = new List<LanguageItem>();
            var culture = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures).Select(l => l.NativeName).Distinct().ToList();
            foreach (var l in culture)
            {
                Random rnd = new Random();
                var selectedColor = colors[rnd.Next(1, colors.Count() - 1)];
                items.Add(new LanguageItem() { Color = selectedColor.Name.ToLower(), Name = l });
            }

            

            return items;
        }

        public static List<LanguageItem> GetCommon( )
        {
            var items = new List<LanguageItem>();
            items.Add(new LanguageItem() { Color = "red", Name = "en" });
            items.Add(new LanguageItem() { Color = "blue", Name = "fr" });
            items.Add(new LanguageItem() { Color = "white", Name = "es" });
            return items;
        
        }
    }
}
