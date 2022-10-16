using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Patient_Record.Pages
{
    public partial class Users : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        private string email;
        string title = "Patient Management System";
        public Users()
        {
            InitializeComponent();
            loadUser();
            cbGender.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Models.User module = new Models.User(this);
            module.ShowDialog();
        }

        #region method
        public void loadUser()
        {
            string sql = "";
            string gender = cbGender.Text;
            if (gender == "Gender (All)")
                sql = "SELECT id, name, gender, phone, email FROM tbUsers WHERE name LIKE '%" + txtSearch.Text + "%'";
            else sql = "SELECT id, name, gender, phone, email FROM tbUsers WHERE name LIKE '%" + txtSearch.Text + "%'and gender='" + gender + "'";
            try
            {
                int i = 0;
                dgvUser.Rows.Clear();
                cm = new SqlCommand(sql, dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to to sent doctor data to the module 
                Models.User module = new Models.User(this);
                module.loadUsers(dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString());
                module.ShowDialog();
            }
            else if (colName == "Delete") // if you want to delete the record to click the delete icon on the datagridview
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("DELETE FROM tbUsers WHERE id LIKE'" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.open();
                        cm.ExecuteNonQuery();
                        dbcon.close();
                        MessageBox.Show("Record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadUser();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadUser();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadUser();
        }

        private void btnPassChange_Click(object sender, EventArgs e)
        {
            Auth.ResetPassword module = new Auth.ResetPassword();
            module.txtEmail.Text = email;
            module.ShowDialog();
            this.Dispose();
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnPassChange.Visible = true;
            email = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
        }


        private void dgvUser_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            btnPassChange.Visible = false;
        }
    }


}
