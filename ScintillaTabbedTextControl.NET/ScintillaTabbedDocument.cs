#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

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

using System.Collections.Generic;
using ScintillaNET;
using static VPKSoft.ScintillaLexers.LexerEnumerations;

namespace VPKSoft.ScintillaTabbedTextControl
{
    /// <summary>
    /// A container class for a single tabbed document on the control.
    /// </summary>
    public class ScintillaTabbedDocument
    {
        /// <summary>
        /// Gets a column where the caret is in the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int Column { get; internal set; } = -1;

        /// <summary>
        /// Gets a line number where the caret is in the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int LineNumber { get; internal set; } = -1;

        /// <summary>
        /// Gets a position in character number where the caret is in the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int Position { get; internal set; } = -1;

        /// <summary>
        /// Gets the length of the selection in characters of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionLength { get; internal set; } = -1;

        /// <summary>
        /// Gets the count of rows in the selection of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionRows { get; internal set; } = -1;

        /// <summary>
        /// Gets the starting row in the selection of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionStartLine { get; internal set; } = -1;

        /// <summary>
        /// Gets the ending row in the selection of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionEndLine { get; internal set; } = -1;

        /// <summary>
        /// Gets the starting column in the selection of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionStartColumn { get; internal set; } = -1;

        /// <summary>
        /// Gets the ending column in the selection of the <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        public int SelectionEndColumn { get; internal set; } = -1;

        /// <summary>
        /// Gets or sets the identifier (database) for the document.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int ID { get; set; } = -1;

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file name without a path.
        /// </summary>
        public string FileNameNotPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the an instance of the file FileTabButton class indicating this document as a tab.
        /// </summary>
        public FileTabButton FileTabButton { get; set; } = new FileTabButton();

        /// <summary>
        /// Gets or sets the Scintilla control associated with the tab in the control.
        /// </summary>
        public Scintilla Scintilla { get; set; } = new Scintilla();

        // a field for the LexerType property..
        private LexerType lexerType = LexerType.Unknown;

        /// <summary>
        /// Gets or sets the type of the lexer.
        /// </summary>
        public LexerType LexerType
        {
            get => lexerType;

            set
            {
                ScintillaLexers.ScintillaLexers.CreateLexer(Scintilla, value);
                lexerType = value;
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the class.
        /// </summary>
        /// <value>
        /// An System.Object that contains data about the class. The default is null.
        /// </value>
        public object Tag { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 0 that contains data about the class.
        /// </summary>
        public object Tag0 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 1 that contains data about the class.
        /// </summary>
        public object Tag1 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 2 that contains data about the class.
        /// </summary>
        public object Tag2 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 3 that contains data about the class.
        /// </summary>
        public object Tag3 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 4 that contains data about the class.
        /// </summary>
        public object Tag4 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 5 that contains data about the class.
        /// </summary>
        public object Tag5 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 6 that contains data about the class.
        /// </summary>
        public object Tag6 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 7 that contains data about the class.
        /// </summary>
        public object Tag7 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 8 that contains data about the class.
        /// </summary>
        public object Tag8 { get; set; } = null;

        /// <summary>
        /// Gets or sets the object 9 that contains data about the class.
        /// </summary>
        public object Tag9 { get; set; } = null;

        /// <summary>
        /// Gets or sets the list of objects that contains data about the class.
        /// </summary>
        /// <value>
        /// A list of System.Object's that contains data about the class. The default is an empty list.
        /// </value>
        public List<object> Tags { get; set; } = new List<object>();

        /// <summary>
        /// Gets the zoom percentage of the <see cref="Scintilla"/> document.
        /// </summary>
        public int ZoomPercentage
        {
            get => ScintillaZoomPercentage.ZoomPercentageFromPoints(Scintilla);

            set => Scintilla.Zoom = ScintillaZoomPercentage.PointsFromZoomPercentage(value);
        }

        /// <summary>
        /// Gets or sets the last caret position of the <see cref="Scintilla"/> document instance within this class.
        /// </summary>
        internal int LastCaretPos { get; set; } = 0;
    }
}
