
using System.Text;
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
        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstButton)
        {
            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");

            if (IsButton(element) && isFirstButton && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");
                isFirstButton = false;

            }
            else if (IsButton(element) && isFirstButton && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
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
