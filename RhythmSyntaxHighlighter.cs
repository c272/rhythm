using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScintillaNET;

namespace Rhythm
{
    public static class RhythmSyntaxHighlighter
    {
        //Style the text in one of the editor windows.
        public static void Style(object sender, StyleNeededEventArgs e)
        {
            //Get the text editor from the sender.
            Scintilla textArea = (Scintilla)sender;

            //Get the positions to style from.
            var startPos = textArea.GetEndStyled();
            var endPos = e.Position;

            //Reference the Algo lexer.
        }
    }
}
