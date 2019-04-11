﻿#region License
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

using System.Windows.Forms;
using static VPKSoft.ScintillaLexers.LexerEnumerations;

namespace TestApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void mnuOpen_Click(object sender, System.EventArgs e)
        {
            if (odAnyFile.ShowDialog() == DialogResult.OK)
            {
                if (sttcMain.AddDocument(odAnyFile.FileName, -1))
                {
                    sttcMain.LastAddedDocument.FileTabButton.ContextMenuStrip = contextMenuStrip1;
                }                
            }
        }

        private void testToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (sttcMain.CurrentDocument != null) // the first null check..
            {
                var document = sttcMain.CurrentDocument; // get the active document..

                MessageBox.Show(document.FileName);

            }
        }

        private void mnuCloseActiveTab_Click(object sender, System.EventArgs e)
        {
            sttcMain.CloseDocument(sttcMain.CurrentDocument);
        }

        private void sttcMain_AcceptNewFileName(object sender, VPKSoft.ScintillaTabbedTextControl.AcceptNewFileNameEventArgs e)
        {
            if (e.FileName == "new 2")
            {
                e.Accept = false;
            }
        }

        private void mnuNew_Click(object sender, System.EventArgs e)
        {
            sttcMain.AddNewDocument();
        }

        private void MnuOpenWithDef_Click(object sender, System.EventArgs e)
        {
            if (odAnyFile.ShowDialog() == DialogResult.OK)
            {
                if (sttcMain.AddDocument(odAnyFile.FileName, -1))
                {
                    string xmlLexerDefinitionFileName = @"C:\Program Files (x86)\Notepad++\themes\Monokai.xml";
                    sttcMain.LastAddedDocument.FileTabButton.ContextMenuStrip = contextMenuStrip1;
                    sttcMain.SetLexer(odAnyFile.FileName, LexerType.Cs, xmlLexerDefinitionFileName);
                }
            }
        }
    }
}
