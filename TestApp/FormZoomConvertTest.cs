using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.ScintillaTabbedTextControl;

namespace TestApp
{
    public partial class FormZoomConvertTest : Form
    {
        public FormZoomConvertTest()
        {
            InitializeComponent();
        }

        private void BtCalculate_Click(object sender, EventArgs e)
        {
            lbZoom1.Items.Clear();
            lbZoom2.Items.Clear();
            lbZoom3.Items.Clear();
            for (int i = -10; i <= 20; i++)
            {
                var zoomPercentage = ScintillaZoomPercentage.ZoomPercentageFromPoints(i);
                lbZoom1.Items.Add(i + " points");
                var zoomPoints = ScintillaZoomPercentage.PointsFromZoomPercentage(zoomPercentage);
                lbZoom2.Items.Add(zoomPercentage + "%");

                lbZoom3.Items.Add(zoomPoints + " points");
            }
        }
    }
}
