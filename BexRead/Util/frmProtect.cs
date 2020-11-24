using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BexFileRead.Util
{
    public partial class frmProtect : Form
    {
        public frmProtect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var decriptText = Protection.EncryptString(txtIn.Text);
            txtOut.Text = decriptText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var decriptText = Protection.DecryptString(txtIn.Text);
            txtOut.Text = decriptText;
        }
    }
}
