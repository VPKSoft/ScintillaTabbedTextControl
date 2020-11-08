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

using System.Globalization;
using ScintillaNET;

namespace VPKSoft.ScintillaTabbedTextControl
{
    /// <summary>
    /// A class to help to calculate the <see cref="Scintilla"/> zoom percentage from and to points.
    /// </summary>
    public class ScintillaZoomPercentage
    {
        /// <summary>
        /// Gets the zoom percentage value from given <see cref="Scintilla"/> instance.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla"/> class instance to be used to get the percentage from the zoom points.</param>
        /// <returns>System.Int32.</returns>
        public static int ZoomPercentageFromPoints(Scintilla scintilla)
        {
            // the zoom of the Scintilla is between -10 and +20..

            return ZoomPercentageFromPoints(scintilla.Zoom);
        }

        /// <summary>
        /// Gets the zoom percentage value from given integer value.
        /// </summary>
        /// <param name="zoomPoints">The value of zoom to be used to get the percentage from the zoom points.</param>
        /// <returns>System.Int32.</returns>
        public static int ZoomPercentageFromPoints(int zoomPoints)
        {
            // the zoom of the Scintilla is between -10 and +20..

            // a negative value means below hundred percent..
            if (zoomPoints < 0)
            {
                double zoom = 10 + zoomPoints;
                zoom *= 10;
                return (int) zoom == 0 ? 1: (int)zoom;
            }

            // a positive value means above hundred percent..
            if (zoomPoints > 0)
            {
                double zoom = (double)zoomPoints / 20;
                zoom *= 100;
                zoom += 100;
                return (int) zoom == 0 ? 1: (int)zoom;
            }

            // 0 means a hundred percent zoom..
            return 100;
        }


        /// <summary>
        /// Calculates the amount of zoom points from a given percentage amount for a <see cref="Scintilla"/> control.
        /// </summary>
        /// <param name="percentage">The percentage of the requested zoom.</param>
        /// <returns>System.Int32.</returns>
        public static int PointsFromZoomPercentage(int percentage)
        {
            // the zoom of the Scintilla is between -10 and +20..

            // a negative percentage is not allowed..
            if (percentage < 0)
            {
                return 0;
            }

            // a percentage from 0 to 5 percent means -10 points..
            if (percentage <= 5)
            {
                return -10;
            }

            // form 6 to 99 the return value will be a negative value of points..
            if (percentage > 5 && percentage < 100)
            {
                double zoom = (double)percentage / 10;
                zoom = zoom - 10;
                return (int) zoom;
            }

            // a percentage larger than 100 means point value above 0 points..
            if (percentage > 100)
            {
                double zoom = (double)percentage / 10;
                zoom = zoom - 10;
                zoom *= 2;
                return (int) zoom;
            }

            // 0 means 100 percent..
            return 0;
        }

        /// <summary>
        /// Calculates the amount of zoom points from a given percentage amount for a <see cref="Scintilla"/> control.
        /// </summary>
        /// <param name="percentageString">A string to parse into an integer percentage value of the requested zoom points.</param>
        /// <returns>System.Int32.</returns>
        public static int PointsFromZoomPercentage(string percentageString)
        {
            if (double.TryParse(percentageString.Trim().Replace("%", string.Empty).Replace(",", "."), 
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var percentage))
            {
                return PointsFromZoomPercentage((int) percentage);
            }

            return 100;
        }
    }

}
