
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.Windows.Automation;
using System.Windows.Forms;
using VisualUIAVerify.XMLAutomation.Constants;


namespace VisualUIAVerify.XMLAutomation.Strategies
{
    public class ButtonStrategy : IXMLGenerationStrategy
    {

        
        public static bool IsButton(TreeNode element)          
        {
            var elementType = UIElements.UIElementType(element.Text);
            return  elementType == UIElementDescriptor.Button;
        }

        public void AppendElementHopper(StringBuilder xmlBuilder,  List<string> elementHopper)
        {
            xmlBuilder.Append("\r\n<listOfElementHopper>");
            foreach (string item in elementHopper)
            {
                if (item != "")
                {
                    xmlBuilder.Append($"<ElementHopper AutomationID=\"{item}\"/>");
                }

            }
            xmlBuilder.Append("\r\n</listOfElementHopper>");
        }
        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstButton, List<string> elementHopper)
        {
            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");
            
            TreeWalker treeWalker = TreeWalker.ControlViewWalker;

            AutomationElement parentElement = treeWalker.GetParent(automationElement);
            if (IsButton(element) && isFirstButton && !UIElements.ISNextSiblingElementExists(element))
            {
        
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase AutomationID=\"{parentElement.Current.AutomationId}\" Key=\"{defaultValue}\" Name=\"{defaultValue}\">");
               
                AppendElementHopper(xmlBuilder, elementHopper);
                xmlBuilder.Append($"\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\"><ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");
                isFirstButton = false;

            }
            else if (IsButton(element) && isFirstButton && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase AutomationID=\"{parentElement.Current.AutomationId}\" Key=\"{defaultValue}\" Name=\"{defaultValue}\">");
                AppendElementHopper(xmlBuilder, elementHopper);
                xmlBuilder.Append($"\r\n<SubControls><ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                isFirstButton = false;

            }
            else if (IsButton(element) && !isFirstButton && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");

            }

            else if (IsButton(element) && !isFirstButton && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");

            }
        }
    }
}
