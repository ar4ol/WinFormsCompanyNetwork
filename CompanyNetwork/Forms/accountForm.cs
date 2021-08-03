using CompanyNetwork.Forms;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CompanyNetwork
{
    public partial class accountForm : Form
    {
        private User user;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public int id_open_chat;
        public int id_chat_user;
        internal User User { get => user; set => user = value; }
        public static string date1 = "";
        public static string date2 = "";
        public static bool needFilter = false;


        public accountForm(object get_id)
        {
            InitializeComponent();
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
                            var id = reader.GetValue(0);
                            if ((int)id == (int)get_id)
                            {
                                user = new User(Convert.ToInt32(get_id));
                                user.Fill_User();
                                linkLabel1.Text = user.name + " " + user.surname;
                            }
                        }
                    }
                }
            }
            dataGridViewFill();
        }



        public void dataGridViewFill()
        {
            dataGridView1.Rows.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "Select id, [name] from chats where id in (select id_chat from chat_user where id_employee = " + user.id + ");";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                dataGridView2.Rows.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        dataGridView2.Rows.Add("", reader.GetValue(1), "Удалить", reader.GetValue(0));
                }
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible == true)
            {
                button7.Visible = false;
                dataGridView1.Visible = false;
                dataGridViewFill();
                dataGridView2.Visible = true;
                button1.Visible = false;
            }
            else
            {
                authorizationForm authorization = new authorizationForm();
                authorization.Show();
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
                

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (e.ColumnIndex == 0 && e.RowIndex == dataGridView2.Rows[i].Index)
                {
                    id_open_chat = Convert.ToInt32(dataGridView2.Rows[i].Cells[3].Value);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        button7.Visible = true;
                        dataGridView1.Visible = true;
                        dataGridView2.Visible = false;
                        button1.Visible = true;
                        connection.Open();
                        string request = "select chat_user.id from chat_user left join chats on chats.id = chat_user.id_chat left join employees on employees.id = chat_user.id_employee where chats.id = " + id_open_chat + " and employees.id = " + User.id + "";
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                        dataGridView1.Rows.Clear();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                                id_chat_user = reader.GetInt32(0);
                        }
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        dataGridView1.Visible = true;
                        dataGridView2.Visible = false;
                        button1.Visible = true;
                        connection.Open();
                        string request = "select message.id, message.time, employees.name, employees.surname, message.text from message left join chat_user  on chat_user.id = message.id_chat_user left join employees on chat_user.id_employee = employees.id where message.id_chat =" + id_open_chat;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                        dataGridView1.Rows.Clear();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                                dataGridView1.Rows.Add(reader.GetValue(1), (reader.GetString(2) + " " + reader.GetString(3)), reader.GetValue(4), reader.GetValue(0));
                        }
                    }
                }

                else if ((e.ColumnIndex == 2 && e.RowIndex == dataGridView2.Rows[i].Index))
                {
                    int id = 0;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; select chats.id_owner from chats where chats.id = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }

                        }
                    }
                    if (User.id != id)
                    {
                        MessageBox.Show("Вы не владелец чата!");
                        break;
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; delete message where id_chat = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; delete message where id_chat = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; delete chat_user where id_chat = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; delete chats where id = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                    }
                    MessageBox.Show("Удаление успешно!");
                    dataGridViewFill();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newChatForm nc = new newChatForm(this);
            nc.Show();
            this.Hide();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (e.ColumnIndex == 1 && e.RowIndex == dataGridView1.Rows[i].Index)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("select chat_user.id_employee from chat_user where chat_user.id = (select message.id_chat_user from message where message.id = "+dataGridView1.Rows[i].Cells[3].Value+")");
                        command.Connection = connection;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    ColleagueShow cs = new ColleagueShow(reader.GetInt32(0));
                                    cs.Show();
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewMessage nm = new NewMessage(this);
            nm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible == false)
            {
                if (textBox3.Text != "")
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[1].Value.ToString().ToLower().Contains(textBox3.Text.ToLower()))
                        {
                            dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                            dataGridView2.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                        }
                        else
                        {
                            dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(23, 33, 43);
                            dataGridView2.Rows[i].Cells[1].Style.ForeColor = Color.White;
                        }

                    }
                }
            }
            else
            {
                if (textBox3.Text != "")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString().ToLower().Contains(textBox3.Text.ToLower()))
                        {
                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(23, 33, 43);
                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.White;
                        }
                        if (dataGridView1.Rows[i].Cells[2].Value.ToString().ToLower().Contains(textBox3.Text.ToLower()))
                        {
                            dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                            dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Black;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.FromArgb(23, 33, 43);
                            dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.White;
                        }
                    }
                }
            }
        }

        public void Filter()
        {
            foreach(DataGridViewRow i in dataGridView1.Rows)
            {
                i.Visible = true;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime datemessage = Convert.ToDateTime(dataGridView1.Rows[i].Cells[0].Value.ToString().Remove(10));
                if (datemessage < Convert.ToDateTime(date2) || datemessage > Convert.ToDateTime(date1))
                {
                    dataGridView1.Rows[i].Visible = false;
                }
            }
            needFilter = false;
        }
        
        
        private void accountForm_VisibleChanged(object sender, EventArgs e)
        {
            if (needFilter == true)
                Filter();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FilterMessage fm = new FilterMessage(this);
            fm.Show();
            this.Hide();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (e.RowIndex == dataGridView2.Rows[i].Index)
                {
                    id_open_chat = Convert.ToInt32(dataGridView2.Rows[i].Cells[3].Value);
                    int id = 0;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string request = "use employees; select chats.id_owner from chats where chats.id = " + dataGridView2.Rows[i].Cells[3].Value;
                        SqlCommand command = new SqlCommand(request);
                        command.Connection = connection;
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }

                        }
                    }
                    if (User.id != id)
                    {
                        MessageBox.Show("Вы не владелец чата!");
                        break;
                    }
                    else
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string request = "use employees; update chats set name = '"+dataGridView2.Rows[i].Cells[1].Value+"' where chats.id = " + dataGridView2.Rows[i].Cells[3].Value;
                            SqlCommand command = new SqlCommand(request);
                            command.Connection = connection;
                            SqlDataReader reader = command.ExecuteReader();
                        }
                    }
                    MessageBox.Show("Переимнование успешно!");
                    dataGridViewFill();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            dataGridView2.Rows.Clear();
            dataGridViewFill();
                dataGridView1.Rows.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "select message.id, message.time, employees.name, employees.surname, message.text from message left join chat_user  on chat_user.id = message.id_chat_user left join employees on chat_user.id_employee = employees.id where message.id_chat =" + id_open_chat;
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            dataGridView1.Rows.Add(reader.GetValue(1), (reader.GetString(2) + " " + reader.GetString(3)), reader.GetValue(4), reader.GetValue(0));
                    }
                }
        }

        public void Message_Sent()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                dataGridView1.Visible = true;
                dataGridView2.Visible = false;
                button1.Visible = true;
                connection.Open();
                string request = "select message.id, message.time, employees.name, employees.surname, message.text from message left join chat_user  on chat_user.id = message.id_chat_user left join employees on chat_user.id_employee = employees.id where message.id_chat =" + id_open_chat;
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                dataGridView1.Rows.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        dataGridView1.Rows.Add(reader.GetValue(1), (reader.GetString(2) + " " + reader.GetString(3)), reader.GetValue(4), reader.GetValue(0));
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ColleagueShow cs = new ColleagueShow(user.id);
            cs.Show();
        }
    }
}
