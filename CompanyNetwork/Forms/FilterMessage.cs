using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class FilterMessage : Form
    {
        accountForm _a;

        public FilterMessage(accountForm a)
        {
            InitializeComponent();
            _a = a;
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

        private void restorButton_Click(object sender, EventArgs e)
        {
            accountForm.date1 = dateTimePicker1.Text;
            accountForm.date2 = dateTimePicker2.Text;
            accountForm.needFilter = true;
            this.Hide();
            _a.Show();
        }
    }
}
