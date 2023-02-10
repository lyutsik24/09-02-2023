using MySql.Data.MySqlClient;
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
    public partial class Reg : Form
    {
        public Reg()
        {
            InitializeComponent();
            txtPass1.UseSystemPasswordChar = true;
            txtPass2.UseSystemPasswordChar = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            else if (string.IsNullOrEmpty(txtPass1.Text))
            {
                MessageBox.Show("Введите новый пароль", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            else if (string.IsNullOrEmpty(txtPass2.Text))
            {
                MessageBox.Show("Повторите новый пароль", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            else if (string.Equals(txtPass1.Text, txtPass2.Text) == false)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            else
            {
                using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.AuthConnectionString))
                {
                    try
                    {
                        string query = "INSERT INTO `Auth`.`auth` (`auth_log`, `auth_pwd`) VALUES (@plog, @ppass)";
                        DataTable dt = new DataTable();
                        MySqlCommand cm = new MySqlCommand(query, cn);
                        cm.Parameters.AddWithValue("@plog", txtLogin.Text);
                        cm.Parameters.AddWithValue("@ppass", txtPass1.Text);
                        cn.Open();
                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Аккаунт успешно создан", "Успех", MessageBoxButtons.OK);
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
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Auth mForm = new Auth();
            this.Hide();
            mForm.Show();
        }

        private void Reg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
