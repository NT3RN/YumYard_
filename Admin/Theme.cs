using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YumYard.DatabaseAccess;
using static YumYard.Admin.Dashboard;

namespace YumYard.Admin
{
    public partial class Theme : Form
    {
        private string selectedImagePath = "";

        public Theme()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select a Background Image"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                picTheme.Image = Image.FromFile(selectedImagePath);
                lblThemeC.Visible = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (picTheme.Image == null)
                {
                    MessageBox.Show("Please select an image before updating.");
                    return;
                }

                byte[] imageBytes = ImageToByteArray(picTheme.Image);

                using (SqlConnection conn = new SqlConnection(DbAccess.ConnectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO RestaurantTheme (tPic) OUTPUT INSERTED.tID VALUES (@Image)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Image", imageBytes);

                        int newThemeID = (int)cmd.ExecuteScalar();
                        MessageBox.Show($"✅ Theme image saved successfully with ID: {newThemeID}");
                    }
                }

                
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("❌ Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error saving data: " + ex.Message);
            }
            NavigationHelper.OpenDashboard(this);
        }


        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            NavigationHelper.OpenDashboard(this);
        }

        private void btnUserInfo_Click(object sender, EventArgs e)
        {
            NavigationHelper.OpenUserInfo(this);
        }

        private void btnRestaurantManagement_Click(object sender, EventArgs e)
        {
            NavigationHelper.OpenRestaurantManagement(this);
        }

        private void btnTheme_Click(object sender, EventArgs e)
        {
            NavigationHelper.OpenTheme(this);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            NavigationHelper.OpenLogout(this);
        }

        private void Theme_Load(object sender, EventArgs e)
        {

        }
    }
}
