using System.Drawing;
using System;
using System.Collections.Generic;
using ScintillaNET;

namespace Rhythm
{
    /// <summary>
    /// Class to manage Scintilla text editor components.
    /// </summary>
    public class ScintillaUtils
    {
        //Background colour of text area.
		private const int backColour = 0x2A211C;

        //Foreground colour of text area.
        private const int foreColour = 0xB7B7B7;

        //Enable code folding buttons.
        private const bool enableCodeFoldButtons = true;

        //Converts a hexadecimal integer to an RGB colour.
        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        //Configure the syntax highlighting for a specific editor.
        public static void ConfigureSyntaxColouring(Scintilla TextArea)
        {
            //Fill the docked area, enable syntax highlighting on text change.
            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            //TextArea.TextChanged += (this.OnTextChanged); //todo, not completed lexer yet

            //No wrap, and create some indent guides.
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            //Colour configuration.
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));

            // Configure the default style.
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            //Configure the line numbers on the left.
            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(backColour);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(foreColour);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(foreColour);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(backColour);

            var nums = TextArea.Margins[0];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            //Enable code folding.
            EnableCodeFolding(TextArea);

            //Set the lexer styling and pass the event to the syntax highlighting handler.
            TextArea.Lexer = Lexer.Container;
            TextArea.StyleNeeded += RhythmSyntaxHighlighter.Style;
        }

        //Enables code folding on a ScintillaNET TextArea.
        private static void EnableCodeFolding(Scintilla TextArea)
        {
            TextArea.SetFoldMarginColor(true, IntToColor(backColour));
            TextArea.SetFoldMarginHighlightColor(true, IntToColor(backColour));

            // Enable code folding
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            TextArea.Margins[1].Type = MarginType.Symbol;
            TextArea.Margins[1].Mask = Marker.MaskFolders;
            TextArea.Margins[1].Sensitive = true;
            TextArea.Margins[1].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(IntToColor(backColour)); // styles for [+] and [-]
                TextArea.Markers[i].SetBackColor(IntToColor(foreColour)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            TextArea.Markers[Marker.Folder].Symbol = enableCodeFoldButtons ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = enableCodeFoldButtons ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = enableCodeFoldButtons ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = enableCodeFoldButtons ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }
    }
}