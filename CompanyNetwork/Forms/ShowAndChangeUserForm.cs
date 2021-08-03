using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class ShowAndChangeUserForm : Form
    {
        User user;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        adminForm _a;

        public ShowAndChangeUserForm(int get_id, adminForm a)
        {
            InitializeComponent();
            _a = a;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from employees");
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            if (reader.GetInt32(0) == get_id)
                            {
                                user = new User(Convert.ToInt32(reader.GetValue(0)));
                                user.Fill_User();
                                FillForm();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void FillForm()
        {
            comboBox2.Visible = false;
            button10.Visible = true;
            pictureBox1.Image = Image.FromFile(user.foto);
            nameBox.Text = user.name;
            firstNameBox.Text = user.surname;
            textBox1.Text = user.dateOfBirhth;
            passportTextBox.Text = user.passport;
            emailBox.Text = user.email;
            loginBox.Text = user.login;
            phoneBox.Text = user.phone_number;
            textBox6.Text = user.card.position;
            textBox2.Text = user.card.salary + "";
            textBox3.Text = user.card.premium + "";
            textBox4.Text = user.card.fine + "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "use employees; select [name] from department";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetValue(0) + "");
                        }
                    }
                }
                request = "use employees; select [name] from department where id = (select department from [card] where [card].id = (select card from employees where login = '" + user.login + "'));";
                command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Text = reader.GetValue(0) + "";
                        }
                    }
                }
            }
            if (user.card.position == "Уволен")
            {
                label16.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = true;
                button9.Visible = false;
                button11.Visible = true;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "use employees; select [name] from department";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if(reader.GetString(0) != "Уволенные сотрудники")
                                    comboBox2.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            
            loginBox.Text = user.login;
            passwordBox.Text = user.password;
            passwordBox2.Text = user.password;
        }
        
        private void button10_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "use employees; update [card] set department = (select department.id from department where department.name = 'Уволенные сотрудники'), position = 'Уволен', salary = 0, premium = 0, fine = 0 from card where card.id = "+user.card.id+"";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();

                MessageBox.Show("Уволен!");
                adminForm a = new adminForm();
                a.Show();
                this.Hide();
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "openFileDialog1")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = String.Format("use employees; update employees set [name] = '{0}', [surname] = '{1}', [dateOfBirth] = '{2}', [email] = '{3}', [phone_number] = '{4}',  [passport]='{5}', [login]='{6}', [password]='{7}' where employees.id = '" + user.id + "'",
                    nameBox.Text, firstNameBox.Text, textBox1.Text, emailBox.Text, phoneBox.Text, passportTextBox.Text, loginBox.Text, passwordBox.Text);
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                    request = String.Format("use employees; update card set [position] = '{0}', [salary] = '{1}', [premium] = '{2}', [fine] = '{3}', [department] = (select department.id from department where [name] = '{4}') where id = '{5}'",
                    textBox6.Text, textBox2.Text, textBox3.Text, textBox4.Text, comboBox1.Text, user.card.id);
                    command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                }
                MessageBox.Show("Информация сохранена!");
                adminForm a = new adminForm();
                a.Show();
                this.Hide();
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = String.Format("use employees; update employees set [name] = '{0}', [surname] = '{1}', [dateOfBirth] = '{2}', [email] = '{3}', [phone_number] = '{4}',  [passport]='{5}', [login]='{6}', [password]='{7}', [foto] = '{8}' where employees.id = '" + user.id + "'",
                    nameBox.Text, firstNameBox.Text, textBox1.Text, emailBox.Text, phoneBox.Text, passportTextBox.Text, loginBox.Text, passwordBox.Text, openFileDialog1.FileName);
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                    request = String.Format("use employees; update card set [position] = '{0}', [salary] = '{1}', [premium] = '{2}', [fine] = '{3}', [department] = (select department.id from department where [name] = '{4}') where id = '{5}'",
                    textBox6.Text, textBox2.Text, textBox3.Text, textBox4.Text, comboBox1.Text, user.card.id);
                    command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                }
                MessageBox.Show("Информация сохранена!");
                adminForm a = new adminForm();
                a.Show();
                this.Hide();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "Уволен")
            {
                MessageBox.Show("Введите новую должность!");
                return;
            }
            if (comboBox2.Text != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string request = "use employees; update [card] set department = (select department.id from department where department.[name] = '" + comboBox2.Text + "'), position = '"+textBox6.Text+"' from card where card.id = "+user.card.id+";";
                    SqlCommand command = new SqlCommand(request);
                    connection.Open();
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                }
                adminForm a = new adminForm();
                a.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Выберите отдел!");
        }
        
        private void backButton_Click(object sender, EventArgs e)
        {
            _a.Show();
            this.Hide();
        }

        private void backButton_MouseMove(object sender, MouseEventArgs e)
        {
            backButton1.ForeColor = Color.White;
        }

        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            backButton1.ForeColor = Color.FromArgb(74, 88, 101);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
