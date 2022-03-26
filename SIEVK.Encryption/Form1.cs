using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIEVK.BusinessService;

namespace SIEVK.Encryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                SIEVK.BusinessService.Common.SecurityService ss = new BusinessService.Common.SecurityService();
                txtOutput.Text = ss.Encrypt(txtInput.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                SIEVK.BusinessService.Common.SecurityService ss = new BusinessService.Common.SecurityService();
                txtOutput.Text = ss.Decrypt(txtInput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
