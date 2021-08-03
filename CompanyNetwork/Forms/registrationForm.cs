using System;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CompanyNetwork
{

    public partial class registrationForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;        
        public registrationForm()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(DateTime.Now).Remove(10);
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
                            if (reader.GetString(0) != "Уволенные сотрудники")
                                comboBox1.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
        private void registerButton_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                if (Confirm())
                {
                    adminForm authorization = new adminForm();
                    authorization.Show();
                    this.Hide();
                }

            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            adminForm authorization = new adminForm();
            authorization.Show();
            this.Hide();
        }

        private void KeyPressLetter(KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back)
                return;
            e.Handled = true;
        }
        
        private void KeyPressLetterDigit(KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar) 
                || e.KeyChar == (char)Keys.Back)
                return;
            e.Handled = true;
        }

        private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetter(e);
        }

        private void firstNameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetter(e);
        }
        
        private void loginBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetterDigit(e);
        }

        private void passwordBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetterDigit(e);
        }

        private void passwordBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetterDigit(e);
        }

        private void keyWordBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressLetter(e);
        }

        private void ddBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void backButton_MouseMove(object sender, MouseEventArgs e)
        {
            backButton.ForeColor = Color.White;
        }

        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            backButton.ForeColor = Color.FromArgb(74,88,101);
        }
               

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker1.Text;
        }

        
        private new bool Validate()
        {                
            string patternLog = "^[A-Za-z0-9]{4,16}$";
            string patternPass = "^[A-Яа-яA-Za-z0-9]{4,16}$";
            string patternPhone = "^[+380][0-9]{12}$";
            string patternEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            string patternName = "^[А-Я][а-я]{1,15}$";
            string patternPass1 = "^[0-9]{9}$";
            string patternPass2 = "^[А-Яа-я][А-Яа-я][0-9]{6}$";
            if (!Regex.IsMatch(nameBox.Text, patternName, RegexOptions.None))
            {
                nameBox.Text = "";
                MessageBox.Show("Имя должно начинаться с большой буквы и быть" +
                    " длинной 2-16 букв.\n\n Разрешены только буквы кириллицы!");
                return false;
            }
            if (!Regex.IsMatch(firstNameBox.Text, patternName, RegexOptions.None))
            {
                firstNameBox.Text = "";
                MessageBox.Show("Фамилия должна начинаться с большой буквы и быть" +
                    " длинной 2-16 букв.\n\n Разрешены только буквы кириллицы!");
                return false;
            }
            if (!Regex.IsMatch(passportTextBox.Text,
                patternPass1,
                RegexOptions.None)
               && !Regex.IsMatch(passportTextBox.Text,
               patternPass2,
               RegexOptions.None))
            {
                passportTextBox.Text = "";
                MessageBox.Show("Не корректный номер паспорта!\n\n" +
                    "Примеры пасспорта: \"012345678\" , \"МК012345\"");
                return false;
            }
            if (passportTextBox.Text == "000000000"
                || passportTextBox.Text.Contains("000000")
                && passportTextBox.Text.Length == 8)
            {
                passportTextBox.Text = "";
                MessageBox.Show("Не корректный номер паспорта!\n");
                return false;
            }
            if (!Regex.IsMatch(emailBox.Text, patternEmail, RegexOptions.None))
            {
                emailBox.Text = "";
                MessageBox.Show("Не корректная електронная почта! Повторите попытку.");
                return false;
            }
            if (!Regex.IsMatch(phoneBox.Text, patternPhone, RegexOptions.None))
            {
                phoneBox.Text = "";
                MessageBox.Show("Не корректный номер телефона!\n\n" +
                    "Номер телефона должен начинаться с +380 и быть длиной 12 цифр!");
                return false;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = @"USE employees; select * from employees where [login] = '" + loginBox.Text + "';";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует");
                        return false;
                    }
                }                    
            }
            
            if (!Regex.IsMatch(loginBox.Text, patternLog, RegexOptions.None))
            {
                loginBox.Text = "";
                MessageBox.Show("Логин должен быть длинной  4-16 букв.\n\n" +
                    " Разрешены только буквы латиницы и цифры!");
                return false;
            }
            if (!Regex.IsMatch(passwordBox.Text, patternPass, RegexOptions.None))
            {
                passwordBox.Text = "";
                MessageBox.Show("Пароль должен быть длинной " +
                    "от 4 до 16 символов.\n\n" +
                    " Разрешены только буквы и цифры!");
                return false;
            }
            if (passwordBox.Text != passwordBox2.Text)
            {
                passwordBox.Text = "";
                passwordBox2.Text = "";
                MessageBox.Show("Пароли не совпадают!");
                return false;
            }

            
            return true;
        }

        
        public bool Confirm()
        {
            object id = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = @"USE employees; " +
                    "insert into card([position], [salary], [department])" +
                    "values " +
                    "('" + textBox6.Text + "', '" + textBox2.Text + "', (select id from department where name = '" + comboBox1.Text + "'));";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader()) { }
                command = new SqlCommand("select top 1 card.id  from card order by card.id desc");
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {                    
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            id = reader.GetValue(0);
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = @"use employees; " +
                "insert into employees ([name], surname, dateOfBirth, email, phone_number, foto, passport, [login], [password], [card])" +
                    "values " +
                    "('" + nameBox.Text + "', '" + firstNameBox.Text + "', '" + dateTimePicker1.Text + "', '" + emailBox.Text + "', '" + phoneBox.Text
                    + "', '" + openFileDialog1.FileName + "', '" + passportTextBox.Text + "', '" + loginBox.Text + "', '" + passwordBox.Text + "', '" + id + "');";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader()) { }
                return true;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
