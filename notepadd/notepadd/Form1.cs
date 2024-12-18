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
    public partial class Form1 : Form
    {

        private string connectionString = "Server=CEPNI;Database=notepaddDb;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            DataGridViewCheckBoxColumn checkBoxColumn= new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Ezberlendi";
            checkBoxColumn.Name = "Ezberlendi";
            dgvWords.Columns.Insert(0,checkBoxColumn);
        }

        // Not Sil Butonu
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            string englishWord = txtEnglish.Text;

            if (string.IsNullOrWhiteSpace(englishWord))
            {
                MessageBox.Show("Lütfen silmek istediğiniz İngilizce kelimeyi girin!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Words WHERE EnglishWord = @EnglishWord";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EnglishWord", englishWord);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kelime başarıyla silindi!");
                }
                else
                {
                    MessageBox.Show("Belirtilen kelime bulunamadı.");
                }
            }

            txtEnglish.Clear();
        }
        //Notun Yazıldığı Tarih
        private void btnShowDate_Click(object sender, EventArgs e)
        {
            string englishWord = txtEnglish.Text;

            if (string.IsNullOrWhiteSpace(englishWord))
            {
                MessageBox.Show("Lütfen tarihini görmek istediğiniz İngilizce kelimeyi girin!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CreatedAt FROM Dictionary WHERE EnglishWord = @EnglishWord";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EnglishWord", englishWord);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateTime createdAt = reader.GetDateTime(0);
                    MessageBox.Show($"Kelimenin eklendiği tarih: {createdAt}");
                }
                else
                {
                    MessageBox.Show("Belirtilen kelime bulunamadı.");
                }

                connection.Close();

            }
        }

        //Notu Kaydet
        private void btnSave_Click(object sender, EventArgs e)
        {
            //kelimeEkle();
            string englishWord = txtEnglish.Text;
            string turkishWord = txtTurkish.Text;
            int userId = GlobalVariables.LoggedInUserId;


            if (string.IsNullOrWhiteSpace(englishWord) || string.IsNullOrWhiteSpace(turkishWord))
            {
                MessageBox.Show("Lütfen her iki alanı da doldurun!");
                return;
            }
            else if( englishWord.Length>60 || turkishWord.Length > 60)
            {
                MessageBox.Show("Kelimeniz veya notunuz 60 harften uzun olamaz");
                return;
            }
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //kelime daha önce eklenmişmi kontrolü
                string checkQuery = "SELECT COUNT(*) FROM Words WHERE EnglishWord = @EnglishWord AND TurkishWord = @TurkishWord";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@EnglishWord", englishWord);
                checkCommand.Parameters.AddWithValue("@TurkishWord", turkishWord);

                int count = (int)checkCommand.ExecuteScalar(); // Mevcut kayıt sayısını döndürür

                if (count > 0)
                {
                    MessageBox.Show("Bu kelime zaten eklenmiş!");
                }
                else
                {
                 
                    string query = "INSERT INTO Words (EnglishWord, TurkishWord, UserId) VALUES (@EnglishWord, @TurkishWord, @UserId)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EnglishWord", englishWord);
                    command.Parameters.AddWithValue("@TurkishWord", turkishWord);
                    command.Parameters.AddWithValue("@UserId", userId);

                   
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Kelime başarıyla eklendi!");
                }
              
            }

            txtEnglish.Clear();
            txtTurkish.Clear();
        }

    //    public void kelimeEkle()
    //    {
    //        using (SqlConnection connection = new SqlConnection(connectionString))
    //        {
    //            connection.Open();
    //            List<(string English, string Turkish)> defaultWords = new List<(string English, string Turkish)>
    //        {
    //             ("About", "hakkında"),
    //("All", "bütün, tüm"),
    //("Also", "ayrıca, -da"),
    //("And", "ve"),
    //("Be", "olmak"),
    //("Because", "çünkü"),
    //("But", "fakat, ama"),
    //("By", "tarafından, yanında, ile"),
    //("Can", "-ebilmek"),
    //("Come", "gelmek"),
    //("Continue", "devam etmek"),
    //("Could", "-ebilir"),
    //("Day", "gün"),
    //("Do", "yapmak"),
    //("Even", "bile"),
    //("Find", "bulmak"),
    //("First", "ilk"),
    //("For", "için, çünkü"),
    //("From", "-den, -dan"),
    //("Get", "elde etmek, edinmek, ulaşmak"),
    //("Give", "vermek"),
    //("Go", "gitmek"),
    //("Gave", "sahip olmak"),
    //("He", "o (erkek)"),
    //("Her", "onun, onu (kadın)"),
    //("Here", "burada"),
    //("Him", "onun (erkek)"),
    //("His", "onun (erkek)"),
    //("How", "nasıl"),
    //("I", "ben"),
    //("If", "eğer"),
    //("In", "içinde"),
    //("Into", "içine"),
    //("It", "o, onu (cansız varlık, hayvan)"),
    //("Its", "onun (cansız varlık, hayvan)"),
    //("Just", "henüz, sadece"),
    //("Know", "bilmek"),
    //("Later", "daha sonra"),
    //("Like", "gibi, hoşlanmak"),
    //("Look", "bakmak, görünmek"),
    //("Make", "yapmak"),
    //("Man", "adam"),
    //("Many", "çoğu"),
    //("Me", "beni, bana"),
    //("More", "daha çok"),
    //("My", "benim"),
    //("New", "yeni"),
    //("No", "hayır"),
    //("Not", "değil, olumsuzluk eki"),
    //("Now", "şimdi"),
    //("Of", "-in, -ın"),
    //("On", "üzerinde"),
    //("One", "bir"),
    //("Only", "sadece"),
    //("Or", "veya, ya da"),
    //("Other", "diğer"),
    //("Our", "bizim"),
    //("Out", "dışarı, dışarda"),
    //("People", "insanlar"),
    //("Say", "söylemek"),
    //("See", "görmek"),
    //("Set", "ayarlamak"),
    //("She", "o (kadın)"),
    //("So", "böylece, öyleyse"),
    //("Some", "bazı, biraz"),
    //("Take", "almak, götürmek, çekmek"),
    //("Tell", "söylemek"),
    //("Than", "-e göre, kıyasla"),
    //("That", "o (uzaktaki)"),
    //("The", "belli bir objeyi/yeri/kişiyi tanımlamak için kullanılır"),
    //("Their", "onların"),
    //("Them", "onları, onlara"),
    //("Then", "o zaman, ondan sonra"),
    //("There", "orada"),
    //("These", "bunlar (yakın)"),
    //("They", "onlar"),
    //("Thing", "şey"),
    //("Think", "düşünmek"),
    //("This", "bu (yakın)"),
    //("Those", "onlar (uzak)"),
    //("Time", "zaman, süre"),
    //("To", "-e, -a"),
    //("Two", "iki"),
    //("Up", "yukarı"),
    //("Use", "kullanmak"),
    //("Very", "çok"),
    //("Want", "istemek"),
    //("Way", "yol"),
    //("We", "biz"),
    //("Well", "iyi"),
    //("What", "ne"),
    //("When", "ne zaman"),
    //("Which", "hangi"),
    //("Who", "kim"),
    //("Will", "-ecek, -acak"),
    //("With", "ile, birlikte"),
    //("Would", "-ecekti, -erdi"),
    //("Year", "yıl"),
    //("You", "sen"),
    //("Your", "senin")
    //        };

    //            foreach (var word in defaultWords)
    //            {
    //                string query = "INSERT INTO Words (UserId, EnglishWord, TurkishWord) VALUES (@UserId, @EnglishWord, @TurkishWord)";
    //                using (SqlCommand command = new SqlCommand(query, connection))
    //                {
    //                    command.Parameters.AddWithValue("@EnglishWord", word.English);
    //                    command.Parameters.AddWithValue("@TurkishWord", word.Turkish);

    //                    command.ExecuteNonQuery();
    //                }
    //            }

    //        }

    //    }



        private void btnListAll_Click(object sender, EventArgs e)
        {

            int userId = GlobalVariables.LoggedInUserId;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT EnglishWord AS 'İngilizce Kelime', TurkishWord AS 'Türkçe Karşılık' " +
                               "FROM Words WHERE UserId = @UserId";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@UserId", userId);

                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();

                dgvWords.DataSource = dataTable;
            }
        }

        private void dgvWords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // DataGridView'den tıklanan satırı al
                DataGridViewRow row = dgvWords.Rows[e.RowIndex];

                // Satırdaki İngilizce ve Türkçe kelimeleri TextBox'lara aktar
                txtEnglish.Text = row.Cells["İngilizce Kelime"].Value.ToString();
                txtTurkish.Text = row.Cells["Türkçe Karşılık"].Value.ToString();
            }

            if (e.ColumnIndex == dgvWords.Columns["Ezberlendi"].Index && e.RowIndex >= 0)
            {
                // Satırdaki CheckBox'ın durumunu kontrol et
                bool isChecked = Convert.ToBoolean(dgvWords.Rows[e.RowIndex].Cells["Ezberlendi"].Value);

                // Tıklanan satırın checkbox'ını tersine çevir (işaretle/işaretten çıkar)
                dgvWords.Rows[e.RowIndex].Cells["Ezberlendi"].Value=!isChecked;

                // Burada kelimeyi işaretleyip işaretlemediğini veritabanına kaydet
                string englishWord = dgvWords.Rows[e.RowIndex].Cells["İngilizce Kelime"].Value.ToString();
                string turkishWord = dgvWords.Rows[e.RowIndex].Cells["Türkçe Karşılık"].Value.ToString();
                bool isEzberlendi = !isChecked;

                // Veritabanına güncelleme yapabilirsiniz (öneri)
                UpdateWordStatus(englishWord, turkishWord, isEzberlendi);
            }
        }

        private void UpdateWordStatus(string englishWord, string turkishWord, bool isEzberlendi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Words SET IsEzberlendi = @IsEzberlendi WHERE EnglishWord = @EnglishWord AND TurkishWord = @TurkishWord";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EnglishWord", englishWord);
                command.Parameters.AddWithValue("@TurkishWord", turkishWord);
                command.Parameters.AddWithValue("@IsEzberlendi", isEzberlendi);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
