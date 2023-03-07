using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class OptionElementTest
    {
        public static List<OptionElement> GetElements()
        {
            List<OptionElement> optionElements = new List<OptionElement>
            {
                new OptionElement() { Value = 1, Name = "Madrid", Active = true },
                new OptionElement() { Value = 2, Name = "Paris", Active = true },
                new OptionElement() { Value = 3, Name = "Berlin", Active = true },
                new OptionElement() { Value = 4, Name = "London", Active = true },
                new OptionElement() { Value = 5, Name = "Rome", Active = true }
            };

            return optionElements.OrderBy(x => x.Name).ToList();
        }

        public static List<OptionElement> GetSearchTextBoxElements()
        {
            List<OptionElement> optionElements = new List<OptionElement>();
            int counter = 1;
            foreach (var item in Animal.GetAll())
            {
                OptionElement element = new OptionElement() { Value = counter, Name = item.Name, Active = true};
                counter++;
                optionElements.Add(element);
            }


            return optionElements.OrderBy(x => x.Name).ToList();
        }
    }
}
