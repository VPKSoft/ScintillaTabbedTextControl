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

namespace VPKSoft.ScintillaTabbedTextControl
{
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
    /// A class which is passed to via the AcceptNewFileName event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class AcceptNewFileNameEventArgs: EventArgs
    {
        /// <summary>
        /// Gets a file name which the control is proposing for a new file name.
        /// </summary>
        public string FileName { get; internal set; }

        /// <summary>
        /// Gets or sets a flag indicating whether a new file name should be proposed.
        /// </summary>
        public bool Accept { get; set; }
    }

    /// <summary>
    /// A class which instance is passed via a parameter to the TabActivated event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TabActivatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument class which was activated.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;
    }

    /// <summary>
    /// A class which instance is passed via a parameter to the TabClosed event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TabClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument class which was closed.
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
    /// A class which instance is passed via a parameter to the DocumentZoomChanged event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ScintillaZoomChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an instance to a ScintillaTabbedDocument which zoom was changed.
        /// </summary>
        public ScintillaTabbedDocument ScintillaTabbedDocument { get; set; } = null;

        /// <summary>
        /// Gets the zoom percentage to witch to zoom percentage did change into.
        /// </summary>
        public int ZoomPercentage { get; internal set; }
    }
}
