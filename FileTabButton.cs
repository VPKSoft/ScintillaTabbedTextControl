#region License
/*
MIT License

Copyright (c) 2019 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VPKSoft.ScintillaTabbedTextControl
{
    /// <summary>
    /// A sub-control for the ScintillaTabbedTextControl control.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class FileTabButton : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileTabButton"/> class.
        /// </summary>
        public FileTabButton()
        {
            InitializeComponent();
            IsSaved = IsSaved; // set the file modified indicator by this dummy logic..
        }

        /// <summary>
        /// The file tab button is requesting a re-layout of for the main control.
        /// </summary>
        public EventHandler<EventArgs> RequestLayout;

        #region PublicProperties

        /// <summary>
        /// Gets or sets the text associated with this control. 
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the text associated with this control.")]
        public new string Text
        {
            get => lbCaption.Text;

            set
            {
                if (lbCaption.Text != value)
                {
                    lbCaption.Text = value;
                    InternalLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active and changes the control's border style according to the value.
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get => BorderStyle == BorderStyle.None;

            set
            {
                if (value && BorderStyle != BorderStyle.None ||
                    !value && BorderStyle != BorderStyle.Fixed3D)
                {
                    BorderStyle = value ? BorderStyle.None : BorderStyle.Fixed3D;
                    InternalLayout();
                }
            }
        }

        // an indicator whether the document has been changed after it's initial loading..
        private bool isSaved;

        /// <summary>
        /// Gets or sets a value indicating whether the document has been changed after it's initial loading. An image is also set to indicate the state.
        /// </summary>
        [Browsable(false)]
        public bool IsSaved
        {
            get => isSaved;

            set
            {
                if (value != isSaved)
                {
                    isSaved = value;
                    InternalLayout();
                }
            }
        }

        // an indicator image of whether the document hasn't been changed after initial loading..
        private Image savedImage = Properties.Resources.Save;

        /// <summary>
        /// Gets or sets the indicator image of whether the document hasn't been changed after initial loading.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("An indicator image of whether the document hasn't been changed after initial loading.")]
        public Image SavedImage
        {
            get => savedImage;

            set
            {
                if (savedImage != value)
                {
                    savedImage = value;
                    InternalLayout();
                }
            }
        }

        // an indicator image of whether the document has been changed after initial loading..
        private Image changedImage = Properties.Resources.Save_Red;

        /// <summary>
        /// Gets or sets the indicator image of whether the document has been changed after initial loading.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("An indicator image of whether the document has been changed after initial loading.")]
        public Image ChangedImage
        {
            get => changedImage;

            set
            {
                if (changedImage != value)
                {
                    changedImage = value;
                    InternalLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the close button image.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the close button image.")]
        public Image CloseButtonImage
        {
            get => btClose.Image;

            set
            {
                if (btClose.Image != value)
                {
                    btClose.Image = value;
                    InternalLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the shortcut menu associated with the control.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets the shortcut menu associated with the control.")]
        public override ContextMenu ContextMenu
        {
            get => base.ContextMenu;

            set
            {
                // set the context menu to all child controls and for the base control..
                btClose.ContextMenu = value;
                pnSaveIndicator.ContextMenu = value;
                tlpMain.ContextMenu = value;
                lbCaption.ContextMenu = value;
                base.ContextMenu = value;
            }
        }

        /// <summary>
        /// Gets or sets the System.Windows.Forms.ContextMenuStrip associated with this control.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets the System.Windows.Forms.ContextMenuStrip associated with this control.")]
        public override ContextMenuStrip ContextMenuStrip
        {
            get => base.ContextMenuStrip;

            set
            {
                // set the context menu strip to all child controls and for the base control..
                btClose.ContextMenuStrip = value;
                pnSaveIndicator.ContextMenuStrip = value;
                tlpMain.ContextMenuStrip = value;
                lbCaption.ContextMenuStrip = value;
                base.ContextMenuStrip = value;
            }
        }

        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the cursor that is displayed when the mouse pointer is over the control.")]
        public override Cursor Cursor
        {
            get => base.Cursor;

            set
            {
                // set the cursor to all child controls and for the base control..
                btClose.Cursor = value;
                pnSaveIndicator.Cursor = value;
                tlpMain.Cursor = value;
                lbCaption.Cursor = value;
                base.Cursor = value;
            }
        }

        #endregion

        #region PublicEvents

        /// <summary>
        /// A delegate for the TabClosing event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="TabClosingEventArgs"/> instance containing the event data.</param>
        public delegate void OnTabClosing(object sender, TabClosingEventArgs e);

        /// <summary>
        /// Occurs when close button of the "tab" has been clicked.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("An event that occurs when close button of the tab was clicked.")]
        public event OnTabClosing TabClosing;

        #endregion

        #region DelegatedEvents

        /// <summary>
        /// Raises the Click event if any of the controls on this control is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ControlClickDelegation(object sender, EventArgs e)
        {
            // if the tab was requested to be closed..
            if (sender.Equals(btClose))
            {
                TabClosing?.Invoke(this, new TabClosingEventArgs());
            }

            OnClick(e); // raise the event..
        }

        /// <summary>
        /// Delegates the mouse down event to be raised on this control level.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ControlMouseDownDelegation(object sender, MouseEventArgs e)
        {
            if (!sender.Equals(this))
            {
                // recalculate the point to match the control..
                Point delegatePoint = PointToClient((((Control) sender).PointToScreen(e.Location)));

                // raise the "delegated" event..
                OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, delegatePoint.X, delegatePoint.Y, e.Delta));
            }
        }

        /// <summary>
        /// Delegates the mouse move event to be raised on this control level.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ControlMouseMoveDelegation(object sender, MouseEventArgs e)
        {
            if (!sender.Equals(this))
            {
                // recalculate the point to match the control..
                Point delegatePoint = PointToClient((((Control) sender).PointToScreen(e.Location)));

                // raise the "delegated" event..
                OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, delegatePoint.X, delegatePoint.Y, e.Delta));
            }
        }

        /// <summary>
        /// Delegates the mouse move up to be raised on this control level.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ControlMouseUpDelegation(object sender, MouseEventArgs e)
        {
            if (!sender.Equals(this))
            {
                // recalculate the point to match the control..
                Point delegatePoint = PointToClient((((Control) sender).PointToScreen(e.Location)));

                // raise the "delegated" event..
                OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, delegatePoint.X, delegatePoint.Y, e.Delta));
            }
        }
        #endregion

        #region TabClosingEventArgs

        /// <summary>
        /// A class which is passed to the TabClosing event as an argument.
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class TabClosingEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets a value indicating whether tab should be allowed to close.
            /// </summary>
            public bool Cancel { get; set; } = false;
        }
        #endregion

        #region Layout
        // a flag indicating whether to perform layout on the tab..
        private bool layoutSuspended;

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        public new void SuspendLayout()
        {
            base.SuspendLayout();
            layoutSuspended = true;
        }

        /// <summary>
        /// Resumes usual layout logic.
        /// </summary>
        public new void ResumeLayout()
        {
            base.ResumeLayout();
            layoutSuspended = false;
            RequestLayout?.Invoke(this, new EventArgs());
        }

        private void InternalLayout()
        {
            if (layoutSuspended)
            {
                return;
            }

            RequestLayout?.Invoke(this, new EventArgs());
            pnSaveIndicator.BackgroundImage = isSaved ? SavedImage : ChangedImage;
        }
        #endregion
    }
}
