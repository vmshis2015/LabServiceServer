using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

namespace Vietbait.DemoProject
{
    public partial class Passwords : Form
    {
        public Passwords()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if (textBox1.Text.Trim() == "hadikay" || textBox1.Text == "ha.dk")
            {
                this.Hide();
              TestDataForm a = new TestDataForm();
                a.ShowDialog();

             
            }
            else
            {
                MessageBox.Show("Sai mật khẩu mời bạn thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Passwords_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Passwords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Passwords_Load(object sender, EventArgs e)
        {

            //hmod = LoadLibrary("SciLexer.DLL");
            //if (hmod == NULL)
            //{
            //    MessageBox(hwndParent,
            //    "The Scintilla DLL could not be loaded.",
            //    "Error loading Scintilla",
            //    MB_OK | MB_ICONERROR);
            //}

        }

        private void Passwords_Enter(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
          
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }
    }
}
