using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPKSoft.ScintillaTabbedTextControl
{
    /// <summary>
    /// A user control for a non-focusable button.
    /// Implements the <see cref="System.Windows.Forms.Button" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Button" />
    public partial class NoFocusButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoFocusButton"/> class.
        /// </summary>
        public NoFocusButton()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
