using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09_02_2023
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            User.UserId = null;
            User.UserLogin = null;
            User.UserRole = null;
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtID.Text = User.UserId.ToString();
            lblLogin.Text = User.UserLogin;
            txtRole.Text = User.UserRole;
            btnOk.Select();
        }
    }
}
