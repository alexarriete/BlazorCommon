using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class MyTabs
    {
        public static string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public static List<TabElement> GetTabElements()
        {
            List<TabElement> tabElements = new List<TabElement>();
            tabElements.Add(new TabElement()
            {
                CssId = "id1",
                ButtonName = "Html Text",
                Content = $"<p style='color:red'>{LoremIpsum}</p>",
                Active = true,
                Type = TabType.HtmlText,
                Title = "Button 1 content example",
                Description = "This is a short introduction to our content.",
                Footer = "That's all. Footer comes at the botton."
            }
            );
            tabElements.Add(new TabElement()
            {
                CssId = "id2",
                ButtonName = "Plain Text",
                Content = LoremIpsum,
                Type = TabType.Text,
                Title = "Button 2 content example",
                Description = "This is a short introduction to our content.",
                Footer = "That's all. Footer comes at the botton."
            });
            tabElements.Add(new TabElement()
            {
                CssId = "id3",
                ButtonName = "A Grid",
                Type = TabType.Grid,
                GridConfig = new Grid.GridConfigurationBase()
              
            });

            return tabElements;
        }


    }
}
