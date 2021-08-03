using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class NewMessage : Form
    {
        private accountForm _a;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public NewMessage(accountForm a)
        {
            InitializeComponent();
            _a = a;
        }

        private void regDepButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "use employees; insert into message (id_chat_user, id_chat, text, time) values (" + _a.id_chat_user + ", " + _a.id_open_chat + ", '" + depNameBox.Text + "', '" + DateTime.Now + "');";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
            }
            MessageBox.Show("Cообщение отправлено!");
            _a.Message_Sent();
            this.Close();
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
