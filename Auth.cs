using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _09_02_2023
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.AuthConnectionString))
            {
                try
                {
                    string query = @"SELECT 
	                                    `auth_id` AS `№`,
	                                    `auth_log` AS `Логин`,
	                                    `role_name` AS `Роль`
                                     FROM `Auth`.`auth`
	                                 JOIN `Auth`.`rols` ON `Auth`.`auth`.`auth_role` = `Auth`.`rols`.`role_id`
                                     WHERE `auth_log` = @user AND `auth_pwd` = @password";
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    da.SelectCommand.Parameters.AddWithValue("@user", txtLogin.Text);
                    da.SelectCommand.Parameters.AddWithValue("@password", txtPassword.Text);
                    da.Fill(dt);

                    if (txtLogin.Text == "")
                    {
                        MessageBox.Show("Вы не ввели логин. Перепроверьте вводимые данные!");
                    }
                    else if (txtPassword.Text == "")
                    {
                        MessageBox.Show("Вы не ввели пароль. Перепроверьте вводимые данные!"); 
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        User.UserId = Convert.ToInt32(dt.Rows[0]["№"]);
                        User.UserLogin = dt.Rows[0]["Логин"].ToString();
                        User.UserRole = dt.Rows[0]["Роль"].ToString();
                        Main mainForm = new Main();
                        this.Hide();
                        mainForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Перепроверьте вводимые данные!", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        private void lblReset_DoubleClick(object sender, EventArgs e)
        {
            Reset mForm = new Reset();
            mForm.ShowDialog();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            Reg mForm = new Reg();
            this.Hide();
            mForm.Show();
        }
    }
}
