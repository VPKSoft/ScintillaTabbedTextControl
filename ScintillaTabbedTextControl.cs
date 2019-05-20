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
using System.Linq;
using System.Text.RegularExpressions;
using static VPKSoft.ScintillaLexers.LexerEnumerations;

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
        /// Gets or sets a value indicating whether to use code indenting in the <see cref="Scintilla"/> control.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether to use code indenting in the Scintilla control.")]
        public bool UseCodeIndenting { get; set; } = false;

        /// <summary>
        /// Gets or sets the tab width in space characters>.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets the tab width in space characters>.")]
        public int TabWidth { get; set; } = 4;

        // a list of reject files by the AcceptNewFileName event..
        private readonly List<string> rejectedFileNames = new List<string>();

        /// <summary>
        /// Gets a value indicating what is the next number appended to a new file name.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or a value indicating what is the next number appended to a new file name.")]
        public int NewFileCounter
        {
            get
            {
                // create a regular expression to match the new file naming pattern..
                Regex regex = new Regex(@"(\d+)$", RegexOptions.CultureInvariant);

                // select all the matching files from the documents collection..
                List<string> fileNames =
                    Documents.
                        Where(f => regex.IsMatch(f.FileName)). // check for the match..
                        Select(f => f.FileName).ToList(); // select the full file name..

                // add the rejected file names rejected externally by the AcceptNewFileName event..
                fileNames.AddRange(rejectedFileNames);

                // start the counter from the default value of one..
                int counter = 1;

                // generate a new file..
                string fileName = NewFilenameStart + counter;

                // while the generated new file name exits within the control's documents..
                while (fileNames.Exists(f => f == fileName))
                {
                    counter++;
                    // ..keep on generating..
                    fileName = NewFilenameStart + counter;
                }

                // return the result..
                return counter;
            }
        }

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
        /// Gets the index of the current document.
        /// </summary>
        [Browsable(false)] // not in designer time..
        public int CurrentDocumentIndex => LeftFileIndex;

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
        private int leftFileIndex;

        /// <summary>
        /// Gets or sets the  left-most tab index in the tab container.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [Browsable(false)] // not in designer time..
        public int LeftFileIndex
        {
            get
            {
                return leftFileIndex;
            }

            set
            {
                // no reason to throw exceptions if nothing can be set...
                if (value == 0 && DocumentsCount == 0) 
                {
                    leftFileIndex = value;
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
        private Image savedImage = Properties.Resources.Save;

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
                return savedImage;
            }

            set
            {
                savedImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.SavedImage = savedImage;
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
            get
            {
                return changedImage;
            }

            set
            {
                changedImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.SavedImage = changedImage;
                }
            }
        }

        // an image used on tab's close button..
        private Image closeButtonImage = Properties.Resources.Cancel;

        /// <summary>
        /// Gets or sets the close button image.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the close button image.")]
        public Image CloseButtonImage
        {
            get => closeButtonImage;

            set
            {
                closeButtonImage = value;

                // delegate the new value to the tab controls..
                foreach (ScintillaTabbedDocument document in Documents)
                {
                    document.FileTabButton.CloseButtonImage = closeButtonImage;
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
        /// Closes all the documents on the control without raising any events.
        /// </summary>
        public void CloseAllDocuments()
        {
            CloseAllDocuments(false);
        }

        /// <summary>
        /// Closes all the documents on the control.
        /// </summary>
        /// <param name="raiseEvent">A flag indicating if a <see cref="TabClosing"/> event should be raised upon the closing of all the tabbed documents.</param>
        public void CloseAllDocuments(bool raiseEvent)
        {
            // set the preventTabActivatedEvent flag based on the parameter..
            preventTabActivatedEvent = !raiseEvent;

            try
            {
                // reverse-loop though all the documents and close them..
                for (int i = DocumentsCount - 1; i >= 0; i--)
                {
                    CloseDocument(i, raiseEvent);
                }
            }
            catch
            {
                // something went wrong..
            }

            // set the preventTabActivatedEvent to it's default value..
            preventTabActivatedEvent = false;
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
                    leftFileIndex--;
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
                document.Scintilla.InsertCheck -= Scintilla_InsertCheck;
                document.Scintilla.CharAdded -= Scintilla_CharAdded;

                document.FileTabButton.Click -= FileTabButton_Click;
                document.FileTabButton.MouseMove -= FileTabButton_MouseMove;
                document.FileTabButton.MouseUp -= FileTabButton_MouseUp;
                document.FileTabButton.MouseDown -= FileTabButton_MouseDown;
                Documents.RemoveAt(index);

                // in case there are documents still open and the left index has been set to a negative value
                // set the left index to zero.
                if (leftFileIndex == -1 && DocumentsCount > 0)
                {
                    leftFileIndex = 0;
                }

                LayoutTabs(); // perform the layout..
            }
        }


        // a counter for naming new tabs, hopefully ulong.MaxValue will be enough (18446744073709551615)..
        private ulong docNameCounter;


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
                    if (activate)
                    {
                        ActivateDocument(document.FileName);
                    }

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

        #region SetLexers
        /// <summary>
        /// Sets the lexer type for a document with a given file name.
        /// </summary>
        /// <param name="fileName">The name of the file of which document's lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <returns>True if the operation was successful; otherwise false.</returns>
        public bool SetLexer(string fileName, LexerType lexerType)
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
        public bool SetLexer(int index, LexerType lexerType)
        {
            if (index >= 0 && index < DocumentsCount)
            {
                ScintillaLexers.ScintillaLexers.CreateLexer(Documents[index].Scintilla, lexerType);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <param name="useSelectionColors">A flag indicating whether to color the selection.</param>
        /// <param name="useMarginColors">A flag indicating whether to color the margin.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace, bool useSelectionColors,
            bool useMarginColors)
        {
            if (index >= 0 && index < DocumentsCount)
            {
                ScintillaLexers.ScintillaLexers.CreateLexerFromFile(Documents[index].Scintilla, lexerType, fileName,
                    useGlobalOverride, font, useWhiteSpace, useSelectionColors, useMarginColors);
                Documents[index].LexerType = lexerType;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <param name="useSelectionColors">A flag indicating whether to color the selection.</param>
        /// <param name="useMarginColors">A flag indicating whether to color the margin.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType, 
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace, bool useSelectionColors,
            bool useMarginColors)
        {
            int index = Documents.FindIndex(f => f.FileName == scintillaFileName);
            if (index >= 0 && index < DocumentsCount)
            {
                ScintillaLexers.ScintillaLexers.CreateLexerFromFile(Documents[index].Scintilla, lexerType, fileName,
                    useGlobalOverride, font, useWhiteSpace, useSelectionColors, useMarginColors);
                Documents[index].LexerType = lexerType;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <param name="useSelectionColors">A flag indicating whether to color the selection.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType,
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace, bool useSelectionColors)
        {
            return SetLexer(Documents.FindIndex(f => f.FileName == scintillaFileName), lexerType, fileName,
                useGlobalOverride, font, useWhiteSpace, useSelectionColors,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType,
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace)
        {
            return SetLexer(Documents.FindIndex(f => f.FileName == scintillaFileName), lexerType, fileName,
                useGlobalOverride, font, useWhiteSpace, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType,
            string fileName, bool useGlobalOverride, bool font)
        {
            return SetLexer(Documents.FindIndex(f => f.FileName == scintillaFileName), lexerType, fileName,
                useGlobalOverride, font, true, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType,
            string fileName, bool useGlobalOverride)
        {
            return SetLexer(Documents.FindIndex(f => f.FileName == scintillaFileName), lexerType, fileName,
                useGlobalOverride, true, true, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given scintilla file name using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="scintillaFileName">The file name opened in the <see cref="ScintillaTabbedTextControl"/>.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(string scintillaFileName, LexerType lexerType,
            string fileName)
        {
            return SetLexer(Documents.FindIndex(f => f.FileName == scintillaFileName), lexerType, fileName,
                true, true, true, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <param name="useSelectionColors">A flag indicating whether to color the selection.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace, bool useSelectionColors)
        {
            return SetLexer(index, lexerType, fileName, useGlobalOverride, font, useWhiteSpace, useSelectionColors,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <param name="useWhiteSpace">A flag indicating whether to color the white space symbol.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName, bool useGlobalOverride, bool font, bool useWhiteSpace)
        {
            return SetLexer(index, lexerType, fileName, useGlobalOverride, font, useWhiteSpace, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <param name="font">A flag indicating whether to use the defined font name from the XML document or not.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName, bool useGlobalOverride, bool font)
        {
            return SetLexer(index, lexerType, fileName, useGlobalOverride, font, true, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <param name="useGlobalOverride">A flag indicating whether the style "Global override" should be set for the lexer from the XML document.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName, bool useGlobalOverride)
        {
            return SetLexer(index, lexerType, fileName, useGlobalOverride, true, true, true,
                true);
        }

        /// <summary>
        /// Sets the lexer type for a document with a given index using lexer from XML file used by the Notepad++ software.
        /// </summary>
        /// <param name="index">The index of the document which lexer type to set.</param>
        /// <param name="lexerType">Type of the lexer.</param>
        /// <param name="fileName">A file name to get the lexer type from.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public bool SetLexer(int index, LexerType lexerType, 
            string fileName)
        {
            return SetLexer(index, lexerType, fileName, true, true, true, true,
                true);
        }
        #endregion

        /// <summary>
        /// Gets the latest added document to the control.
        /// </summary>
        [Browsable(false)]
        public ScintillaTabbedDocument LastAddedDocument { get; private set; }

        /// <summary>
        /// Add a document to the control.
        /// </summary>
        /// <param name="fileName">A file name of a document to be added.</param>
        /// <param name="ID">An unique identifier for the document. A value of -1 indicates that the document has no ID.</param>
        /// <param name="stream">An optional stream to load the contents for the document.</param>
        /// <returns>True if the document was successfully added; otherwise false.</returns>
        // ReSharper disable once InconsistentNaming
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
        // ReSharper disable once InconsistentNaming
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
                    string docText;
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


                    LexerType lexerType = ScintillaLexers.HelperClasses.LexerFileExtensions.LexerTypeFromFileName(fileName);

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
                    document.Scintilla.InsertCheck += Scintilla_InsertCheck;
                    document.Scintilla.CharAdded += Scintilla_CharAdded;

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
                // create a generated file name which doesn't exist in within the control..
                fileName = NewFilenameStart + NewFileCounter;

                // clear the list of new file names rejected by the AcceptNewFileName event..
                rejectedFileNames.Clear();

                // create a new instance of AcceptNewFileNameEventArgs class with Accept property assumed to true..
                AcceptNewFileNameEventArgs acceptNewFileNameEventArgs = new AcceptNewFileNameEventArgs { Accept = true, FileName = fileName };

                // loop with the external file name validation if the event is subscribed..
                while (AcceptNewFileName != null)
                {
                    // raise the event for an external validation for the new file name if subscribed..
                    AcceptNewFileName(this, acceptNewFileNameEventArgs);

                    // the external validation didn't accept the suggested file name, so create a new file name..
                    if (!acceptNewFileNameEventArgs.Accept)
                    {
                        rejectedFileNames.Add(acceptNewFileNameEventArgs.FileName);
                        fileName = NewFilenameStart + NewFileCounter;

                        // re-set the values for the AcceptNewFileNameEventArgs class instance..
                        acceptNewFileNameEventArgs.FileName = fileName;
                        acceptNewFileNameEventArgs.Accept = true;
                    }
                    else
                    {
                        // the external validation succeeded, so break to loop..
                        break;
                    }
                }

                // clear the list of new file names rejected by the AcceptNewFileName event..
                rejectedFileNames.Clear();

                // create a new ScintillaTabbedDocument class instance..
                ScintillaTabbedDocument document =
                    new ScintillaTabbedDocument()
                    {
                        FileName = fileName, // and initialize it's members..
                        FileNameNotPath = fileName,
                        FileTabButton = new FileTabButton() { Top = 0, Name = "doc_tab" + docNameCounter++, IsSaved = false },
                        Scintilla = new Scintilla() { Dock = DockStyle.Fill },
                        LexerType = LexerType.Unknown
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
                document.Scintilla.InsertCheck += Scintilla_InsertCheck;
                document.Scintilla.CharAdded += Scintilla_CharAdded;

                document.FileTabButton.Click += FileTabButton_Click;
                document.FileTabButton.MouseMove += FileTabButton_MouseMove;
                document.FileTabButton.MouseUp += FileTabButton_MouseUp;
                document.FileTabButton.MouseDown += FileTabButton_MouseDown;
                document.FileTabButton.TabClosing += FileTabButton_TabClosing;

                document.Scintilla.Tag = -1;

                ScintillaLexers.ScintillaLexers.CreateLexer(document.Scintilla, LexerType.Text);

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
        public event OnTabActivated TabActivated;

        /// <summary>
        /// Occurs when close button of the "tab" has been clicked.
        /// </summary>
        [Browsable(true)]
        public event OnTabClosing TabClosing;

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
        /// A delegate for the AcceptNewFileName event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="AcceptNewFileNameEventArgs"/> instance containing the event data.</param>
        public delegate void OnAcceptNewFileName(object sender, AcceptNewFileNameEventArgs e);

        /// <summary>
        /// Occurs when the control is requested to add a new file tab to the control and is requiring for the suggested file name to be accepted.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public event OnAcceptNewFileName AcceptNewFileName;

        /// <summary>
        /// Occurs when the caret position has been changed on the <see cref="ScintillaTabbedDocument"/>.
        /// </summary>
        [Browsable(true)]
        public event OnCaretPositionChanged CaretPositionChanged;

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
        public event MouseEventHandler DocumentMouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control <see cref="Scintilla"/> and a mouse button is released.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse pointer is over the control (Scintilla) and a mouse button is released.")]
        public event MouseEventHandler DocumentMouseUp;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control <see cref="Scintilla"/>).
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse pointer is moved over the control (Scintilla).")]
        public event MouseEventHandler DocumentMouseMove;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control <see cref="Scintilla"/> has focus.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control (Scintilla) has focus.")]
        public event MouseEventHandler DocumentMouseWheel;

        /// <summary>
        /// Occurs when the control <see cref="Scintilla"/> is clicked by the mouse.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the control (Scintilla) is clicked by the mouse.")]
        public event MouseEventHandler DocumentMouseClick;

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
        public event OnSelectionChanged SelectionChanged;

        /// <summary>
        /// Occurs when the contents of a tabbed document has been changed.
        /// </summary>
        [Browsable(true)]
        public event OnDocumentTextChanged DocumentTextChanged;
        #endregion

        #region FileTabButtonEvents
        // a flag indicating if a tab is being dragger..
        bool tabDragging;

        // an indicator if the user has allowed a tab to close..
        private bool tabClosing;

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
                    leftFileIndex--;
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
                Documents[docIndex].Scintilla.InsertCheck -= Scintilla_InsertCheck;
                Documents[docIndex].Scintilla.CharAdded -= Scintilla_CharAdded;


                Documents[docIndex].FileTabButton.Click -= FileTabButton_Click;
                Documents[docIndex].FileTabButton.MouseMove -= FileTabButton_MouseMove;
                Documents[docIndex].FileTabButton.MouseUp -= FileTabButton_MouseUp;
                Documents[docIndex].FileTabButton.MouseDown -= FileTabButton_MouseDown;
                Documents.RemoveAt(docIndex);

                // in case there are documents still open and the left index has been set to a negative value
                // set the left index to zero.
                if (leftFileIndex == -1 && DocumentsCount > 0)
                {
                    leftFileIndex = 0;
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
        public bool SuspendTextChangedEvents { get; set; }

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
        private Scintilla previousDocument;

        /// <summary>
        /// A flag indicating whether the <see cref="TabActivated"/> event should be suspended.
        /// </summary>
        private bool preventTabActivatedEvent;

        /// <summary>
        /// Perform layout for the tabbed documents on the control.
        /// </summary>
        /// <param name="index">An index for the first document to be shown on the tab panel.</param>
        private void LayoutTabs(int index = -1)
        {
            // don't accept a -1 index for nothing..
            leftFileIndex = index == -1 ? leftFileIndex : index;

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
                if (i == leftFileIndex)
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
                ((FileTabButton) pnScrollingTabContainer.Controls[i]).IsActive = i == leftFileIndex;
                // ..and also set the tag to indicate it's order in the parent container..
                (pnScrollingTabContainer.Controls[i] as FileTabButton).Tag = // save the index to the Tag..
                    pnScrollingTabContainer.Controls.GetChildIndex(pnScrollingTabContainer.Controls[i]);

                // if the index matches the active tab..
                if (i == leftFileIndex)
                {
                    // ..save it's left position to another variable..
                    leftAtIndex = leftAt;

                    // ..also save the index of the active tab..
                    activeTabIndex = i;
                }

                // increase the left counter..
                leftAt += pnScrollingTabContainer.Controls[i].Width;
            }

            // only try to raise the TabActivated event if it's not suspended via the preventTabActivatedEvent flag..
            if (!preventTabActivatedEvent)
            {
                // if subscribed, raise the TabActivated event and give it the active ScintillaTabbedDocument class instance
                // as a parameter..
                TabActivated?.Invoke(this, new TabActivatedEventArgs() { ScintillaTabbedDocument = Documents[activeTabIndex] });
            }

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
                (leftFileIndex == -1 ? 0 : pnScrollingTabContainer.Controls[leftFileIndex].Width);
            
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
                    int newLeft = clientRect.Width - leftAtIndex - pnScrollingTabContainer.Controls[leftFileIndex].Width;
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
        // ReSharper disable once UnusedMember.Local
        private int GetDocumentIndex(Scintilla scintilla)
        {
            return scintilla == null ? -1 : Documents.FindIndex(f => f.Scintilla.Equals(scintilla));
        }
        #endregion

        #region "CodeIndent Handlers"
        // This (C): https://gist.github.com/Ahmad45123/f2910192987a73a52ab4
        // ReSharper disable once InconsistentNaming
        private const int SCI_SETLINEINDENTATION = 2126;
        // ReSharper disable once InconsistentNaming
        private const int SCI_GETLINEINDENTATION = 2127;
        void SetIndent(Scintilla scintilla, int line, int indent)
        {
            scintilla.DirectMessage(SCI_SETLINEINDENTATION, new IntPtr(line), new IntPtr(indent));
        }
        int GetIndent(Scintilla scintilla, int line)
        {
            return (scintilla.DirectMessage(SCI_GETLINEINDENTATION, new IntPtr(line), IntPtr.Zero).ToInt32());
        }
        #endregion

        #region InternalEvents
        private void Scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            // This (C): https://gist.github.com/Ahmad45123/f2910192987a73a52ab4
            //Codes for the handling the Indention of the lines.
            //They are manually added here until they get officially added to the Scintilla control.

            if (!UseCodeIndenting)
            {
                return;
            }

            var scintilla = (Scintilla) sender;

            //The '}' char.
            if (e.Char == '}') {
                int curLine = scintilla.LineFromPosition(scintilla.CurrentPosition);
		
                if (scintilla.Lines[curLine].Text.Trim() == "}") { //Check whether the bracket is the only thing on the line.. For cases like "if() { }".
                    SetIndent(scintilla, curLine, GetIndent(scintilla, curLine) - TabWidth);
                }
            }
        }

        private void Scintilla_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            // This (C): https://gist.github.com/Ahmad45123/f2910192987a73a52ab4
            if (!UseCodeIndenting)
            {
                return;
            }

            var scintilla = (Scintilla) sender;

            if ((e.Text.EndsWith("\r") || e.Text.EndsWith("\n"))) {
                int startPos = scintilla.Lines[scintilla.LineFromPosition(scintilla.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = scintilla.GetTextRange(startPos, (endPos - startPos)); //Text until the caret so that the whitespace is always equal in every line.
		
                Match indent = Regex.Match(curLineText, "^[ \\t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch(curLineText, "{\\s*$")) {
                    e.Text = (e.Text + "\t");
                }
            }
        }

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
                Documents[i].Scintilla.InsertCheck -= Scintilla_InsertCheck;
                Documents[i].Scintilla.CharAdded -= Scintilla_CharAdded;

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
}
