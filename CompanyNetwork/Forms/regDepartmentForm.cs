using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class regDepartmentForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public regDepartmentForm()
        {
            InitializeComponent();
        }
        
        private void regDepButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select department.name from [department]");
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string HasDep = reader.GetString(0);
                            if (HasDep == depNameBox.Text)
                            {
                                MessageBox.Show("Отдел с таким названием уже существует!\nВыберите другое название!");
                                return;
                            }
                        }
                    }
                }
                adminForm af = new adminForm();
                af.Show();
                this.Hide();
            }
            using ( SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into [department] values ('" + depNameBox.Text + "');");
                command.Connection = connection;
                using (command.ExecuteReader()) { }
                adminForm af = new adminForm();
                af.Show();
                this.Hide();
            }
        }

        private void backButton_MouseMove(object sender, MouseEventArgs e)
        {
            backButton.ForeColor = Color.White;
        }

        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            backButton.ForeColor = Color.FromArgb(74, 88, 101);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            adminForm af = new adminForm();
            af.Show();
            this.Hide();
        }

    }
}
