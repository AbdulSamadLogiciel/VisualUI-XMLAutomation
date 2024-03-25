using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace VisualUIAVerify.XMLAutomation.Strategies
{
    public interface IXMLGenerationStrategy
    {
        void StrategicXMLGeneration(TreeNode element, StringBuilder xmlBuilder, ref bool isFirst, List<string> elementHopper);
    }
}
