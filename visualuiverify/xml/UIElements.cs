using System;
using System.Text;
using System.Web.UI.Design;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;
using VisualUIAVerify.Controls;
using VisualUIAVerify.xml.Strategies;
using VisualUIAVerify.XMLAutomation.Strategies;

namespace VisualUIAVerify.XMLAutomation
{
    public class UIElements
    {
       


        public static bool IsPreviousAndCurrentElementSame(TreeNode element)
        {
            var previousSiblingElement = GetPreviousSiblingElement(element)?.Text;
            if (previousSiblingElement == null)
            {
                return false;
            }
            var previousUIElement = UIElementType(previousSiblingElement);
            var currentElement = UIElementType(element.Text);
            return previousUIElement == currentElement;
        }
        public static bool IsPreviousSiblingElementExists(TreeNode element)
        {
            var previousElement = GetPreviousSiblingElement(element);
            if (previousElement == null)
            {
                return false;
            }
            return true;
        }

        public static TreeNode GetPreviousSiblingElement(TreeNode element)
        {

            TreeNode parentElement = element.Parent;

            if (parentElement != null)
            {

                TreeNodeCollection siblingElements = parentElement.Nodes;


                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }


                if (currentIndex != -1 && currentIndex > 0)
                {
                    var prevElement = siblingElements[currentIndex - 1];
                    if (prevElement.Checked)
                    {
                        return prevElement;
                    }
               

                }
            }
            return null;
        }

        public static bool ISNextSiblingElementExists(TreeNode element)
        {
            var nextElement = GetNextSiblingElement(element);
            if (nextElement == null)
            {
                return false;
            }
            return true;
        }
        public static AutomationElement GetAutomationElement(TreeNode element)
        {
            AutomationElementTreeNode automationElementTreeNode = element.Tag as AutomationElementTreeNode;
            return automationElementTreeNode.AutomationElement;
        }
        public static string UIElementType(string text)
        {

           
            string[] parts = text.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            string firstWord = parts[0];
            return firstWord;
        }
        public static TreeNode GetNextSiblingElement(TreeNode element)
        {


            TreeNode parentElement = element.Parent;

            if (parentElement != null)
            {

                TreeNodeCollection siblingElements = parentElement.Nodes;


                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }


                if (currentIndex != -1 && currentIndex < siblingElements.Count - 1)
                {
                    TreeNode nextSibling = siblingElements[currentIndex + 1];
                    if (UIElementType(nextSibling.Text) == UIElementType(element.Text) && nextSibling.Checked)
                    {
                        return nextSibling;
                    }


                }

            }
            return null;
        }
        /*
        public static bool IsElementHopper(TreeNode node, ref bool isElementHopper)
        {
          
        
            TreeNodeCollection children = node.Nodes;

            foreach (TreeNode child in children)
            {
                var automationElement = GetAutomationElement(child);
                if (automationElement.Current.AutomationId == "layoutPanelleft")
                {
                    var data = child.Text;
                }

                if ((ButtonStrategy.IsButton(child) || TextFieldStrategy.IsText(child) || TextLabelStrategy.IsLabel(child)) && child.Checked)
                {
                    isElementHopper = true;
                    break;
                }

                IsElementHopper(child, ref isElementHopper);

            }
            return isElementHopper;
        }*/
        public static bool HasChild(TreeNode element)
        {
            TreeNodeCollection nodes = element.Nodes;
            return nodes.Count > 0;
        }

        public static object IsInvokePattern(AutomationElement element)
        {
            object patternObj;
            if (element.TryGetCurrentPattern(InvokePattern.Pattern, out patternObj))
            {
                return patternObj;
               
            }
            return null;
        }
        public static ValuePattern IsValuePattern(AutomationElement element)
        {
            object patternObj;
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            {
                return patternObj as ValuePattern;
            }
            return null;
        }

        public static bool IsElementHopper(TreeNode node)
        { 
           TreeNodeCollection children = node.Nodes;
           return children.Count > 0;   
        }
    }


}
