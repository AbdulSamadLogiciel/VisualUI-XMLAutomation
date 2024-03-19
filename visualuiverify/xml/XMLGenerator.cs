
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
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
        }
        public static void GenerateXML(TreeNode rootNode)
        {
            try
            {

                StringBuilder xmlBuilder = new StringBuilder();
                xmlBuilder.AppendLine("<EmbeddedControlBase>\n<SubControls>");

                Console.WriteLine("UI Elements:");
                ParseUIElements(rootNode, xmlBuilder);

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
        public static void ParseUIElements(TreeNode node, StringBuilder xmlBuilder)
        {
           
            GenerateNodeXML(node, xmlBuilder);

            TreeNodeCollection children = node.Nodes;

            foreach (TreeNode child in children)
            {
               
                    ParseUIElements(child, xmlBuilder);

            }
        }

        static void GenerateNodeXML(TreeNode node, StringBuilder xmlBuilder)
        {
            if (node.Checked)
            {
                AppendXML(node, xmlBuilder);
            }
        }

        static void AppendXML(TreeNode element, StringBuilder xmlBuilder)
        {

       
            if (!UIElements.IsPreviousAndCurrentElementSame(element) && ButtonStrategy.IsButton(element))
            {
                isFirstButton = true;
            }


            if (!UIElements.IsPreviousAndCurrentElementSame(element) && TextFieldStrategy.IsText(element))
            {
                isFirstText = true;
            }

            if (WindowStrategy.IsWindow(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Window);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstWindow);
            }
            else 
            if (ButtonStrategy.IsButton(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Button);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstButton);
            }
            else
            if (TextFieldStrategy.IsText(element))
            {
                IXMLGenerationStrategy strategy = Factory.CreateStrategy(UIElementDescriptor.Text);
                strategy.StrategicXMLGeneration(element, xmlBuilder, ref isFirstText);
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
