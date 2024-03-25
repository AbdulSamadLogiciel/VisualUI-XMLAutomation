
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using VisualUIAVerify.XMLAutomation.Constants;



namespace VisualUIAVerify.XMLAutomation.Strategies
{
    public class TextFieldStrategy : IXMLGenerationStrategy
    {

        public static bool IsText(TreeNode element)
        {
            return UIElements.UIElementType(element.Text) == UIElementDescriptor.Text;
        }
        public void AppendElementHopper(StringBuilder xmlBuilder, List<string> elementHopper)
        {
            xmlBuilder.Append("\r\n<listOfElementHopper>");
            foreach (string item in elementHopper)
            {
                    xmlBuilder.Append($"<ElementHopper AutomationID=\"{item}\"/>");
            }
            xmlBuilder.Append("\r\n</listOfElementHopper>");
        }
        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstText, List<string> elementHopper)
        {
            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");
            
            TreeWalker treeWalker = TreeWalker.ControlViewWalker;

            AutomationElement parentElement = treeWalker.GetParent(automationElement);

            if (IsText(element) && isFirstText && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase AutomationID=\"{parentElement.Current.AutomationId}\" Key=\"{defaultValue}\" >");
                AppendElementHopper(xmlBuilder, elementHopper);
                xmlBuilder.Append($"\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\"></TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;

            }
            else if (IsText(element) && isFirstText && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"{defaultValue}\" AutomationID=\"{parentElement.Current.AutomationId}\">");
                AppendElementHopper(xmlBuilder, elementHopper);
                xmlBuilder.Append("$\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                isFirstText = false;

            }
            else if (IsText(element) && !isFirstText && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");

            }

            else if (IsText(element) && !isFirstText && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");

            }
        }
    }
}
