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
    public partial class Reset : Form
    {

        public Reset()
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
                        string query = "UPDATE `Auth`.`auth` SET `auth_pwd` = @password WHERE (`auth_id` = @id);";
                        DataTable dt = new DataTable();
                        MySqlCommand cm = new MySqlCommand(query, cn);
                        cm.Parameters.AddWithValue("@id", User.UserId);
                        cm.Parameters.AddWithValue("@password", txtPass1.Text);
                        cn.Open();
                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Пароль успешно изменён", "Успех", MessageBoxButtons.OK);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLogin.Clear();
            txtPass1.Clear();
            txtPass2.Clear();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Auth mForm = new Auth();
            this.Hide();
            mForm.Show();
        }

        /// <summary>
        /// Проверка логина в БД
        /// </summary>
        private void txtLogin_Leave(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.AuthConnectionString))
            {
                try
                {
                    string query = "SELECT * FROM `Auth`.`auth` WHERE `auth_log` = @login";
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    da.SelectCommand.Parameters.AddWithValue("@login", txtLogin.Text);
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        User.UserId = Convert.ToInt32(dt.Rows[0]["auth_id"]);
                        txtPass1.ReadOnly = false;
                        txtPass2.ReadOnly = false;
                    }
                    else
                    {
                        MessageBox.Show("Проверьте логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPass1.ReadOnly = true;
                        txtPass2.ReadOnly = true;
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

        private void Reset_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
