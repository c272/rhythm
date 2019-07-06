using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI;
using DarkUI.Forms;

namespace easyCase2
{
    public partial class Error : DarkForm
    {
        private string errorMessage;

        public static bool SupressErrors { get; internal set; }

        public Error(string error)
        {
            if (SupressErrors) { Dispose(); }

            errorMessage = error;
            InitializeComponent();
            Show();
        }

        private void Error_Load(object sender, EventArgs e)
        {
            //Set the size of the form based on the length of the text.
            int numExtraLines = (errorMessage.Length / 37 - 1);
            int numExtraLength = numExtraLines - 2;
            if (numExtraLength < 0) { numExtraLength = 0; }

            this.Height += 11 * numExtraLength;
            errorTxt.Height += 17 * numExtraLines;

            //Set the text.
            errorTxt.Text = errorMessage;
        }

        private void errorOK_Click(object sender, EventArgs e)
        {
            Hide();
            Dispose();
        }
    }
}
