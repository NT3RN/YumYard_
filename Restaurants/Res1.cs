using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using YumYard.Customer;
using YumYard.Customer.Forms;
using YumYard.Rform;
using YumYard.DatabaseAccess;

namespace YumYard.Restaurants
{
    public partial class Res1 : Form
    {
        private string Uemail;
        private string Uid;
        private string restaurantID;
        private string rName;
        public Res1(string userEmail, string userID, string resName, int resID)
        {
            InitializeComponent();
            Uemail = userEmail;
            Uid = userID;
            lblTitle.Text = resName;
            rName = resName;
            restaurantID = Convert.ToString(resID);
            LoadForm(new resWelcome(resName));
        }

        private void NonActiveButton(Button nonActiveButton)
        {
            nonActiveButton.BackColor = Color.Goldenrod;
        }

        private void ActiveButton(Button activeButton)
        {
            activeButton.BackColor = Color.DarkOrange;
        }

        private void LoadForm(Form form)
        {
            if (this.panelRes.Controls.Count > 0)
                this.panelRes.Controls.RemoveAt(0);

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            this.panelRes.Controls.Add(form);
            this.panelRes.Tag = form;
            form.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            string error;

            string themeQuery = "SELECT TOP 1 tPic FROM RestaurantTheme WHERE tPic IS NOT NULL ORDER BY tID DESC";
            var themeResult = DbAccess.GetData(themeQuery, out error);

            byte[] themeImage = null;

            if (themeResult != null && themeResult.Rows.Count > 0 && themeResult.Rows[0]["tPic"] != DBNull.Value)
            {
                themeImage = (byte[])themeResult.Rows[0]["tPic"];
            }
            else
            {
                MessageBox.Show("No valid theme image found in RestaurantTheme table.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            byte[][] restaurantImages = new byte[4][];  

            string restaurantQuery = "SELECT TOP 4 ImageData FROM Restaurant ORDER BY (SELECT NULL)";
            var restaurantResult = DbAccess.GetData(restaurantQuery, out error);

            if (restaurantResult != null && restaurantResult.Rows.Count > 0)
            {
                for (int i = 0; i < restaurantResult.Rows.Count && i < 4; i++)  
                {
                    if (restaurantResult.Rows[i]["ImageData"] != DBNull.Value)
                    {
                        restaurantImages[i] = (byte[])restaurantResult.Rows[i]["ImageData"];
                    }
                    else
                    {
                        restaurantImages[i] = null; 
                    }
                }
            }

            ResturantPicker resturantPicker = new ResturantPicker(Uemail);
            resturantPicker.Show();
            this.Hide();
        }


        private void btnOrderInfo_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Order";
            ActiveButton(btnOrderInfo);
            LoadForm(new r1Order(Uid, restaurantID));
        }
    }
}
