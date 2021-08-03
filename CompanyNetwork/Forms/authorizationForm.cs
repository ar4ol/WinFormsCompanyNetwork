using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CompanyNetwork
{
    public partial class authorizationForm : Form
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public authorizationForm()
        {
            InitializeComponent();
        }
        
        private void loginButton_Click(object sender, EventArgs e)
        {
            if(passwordTextBox.Text == "admin1" &&  loginTextBox.Text == "admin")
            {
                adminForm a = new adminForm();
                a.Show();
                this.Hide();
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                bool found = false;
                SqlCommand command = new SqlCommand("select * from employees, card, department where employees.card = card.id and card.department = department.id and department.name !='Уволенные сотрудники'");
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var login = reader.GetValue(8);
                            var password = reader.GetValue(9);
                            if (loginTextBox.Text == Convert.ToString(login)
                                && passwordTextBox.Text == Convert.ToString(password))
                            {

                                found = true;
                                accountForm a = new accountForm(reader.GetValue(0));
                                a.Show();
                                this.Hide();
                                break;
                            }
                        }
                        if (!found)
                            MessageBox.Show("Неправильный логин или пароль!");
                    }
                }
                    
            }
                
        }

        //event when buttons presses in passwordTextBox
        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar) 
                || e.KeyChar == (char)Keys.Back)
                return;
            e.Handled = true;
        }

        //registration button start
        private void reg_button_Click(object sender, EventArgs e)
        {
            registrationForm registration = new registrationForm();
            registration.Show();
            this.Hide();
        }

        //event when buttons presses in loginTextBox         
        private void loginTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar)
                || e.KeyChar == (char)Keys.Back)
                return;
            e.Handled = true;
        }

            
        
    }
}
