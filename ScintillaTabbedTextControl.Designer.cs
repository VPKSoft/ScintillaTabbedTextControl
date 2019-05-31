namespace VPKSoft.ScintillaTabbedTextControl
{
    partial class ScintillaTabbedTextControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScintillaTabbedTextControl));
            this.pnScrollingTabContainer = new System.Windows.Forms.Panel();
            this.tlpTopContainer = new System.Windows.Forms.TableLayoutPanel();
            this.tlpNavigationButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btPrevious = new VPKSoft.ScintillaTabbedTextControl.NoFocusButton();
            this.btNext = new VPKSoft.ScintillaTabbedTextControl.NoFocusButton();
            this.pnTabContainer = new System.Windows.Forms.Panel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.scintillaAutoComplete = new AutocompleteMenuNS.AutocompleteMenu();
            this.tlpTopContainer.SuspendLayout();
            this.tlpNavigationButtons.SuspendLayout();
            this.pnTabContainer.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnScrollingTabContainer
            // 
            this.pnScrollingTabContainer.Location = new System.Drawing.Point(0, 0);
            this.pnScrollingTabContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnScrollingTabContainer.Name = "pnScrollingTabContainer";
            this.pnScrollingTabContainer.Size = new System.Drawing.Size(732, 37);
            this.pnScrollingTabContainer.TabIndex = 0;
            this.pnScrollingTabContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            this.pnScrollingTabContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragEnter);
            this.pnScrollingTabContainer.DragOver += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragOver);
            this.pnScrollingTabContainer.DragLeave += new System.EventHandler(this.PnScrollingTabContainer_DragLeave);
            // 
            // tlpTopContainer
            // 
            this.tlpTopContainer.AutoSize = true;
            this.tlpTopContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTopContainer.ColumnCount = 2;
            this.tlpTopContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpTopContainer.Controls.Add(this.tlpNavigationButtons, 1, 0);
            this.tlpTopContainer.Controls.Add(this.pnTabContainer, 0, 0);
            this.tlpTopContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpTopContainer.Location = new System.Drawing.Point(3, 3);
            this.tlpTopContainer.Name = "tlpTopContainer";
            this.tlpTopContainer.RowCount = 1;
            this.tlpTopContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopContainer.Size = new System.Drawing.Size(794, 37);
            this.tlpTopContainer.TabIndex = 1;
            this.tlpTopContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // tlpNavigationButtons
            // 
            this.tlpNavigationButtons.AutoSize = true;
            this.tlpNavigationButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpNavigationButtons.ColumnCount = 3;
            this.tlpNavigationButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpNavigationButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpNavigationButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpNavigationButtons.Controls.Add(this.btPrevious, 0, 0);
            this.tlpNavigationButtons.Controls.Add(this.btNext, 1, 0);
            this.tlpNavigationButtons.Location = new System.Drawing.Point(729, 3);
            this.tlpNavigationButtons.Name = "tlpNavigationButtons";
            this.tlpNavigationButtons.RowCount = 1;
            this.tlpNavigationButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNavigationButtons.Size = new System.Drawing.Size(62, 31);
            this.tlpNavigationButtons.TabIndex = 2;
            this.tlpNavigationButtons.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // btPrevious
            // 
            this.btPrevious.Image = global::VPKSoft.ScintillaTabbedTextControl.Properties.Resources.Playback;
            this.btPrevious.Location = new System.Drawing.Point(3, 3);
            this.btPrevious.Name = "btPrevious";
            this.btPrevious.Size = new System.Drawing.Size(25, 25);
            this.btPrevious.TabIndex = 0;
            this.btPrevious.UseVisualStyleBackColor = true;
            this.btPrevious.Click += new System.EventHandler(this.btPrevious_Click);
            this.btPrevious.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // btNext
            // 
            this.btNext.Image = global::VPKSoft.ScintillaTabbedTextControl.Properties.Resources.Play;
            this.btNext.Location = new System.Drawing.Point(34, 3);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(25, 25);
            this.btNext.TabIndex = 1;
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btnNext_Click);
            this.btNext.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // pnTabContainer
            // 
            this.pnTabContainer.Controls.Add(this.pnScrollingTabContainer);
            this.pnTabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTabContainer.Location = new System.Drawing.Point(0, 0);
            this.pnTabContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnTabContainer.Name = "pnTabContainer";
            this.pnTabContainer.Size = new System.Drawing.Size(726, 37);
            this.pnTabContainer.TabIndex = 3;
            this.pnTabContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpTopContainer, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(800, 450);
            this.tlpMain.TabIndex = 2;
            this.tlpMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnScrollingTabContainer_DragDrop);
            // 
            // scintillaAutoComplete
            // 
            this.scintillaAutoComplete.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("scintillaAutoComplete.Colors")));
            this.scintillaAutoComplete.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scintillaAutoComplete.ImageList = null;
            this.scintillaAutoComplete.Items = new string[0];
            this.scintillaAutoComplete.TargetControlWrapper = null;
            // 
            // ScintillaTabbedTextControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "ScintillaTabbedTextControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.SizeChanged += new System.EventHandler(this.ScintillaTabbedTextControl_Resize);
            this.Resize += new System.EventHandler(this.ScintillaTabbedTextControl_Resize);
            this.tlpTopContainer.ResumeLayout(false);
            this.tlpTopContainer.PerformLayout();
            this.tlpNavigationButtons.ResumeLayout(false);
            this.pnTabContainer.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnScrollingTabContainer;
        private System.Windows.Forms.TableLayoutPanel tlpTopContainer;
        private NoFocusButton btPrevious;
        private System.Windows.Forms.TableLayoutPanel tlpNavigationButtons;
        private NoFocusButton btNext;
        private System.Windows.Forms.Panel pnTabContainer;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private AutocompleteMenuNS.AutocompleteMenu scintillaAutoComplete;
    }
}
