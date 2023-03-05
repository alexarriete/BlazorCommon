using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public enum BoxColor
    {
        red,
        green, blue,
        yellow,
        magenta,
        cyan,
        lime,
        black,white

    }
    public class LanguageItem
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
