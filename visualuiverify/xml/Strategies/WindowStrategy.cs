using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VisualUIAVerify.XMLAutomation.Constants;


namespace VisualUIAVerify.XMLAutomation.Strategies
{
    public class WindowStrategy : IXMLGenerationStrategy
    {

        public static bool IsWindow(TreeNode element)
        {
            var uiElement = UIElements.UIElementType(element.Text);
            return uiElement == UIElementDescriptor.Window;
        }

       
        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstWindow, Stack<TreeNode> elementHopper)
        {

            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");

            if (IsWindow(element) && isFirstWindow && element.Checked)
            {
                xmlBuilder.Append($"<WindowEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>");
                isFirstWindow = false;

            }
            else if (IsWindow(element) && !isFirstWindow)
            {
                xmlBuilder.Append($"\r\n</SubControls>\r\n</WindowEmbeddedControl>");
                xmlBuilder.Append($"\r\n<WindowEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>");
            }
        }
    }
}
