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

        /// <summary>
        /// Gets or sets the type of the lexer.
        /// </summary>
        public LexerType LexerType { get; set; } = LexerType.Unknown;

        /// <summary>
        /// Gets or sets the object that contains data about the class.
        /// </summary>
        /// <value>
        /// An System.Object that contains data about the class. The default is null.
        /// </value>
        public object Tag { get; set; } = null;

        /// <summary>
        /// Gets or sets the list of objects that contains data about the class.
        /// </summary>
        /// <value>
        /// A list of System.Object's that contains data about the class. The default is an empty list.
        /// </value>
        public List<object> Tags { get; set; } = new List<object>();
    }

}
