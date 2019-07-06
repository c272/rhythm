using System.Drawing;
using System;
using System.Collections.Generic;
using ScintillaNET;
using System.Text;

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
            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(0x3C3F41);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(0x569CD6);

            var nums = TextArea.Margins[0];

            //This adds some padding to the right of the numbers before the text begins.
            TextArea.Margins[1].Width = 10;
            TextArea.Margins[1].Type = MarginType.BackColor;
            nums.Width = 45;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            //Enable code folding.
            //This isn't done for any languages at the moment.
            //EnableCodeFolding(TextArea);
        }

        //Set the syntax highlighting for a given textarea, with a known extension.
        public static void SetHighlighting(string extension, Scintilla TextArea)
        {
            //Set the lexer styling and pass the event to the syntax highlighting handler if necessary.
            switch (extension)
            {
                case ".ag":
                    //special styling required
                    TextArea.Lexer = Lexer.Container;
                    TextArea.StyleNeeded += RhythmSyntaxHighlighter.Style;
                    SetAlgoStyles(TextArea);
                    break;
                case ".cs":
                case ".cpp":
                    TextArea.Lexer = Lexer.Cpp;
                    break;
                case ".ada":
                    TextArea.Lexer = Lexer.Ada;
                    break;
                case ".asm":
                case ".s":
                    TextArea.Lexer = Lexer.Asm;
                    break;
                case ".bat":
                    TextArea.Lexer = Lexer.Batch;
                    break;
                case ".b":
                    TextArea.Lexer = Lexer.BlitzBasic;
                    break;
                case ".css":
                case ".style":
                    TextArea.Lexer = Lexer.Css;
                    break;
                case ".f":
                case ".for":
                case ".f90":
                    TextArea.Lexer = Lexer.Fortran;
                    break;
                case ".html":
                case ".htm":
                    TextArea.Lexer = Lexer.Html;
                    break;
                case ".json":
                    TextArea.Lexer = Lexer.Json;
                    break;
                case ".lsp":
                case ".rkt":
                case ".ss":
                case ".scm":
                case ".sch":
                    TextArea.Lexer = Lexer.Lisp;
                    break;
                case ".lua":
                    TextArea.Lexer = Lexer.Lua;
                    break;
                case ".md":
                case ".markdown":
                    TextArea.Lexer = Lexer.Markdown;
                    break;
                case ".pas":
                case ".pp":
                    TextArea.Lexer = Lexer.Pascal;
                    break;
                case ".pl":
                    TextArea.Lexer = Lexer.Perl;
                    break;
                case ".php":
                    TextArea.Lexer = Lexer.PhpScript;
                    break;
                case ".ps1":
                    TextArea.Lexer = Lexer.PowerShell;
                    break;
                case ".ini":
                case ".cfg":
                    TextArea.Lexer = Lexer.Properties;
                    break;
                case ".py":
                case ".py2":
                case ".py3":
                    TextArea.Lexer = Lexer.Python;
                    break;
                case ".r":
                    TextArea.Lexer = Lexer.R;
                    break;
                case ".rb":
                    TextArea.Lexer = Lexer.Ruby;
                    break;
                case ".st":
                case ".smalltalk":
                    TextArea.Lexer = Lexer.Smalltalk;
                    break;
                case ".sql":
                case ".mysql":
                    TextArea.Lexer = Lexer.Sql;
                    break;
                case ".vb":
                    TextArea.Lexer = Lexer.Vb;
                    break;
                case ".vbs":
                    TextArea.Lexer = Lexer.VbScript;
                    break;
                case ".v":
                    TextArea.Lexer = Lexer.Verilog;
                    break;
                case ".xml":
                    TextArea.Lexer = Lexer.Xml;
                    break;
                default:
                    //unknown language
                    TextArea.Lexer = Lexer.Null;
                    break;
            }
        }

        //Sets the style values for a given text area to comply with Algo specification.
        private static void SetAlgoStyles(Scintilla TextArea)
        {
            //Normal (non-styled)
            TextArea.Styles[0].ForeColor = Color.White;

            //Keywords (let, for, while, etc).
            TextArea.Styles[1].ForeColor = IntToColor(0x569CD6);

            //Comments
            TextArea.Styles[2].ForeColor = IntToColor(0x43A63A);

            //Strings
            TextArea.Styles[3].ForeColor = IntToColor(0xD69D85);

            //Numbers
            TextArea.Styles[4].ForeColor = Color.LightSeaGreen;
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

        //Validate the inserted characters for a given Scintilla TextArea control, before they're inserted.
        public static void ValidateInputCharacters(object sender, InsertCheckEventArgs e)
        {
            e.Text = RemoveControlCharacters(e.Text);
        }

        //Removes control codes from a given string.
        public static string RemoveControlCharacters(string inString)
        {
            //Characters that are exempt from being pruned.
            List<char> exemptChars = new List<char>() { '\n', '\r', '\t' };

            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if (!char.IsControl(ch) || exemptChars.Contains(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }
    }
}