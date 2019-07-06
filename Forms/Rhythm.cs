using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            //Enable hotkeys.
            //..
             
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
            editorTabs.TabPages.Add("Untitled*");

            //Configure the tab (enable X button and give tab info)
            TabPage last = editorTabs.TabPages[editorTabs.TabPages.Count - 1];
            last.Tag = new EditorTabInfo();

            //Create the text editor control for this tab.
            Scintilla textEditor = new Scintilla();
            textEditor.Name = "ScintillaTextArea";
            editorTabs.TabPages[editorTabs.TabPages.Count - 1].Controls.Add(textEditor);

            //Configure the syntax colouring, etc. for this editor.
            ScintillaUtils.ConfigureSyntaxColouring(textEditor);
        }

        //When the "Save" button is pressed in the "File" toolstrip.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Get the currently open tab, check if a save has already been completed.
            int tab = editorTabs.SelectedIndex;
            EditorTabInfo tabInfo = (EditorTabInfo)editorTabs.TabPages[tab].Tag;
            Scintilla textArea = GetTextAreaFromTab(tab);

            //If it's not been saved, then get a new saveLoc, otherwise use the old one.
            string saveLocation = "";
            if (tabInfo.FileLocation == "")
            {
                //NOT SAVED ALREADY
                //Open a Save dialog.
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Algo files (*.ag)|*.ag|All files (*.*)|*.*";
                    dialog.FilterIndex = 2;
                    dialog.RestoreDirectory = true;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        //Set the save location.
                        saveLocation = dialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            } else
            {
                //Just re-save to the location.
                saveLocation = tabInfo.FileLocation;
            }

            //Saving file, setting proper tab information.
            try
            {
                File.WriteAllText(saveLocation, textArea.Text);
            }
            catch (Exception ex)
            {
                
            }

        }
        
        //Returns the text area for the given tab.
        private Scintilla GetTextAreaFromTab(int tab)
        {
            return (Scintilla)(editorTabs.TabPages[tab].Controls.Find("ScintillaTextArea", true)[0]);
        }
    }
}
