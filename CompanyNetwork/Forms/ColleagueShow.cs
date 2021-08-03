using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyNetwork.Forms
{
    public partial class ColleagueShow : Form
    {
        User user;
        public ColleagueShow(int id)
        {
            InitializeComponent();
            user = new User(id);
            user.Fill_User();
            Fill();
        }

        public void Fill()
        {
            pictureBox1.Image = Image.FromFile(user.foto);
            nameBox.Text = user.name;
            firstNameBox.Text = user.surname;
            textBox1.Text = user.dateOfBirhth;
            passportNumbBox.Text = user.phone_number;
            textBox2.Text = user.card.position;
            loginBox.Text = user.card.name_department;
        }
      
    }
}
