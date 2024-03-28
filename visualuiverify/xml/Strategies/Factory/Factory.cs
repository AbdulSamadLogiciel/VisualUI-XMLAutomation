using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualUIAVerify.XMLAutomation.Constants;
using VisualUIAVerify.XMLAutomation.Strategies;

namespace VisualUIAVerify.xml.Strategies.Factory
{
    public class Factory
    {
        public static IXMLGenerationStrategy CreateStrategy(string elementType)
        {
            switch (elementType)
            {
                case UIElementDescriptor.Window:
                    return new WindowStrategy();
                case UIElementDescriptor.Text:
                    return new TextFieldStrategy();
                case UIElementDescriptor.Button:
                    return new ButtonStrategy();
                case UIElementDescriptor.Label:
                    return new TextLabelStrategy();
                default:
                    throw new ArgumentException("Invalid product type");
            }
        }
    }
}
