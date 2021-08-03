using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CompanyNetwork.Классы
{
    class Card
    {
        public int id;
        public string position;
        public int salary;
        public int premium;
        public int fine;
        public int department;
        public string name_department;

        public Card(int get_id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from [card] where [card].id = (select employees.[card] from employees where employees.id =" + get_id + ");");
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            id = (int)reader.GetValue(0);
                            position = (string)reader.GetValue(1);
                            salary = (int)reader.GetValue(2);
                            department = (int)reader.GetValue(5);
                            if (reader.GetValue(3) == DBNull.Value)
                            {
                                premium = 0;
                            }
                            else
                            {
                                premium = Convert.ToInt32(reader.GetValue(3));
                            }
                            if (reader.GetValue(4) == DBNull.Value)
                            {
                                fine = 0;
                            }
                            else
                            {
                                fine = Convert.ToInt32(reader.GetValue(4));
                            }
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("use employees; select [name] from department where id = " + department);
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            name_department = reader.GetValue(0) + "";
                        }
                    }
                }
                    
            
            }
        }
    }
}
