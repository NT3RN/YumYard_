

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YumYard.DatabaseAccess; 

namespace YumYard.Admin
{
    public partial class UserInfo : Form
    {
        public UserInfo()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadUserData();
        }

        public void LoadUserData()
        {
            try
            {
                string query = "SELECT C_ID, C_Name, C_Password, C_Email, C_Gender FROM Customer"; 
                string error;
                DataTable dt = DbAccess.GetData(query, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show("Error loading user data: " + error);
                    return;
                }

                dbdTest.DataSource = dt;  

                if (dbdTest.Columns["C_ID"] != null)
                {
                    dbdTest.Columns["C_ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }





        private void button2_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenDashboard(this);
        }

        public void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                string error;

                string getUserIdsQuery = "SELECT C_ID FROM Customer";
                DataTable dbUsers = DbAccess.GetData(getUserIdsQuery, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show("Error fetching user IDs: " + error);
                    return;
                }

                HashSet<int> dbUserIds = new HashSet<int>();
                foreach (DataRow row in dbUsers.Rows)
                {
                    dbUserIds.Add(Convert.ToInt32(row["C_ID"]));
                }

                HashSet<int> gridUserIds = new HashSet<int>();
                foreach (DataGridViewRow row in dbdTest.Rows)
                {
                    if (!row.IsNewRow && row.Cells["C_ID"].Value != null && row.Cells["C_ID"].Value != DBNull.Value)
                    {
                        gridUserIds.Add(Convert.ToInt32(row.Cells["C_ID"].Value));
                    }
                }

                foreach (int userId in dbUserIds)
                {
                    if (!gridUserIds.Contains(userId))
                    {
                        string deleteQuery = $"DELETE FROM Customer WHERE C_ID = {userId}";
                        DbAccess.ExecuteQuery(deleteQuery, out error);
                        if (!string.IsNullOrEmpty(error))
                        {
                            MessageBox.Show("Error deleting user: " + error);
                            return;
                        }
                    }
                }

                foreach (DataGridViewRow row in dbdTest.Rows)
                {
                    if (!row.IsNewRow) 
                    {
                        object idValue = row.Cells["C_ID"].Value;
                        int userId = (idValue == null || idValue == DBNull.Value || idValue.ToString() == "") ? 0 : Convert.ToInt32(idValue);

                        string name = row.Cells["C_Name"].Value?.ToString();
                        string password = row.Cells["C_Password"].Value?.ToString();
                        string email = row.Cells["C_Email"].Value?.ToString();
                        string gender = row.Cells["C_Gender"].Value?.ToString();

                        if (userId > 0)
                        {
                            string updateQuery = $"UPDATE Customer SET C_Name = '{name}', C_Password = '{password}', C_Email = '{email}', C_Gender = '{gender}' WHERE C_ID = {userId}";
                            DbAccess.ExecuteQuery(updateQuery, out error);
                        }
                        else
                        {
                            string insertQuery = $"INSERT INTO Customer (C_Name, C_Password, C_Email, C_Gender) VALUES ('{name}', '{password}', '{email}', '{gender}')";
                            DbAccess.ExecuteQuery(insertQuery, out error);
                        }

                        if (!string.IsNullOrEmpty(error))
                        {
                            MessageBox.Show("Error saving data: " + error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Data saved successfully!");
                LoadUserData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        public void LoadUserDataPublic()
        {
            LoadUserData(); 
        }






        private void button4_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenOrderHistory(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenRestaurantManagement(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenTheme(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenVoucher(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard.NavigationHelper.OpenLogout(this);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dbdTest_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UserInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
