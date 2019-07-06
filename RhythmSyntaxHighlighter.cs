using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScintillaNET;
using Algo;
using Antlr4.Runtime;

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

            //Get the string to style.
            string toStyle = textArea.Text.Substring(startPos, endPos - startPos);

            //Pass the string into the Algo lexer.
            var chars = new AntlrInputStream(toStyle);
            var lexer = new algoLexer(chars);

            //Enumerate over each token in the lexer, and style them with the corresponding colours.
            var tokens = lexer.GetAllTokens();
            var vocab = lexer.Vocabulary;
            foreach (var tok in tokens)
            {
                //Get the "real" name of the token.
                string tokenName = vocab.GetSymbolicName(tok.Type);

                //Switch on the name, for the correct colour.
                switch (tokenName)
                {
                    
                }
            }
        }
    }
}
