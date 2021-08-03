using System;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CompanyNetwork
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new authorizationForm());
        }
    }
}
