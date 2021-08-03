using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class delDepartmentForm : Form
    {
        private readonly string connectionString  = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public delDepartmentForm()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string request = "use employees; select [name] from department";
                SqlCommand command = new SqlCommand(request);
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if(reader.GetString(0) != "Уволенные сотрудники")
                            comboBox1.Items.Add(reader.GetString(0));
                    }
                }
            }
            
        }

        private void regDepButton_Click(object sender, System.EventArgs e)
        {
            if(comboBox2.Text == "Уволить")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "use employees; update [card] set department = (select department.id from department where department.name = 'Уволенные сотрудники'), position = 'Уволен', salary = 0, premium = 0, fine = 0 from card where department in (select department.id from department where department.[name] = '" + comboBox1.Text + "');";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                }
                               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "use employees; delete from department where [name] = '" + comboBox1.Text + "';";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                }

            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string request = "use employees; update[card] set department = (select department.id from department where department.[name] = '" + comboBox3.Text + "') where department in (select department.id from department where department.[name] = '" + comboBox1.Text + "'); ;";
                    SqlCommand command = new SqlCommand(request);
                    connection.Open();
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "use employees; delete from department where [name] = '" + comboBox1.Text + "';";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                }
            }
            MessageBox.Show("Удаление успешно!");
            adminForm a = new adminForm();
            a.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(comboBox2.Text != "Уволить")
            {
                comboBox3.Visible = true;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string request = "use employees; select [name] from department";
                    SqlCommand command = new SqlCommand(request);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) != "Уволенные сотрудники" && reader.GetString(0) != comboBox1.Text)
                                comboBox3.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            else
            {
                comboBox3.Visible = false;
            }
        }

        private void backButton_Click(object sender, System.EventArgs e)
        {
            adminForm a = new adminForm();
            a.Show();
            this.Hide();
        }
    }
}
