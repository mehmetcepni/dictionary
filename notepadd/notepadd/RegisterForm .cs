using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notepadd
{
    public partial class RegisterForm : Form
    {
        private string connectionString = "Server=CEPNI;Database=notepaddDb;Integrated Security=True;";
        public RegisterForm()
        {
            InitializeComponent();
            dateLbll.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }



        private void btnRegister_Click(object sender, EventArgs e)
        {
            string newUsername = txtNewUsername.Text;
            string newPassword = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre girin!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", newUsername);
                command.Parameters.AddWithValue("@Password", newPassword);

                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Kayıt başarılı!");
                this.Close(); //kayıt ol formunu kapat
            }
        }

       
    }
}
