
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using VisualUIAVerify.xml.Strategies;
using VisualUIAVerify.xml.Strategies.Factory;
using VisualUIAVerify.XMLAutomation.Constants;
using VisualUIAVerify.XMLAutomation.Strategies;

namespace VisualUIAVerify.XMLAutomation
{
    public class XMLGenerator
    {
        private static bool isFirstText = true;
        private static bool isFirstWindow = true;
        private static bool isFirstButton = true;
        private static bool isFirstElementUIElement = true;
        public static bool isElementHopper = false;
        public static bool isFirstLabel = true;
        public static bool isFirstXMLGenerated = false;
  
        private static Stack<TreeNode> elementHopperNode = new Stack<TreeNode>();
        public static void SaveXML(StringBuilder xmlString)
        {
            string relativePath = @"UIAutomation.xml";
            string beautifiedXml = BeautifyXml(xmlString.ToString());
            File.WriteAllText(relativePath, beautifiedXml);
            Console.WriteLine("XML data has been written to the file successfully.");
            FlushState();

        }
        public static void FlushState()
        {
            isFirstText = true;
            isFirstWindow = true;
            isFirstButton = true;
            isFirstElementUIElement = true;
            isElementHopper = true;
            isFirstLabel = true;
            elementHopperNode.Clear();
        }
        public static void GenerateXML(TreeNode rootNode)
        {
            try
            {
              

                StringBuilder xmlBuilder = new StringBuilder();
                xmlBuilder.AppendLine("<EmbeddedControlBase>\n<SubControls>");

                Console.WriteLine("UI Elements:");
                ParseUIElements(rootNode, xmlBuilder, elementHopperNode);

                if (!isFirstWindow)
                {
                    xmlBuilder.AppendLine("\r\n</SubControls>\r\n</WindowEmbeddedControl>");

                }
                xmlBuilder.AppendLine("</SubControls>\n</EmbeddedControlBase>");
                SaveXML(xmlBuilder);
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: ${ex.Message}");
            }
        }
        public static void ParseUIElements(TreeNode node, StringBuilder xmlBuilder, Stack<TreeNode> elementHopper)
        {
           
            GenerateNodeXML(node, xmlBuilder, elementHopper);

            TreeNodeCollection children = node.Nodes;

            foreach (TreeNode child in children)
            {
               
                    ParseUIElements(child, xmlBuilder, elementHopper);

            }
          

        }
     
        static void GenerateNodeXML(TreeNode node, StringBuilder xmlBuilder, Stack<TreeNode> elementHopper)
        {
            if (node.Checked)
            {
                AppendXML(node, xmlBuilder, elementHopper);
            }
        }

      

        static void AppendXML(TreeNode element, StringBuilder xmlBuilder, Stack<TreeNode> elementHopper)
        {

            if (elementHopperNode.Count > 0 && !element.Parent.Equals(elementHopperNode.Peek()) && !isFirstElementUIElement)
            {        
                elementHopperNode.Pop();
            }

            if (UIElements.IsElementHopper(element))
            {
                    elementHopper.Push(element);
            }

            if (!UIElements.IsPreviousAndCurrentElementSame(element) && ButtonStrategy.IsButton(element))
            {
                isFirstButton = true;
            }
            if (!UIElements.IsPreviousAndCurrentElementSame(element) && TextFieldStrategy.IsText(element))
            {
                isFirstText = true;
            }
            if (!UIElements.IsPreviousAndCurrentElementSame(element) && TextLabelStrategy.IsLabel(element))
            {
                isFirstLabel = true;
            }

            ExecuteStrategy(element, xmlBuilder, elementHopper);
            isFirstElementUIElement = false;
            
        }
        public static void ExecuteStrategy(TreeNode element, StringBuilder xmlBuilder, Stack<TreeNode> elementHopper)
        {

            if (WindowStrategy.IsWindow(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Window);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstWindow, elementHopper);
            }
            else
            if (ButtonStrategy.IsButton(element) && !UIElements.HasChild(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Button);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstButton, elementHopper);
            }
            else
            if (TextFieldStrategy.IsText(element) && !UIElements.HasChild(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Text);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstText, elementHopper);

            }
            else
            if (TextLabelStrategy.IsLabel(element) && !UIElements.HasChild(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Label);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstLabel, elementHopper);

            }
        }
        static string BeautifyXml(string xml)
        {
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);


                StringBuilder stringBuilder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\n"; // Optional: You can change the newline characters to your preference

                // Write the XML to the string writer with indentation
                using (XmlWriter writer = XmlWriter.Create(stringBuilder, settings))
                {
                    doc.Save(writer);
                }

                // Return the formatted XML string
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error beautifying XML: " + ex.Message);
                return xml;
            }
        }
    }
}
