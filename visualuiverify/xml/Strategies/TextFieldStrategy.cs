
using System.Text;
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
        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstText)
        {
            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");
            if (IsText(element) && isFirstText && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"{defaultValue}\" AutomationID=\"{defaultValue}\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;

            }
            else if (IsText(element) && isFirstText && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"{defaultValue}\" AutomationID=\"{defaultValue}\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
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
