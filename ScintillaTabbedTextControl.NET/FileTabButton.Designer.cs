namespace VPKSoft.ScintillaTabbedTextControl
{
    partial class FileTabButton
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.btClose = new System.Windows.Forms.Button();
            this.lbCaption = new System.Windows.Forms.Label();
            this.pnSaveIndicator = new System.Windows.Forms.Panel();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.Controls.Add(this.btClose, 2, 0);
            this.tlpMain.Controls.Add(this.lbCaption, 1, 0);
            this.tlpMain.Controls.Add(this.pnSaveIndicator, 0, 0);
            this.tlpMain.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(106, 35);
            this.tlpMain.TabIndex = 0;
            this.tlpMain.Click += new System.EventHandler(this.ControlClickDelegation);
            this.tlpMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlMouseDownDelegation);
            this.tlpMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlMouseMoveDelegation);
            this.tlpMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlMouseUpDelegation);
            // 
            // btClose
            // 
            this.btClose.Image = global::VPKSoft.ScintillaTabbedTextControl.Properties.Resources.Cancel;
            this.btClose.Location = new System.Drawing.Point(73, 3);
            this.btClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(29, 29);
            this.btClose.TabIndex = 3;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.ControlClickDelegation);
            this.btClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlMouseDownDelegation);
            this.btClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlMouseMoveDelegation);
            this.btClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlMouseUpDelegation);
            // 
            // lbCaption
            // 
            this.lbCaption.AutoSize = true;
            this.lbCaption.Location = new System.Drawing.Point(37, 9);
            this.lbCaption.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(32, 15);
            this.lbCaption.TabIndex = 2;
            this.lbCaption.Text = "file 1";
            this.lbCaption.Click += new System.EventHandler(this.ControlClickDelegation);
            this.lbCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlMouseDownDelegation);
            this.lbCaption.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlMouseMoveDelegation);
            this.lbCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlMouseUpDelegation);
            // 
            // pnSaveIndicator
            // 
            this.pnSaveIndicator.BackgroundImage = global::VPKSoft.ScintillaTabbedTextControl.Properties.Resources.Save;
            this.pnSaveIndicator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnSaveIndicator.Location = new System.Drawing.Point(4, 3);
            this.pnSaveIndicator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnSaveIndicator.Name = "pnSaveIndicator";
            this.pnSaveIndicator.Size = new System.Drawing.Size(29, 29);
            this.pnSaveIndicator.TabIndex = 4;
            this.pnSaveIndicator.Click += new System.EventHandler(this.ControlClickDelegation);
            this.pnSaveIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlMouseDownDelegation);
            this.pnSaveIndicator.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlMouseMoveDelegation);
            this.pnSaveIndicator.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlMouseUpDelegation);
            // 
            // FileTabButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.tlpMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FileTabButton";
            this.Size = new System.Drawing.Size(106, 35);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label lbCaption;
        private System.Windows.Forms.Panel pnSaveIndicator;
    }
}
