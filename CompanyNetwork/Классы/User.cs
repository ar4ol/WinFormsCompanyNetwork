using CompanyNetwork.Классы;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CompanyNetwork
{
    class User
    {
        public int id;
        public string name;
        public string surname;
        public string dateOfBirhth;
        public string email;
        public string phone_number;
        public string login;
        public string password;
        public string foto;
        public string passport;
        public string keyword;
        public Card card;

        public User(int get_id)
        {
            id = get_id;
        }
        
        public void Fill_User()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
                            var my_id = reader.GetValue(0);
                            if ((int)my_id == id)
                            {
                                name = (string)reader.GetValue(1);
                                surname = (string)reader.GetValue(2);
                                dateOfBirhth = (string)reader.GetValue(3);
                                email = (string)reader.GetValue(4);
                                phone_number = (string)reader.GetValue(5);
                                login = (string)reader.GetValue(8);
                                password = (string)reader.GetValue(9);
                                foto = (string)reader.GetValue(6);
                                passport = (string)reader.GetValue(7);
                            }
                        }
                    }
                }
                    
                card = new Card(id);
            }
                
        }
    }
}
