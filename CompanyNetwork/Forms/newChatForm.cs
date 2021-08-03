using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class newChatForm : Form
    {
        string patternName = "^[а-я]{1,15}$";
        accountForm _a;

        public newChatForm(accountForm a)
        {
            InitializeComponent();
            _a = a;
        }

        private void regDepButton_Click(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(depNameBox.Text, patternName, RegexOptions.None))
            {
                MessageBox.Show("Названия чата может содержать только кирилицу и быть длинной от 2 до 16 символов.");
                depNameBox.Text = "";
            }
            else
            {
                addUsersToChatForm autcf = new addUsersToChatForm(depNameBox.Text, _a);
                autcf.Show();
                this.Hide();
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
                _a.Show();
                this.Hide();
        }

        private void backButton_MouseMove(object sender, MouseEventArgs e)
        {
            backButton.ForeColor = Color.White;
        }

        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            backButton.ForeColor = Color.FromArgb(74, 88, 101);
        }
    }
}
