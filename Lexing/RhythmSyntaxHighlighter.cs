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
        //Colouring constants for Scintilla.
        public const int COLOUR_KEYWORD = 1;
        public const int COLOUR_COMMENTS = 2;
        public const int COLOUR_STRINGS = 3;
        public const int COLOUR_NUMBERS = 4;

        //Style the text in one of the editor windows.
        public static void Style(object sender, StyleNeededEventArgs e)
        {
            //Get the text editor from the sender.
            Scintilla textArea = (Scintilla)sender;

            //Get the positions to style from.
            var startPos = 0;
            var endPos = e.Position;
            textArea.StartStyling(startPos);

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
                int colourInt = 0;
                switch (tokenName)
                {
                    case "INTEGER":
                    case "FLOAT":
                    case "RATIONAL":
                        //Number colour.
                        colourInt = COLOUR_NUMBERS;
                        break;
                    case "STRING":
                        //String colour.
                        colourInt = COLOUR_STRINGS;
                        break;
                    case "NULL":
                    case "BOOLEAN":
                    case "LET_SYM":
                    case "FOR_SYM":
                    case "FOREACH_SYM":
                    case "ADD_SYM":
                    case "BREAK_SYM":
                    case "AT_SYM":
                    case "REMOVE_SYM":
                    case "FROM_SYM":
                    case "WHILE_SYM":
                    case "IN_SYM":
                    case "IF_SYM":
                    case "UP_SYM":
                    case "TO_SYM":
                    case "AS_SYM":
                    case "ENUM_SYM":
                    case "LIB_SYM":
                    case "SIG_FIG_SYM":
                    case "OBJ_SYM":
                    case "ELSE_SYM":
                    case "IMPORT_SYM":
                    case "RETURN_SYM":
                    case "PRINT_SYM":
                    case "DISREGARD_SYM":
                    case "EXTERNAL_SYM":
                        //Keywords.
                         colourInt = COLOUR_KEYWORD;
                        break;
                    case "COMMENT":
                        //Comment.
                        colourInt = COLOUR_COMMENTS;
                        break;
                }

                //Styling colour was set, so set it on text area.
                textArea.SetStyling(tok.Text.Length, colourInt);
            }
        }
    }
}
