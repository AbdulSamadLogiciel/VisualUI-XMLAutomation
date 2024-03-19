using System;
using System.Windows.Automation;
using System.Windows.Forms;
using VisualUIAVerify.Controls;

namespace VisualUIAVerify.XMLAutomation
{
    public class UIElements
    {
        public UIElements()
        {
        }


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
    }


}
