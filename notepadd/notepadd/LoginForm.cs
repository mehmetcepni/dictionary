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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace notepadd
{
    public partial class LoginForm : Form
    {

        private string connectionString = "Server=CEPNI;Database=notepaddDb;Integrated Security=True;";

        public LoginForm()
        {
            InitializeComponent();
            dateLbl.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

       

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username=txtUsername.Text;
            string password=txtPassword.Text;

            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password) )
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi girin!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId, Username FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    GlobalVariables.LoggedInUserId = reader.GetInt32(0); // UserId'yi kaydet
                    GlobalVariables.LoggedInUsername = reader.GetString(1); // Username'i kaydet

                    MessageBox.Show("Giriş başarılı!");
                    this.Hide();
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                    // Kullanıcının adına göre karşılama metni
                    //Label kullaniciLbl = new Label();
                    //kullaniciLbl.AutoSize = true;
                    //kullaniciLbl.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //kullaniciLbl.Location = new System.Drawing.Point(13, 12);
                    //kullaniciLbl.Name = "kullaniciLbl";
                    //kullaniciLbl.Size = new System.Drawing.Size(66, 27);
                    //kullaniciLbl.TabIndex = 6;
                    //kullaniciLbl.Text = "label1";
                    //kullaniciLbl.Text = username + " WORD BOOK";
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                }

                connection.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }


    }
}
