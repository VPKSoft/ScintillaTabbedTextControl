namespace TestApp
{
    partial class FormZoomConvertTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbZoom1 = new System.Windows.Forms.ListBox();
            this.lbZoom2 = new System.Windows.Forms.ListBox();
            this.lbZoom3 = new System.Windows.Forms.ListBox();
            this.btCalculate = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.lbZoom1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbZoom2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbZoom3, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(776, 396);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbZoom1
            // 
            this.lbZoom1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbZoom1.FormattingEnabled = true;
            this.lbZoom1.Location = new System.Drawing.Point(3, 3);
            this.lbZoom1.Name = "lbZoom1";
            this.lbZoom1.Size = new System.Drawing.Size(252, 390);
            this.lbZoom1.TabIndex = 0;
            // 
            // lbZoom2
            // 
            this.lbZoom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbZoom2.FormattingEnabled = true;
            this.lbZoom2.Location = new System.Drawing.Point(261, 3);
            this.lbZoom2.Name = "lbZoom2";
            this.lbZoom2.Size = new System.Drawing.Size(252, 390);
            this.lbZoom2.TabIndex = 1;
            // 
            // lbZoom3
            // 
            this.lbZoom3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbZoom3.FormattingEnabled = true;
            this.lbZoom3.Location = new System.Drawing.Point(519, 3);
            this.lbZoom3.Name = "lbZoom3";
            this.lbZoom3.Size = new System.Drawing.Size(254, 390);
            this.lbZoom3.TabIndex = 2;
            // 
            // btCalculate
            // 
            this.btCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCalculate.Location = new System.Drawing.Point(713, 415);
            this.btCalculate.Name = "btCalculate";
            this.btCalculate.Size = new System.Drawing.Size(75, 23);
            this.btCalculate.TabIndex = 1;
            this.btCalculate.Text = "Calculate";
            this.btCalculate.UseVisualStyleBackColor = true;
            this.btCalculate.Click += new System.EventHandler(this.BtCalculate_Click);
            // 
            // FormZoomConvertTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btCalculate);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormZoomConvertTest";
            this.Text = "Zoom convert test";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lbZoom1;
        private System.Windows.Forms.ListBox lbZoom2;
        private System.Windows.Forms.ListBox lbZoom3;
        private System.Windows.Forms.Button btCalculate;
    }
}