using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;
using ScintillaNET;

namespace Rhythm
{
    public partial class Rhythm : DarkForm
    {
        public Rhythm()
        {
            InitializeComponent();
        }

        //Execute things on load.
        private void Rhythm_Load(object sender, EventArgs e)
        {
            //Create a new tab on load.
            newToolStripMenuItem_Click(this, null);
        }

        //When the "New" button is pressed in the "File" toolstrip.
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create the tab.
            editorTabs.TabPages.Add("Untitled (*)");

            //Create the text editor control for this tab.
            Scintilla textEditor = new Scintilla();
            editorTabs.TabPages[editorTabs.TabPages.Count - 1].Controls.Add(textEditor);

            //Configure the syntax colouring, etc. for this editor.
            ScintillaUtils.ConfigureSyntaxColouring(textEditor);
        }
    }
}
