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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET; // (C)::https://github.com/jacobslusser/ScintillaNET
using System.IO;
using System.Text;

namespace VPKSoft.ScintillaTabbedTextControl
{
    /// <summary>
    /// A control which displays tabbed ScintillaNET controls.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class ScintillaTabbedTextControl: UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScintillaTabbedTextControl"/> class.
        /// </summary>
        public ScintillaTabbedTextControl()
        {
            InitializeComponent();
            tlpTopContainer.BorderStyle = BorderStyle.Fixed3D; // this is the not "active" state..

            // subscribe an event to unsubscribe all the other event subscriptions on dispose..
            Disposed += ScintillaTabbedTextControl_Disposed; 
        }

        #region PublicProperties
        /// <summary>
        /// Gets the documents (ScintillaNET) opened on the control.
        /// </summary>
        [Browsable(false)]
        public List<ScintillaTabbedDocument> Documents { get; } = new List<ScintillaTabbedDocument>();

        /// <summary>
        /// Gets or sets a value indicating whether the right mouse button also activates a tab on to control.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether the right mouse button also activates a tab on to control.")]
        public bool RightButtonTabActivation { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the right mouse button is also used to drag the tabs in the control.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether the right mouse button is also used to drag the tabs in the control.")]
        public bool RightButtonTabDragging { get; set; } = false;


        /// <summary>
        /// Gets the active ScintillaTabbedDocument class instance from the control.
        /// </summary>
        [Browsable(false)]
        public ScintillaTabbedDocument CurrentDocument
        {
            get
            {
                if (DocumentsCount == 0)
                {
                    return null;
                }
                else
                {
                    if (LeftFileIndex >= 0 && LeftFileIndex < DocumentsCount)
                    {
                        return Documents[LeftFileIndex];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the document count value.
        /// </summary>
        [Browsable(false)] // no designer..
        public int DocumentsCount => Documents.Count;

        /// <summary>
        /// Gets or sets a value for a starting text of a new file.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value for a starting text of a new file.")]
        public string NewFilenameStart { get; set; } = "new ";

        // the left-most tab index in the container..
        private int _LeftFileIndex = 0;

        /// <summary>
        /// Gets or sets the  left-most tab index in the tab container.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [Browsable(false)] // not in designer time..
        public int LeftFileIndex
        {
            get
            {
                return _LeftFileIndex;
            }

            set
            {
                // no reason to throw exceptions if nothing can be set...
                if (value == 0 && DocumentsCount == 0) 
                {
                    _LeftFileIndex = value;
                }
                // an invalid index was given so do raise an exception..
                else if (value < 0 || value >= DocumentsCount)
                {
                    throw new IndexOutOfRangeException();
                }
                LayoutTabs(value); // the value will be saved via the layout call..
            }
        }

        // an indicator image of whether the document hasn't been changed after initial loading..
        private Image _SavedImage = Properties.Resources.Save;

        /// <summary>
        /// Gets or sets the indicator image of whether the document hasn't been changed after initial loading.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("An indicator image of whether the document hasn't been changed after initial loading.")]
        public Image SavedImage
        {
            get
            {
                return _SavedImage;
            }

            set
            {
                _SavedImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.SavedImage = _SavedImage;
                }
            }
        }

        // an indicator image of whether the document has been changed after initial loading..
        private Image _ChangedImage = Properties.Resources.Save_Red;

        /// <summary>
        /// Gets or sets the indicator image of whether the document has been changed after initial loading.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("An indicator image of whether the document has been changed after initial loading.")]
        public Image ChangedImage
        {
            get
            {
                return _ChangedImage;
            }

            set
            {
                _ChangedImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.SavedImage = _ChangedImage;
                }
            }
        }

        // an image used on tab's close button..
        private Image _CloseButtonImage = Properties.Resources.Cancel;

        /// <summary>
        /// Gets or sets the close button image.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the close button image.")]
        public Image CloseButtonImage
        {
            get => _CloseButtonImage;

            set
            {
                _CloseButtonImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.CloseButtonImage = _CloseButtonImage;
                }
            }
        }
        #endregion

        #region PublicMethods        
        /// <summary>
        /// Closes the document with a given index.
        /// <note type="note">No events will be raised with this call.</note>
        /// </summary>
        /// <param name="index">The index of the document within the control.</param>
        public void CloseDocument(int index)
        {
            CloseDocument(index, false);
        }

        /// <summary>
        /// Closes the given <see cref="ScintillaTabbedDocument"/> document.
        /// <note type="note">No events will be raised with this call.</note>
        /// </summary>
        /// <param name="document">The document to be closed.</param>
        public void CloseDocument(ScintillaTabbedDocument document)
        {
            int index = Documents.IndexOf(document);
            CloseDocument(index, false);
        }

        /// <summary>
        /// Closes the given <see cref="ScintillaTabbedDocument"/> document.
        /// </summary>
        /// <param name="document">The document to be closed.</param>
        /// <param name="raiseEvent">A flag indicating if a <see cref="TabClosing"/> event should be raised upon the closing of the tab in question.</param>
        public void CloseDocument(ScintillaTabbedDocument document, bool raiseEvent)
        {
            int index = Documents.IndexOf(document);
            CloseDocument(index, raiseEvent);
        }

        /// <summary>
        /// Closes the document with a given index.
        /// </summary>
        /// <param name="index">The index of the document within the control.</param>
        /// <param name="raiseEvent">A flag indicating if a <see cref="TabClosing"/> event should be raised upon the closing of the tab in question.</param>
        public void CloseDocument(int index, bool raiseEvent)
        {
            // avoid an invalid index error by validating the index first..
            if (index < 0 || index >= DocumentsCount)
            {
                return;
            }

            ScintillaTabbedDocument document = Documents[index];

            TabClosingEventArgsExt e = new TabClosingEventArgsExt() { Cancel = false, ScintillaTabbedDocument = document };

            // raise the event based on the given raiseEvent flag..
            if (raiseEvent)
            {
                // ..and if the event is subscribed..
                TabClosing?.Invoke(this, e);
            }

            if (!e.Cancel)
            {
                pnScrollingTabContainer.Controls.Remove(document.FileTabButton);

                if (LeftFileIndex >= (int)(document.FileTabButton).Tag)
                {
                    _LeftFileIndex--;
                }

                // do some cleanup (unsubscribe the events)..
                document.Scintilla.TextChanged -= Scintilla_TextChanged;
                document.Scintilla.UpdateUI -= Scintilla_UpdateUI;
                document.Scintilla.MouseDown -= Scintilla_MouseDown;
                document.Scintilla.MouseUp -= Scintilla_MouseUp;
                document.Scintilla.MouseMove -= Scintilla_MouseMove;
                document.Scintilla.MouseWheel -= Scintilla_MouseWheel;
                document.Scintilla.MouseClick -= Scintilla_MouseClick;
                document.Scintilla.MouseDoubleClick -= Scintilla_MouseDoubleClick;


                document.FileTabButton.Click -= FileTabButton_Click;
                document.FileTabButton.MouseMove -= FileTabButton_MouseMove;
                document.FileTabButton.MouseUp -= FileTabButton_MouseUp;
                document.FileTabButton.MouseDown -= FileTabButton_MouseDown;
                Documents.RemoveAt(index);

                // in case there are documents still open and the left index has been set to a negative value
                // set the left index to zero.
                if (_LeftFileIndex == -1 && DocumentsCount > 0)
                {
                    _LeftFileIndex = 0;
                }

                LayoutTabs(); // perform the layout..
            }
        }


        // a counter for naming new tabs, hopefully ulong.MaxValue will be enough (18446744073709551615)..
        private ulong docNameCounter = 0;


        /// <summary>
        /// Adds a new document to the control with no lexer.
        /// </summary>
        /// <returns>True if the document was successfully added; otherwise false.</returns>
        public bool AddNewDocument()
        {
            return AddDocument(string.Empty, -1);
        }

        /// <summary>
        /// Checks if the control already contains a document with a given file name.
        /// </summary>
        /// <param name="fileName">Name of the file which existence to check.</param>
        /// <param name="activate">A flag to indicate a found document should be activated.</param>
        /// <returns>True if the document exists in the document collection; otherwise false.</returns>
        public bool DocumentExists(string fileName, bool activate)
        {
            // loop through the documents..
            foreach (ScintillaTabbedDocument document in Documents)
            {
                if (document.FileName == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Activates a document with a given file name.
        /// </summary>
        /// <param name="fileName">Name of the file of the document which to activate.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool ActivateDocument(string fileName)
        {
            int idx = Documents.FindIndex(f => f.FileName == fileName);
            if (idx != -1)
            {
                return ActivateDocument(idx);
            }
            return false;
        }

        /// <summary>
        /// Activates a document with a given index.
        /// </summary>
        /// <param name="index">The index of the document to activate.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool ActivateDocument(int index)
        {
            try
            {
                // just set the index..
                LeftFileIndex = index;

                // success..
                return true;
            }
            catch
            {
                // fail..
                return false;
            }
        }

        /// <summary>
        /// Reloads the document from the file system with a given file name.
        /// <note type="note">A DocumentTextChanged event is also raised if the document is successfully reloaded.</note>
        /// </summary>
        /// <param name="fileName">The name of the file of which document's contents to reload.</param>
        /// <param name="stream">An optional stream to load the documents contents from.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool ReloadDocumentFileSys(string fileName, Stream stream = null)
        {
            // just call the overload..
            return ReloadDocumentFileSys(Documents.FindIndex(f => f.FileName == fileName), stream);
        }

        /// <summary>
        /// Reloads the document from the file system with a given index.
        /// <note type="note">A DocumentTextChanged event is also raised if the document is successfully reloaded.</note>
        /// </summary>
        /// <param name="index">The index of the document to reload.</param>
        /// <param name="stream">An optional stream to load the documents contents from.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool ReloadDocumentFileSys(int index, Stream stream = null)
        {
            return ReloadDocumentFileSys(index, Encoding.UTF8, stream);
        }


        /// <summary>
        /// Reloads the document from the file system with a given index.
        /// <note type="note">A DocumentTextChanged event is also raised if the document is successfully reloaded.</note>
        /// </summary>
        /// <param name="index">The index of the document to reload.</param>
        /// <param name="encoding">An encoding to be used to reload the document.</param>
        /// <param name="stream">An optional stream to load the documents contents from.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool ReloadDocumentFileSys(int index, Encoding encoding, Stream stream = null)
        {
            // check the validity of the given index..
            if (index >= 0 && index < DocumentsCount)
            {
                if (stream != null)
                {
                    stream.Position = 0;
                    // ..read the Scintilla instance's text from the stream..
                    stream.Position = 0;
                    using (StreamReader streamReader = new StreamReader(stream, encoding))
                    {
                        Documents[index].Scintilla.Text = streamReader.ReadToEnd();
                    }
                    return true;
                }
                // check that the file actually exists..
                else if (File.Exists(Documents[index].FileName))
                {
                    Documents[index].Scintilla.Text = File.ReadAllText(Documents[index].FileName);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the lexer type for a document with a given file name.
        /// </summary>
        /// <param name="fileName">The name of the file of which document's lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool SetLexer(string fileName, ScintillaLexers.LexerType lexerType)
        {
            // just call the overload..
            return SetLexer(Documents.FindIndex(f => f.FileName == fileName), lexerType);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool SetLexer(int index, ScintillaLexers.LexerType lexerType)
        {
            if (index >= 0 && index < DocumentsCount)
            {
                ScintillaLexers.ScintillaLexers.CreateLexer(Documents[index].Scintilla, lexerType);
                Documents[index].LexerType = lexerType;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the latest added document to the control.
        /// </summary>
        [Browsable(false)]
        public ScintillaTabbedDocument LastAddedDocument { get; private set; } = null;

        /// <summary>
        /// Add a document to the control.
        /// </summary>
        /// <param name="fileName">A file name of a document to be added.</param>
        /// <param name="ID">An unique identifier for the document. A value of -1 indicates that the document has no ID.</param>
        /// <param name="stream">An optional stream to load the contents for the document.</param>
        /// <returns>True if the document was successfully added; otherwise false.</returns>
        public bool AddDocument(string fileName, int ID, Stream stream = null)
        {
            return AddDocument(fileName, ID, Encoding.UTF8, stream);
        }


        /// <summary>
        /// Add a document to the control.
        /// </summary>
        /// <param name="fileName">A file name of a document to be added.</param>
        /// <param name="ID">An unique identifier for the document. A value of -1 indicates that the document has no ID.</param>
        /// <param name="encoding">An encoding to be used to load the document.</param>
        /// <param name="stream">An optional stream to load the contents for the document.</param>
        /// <returns>True if the document was successfully added; otherwise false.</returns>
        public bool AddDocument(string fileName, int ID, Encoding encoding, Stream stream = null)
        {
            // if the document already exists with a given file name, just activate it and
            // return true (success)..
            if (DocumentExists(fileName, true))
            {
                LastAddedDocument = null;
                return true;
            }

            // check for the file's existence..
            if (File.Exists(fileName) || stream != null)
            {
                try
                {
                    string docText = string.Empty;
                    // if a stream was gotten as a parameter..
                    if (stream != null)
                    {
                        // ..read the Scintilla instance's text from the stream..
                        stream.Position = 0;
                        using (StreamReader streamReader = new StreamReader(stream, encoding))
                        {
                            docText = streamReader.ReadToEnd();
                        }
                    }
                    else
                    {
                        // read the Scintilla instance's text from a file..
                        try
                        {
                            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                            {
                                using (StreamReader streamReader = new StreamReader(fileStream, encoding))
                                {
                                    docText = streamReader.ReadToEnd();
                                }
                            }
                        }
                        catch
                        {
                            // this is somewhat ridiculous, but the read+write seems to give an access to some files..
                            // (EXCEPTION: The process cannot access the file 'xxx.yyy' because it is being used by another process.)
                            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader streamReader = new StreamReader(fileStream, encoding))
                                {
                                    docText = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }


                    ScintillaLexers.LexerType lexerType = ScintillaLexers.ScintillaLexers.LexerTypeFromFileName(fileName);

                    // create a new ScintillaTabbedDocument class instance..
                    ScintillaTabbedDocument document =
                        new ScintillaTabbedDocument()
                        {
                            FileName = fileName, // and initialize it's members..
                            FileNameNotPath = Path.GetFileName(fileName),
                            FileTabButton = new FileTabButton() { Top = 0, Name = "doc_tab" + docNameCounter++, IsSaved = true },
                            Scintilla = new Scintilla() { Dock = DockStyle.Fill },
                            LexerType = lexerType,
                            ID = ID
                        };

                    pnScrollingTabContainer.Controls.Add(document.FileTabButton);
                    //                document.FileTabButton.Parent = pnScrollingTabContainer;
                    document.FileTabButton.Location = new Point(0, 0);
                    document.FileTabButton.Text = Path.GetFileName(fileName);


                    // scintilla events..
                    document.Scintilla.TextChanged += Scintilla_TextChanged;
                    document.Scintilla.UpdateUI += Scintilla_UpdateUI;
                    document.Scintilla.MouseDown += Scintilla_MouseDown;
                    document.Scintilla.MouseUp += Scintilla_MouseUp;
                    document.Scintilla.MouseMove += Scintilla_MouseMove;
                    document.Scintilla.MouseWheel += Scintilla_MouseWheel;
                    document.Scintilla.MouseClick += Scintilla_MouseClick;
                    document.Scintilla.MouseDoubleClick += Scintilla_MouseDoubleClick;

                    document.FileTabButton.Click += FileTabButton_Click;
                    document.FileTabButton.MouseMove += FileTabButton_MouseMove;
                    document.FileTabButton.MouseUp += FileTabButton_MouseUp;
                    document.FileTabButton.MouseDown += FileTabButton_MouseDown;
                    document.FileTabButton.TabClosing += FileTabButton_TabClosing;

                    document.Scintilla.Tag = -1;
                    SuspendTextChangedEvents = true;
                    document.Scintilla.Text = docText;
                    SuspendTextChangedEvents = false;

                    // set the lexer for the Scintilla..
                    ScintillaLexers.ScintillaLexers.CreateLexer(document.Scintilla, lexerType);

                    Documents.Add(document);

                    LayoutTabs(DocumentsCount - 1); // perform the layout..

                    // TODO::More Lexers..

                    LastAddedDocument = document;
                    return true;
                }
                catch
                {
                    LastAddedDocument = null;
                    return false;
                }
            }
            else if (fileName.Trim() == string.Empty) // a new document is to be created..
            {
                int counter = 1;
                fileName = NewFilenameStart + counter++;
                while (Documents.Exists(f => f.FileNameNotPath == fileName))
                {
                    fileName = NewFilenameStart + counter++;
                }

                // create a new ScintillaTabbedDocument class instance..
                ScintillaTabbedDocument document =
                    new ScintillaTabbedDocument()
                    {
                        FileName = fileName, // and initialize it's members..
                        FileNameNotPath = fileName,
                        FileTabButton = new FileTabButton() { Top = 0, Name = "doc_tab" + docNameCounter++, IsSaved = false },
                        Scintilla = new Scintilla() { Dock = DockStyle.Fill },
                        LexerType = ScintillaLexers.LexerType.Unknown
                    };

                pnScrollingTabContainer.Controls.Add(document.FileTabButton);
                document.FileTabButton.Location = new Point(0, 0);
                document.FileTabButton.Text = fileName;

                // scintilla events..
                document.Scintilla.TextChanged += Scintilla_TextChanged;
                document.Scintilla.UpdateUI += Scintilla_UpdateUI;
                document.Scintilla.MouseDown += Scintilla_MouseDown;
                document.Scintilla.MouseUp += Scintilla_MouseUp;
                document.Scintilla.MouseMove += Scintilla_MouseMove;
                document.Scintilla.MouseWheel += Scintilla_MouseWheel;
                document.Scintilla.MouseClick += Scintilla_MouseClick;
                document.Scintilla.MouseDoubleClick += Scintilla_MouseDoubleClick;

                document.FileTabButton.Click += FileTabButton_Click;
                document.FileTabButton.MouseMove += FileTabButton_MouseMove;
                document.FileTabButton.MouseUp += FileTabButton_MouseUp;
                document.FileTabButton.MouseDown += FileTabButton_MouseDown;
                document.FileTabButton.TabClosing += FileTabButton_TabClosing;

                document.Scintilla.Tag = -1;

                ScintillaLexers.ScintillaLexers.CreateLexer(document.Scintilla, ScintillaLexers.LexerType.Text);

                Documents.Add(document);

                LayoutTabs(DocumentsCount - 1); // perform the layout..
                LastAddedDocument = document;
                return true;
            }
            else
            {
                LastAddedDocument = null;
                return false;
            }
        }
        #endregion

        #region PublicEvents
        /// <summary>
        /// A delegate for the TabClosing event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="TabClosingEventArgsExt"/> instance containing the event data.</param>
        public delegate void OnTabClosing(object sender, TabClosingEventArgsExt e);

        /// <summary>
        /// A delegate for the TabActivated event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="TabActivatedEventArgs"/> instance containing the event data.</param>
        public delegate void OnTabActivated(object sender, TabActivatedEventArgs e);

        /// <summary>
        /// Occurs when a tab gets activated via user or other interaction with the control.
        /// </summary>
        [Browsable(true)]
        public event OnTabActivated TabActivated = null;

        /// <summary>
        /// Occurs when close button of the "tab" has been clicked.
        /// </summary>
        [Browsable(true)]
        public event OnTabClosing TabClosing = null;

        /// <summary>
        /// A delegate for the DocumentTextChanged event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="ScintillaTextChangedEventArgs"/> instance containing the event data.</param>
        public delegate void OnDocumentTextChanged(object sender, ScintillaTextChangedEventArgs e);

        /// <summary>
        /// A delegate for the CaretPositionChanged event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="ScintillaTabbedDocumentEventArgsExt"/> instance containing the event data.</param>
        public delegate void OnCaretPositionChanged(object sender, ScintillaTabbedDocumentEventArgsExt e);

        /// <summary>
        /// Occurs when the caret position has been changed on the <see cref="ScintillaTabbedDocument"/>.
        /// </summary>
        [Browsable(true)]
        public event OnCaretPositionChanged CaretPositionChanged = null;

        /// <summary>
        /// A delegate for the SelectionChanged event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="ScintillaTabbedDocumentEventArgsExt"/> instance containing the event data.</param>
        public delegate void OnSelectionChanged(object sender, ScintillaTabbedDocumentEventArgsExt e);

        /// <summary>
        /// Occurs when the mouse pointer is over the control <see cref="Scintilla"/> and a mouse button is pressed.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse pointer is over the control (Scintilla) and a mouse button is pressed.")]
        public event MouseEventHandler DocumentMouseDown = null;

        /// <summary>
        /// Occurs when the mouse pointer is over the control <see cref="Scintilla"/> and a mouse button is released.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse pointer is over the control (Scintilla) and a mouse button is released.")]
        public event MouseEventHandler DocumentMouseUp = null;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control <see cref="Scintilla"/>).
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse pointer is moved over the control (Scintilla).")]
        public event MouseEventHandler DocumentMouseMove = null;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control <see cref="Scintilla"/> has focus.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control (Scintilla) has focus.")]
        public event MouseEventHandler DocumentMouseWheel = null;

        /// <summary>
        /// Occurs when the control <see cref="Scintilla"/> is clicked by the mouse.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the control (Scintilla) is clicked by the mouse.")]
        public event MouseEventHandler DocumentMouseClick = null;

        /// <summary>
        /// Occurs when the control <see cref="Scintilla"/> is double clicked by the mouse.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the control (Scintilla) is double clicked by the mouse.")]
        public event MouseEventHandler DocumentMouseDoubleClick;

        /// <summary>
        /// Occurs when the selection has been changed on the <see cref="ScintillaTabbedDocument"/>.
        /// </summary>
        [Browsable(true)]
        public event OnSelectionChanged SelectionChanged = null;

        /// <summary>
        /// Occurs when the contents of a tabbed document has been changed.
        /// </summary>
        [Browsable(true)]
        public event OnDocumentTextChanged DocumentTextChanged = null;
        #endregion

        #region FileTabButtonEvents
        // a flag indicating if a tab is being dragger..
        bool tabDragging = false;

        // an indicator if the user has allowed a tab to close..
        private bool tabClosing = false;

        // the tab dragging has been "started"..
        private void FileTabButton_MouseDown(object sender, MouseEventArgs e)
        {
            tabDragging = true; // ..so do set the flag..

            if (e.Button == MouseButtons.Right && RightButtonTabActivation && sender.GetType() == typeof(FileTabButton))
            {
                LeftFileIndex = (int)((FileTabButton)sender).Tag; // activate the clicked tab / document..
            }
        }

        // tab is being "dragged"..
        private void FileTabButton_MouseMove(object sender, MouseEventArgs e)
        {
            // check if the left mouse button is 
            if ((e.Button == MouseButtons.Left || (e.Button == MouseButtons.Right && RightButtonTabDragging)) && tabDragging)
            {
                FileTabButton senderTab = (FileTabButton)sender; // get the sender tab..

                // get the point where the tab is being dragged in it's container..
                Point clientPoint = ((FileTabButton)sender).PointToScreen(e.Location);
                clientPoint = pnScrollingTabContainer.PointToClient(clientPoint);

                // get the control at the point where the tab is being dragged to..
                Control controlAtPoint = pnScrollingTabContainer.GetChildAtPoint(clientPoint);

                // check that there is a control at the dragging point and the control's type is FileTabButton..
                if (controlAtPoint != null && controlAtPoint is FileTabButton)
                {
                    // get the control at the point..
                    FileTabButton sourceTab = (FileTabButton)controlAtPoint;

                    // check that the controls are not same FileTabButton controls..
                    if (!senderTab.Equals(sourceTab)) 
                    {
                        // ..and switch their locations if the controls are different..
                        SwitchTabs((int)senderTab.Tag, (int)sourceTab.Tag);
                    }
                }
            }
        }

        // tab "dragging" has ended..
        private void FileTabButton_MouseUp(object sender, MouseEventArgs e)
        {
            tabDragging = false; // ..so do set the flag..

            if (e.Button == MouseButtons.Right && RightButtonTabActivation && sender.GetType() == typeof(FileTabButton))
            {
                LeftFileIndex = (int)((FileTabButton)sender).Tag; // activate the clicked tab / document..
            }
        }

        // a tab was clicked, i.e. activated..
        private void FileTabButton_Click(object sender, EventArgs e)
        {
            // set the clicked tab as active..
            if (tabClosing)
            {
                tabClosing = false;
                return;
            }
            LeftFileIndex = (int)((FileTabButton)sender).Tag; // activate the clicked tab / document..
        }

        /// <summary>
        /// Handles the TabClosing event of the FileTabButton control.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="TabClosingEventArgs"/> instance containing the event data.</param>
        private void FileTabButton_TabClosing(object sender, TabClosingEventArgs e)
        {
            int docIndex = 0;
            for (int i = 0; i < DocumentsCount; i++)
            {
                if (Documents[i].FileTabButton.Equals(sender))
                {
                    docIndex = i;
                    break;
                }
            }

            TabClosing?.Invoke(this, new TabClosingEventArgsExt() { Cancel = e.Cancel, ScintillaTabbedDocument = Documents[docIndex] });
            if (!e.Cancel)
            {
                tabClosing = true;
                pnScrollingTabContainer.Controls.Remove((Control)sender);

                if (LeftFileIndex >= (int)((Control)sender).Tag)
                {
                    _LeftFileIndex--;
                }

                // do some cleanup (unsubscribe the events)..
                Documents[docIndex].Scintilla.TextChanged -= Scintilla_TextChanged;
                Documents[docIndex].Scintilla.UpdateUI -= Scintilla_UpdateUI;
                Documents[docIndex].Scintilla.MouseDown -= Scintilla_MouseDown;
                Documents[docIndex].Scintilla.MouseUp -= Scintilla_MouseUp;
                Documents[docIndex].Scintilla.MouseMove -= Scintilla_MouseMove;
                Documents[docIndex].Scintilla.MouseWheel -= Scintilla_MouseWheel;
                Documents[docIndex].Scintilla.MouseClick -= Scintilla_MouseClick;
                Documents[docIndex].Scintilla.MouseDoubleClick -= Scintilla_MouseDoubleClick;


                Documents[docIndex].FileTabButton.Click -= FileTabButton_Click;
                Documents[docIndex].FileTabButton.MouseMove -= FileTabButton_MouseMove;
                Documents[docIndex].FileTabButton.MouseUp -= FileTabButton_MouseUp;
                Documents[docIndex].FileTabButton.MouseDown -= FileTabButton_MouseDown;
                Documents.RemoveAt(docIndex);

                // in case there are documents still open and the left index has been set to a negative value
                // set the left index to zero.
                if (_LeftFileIndex == -1 && DocumentsCount > 0)
                {
                    _LeftFileIndex = 0;
                }

                LayoutTabs(); // perform the layout..
            }
        }
        #endregion        

        #region https://github.com/jacobslusser/ScintillaNET/wiki/Displaying-Line-Numbers
        /// <summary>
        /// Gets or set the value if text changed events should be raised by the control or not.
        /// </summary>
        [Browsable(false)]
        public bool SuspendTextChangedEvents { get; set; } = false;

        private void Scintilla_TextChanged(object sender, EventArgs e)
        {
            // save the time when the document was changed..
            DateTime timeStamp = DateTime.Now;
            Scintilla scintilla = (Scintilla)sender;

            int maxLineNumberCharLengthFromTag = (int)scintilla.Tag;

            if (!SuspendTextChangedEvents)
            {
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    if (document.Scintilla.Equals(scintilla))
                    {
                        document.FileTabButton.IsSaved = false;
                        // raise an event if the contents of the document have been changed..
                        DocumentTextChanged?.Invoke(this, 
                            new ScintillaTextChangedEventArgs() { ScintillaTabbedDocument = document, TimeStamp = timeStamp });
                        break;
                    }
                }
            }

            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == maxLineNumberCharLengthFromTag)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            scintilla.Tag = maxLineNumberCharLength;
        }
        #endregion

        #region InternalHelpers
        /// <summary>
        /// Switches two tabs with each other.
        /// </summary>
        /// <param name="index1">The index of the first tab to switch.</param>
        /// <param name="index2">The index of the second tab to switch.</param>
        /// <returns>True if the switch was successful; otherwise false.</returns>
        public bool SwitchTabs(int index1, int index2)
        {
            // disallow invalid indices..
            if (index1 == index2 || index1 < 0 || index2 < 0 || index1 >= DocumentsCount || index2 >= DocumentsCount)
            {
                return false; // ..so in case of an invalid index, just return..
            }

            // clear all controls from the panel holding the tabs..
            pnScrollingTabContainer.Controls.Clear();

            // switch the documents..
            ScintillaTabbedDocument tmpDoc = Documents[index2];
            Documents[index2] = Documents[index1];
            Documents[index1] = tmpDoc;

            pnScrollingTabContainer.SuspendLayout(); // to avoid flickering while layout in progress..
            for (int i = 0; i < DocumentsCount; i++)
            {
                // re-add the remaining documents to the panel..
                pnScrollingTabContainer.Controls.Add(Documents[i].FileTabButton);
            }
            pnScrollingTabContainer.ResumeLayout(); // END: to avoid flickering while layout in progress..

            LayoutTabs(); // perform the layout..

            // set the left index to the value of the second index's parameter with
            // an assumption that tab with the second index should be activated..
            LeftFileIndex = index2;

            return true;
        }

        // to clear the Scintilla control from the control the previously active Scintilla class instance must be saved..
        private Scintilla previousDocument = null;

        /// <summary>
        /// Perform layout for the tabbed documents on the control.
        /// </summary>
        /// <param name="index">An index for the first document to be shown on the tab panel.</param>
        private void LayoutTabs(int index = -1)
        {
            // don't accept a -1 index for nothing..
            _LeftFileIndex = index == -1 ? _LeftFileIndex : index;

            // a value which indicates of how much the next tab should be laid out after
            // the previous tab to the right..
            int leftAt = 0;

            // a value which indicates the left-most coordinate of the 
            // active tab..
            int leftAtIndex = 0;

            // if the previous active tab has been set, remove it from the
            // container's control collection..
            if (previousDocument != null)
            {
                tlpMain.Controls.Remove(previousDocument);
            }

            // if not tabs / documents exist then do nothing..
            if (DocumentsCount == 0)
            {
                previousDocument = null;

                SetPreviousNextButtonStates(); // enable / disable the previous / next buttons..

                return;
            }

            // initialize the active tab index with negative value as
            // this value comes from the control's index in the containing control..
            int activeTabIndex = -1;

            pnScrollingTabContainer.SuspendLayout(); // to avoid flickering while layout in progress..

            // loop through the controls in the collection..
            for (int i = 0; i < pnScrollingTabContainer.Controls.Count; i++)
            {
                // set their left positions correctly..
                pnScrollingTabContainer.Controls[i].Left = leftAt;

                // the top value is always zero..
                pnScrollingTabContainer.Controls[i].Top = 0;

                // this condition would indicate that the active tab matches then 
                // index within the container control's layout..
                if (i == _LeftFileIndex)
                {
                    // ..so do add matching Scintilla to it's container..
                    tlpMain.Controls.Add(Documents[i].Scintilla, 0, 1);
                    previousDocument = Documents[i].Scintilla;
                    previousDocument.Focus();

                    // TODO::Make the auto-complete to work..
                    scintillaAutoComplete.TargetControlWrapper = new ScintillaWrapper(Documents[i].Scintilla);
                    /* Make this work via a XML definition file
                    string[] snippets = { "Whatever", "Bla", "Another_bla", "HEY" };

                        var items = new List<AutocompleteItem>();

                        foreach (var item in snippets)
                            items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });

                    //set as autocomplete source
                    scintillaAutoComplete.SetAutocompleteItems(items);
                    */
                }

                // do the math for a tab which is active..
                (pnScrollingTabContainer.Controls[i] as FileTabButton).IsActive = i == _LeftFileIndex;
                // ..and also set the tag to indicate it's order in the parent container..
                (pnScrollingTabContainer.Controls[i] as FileTabButton).Tag = // save the index to the Tag..
                    pnScrollingTabContainer.Controls.GetChildIndex(pnScrollingTabContainer.Controls[i]);

                // if the index matches the active tab..
                if (i == _LeftFileIndex)
                {
                    // ..save it's left position to another variable..
                    leftAtIndex = leftAt;

                    // ..also save the index of the active tab..
                    activeTabIndex = i;
                }

                // increase the left counter..
                leftAt += pnScrollingTabContainer.Controls[i].Width;
            }

            // if subscribed, raise the TabActivated event and give it the active ScintillaTabbedDocument class instance
            // as a parameter..
            TabActivated?.Invoke(this, new TabActivatedEventArgs() { ScintillaTabbedDocument = Documents[activeTabIndex] });

            // increase the width of the tab container control to match
            // the size of all the tabs..
            pnScrollingTabContainer.Width = leftAt;

            #region some calculation if the active tab is totally visible..
            // no margin so no need for ClientSize..
            Rectangle clientRect = new Rectangle(0, 0, pnTabContainer.Size.Width, pnTabContainer.Size.Height);

            // get the left coordinate of the "tab"..
            int controlLeft = pnScrollingTabContainer.Left + leftAtIndex;

            // get the right coordinate of the "tab"..
            int controlRight = pnScrollingTabContainer.Left + leftAtIndex +
                (_LeftFileIndex == -1 ? 0 : pnScrollingTabContainer.Controls[_LeftFileIndex].Width);
            
            // check if the "tab" is visible..
            bool isVisible = clientRect.Contains(controlLeft, 0) && clientRect.Contains(controlRight, 0);

            // .. if not visible then some more calculation..
            if (!isVisible)
            {
                bool leftSide = controlLeft < clientRect.Width;
                if (leftSide)
                {
                    pnScrollingTabContainer.Left = -leftAtIndex;
                }
                else
                {
                    int newLeft = clientRect.Width - leftAtIndex - pnScrollingTabContainer.Controls[_LeftFileIndex].Width;
                    pnScrollingTabContainer.Left = newLeft;
                }
            }
            #endregion

            pnScrollingTabContainer.ResumeLayout(); // END: to avoid flickering while layout in progress..

            SetPreviousNextButtonStates(); // enable / disable the previous / next buttons..
        }

        /// <summary>
        /// Sets the previous and the next buttons states (enabled / disabled).
        /// </summary>
        private void SetPreviousNextButtonStates()
        {
            // if there are no documents, there is simply no way of
            // going backward / forward with the tabs..
            if (DocumentsCount == 0) 
            {
                btNext.Enabled = false;
                btPrevious.Enabled = false;
                return;
            }

            // determine if going forward is possible..
            if (LeftFileIndex < DocumentsCount - 1)
            {
                btNext.Enabled = true;
            }
            else
            {
                btNext.Enabled = false;
            }

            // determine if going backward is possible..
            if (LeftFileIndex > 0 && DocumentsCount > 0)
            {
                btPrevious.Enabled = true;
            }
            else
            {
                btPrevious.Enabled = false;
            }

        }

        /// <summary>
        /// Gets a ScintillaTabbedDocument with a given object which is assumed to be a <see cref="Scintilla"/> document from the documents collection.
        /// </summary>
        /// <param name="obj">An object of the of type of <see cref="Scintilla"/>.</param>
        /// <returns>A ScintillaTabbedDocument instance if successful; otherwise null.</returns>
        private ScintillaTabbedDocument GetDocument(object obj)
        {
            int idx = GetDocumentIndex(obj);
            return idx == -1 ? null : Documents[idx];
        }

        /// <summary>
        /// Gets an index for a document with a given object which is assumed to be a <see cref="Scintilla"/> document from the documents collection.
        /// </summary>
        /// <param name="obj">An object of the of type of <see cref="Scintilla"/>.</param>
        /// <returns>A non-negative index for the document if successful; otherwise -1.</returns>
        private int GetDocumentIndex(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Scintilla))
            {
                return Documents.FindIndex(f => f.Scintilla.Equals((Scintilla)obj));
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets an index for a document with a given <see cref="Scintilla"/> document from the documents collection.
        /// </summary>
        /// <param name="scintilla">A Scintilla which index to find.</param>
        /// <returns>A non-negative index for the document if successful; otherwise -1.</returns>
        private int GetDocumentIndex(Scintilla scintilla)
        {
            return scintilla == null ? -1 : Documents.FindIndex(f => f.Scintilla.Equals(scintilla));
        }
        #endregion

        #region InternalEvents
        private void Scintilla_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DocumentMouseDoubleClick?.Invoke(sender, e);
        }

        private void Scintilla_MouseClick(object sender, MouseEventArgs e)
        {
            DocumentMouseClick?.Invoke(sender, e);
        }

        private void Scintilla_MouseWheel(object sender, MouseEventArgs e)
        {
            DocumentMouseWheel?.Invoke(sender, e);
        }

        private void Scintilla_MouseMove(object sender, MouseEventArgs e)
        {
            DocumentMouseMove?.Invoke(sender, e);
        }

        private void Scintilla_MouseUp(object sender, MouseEventArgs e)
        {
            DocumentMouseUp?.Invoke(sender, e);
        }

        private void Scintilla_MouseDown(object sender, MouseEventArgs e)
        {
            DocumentMouseDown?.Invoke(sender, e);
        }

        private void btPrevious_Click(object sender, EventArgs e)
        {
            if (LeftFileIndex > 0)
            {
                LeftFileIndex--;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (LeftFileIndex < DocumentsCount - 1)
            {
                LeftFileIndex++;
            }
        }

        private void ScintillaTabbedTextControl_Resize(object sender, EventArgs e)
        {
            LayoutTabs(); // perform the layout..
        }

        private void ScintillaTabbedTextControl_Disposed(object sender, EventArgs e)
        {
            // clean up the event subscriptions..
            for (int i = 0; i < DocumentsCount; i++)
            {
                Documents[i].Scintilla.TextChanged -= Scintilla_TextChanged;
                Documents[i].Scintilla.UpdateUI -= Scintilla_UpdateUI;
                Documents[i].Scintilla.MouseDown -= Scintilla_MouseDown;
                Documents[i].Scintilla.MouseUp -= Scintilla_MouseUp;
                Documents[i].Scintilla.MouseMove -= Scintilla_MouseMove;
                Documents[i].Scintilla.MouseWheel -= Scintilla_MouseWheel;
                Documents[i].Scintilla.MouseClick -= Scintilla_MouseClick;
                Documents[i].Scintilla.MouseDoubleClick -= Scintilla_MouseDoubleClick;

                Documents[i].FileTabButton.Click -= FileTabButton_Click;
                Documents[i].FileTabButton.MouseMove -= FileTabButton_MouseMove;
                Documents[i].FileTabButton.MouseUp -= FileTabButton_MouseUp;
                Documents[i].FileTabButton.MouseDown -= FileTabButton_MouseDown;
            }
            // ..and clean up the cleanup event subscription..
            Disposed -= ScintillaTabbedTextControl_Disposed;
        }

        private void Scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            ScintillaTabbedDocument document = GetDocument(sender);
            if (document != null)
            {
                int currentLine = document.Scintilla.CurrentLine;
                int currentPosition = document.Scintilla.CurrentPosition;
                int column = document.Scintilla.GetColumn(currentPosition);

                int selectionStartLine = document.Scintilla.LineFromPosition(document.Scintilla.SelectionStart);
                int selectionEndLine = document.Scintilla.LineFromPosition(document.Scintilla.SelectionEnd);
                int selectionLength = document.Scintilla.SelectedText.Length;
                int selectionStartColumn = document.Scintilla.GetColumn(document.Scintilla.SelectionStart);
                int selectionEndColumn = document.Scintilla.GetColumn(document.Scintilla.SelectionEnd);

                bool changed = false;
                bool selectionChanged = false;
                if (currentLine != document.LineNumber ||
                    currentPosition != document.Position ||
                    column != document.Column)
                {
                    document.LineNumber = currentLine;
                    document.Position = currentPosition;
                    document.Column = column;
                    changed = true;
                }

                if (selectionStartLine != document.SelectionStartLine ||
                    selectionEndLine != document.SelectionEndLine ||
                    selectionLength != document.SelectionLength ||
                    selectionStartColumn != document.SelectionStartColumn ||
                    selectionEndColumn != document.SelectionEndColumn)
                {
                    document.SelectionStartLine = selectionStartLine;
                    document.SelectionEndLine = selectionEndLine;
                    document.SelectionStartColumn = selectionStartColumn;
                    document.SelectionEndColumn = selectionEndColumn;
                    document.SelectionRows = selectionEndLine - selectionStartLine;
                    document.SelectionLength = selectionLength;
                    selectionChanged = true;
                }

                if (changed && document.FileTabButton.IsActive)
                {
                    CaretPositionChanged?.Invoke(this, new ScintillaTabbedDocumentEventArgsExt() { ScintillaTabbedDocument = document });
                }

                if (selectionChanged && document.FileTabButton.IsActive)
                {
                    SelectionChanged?.Invoke(this, new ScintillaTabbedDocumentEventArgsExt() { ScintillaTabbedDocument = document });
                }
            }
        }
    }
    #endregion

    #region RequiredClasses
    /// <summary>
    /// A class which instance is passed via a parameter to the TabClosing event.
    /// </summary>
    /// <seealso cref="VPKSoft.ScintillaTabbedTextControl.TabClosingEventArgs" />
    public class TabClosingEventArgsExt : TabClosingEventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument class which is to be closed.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;
    }

    /// <summary>
    /// A class which instance is passed via a to a numerous events.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class ScintillaTabbedDocumentEventArgsExt : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument to which the event is related to.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;
    }


    /// <summary>
    /// A class which instance is passed via a parameter to the TabActivated event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TabActivatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument class which is to be closed.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;
    }

    /// <summary>
    /// A class which instance is passed via a parameter to the DocumentTextChanged event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ScintillaTextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument which contents did change.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;

        /// <summary>
        /// Gets or sets the time stamp when document in question was changed..
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }

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
        public ScintillaLexers.LexerType LexerType { get; set; } = ScintillaLexers.LexerType.Unknown;

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
    #endregion
}
