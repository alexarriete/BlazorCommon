using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class Theme
    {
        public string HeaderColor { get; set; }
        public string HeaderFontColor { get; set; }

        public Theme()
        {
            HeaderColor = Color.DarkSeaGreen.Name;
            HeaderFontColor = Color.Black.Name;
        }

        public Theme(Color headerColor, Color fontColor)
        {
            HeaderColor = headerColor.Name;
            HeaderFontColor = fontColor.Name;
        }
    }
}
