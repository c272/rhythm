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
            textEditor.TextChanged += tabTextChanged;
            editorTabs.TabPages[editorTabs.TabPages.Count - 1].Controls.Add(textEditor);

            //Configure the syntax colouring, etc. for this editor.
            ScintillaUtils.ConfigureSyntaxColouring(textEditor);
        }

        //When text is changed in one of the editor tabs.
        private void tabTextChanged(object sender, EventArgs e)
        {
            //If text is changed, and the "NOSAVE" indicator isn't there, put it there.
            if (!editorTabs.TabPages[editorTabs.SelectedIndex].Text.EndsWith("*"))
            {
                //Grab the EditorTabInfo.
                EditorTabInfo info = (EditorTabInfo)editorTabs.TabPages[editorTabs.SelectedIndex].Tag;

                //Disable savedLatest (changes have been made).
                info.SavedLatest = false;

                //Append the nosave text.
                editorTabs.TabPages[editorTabs.SelectedIndex].Text += "*";
            }
        }

        //When the "Save" button is pressed in the "File" toolstrip.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Get the currently open tab, check if a save has already been completed.
            int tab = editorTabs.SelectedIndex;
            
            //Check a tab is loaded.
            if (tab <= -1) { return; }

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
                Error err = new Error("Failed to save to that location. You may not have write permissions for that file/folder (" + ex.Message + ").");
                return;
            }

            tabInfo.FileLocation = saveLocation;
            tabInfo.FileName = new FileInfo(saveLocation).Name;
            tabInfo.SavedLatest = true;
            editorTabs.TabPages[tab].Tag = tabInfo;

            //Set the title to the correct information.
            editorTabs.TabPages[tab].Text = tabInfo.FileName;

            //Get the extension of the file, set highlighting.
            string ext = new FileInfo(saveLocation).Extension;
            ScintillaUtils.SetHighlighting(ext, textArea);
        }
        
        //When the "Open" button is pressed in the "File" toolstrip.
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create an open dialog, get the location to open from.
            string openLocation = "";
            using (var selectFileDialog = new OpenFileDialog())
            {
                if (selectFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openLocation = selectFileDialog.FileName;
                }
                else { return; }
            }

            //Get file info, read all text from file.
            FileInfo fi = new FileInfo(openLocation);
            string toLoad = "";
            try
            {
                toLoad = File.ReadAllText(openLocation);
            } catch(Exception ex)
            {
                Error err = new Error("Failed to load the provided file. You may be lacking file read permissions for this file (" + ex.Message + ").");
                return;
            }

            //Only create a new tab if the latest one isn't just untitled.
            TabPage tab;
            if (editorTabs.TabPages.Count < 1 || editorTabs.TabPages[editorTabs.TabPages.Count - 1].Text != "Untitled*")
            {
                //Create a new tab.
                newToolStripMenuItem_Click(this, null);
            }

            //For that tab, set the file name and information.
            tab = editorTabs.TabPages[editorTabs.TabPages.Count - 1];
            tab.Text = fi.Name;
            EditorTabInfo tabInfo = (EditorTabInfo)tab.Tag;
            tabInfo.FileLocation = fi.FullName;
            tabInfo.FileName = fi.Name;
            tabInfo.SavedLatest = true;
            tab.Tag = tabInfo;
            editorTabs.TabPages[editorTabs.TabPages.Count - 1] = tab;

            //Get the text area to modify for highlighting and to add the text.
            Scintilla textArea = GetTextAreaFromTab(editorTabs.TabPages.Count - 1);
            textArea.Text = toLoad;

            //Set the highlighting based on extension.
            ScintillaUtils.SetHighlighting(fi.Extension, textArea);
        }

        //When the "Save As" button is pressed in the "File" toolstrip.
        private void saveAsFileButton_Click(object sender, EventArgs e)
        {

        }

        //When the "Exit" button is pressed in the "File" toolstrip.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //Returns the text area for the given tab.
        private Scintilla GetTextAreaFromTab(int tab)
        {
            return (Scintilla)(editorTabs.TabPages[tab].Controls.Find("ScintillaTextArea", true)[0]);
        }

        //////////////////////////////
        /// MIRROR BUTTON FUNCTIONS //
        //////////////////////////////
  
        //Mirrors the "New" button in the file tab.
        private void newFileButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        //Mirrors the "Open" button in the file tab.
        private void openFileButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        //Mirrors the "Save" button in the file tab.
        private void saveFileButton_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }
        
    }
}
