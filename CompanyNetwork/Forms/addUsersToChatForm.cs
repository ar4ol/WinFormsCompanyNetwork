using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class addUsersToChatForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string _name;
        int id_chat;
        accountForm _a;
        List<int> id_s = new List<int>();
        List<int> new_ids = new List<int>();

        public addUsersToChatForm(string name, accountForm a)
        {
            InitializeComponent();
            _name = name;
            _a = a;
        }

        private void addUsersToChatForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; select * from employees e left join card c on e.card = c.id left join department d on c.department = d.id where d.name != 'Уволенные сотрудники';";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dataGridView3.Rows.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if(reader.GetInt32(0) != _a.User.id)
                            {                            
                                dataGridView3.Rows.Add(null, reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                id_s.Add(reader.GetInt32(0));
                            }
                        }

                    }
                }
            }
        }

        private void regDepButton_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; insert into chats (name, id_owner) values ('"+ _name +"', "+_a.User.id+")";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader()) { }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; select top 1 chats.id from chats order by id desc;";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id_chat = reader.GetInt32(0);
                        }
                    }
                }
            }

            for (int i = 0; i < new_ids.Count; i++)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "USE employees; insert into chat_user (id_employee, id_chat) values (" + new_ids[i] +", "+id_chat+")";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                }

            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "USE employees; insert into chat_user(id_employee, id_chat) values(" + _a.User.id + ", "+id_chat+");";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader()) { }
            }
            MessageBox.Show("Чат создан!");
            _a.Show();
            _a.dataGridViewFill();
            this.Hide();
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (e.ColumnIndex == 0 && e.RowIndex == dataGridView3.Rows[i].Index)
                {
                    new_ids.Add(id_s[i]);
                }
            }
        }
    }
}
