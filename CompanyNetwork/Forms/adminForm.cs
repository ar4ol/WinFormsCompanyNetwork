using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace CompanyNetwork
{
    public partial class adminForm : Form
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        User u;
        int sumFine = 0;
        int sumPremium = 0;
        int sumSallary = 0;
        int countUser = 0;
        int countDepartment = 0;
        int dtgrd = 0;
        public static bool needFilter = false;
        public static string date1 = "";
        public static string date2 = "";       
        
        public adminForm()
        {
            InitializeComponent();
            label1.Visible = false;
            textBox2.Visible = false;
            button8.Visible = false;
            dataGridView1Fill();
            dataGridView3.Rows.Add("", "Количество сотрудников", "Отчет который отображает количество сотрудников компании");
            dataGridView3.Rows.Add("", "Количеcтво отделов", "Отчет который отображает количество отделов компании");
            dataGridView3.Rows.Add("", "Выплаченные зарплаты", "Отчет который отображает зарплату сотрудников компании");
            dataGridView3.Rows.Add("", "Выплаченные премии", "Отчет который отображает размер выплаченных премий");
            dataGridView3.Rows.Add("", "Штрафы", "Отчет который отображет штрафы наложеные на сотрудников компании");
        
        }

        public void dataGridView1Fill()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; Select * from [department]";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                dataGridView1.Rows.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        dataGridView1.Rows.Add("", reader.GetValue(1), reader.GetValue(0));
                }
            }            
        }
        
        
        private void backButton_Click(object sender, EventArgs e)
        {
            if(dataGridView4.Visible || dataGridView5.Visible || dataGridView6.Visible || dataGridView7.Visible || dataGridView8.Visible)
            {
                dataGridView4.Visible = false;
                dataGridView5.Visible = false;
                dataGridView6.Visible = false;
                dataGridView7.Visible = false;
                dataGridView8.Visible = false;
                dataGridView3.Visible = true;
                label1.Visible = false;
                textBox2.Visible = false;
                button8.Visible = false;
            }
            else if (dataGridView2.Visible == true)
            {
                dataGridView2.Visible = false;
                button1.Enabled = true;
                button1.Visible = true;
                button5.Enabled = true;
                button5.Visible = true;
                button7.Visible = false;
                button7.Enabled = false;
                dataGridView1Fill();
            }
            else
            {
                authorizationForm authorization = new authorizationForm();
                authorization.Show();
                this.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void backButton_MouseMove(object sender, MouseEventArgs e)
        {
            backButton.ForeColor = Color.White;
        }

        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            backButton.ForeColor = Color.FromArgb(74, 88, 101);
        }

        private void add_employee_Click(object sender, EventArgs e)
        {
            registrationForm rf = new registrationForm();
            rf.Show();
            this.Hide();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label1.Visible = true;
            textBox2.Visible = true;
            button8.Visible = true;
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (e.ColumnIndex == 0 && e.RowIndex == dataGridView3.Rows[i].Index)
                {
                    if (dataGridView3.Rows[i].Cells[1].Value + "" == "Количество сотрудников")
                    {
                        dataGridView3.Visible = false;
                        dataGridView4.Visible = true;
                        dataGridView4.Rows.Clear();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "USE employees; select count(*) from employees e left join card c on e.card = c.id left join department d on c.department = d.id where d.name != 'Уволенные сотрудники';";
                            SqlCommand command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        countUser = Convert.ToInt32(reader.GetValue(0));
                                    }
                                }
                            }
                            request = "USE employees; select * from employees e left join card c on e.card = c.id left join department d on c.department = d.id where d.name != 'Уволенные сотрудники';;";
                            command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        u = new User(Convert.ToInt32(reader.GetValue(0)));
                                        u.Fill_User();
                                        dataGridView4.Rows.Add(u.name, u.surname, u.dateOfBirhth, u.card.name_department);
                                    }
                                }
                            }
                        }
                        label1.Text = "Количество сотрудников";
                        textBox2.Text = countUser + "";
                    }
                    else if (dataGridView3.Rows[i].Cells[1].Value + "" == "Количеcтво отделов")
                    {
                        dataGridView3.Visible = false;
                        dataGridView4.Visible = false;
                        dataGridView5.Visible = true;
                        dataGridView5.Rows.Clear();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "USE employees; select count(d.id) from department d where d.name != 'Уволенные сотрудники';";
                            SqlCommand command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        countDepartment = reader.GetInt32(0);
                                    }
                                }
                            }
                            request = "Use employees; Select d.[name], COUNT(c.id) From department d left join [card] c on c.department = d.id where d.[name] != 'Уволенные сотрудники' group by d.[name]";
                            command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dataGridView5.Rows.Add(reader.GetValue(0), reader.GetValue(1));
                                    }
                                }
                            }
                        }
                        label1.Text = "Количество отделов: ";
                        textBox2.Text = countDepartment + "";

                       
                    }
                    else if (dataGridView3.Rows[i].Cells[1].Value + "" == "Выплаченные зарплаты")
                    {
                        dataGridView3.Visible = false;
                        dataGridView4.Visible = false;
                        dataGridView5.Visible = false;
                        dataGridView6.Visible = true;
                        dataGridView6.Rows.Clear();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "Use employees; Select SUM(c.salary) From [card] c left join department d On c.department = d.id where d.name != 'Уволенные сотрудники';";
                            SqlCommand command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    try
                                    {
                                        while (reader.Read())
                                        {
                                            sumSallary = Convert.ToInt32(reader.GetValue(0));
                                        }
                                    }
                                    catch
                                    {
                                        sumSallary = 0;
                                    }
                                }                                
                            }
                            request = "Use employees; Select e.[name], e.surname, d.[name], c.salary From employees e left join[card] c left join department d On c.department = d.id On e.[card] = c.id where d.name != 'Уволенные сотрудники' group by e.[name], e.surname, e.dateOfBirth, d.[name], c.salary";
                            command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dataGridView6.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                    }
                                }
                            }
                        }                        
                        label1.Text = "Выплачено зарплат: ";
                        textBox2.Text = sumSallary + "";
                    }
                    else if (dataGridView3.Rows[i].Cells[1].Value + "" == "Выплаченные премии")
                    {
                        dataGridView3.Visible = false;
                        dataGridView4.Visible = false;
                        dataGridView5.Visible = false;
                        dataGridView6.Visible = false;
                        dataGridView7.Visible = true;
                        dataGridView7.Rows.Clear();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "Use employees; Select SUM(c.premium) From card c left join department d On c.department = d.id where d.name != 'Уволенные сотрудники'";
                            SqlCommand command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    try
                                    {
                                        while (reader.Read())
                                        {
                                            sumPremium = Convert.ToInt32(reader.GetValue(0));
                                        }
                                    }
                                    catch
                                    {
                                        sumPremium = 0;
                                    }
                                }
                            }
                            request = "Use employees; Select e.[name], e.surname, d.[name], c.premium From employees e left join[card] c left join department d On c.department = d.id On e.[card] = c.id where d.name != 'Уволенные сотрудники' group by e.[name], e.surname, e.dateOfBirth, d.[name], c.premium order by c.premium desc";
                            command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.GetValue(3) == DBNull.Value)
                                            dataGridView7.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), "0");
                                        else
                                            dataGridView7.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                    }
                                }
                            }
                        }                        
                        label1.Text = "Выплаченно премий на сумму: ";
                        textBox2.Text = sumPremium + "";
                    }
                    else if (dataGridView3.Rows[i].Cells[1].Value + "" == "Штрафы")
                    {
                        dataGridView3.Visible = false;
                        dataGridView4.Visible = false;
                        dataGridView5.Visible = false;
                        dataGridView6.Visible = false;
                        dataGridView7.Visible = false;
                        dataGridView8.Visible = true;
                        dataGridView8.Rows.Clear();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "Use employees; Select SUM(c.fine) From employees e left join[card] c left join department d On c.department = d.id On e.[card] = c.id where d.name != 'Уволенные сотрудники'";
                            SqlCommand command = new SqlCommand(request);                            
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    try
                                    {
                                        while (reader.Read())
                                        {
                                            sumFine = Convert.ToInt32(reader.GetValue(0));
                                        }
                                    }
                                    catch
                                    {
                                        sumFine = 0;
                                    }
                                }
                            }
                            request = "Use employees; Select e.[name], e.surname, d.[name], c.fine From employees e left join[card] c left join department d On c.department = d.id On e.[card] = c.id where d.name != 'Уволенные сотрудники' group by e.[name], e.surname, e.dateOfBirth, d.[name], c.fine order by c.fine desc";
                            command = new SqlCommand(request);
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if(reader.GetValue(3) == DBNull.Value)
                                            dataGridView8.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), "0");
                                        else
                                            dataGridView8.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                    }
                                }
                            }
                        }
                        
                        label1.Text = " на сумму: ";
                        textBox2.Text = sumFine + "";
                    }
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (e.ColumnIndex == 0 && e.RowIndex == dataGridView1.Rows[i].Index)
                {
                    dtgrd = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                    dataGridView2.Visible = true;
                    button1.Enabled = false;
                    button1.Visible = false;
                    button5.Enabled = false;
                    button5.Visible = false;
                    button7.Visible = true;
                    button7.Enabled = true;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "USE employees; select * from employees where [card] in (select [card].id from [card] where department = " + dataGridView1.Rows[i].Cells[2].Value + ");";
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataGridView2.Rows.Clear();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dataGridView2.Rows.Add("", reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(0));                                    
                                }
                                    
                            }
                        }
                    }                    
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CompanyNetwork.Forms.regDepartmentForm rd = new Forms.regDepartmentForm();
            rd.Show();
            this.Hide();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (e.ColumnIndex == 0 && e.RowIndex == dataGridView2.Rows[i].Index)
                {
                    Forms.ShowAndChangeUserForm sac = new Forms.ShowAndChangeUserForm(Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value), this);
                    sac.Show();
                    this.Hide();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Forms.delDepartmentForm f = new Forms.delDepartmentForm();
            f.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Visible == false)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "USE employees; Select * from [department]";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if ((reader.GetValue(1) + "").ToLower().Contains(textBox1.Text.ToLower()))
                                {
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        if ((dataGridView1.Rows[i].Cells[1].Value + "") == (reader.GetValue(1) + ""))
                                        {
                                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        if ((dataGridView1.Rows[i].Cells[1].Value + "") == (reader.GetValue(1) + ""))
                                        {
                                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(23, 33, 43);
                                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                    
                
                
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "USE employees; Select * from [employees]";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if ((reader.GetValue(1) + "").ToLower().Contains(textBox1.Text.ToLower()))
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if ((dataGridView2.Rows[i].Cells[1].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                                            dataGridView2.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if (!(dataGridView2.Rows[i].Cells[1].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(23, 33, 43);
                                            dataGridView2.Rows[i].Cells[1].Style.ForeColor = Color.White;
                                        }
                                    }
                                }

                                if ((reader.GetValue(2) + "").ToLower().Contains(textBox1.Text.ToLower()))
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if ((dataGridView2.Rows[i].Cells[2].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                                            dataGridView2.Rows[i].Cells[2].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if (!(dataGridView2.Rows[i].Cells[2].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[2].Style.BackColor = Color.FromArgb(23, 33, 43);
                                            dataGridView2.Rows[i].Cells[2].Style.ForeColor = Color.White;
                                        }
                                    }
                                }
                                if ((reader.GetValue(3) + "").ToLower().Contains(textBox1.Text.ToLower()))
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if ((dataGridView2.Rows[i].Cells[3].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                                            dataGridView2.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        if (!(dataGridView2.Rows[i].Cells[3].Value + "").ToLower().Contains(textBox1.Text.ToLower()))
                                        {
                                            dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.FromArgb(23, 33, 43);
                                            dataGridView2.Rows[i].Cells[3].Style.ForeColor = Color.White;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                    
                
               
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Forms.Filter f = new Forms.Filter(this);
            f.Show();
            this.Hide();
        }



        private void Filter()
        {
            DateTime d1 = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);
            dataGridView2.Rows.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; select * from employees where [card] in (select [card].id from [card] where department = " + dtgrd + ");";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dataGridView2.Rows.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (d1 >= Convert.ToDateTime(reader.GetValue(3)) && Convert.ToDateTime(reader.GetValue(3)) >= d2)
                            {

                                dataGridView2.Rows.Add("", reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                            }
                        }
                    }
                }
            }
            needFilter = false;
        }
        
        private void adminForm_VisibleChanged(object sender, EventArgs e)
        {
            if (needFilter == true)
                Filter();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string dt2 = (DateTime.Now+"").Replace('.', '_').Replace(':','_');
            if (dataGridView4.Visible == true)
            {
                StreamWriter streamWriter = new StreamWriter(@"C:\Users\Artem\Desktop\countUsers " + dt2 + ".txt", false);                
                streamWriter.WriteLine("\t\tОтчет по количеству сотрудников\t\t");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine("Имя\t\t" + " " + " " + " " + " " + "Фамилия\t\t" + " " + " " + " " + " " + "Дата Рождения\t" + " " + " " + " " + " " + "Отдел");
                streamWriter.WriteLine();
                for(int i = 0; i < dataGridView4.RowCount; i++)
                {
                    streamWriter.WriteLine(dataGridView4.Rows[i].Cells[0].Value+ "\t\t" + " " + " " + " " + " " +
                        dataGridView4.Rows[i].Cells[1].Value+ "\t\t" + " " + " " + " " + " " +
                        dataGridView4.Rows[i].Cells[2].Value+ "\t\t" + " " + " " + " " + " " +
                        dataGridView4.Rows[i].Cells[3].Value);
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.Write(dt2+"\t\t\t\t\t\t"+label1.Text+" "+textBox2.Text);
                streamWriter.Close();
                MessageBox.Show("Отчет сформирован!");
            }
            else if (dataGridView5.Visible == true)
            {
                    StreamWriter streamWriter = new StreamWriter(@"C:\Users\Artem\Desktop\countDepartment " + dt2 + ".txt", false);
                    streamWriter.WriteLine("\t\tОтчет по количеству отделов\t\t");
                    streamWriter.WriteLine();
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("Название отдела\t\tКоличество сотрудников");
                    streamWriter.WriteLine();
                    for (int i = 0; i < dataGridView5.RowCount; i++)
                    {
                    streamWriter.WriteLine(dataGridView5.Rows[i].Cells[0].Value + " " + " " + " " + " " + "\t"+" " + " " + " " + " "+"\t"+" " + " " + " " + " " +
                        dataGridView5.Rows[i].Cells[1].Value);
                    }
                    streamWriter.WriteLine();
                    streamWriter.WriteLine();
                    streamWriter.WriteLine();
                    streamWriter.Write(dt2 + "\t\t" + label1.Text + " " + textBox2.Text);
                    streamWriter.Close();
                    MessageBox.Show("Отчет сформирован!");
            }
            else if (dataGridView6.Visible == true)
            {
                StreamWriter streamWriter = new StreamWriter(@"C:\Users\Artem\Desktop\sumSalary " + dt2 + ".txt", false);
                streamWriter.WriteLine("\t\tОтчет по зарплатам\t\t");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine("Имя\t" + " " + " " + " " + " " + "Фамилия\t" + " " + " " + " " + " " + "Отдел\t" + " " + " " + " " + " " + "Зарплата");
                streamWriter.WriteLine();
                for (int i = 0; i < dataGridView6.RowCount; i++)
                {
                    streamWriter.WriteLine(dataGridView6.Rows[i].Cells[0].Value + "\t" + " " + " " + " " + " " +
                        dataGridView6.Rows[i].Cells[1].Value + "\t" + " " + " " + " " + " " +
                        dataGridView6.Rows[i].Cells[2].Value + "\t" + " " + " " + " " + " " +
                        dataGridView6.Rows[i].Cells[3].Value + "\t" + " " + " " + " " + " ");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.Write(dt2 + "\t\t\t\t" + label1.Text + " " + textBox2.Text);
                streamWriter.Close();
                MessageBox.Show("Отчет сформирован!");

            }
            else if (dataGridView7.Visible == true)
            {
                StreamWriter streamWriter = new StreamWriter(@"C:\Users\Artem\Desktop\sumPremium " + dt2 + ".txt", false);
                streamWriter.WriteLine("\t\tОтчет по премиям\t\t");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine("Имя\t" + " " + " " + " " + " " + "Фамилия\t" + " " + " " + " " + " " + "Отдел\t" + " " + " " + " " + " " + "Премия");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                for (int i = 0; i < dataGridView7.RowCount; i++)
                {
                    streamWriter.WriteLine(dataGridView7.Rows[i].Cells[0].Value + "\t" + " " + " " + " " + " " + 
                        dataGridView7.Rows[i].Cells[1].Value + "\t" + " " + " " + " " + " " +
                        dataGridView7.Rows[i].Cells[2].Value + "\t" + " " + " " + " " + " " +
                        dataGridView7.Rows[i].Cells[3].Value + "\t"+ " " + " " + " " + " ");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.Write(dt2 + "\t\t\t\t" + label1.Text + " " + textBox2.Text);
                streamWriter.Close();
                MessageBox.Show("Отчет сформирован!");

            }
            else if (dataGridView8.Visible == true)
            {
                StreamWriter streamWriter = new StreamWriter(@"C:\Users\Artem\Desktop\sumFine " + dt2 + ".txt", false);
                streamWriter.WriteLine("\t\tОтчет по штрафам\t\t");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine("Имя\t" + " " + " " + " " + " " + "Фамилия\t" + " " + " " + " " + " " + "Отдел\t" + " " + " " + " " + " " + "Штраф");
                streamWriter.WriteLine();
                for (int i = 0; i < dataGridView8.RowCount; i++)
                {
                    streamWriter.WriteLine(dataGridView8.Rows[i].Cells[0].Value + "\t" + " " + " " + " " + " " +
                        dataGridView8.Rows[i].Cells[1].Value + "\t" + " " + " " + " " + " " +
                        dataGridView8.Rows[i].Cells[2].Value + "\t" + " " + " " + " " + " " +
                        dataGridView8.Rows[i].Cells[3].Value + "\t" + " " + " " + " " + " ");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                streamWriter.Write(dt2 + "\t\t\t\t" + label1.Text + " " + textBox2.Text);
                streamWriter.Close();
                MessageBox.Show("Отчет сформирован!");

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(connectionString);
                sqlconn.Open();
                SqlDataAdapter oda = new SqlDataAdapter(TestInput.Text, sqlconn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                dataGridView9.DataSource = dt;
                sqlconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TestInput.Clear();
            TestInput.Text = "SELECT";
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                button3.Visible = false;
                textBox1.Visible = false;
                tabControl2.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
            }
            else
            {
                button3.Visible = true;
                textBox1.Visible = true;
                tabControl2.Visible = true;
                button6.Visible = true;
                if (dataGridView2.Visible == true)
                {
                    button7.Visible = true;
                }
            }

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (e.RowIndex == dataGridView1.Rows[i].Index)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("use employees;select department.name from [department]");
                        command.Connection = connection;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string HasDep = reader.GetString(0);
                                    if (HasDep == dataGridView1.Rows[i].Cells[1].Value+"")
                                    {
                                        MessageBox.Show("Отдел с таким названием уже существует!\nВыберите другое название!");
                                        dataGridView1.Rows.Clear();
                                        dataGridView1Fill();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; update department set name = '" + dataGridView1.Rows[i].Cells[1].Value + "' where department.id = " + dataGridView1.Rows[i].Cells[2].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                        MessageBox.Show("Переиминование успешно!");
                        break;
                    }
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1Fill();
            if (dataGridView2.Visible == true)
            {
                dataGridView2.Rows.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "USE employees; select * from employees where [card] in (select [card].id from [card] where department = " + dtgrd + ");";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataGridView2.Rows.Clear();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dataGridView2.Rows.Add("", reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                            }

                        }
                    }
                }
            }
        }
        
    }
}
