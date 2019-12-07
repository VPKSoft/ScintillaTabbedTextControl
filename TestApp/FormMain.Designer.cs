namespace TestApp
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenWithDef = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseActiveTab = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoomTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetFileNameLongString = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom70 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom400 = new System.Windows.Forms.ToolStripMenuItem();
            this.odAnyFile = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sttcMain = new VPKSoft.ScintillaTabbedTextControl.ScintillaTabbedTextControl();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuZoom});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuOpenWithDef,
            this.mnuCloseActiveTab,
            this.mnuZoomTest,
            this.mnuSetFileNameLongString});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(276, 22);
            this.mnuNew.Text = "New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(276, 22);
            this.mnuOpen.Text = "Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuOpenWithDef
            // 
            this.mnuOpenWithDef.Name = "mnuOpenWithDef";
            this.mnuOpenWithDef.Size = new System.Drawing.Size(276, 22);
            this.mnuOpenWithDef.Text = "Open with Notepad++ lexer definition";
            this.mnuOpenWithDef.Click += new System.EventHandler(this.MnuOpenWithDef_Click);
            // 
            // mnuCloseActiveTab
            // 
            this.mnuCloseActiveTab.Name = "mnuCloseActiveTab";
            this.mnuCloseActiveTab.Size = new System.Drawing.Size(276, 22);
            this.mnuCloseActiveTab.Text = "Close active tab";
            this.mnuCloseActiveTab.Click += new System.EventHandler(this.mnuCloseActiveTab_Click);
            // 
            // mnuZoomTest
            // 
            this.mnuZoomTest.Name = "mnuZoomTest";
            this.mnuZoomTest.Size = new System.Drawing.Size(276, 22);
            this.mnuZoomTest.Text = "Zoom Test";
            this.mnuZoomTest.Click += new System.EventHandler(this.MnuZoomTest_Click);
            // 
            // mnuSetFileNameLongString
            // 
            this.mnuSetFileNameLongString.Name = "mnuSetFileNameLongString";
            this.mnuSetFileNameLongString.Size = new System.Drawing.Size(276, 22);
            this.mnuSetFileNameLongString.Text = "Set file name to a very long string";
            this.mnuSetFileNameLongString.Click += new System.EventHandler(this.MnuSetFileNameLongString_Click);
            // 
            // mnuZoom
            // 
            this.mnuZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoom50,
            this.mnuZoom70,
            this.mnuZoom100,
            this.mnuZoom150,
            this.mnuZoom200,
            this.mnuZoom400});
            this.mnuZoom.Name = "mnuZoom";
            this.mnuZoom.Size = new System.Drawing.Size(51, 20);
            this.mnuZoom.Text = "Zoom";
            // 
            // mnuZoom50
            // 
            this.mnuZoom50.Name = "mnuZoom50";
            this.mnuZoom50.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom50.Tag = "50";
            this.mnuZoom50.Text = "50%";
            this.mnuZoom50.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // mnuZoom70
            // 
            this.mnuZoom70.Name = "mnuZoom70";
            this.mnuZoom70.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom70.Tag = "70";
            this.mnuZoom70.Text = "70%";
            this.mnuZoom70.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // mnuZoom100
            // 
            this.mnuZoom100.Name = "mnuZoom100";
            this.mnuZoom100.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom100.Tag = "100";
            this.mnuZoom100.Text = "100%";
            this.mnuZoom100.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // mnuZoom150
            // 
            this.mnuZoom150.Name = "mnuZoom150";
            this.mnuZoom150.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom150.Tag = "150";
            this.mnuZoom150.Text = "150%";
            this.mnuZoom150.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // mnuZoom200
            // 
            this.mnuZoom200.Name = "mnuZoom200";
            this.mnuZoom200.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom200.Tag = "200";
            this.mnuZoom200.Text = "200%";
            this.mnuZoom200.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // mnuZoom400
            // 
            this.mnuZoom400.Name = "mnuZoom400";
            this.mnuZoom400.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom400.Tag = "400";
            this.mnuZoom400.Text = "400%";
            this.mnuZoom400.Click += new System.EventHandler(this.MnuZoom_Click);
            // 
            // odAnyFile
            // 
            this.odAnyFile.Filter = "All files|*.*";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // sttcMain
            // 
            this.sttcMain.ChangedImage = ((System.Drawing.Image)(resources.GetObject("sttcMain.ChangedImage")));
            this.sttcMain.CloseButtonImage = ((System.Drawing.Image)(resources.GetObject("sttcMain.CloseButtonImage")));
            this.sttcMain.ColorBraceHighlightBackground = System.Drawing.Color.LightGray;
            this.sttcMain.ColorBraceHighlightBad = System.Drawing.Color.Red;
            this.sttcMain.ColorBraceHighlightForeground = System.Drawing.Color.BlueViolet;
            this.sttcMain.CurrentZoomPercentage = 100;
            this.sttcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sttcMain.LeftFileIndex = 0;
            this.sttcMain.Location = new System.Drawing.Point(0, 24);
            this.sttcMain.Name = "sttcMain";
            this.sttcMain.NewFilenameStart = "new ";
            this.sttcMain.RightButtonTabActivation = true;
            this.sttcMain.RightButtonTabDragging = false;
            this.sttcMain.SavedImage = ((System.Drawing.Image)(resources.GetObject("sttcMain.SavedImage")));
            this.sttcMain.Size = new System.Drawing.Size(800, 426);
            this.sttcMain.SuspendTextChangedEvents = false;
            this.sttcMain.TabIndex = 2;
            this.sttcMain.TabWidth = 4;
            this.sttcMain.UseBraceHighlight = true;
            this.sttcMain.UseCodeIndenting = false;
            this.sttcMain.ZoomPercentageAll = 100;
            this.sttcMain.ZoomSynchronization = false;
            this.sttcMain.TabClosing += new VPKSoft.ScintillaTabbedTextControl.ScintillaTabbedTextControl.OnTabClosing(this.sttcMain_TabClosing);
            this.sttcMain.TabClosed += new VPKSoft.ScintillaTabbedTextControl.ScintillaTabbedTextControl.OnTabClosed(this.SttcMain_TabClosed);
            this.sttcMain.AcceptNewFileName += new VPKSoft.ScintillaTabbedTextControl.ScintillaTabbedTextControl.OnAcceptNewFileName(this.sttcMain_AcceptNewFileName);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sttcMain);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "VPKSoft.ScintillaTabbedTextControl test application";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.OpenFileDialog odAnyFile;
        private VPKSoft.ScintillaTabbedTextControl.ScintillaTabbedTextControl sttcMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseActiveTab;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenWithDef;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom50;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom70;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom100;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom150;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom200;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom400;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomTest;
        private System.Windows.Forms.ToolStripMenuItem mnuSetFileNameLongString;
    }
}

