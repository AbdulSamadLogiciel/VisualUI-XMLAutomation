using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using VisualUIAVerify.XMLAutomation.Constants;
using VisualUIAVerify.XMLAutomation.Strategies;
using VisualUIAVerify.XMLAutomation;


namespace VisualUIAVerify.xml.Strategies
{
    public class TextLabelStrategy : IXMLGenerationStrategy
    {

        public static bool IsLabel(TreeNode element)
        {
            return UIElements.UIElementType(element.Text) == UIElementDescriptor.Label;
        }
        public static string GetDefaultValue(TreeNode element)
        {

            var automationElement = UIElements.GetAutomationElement(element);
            var controlType = UIElements.UIElementType(element.Text);
            var defaultValue = (automationElement.Current.Name != "" ? automationElement.Current.Name : automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : controlType != "" ? controlType : "");
            return defaultValue;
        }
        public void AppendElementHopper(StringBuilder xmlBuilder, Stack<TreeNode> elementHopper, string defaultValue)
        {

            Stack<TreeNode> reverseStack = new Stack<TreeNode>();
            foreach (TreeNode item in elementHopper)
            {
                reverseStack.Push(item);
            }
            xmlBuilder.Append("\r\n<listOfElementHopper>");
            foreach (TreeNode item in reverseStack)
            {
                var automationElement = UIElements.GetAutomationElement(item);
                xmlBuilder.Append($"<ElementHopper AutomationID=\"{(automationElement.Current.AutomationId != "" ? automationElement.Current.AutomationId : GetDefaultValue(item))}\"/>");


            }
            xmlBuilder.Append("\r\n</listOfElementHopper>");
        }

        public void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirstLabel, Stack<TreeNode> elementHopper)
        {
            var automationElement = UIElements.GetAutomationElement(element);
            var defaultValue = GetDefaultValue(element);
            var pattern = UIElements.IsValuePattern(automationElement);
            var patternValue = pattern != null && pattern.Current.Value != "" ? pattern.Current.Value : defaultValue;

            TreeWalker treeWalker = TreeWalker.ControlViewWalker;
            AutomationElement parentElement = treeWalker.GetParent(automationElement);

            if (IsLabel(element) && isFirstLabel && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase AutomationID=\"{parentElement.Current.AutomationId}\" Key=\"{defaultValue}\" >");
                AppendElementHopper( xmlBuilder, elementHopper, defaultValue);
                xmlBuilder.Append($"\r\n<SubControls>\r\n<LabelEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">");
                xmlBuilder.Append($"\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"{patternValue}\"/>\r\n</ExtraInfo>");
                xmlBuilder.Append($"\r\n</LabelEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstLabel = false;

            }
            else if (IsLabel(element) && isFirstLabel && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"{defaultValue}\" AutomationID=\"{parentElement.Current.AutomationId}\">");
                AppendElementHopper(xmlBuilder, elementHopper, defaultValue);
                xmlBuilder.Append($"\r\n<SubControls>\r\n<LabelEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>");
                xmlBuilder.Append($"\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"{patternValue}\"/>\r\n</ExtraInfo>");
                xmlBuilder.Append($"\r\n</LabelEmbeddedControl>");
                isFirstLabel = false;

            }
            else if (IsLabel(element) && !isFirstLabel && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<LabelEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>");
                xmlBuilder.Append($"\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"{patternValue}\"/>\r\n</ExtraInfo>");
                xmlBuilder.Append("\r\n</LabelEmbeddedControl>");
            }

            else if (IsLabel(element) && !isFirstLabel && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<LabelEmbeddedControl Key=\"{defaultValue}\" ControlType=\"{defaultValue}\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"{defaultValue}\"/>\r\n</listOfElementHopper>");
                xmlBuilder.Append($"\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"{patternValue}\"/>\r\n</ExtraInfo>");
                xmlBuilder.Append("\r\n</LabelEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");

            }
        }
    }
}
