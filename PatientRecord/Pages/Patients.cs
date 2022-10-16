using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Patient_Record.Pages
{
    public partial class Patients : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Patient Management System";
        public string pid;
        public Patients()
        {
            InitializeComponent();
            loadPatients();
            cbGender.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Models.Patient module = new Models.Patient(this);
            module.ShowDialog();
        }

        private void dgvPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvPatients.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to to sent doctor data to the module 
                Models.Patient module = new Models.Patient(this);
                module.loadPatient(dgvPatients.Rows[e.RowIndex].Cells[1].Value.ToString());
                module.ShowDialog();
            }
            else if (colName == "Delete") // if you want to delete the record to click the delete icon on the datagridview
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("DELETE FROM tbPatients WHERE id LIKE'" + dgvPatients.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.open();
                        cm.ExecuteNonQuery();
                        dbcon.close();
                        MessageBox.Show("Record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadPatients();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        #region method
        public void loadPatients()
        {
            string sql = "";
            string gender = cbGender.Text;
            if (gender == "Gender (All)")
                sql = "SELECT P.id, name, gender, phone, email, insurance, policy, COUNT(V.pid) visits FROM tbPatients AS P LEFT JOIN tbVisits AS V ON P.id=v.pid WHERE name LIKE '%" + txtSearch.Text + "%' GROUP BY p.id, name,gender, phone, email, insurance, policy";
            else sql = "SELECT P.id, name, gender, phone, email, insurance, policy, COUNT(V.pid) visits FROM tbPatients AS P LEFT JOIN tbVisits AS V ON P.id=v.pid WHERE name LIKE '%" + txtSearch.Text + "%'and gender='" + gender + "' GROUP BY p.id, name,gender, phone, email, insurance, policy";

            try
            {
                int i = 0;
                dgvPatients.Rows.Clear();
                cm = new SqlCommand(sql, dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvPatients.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPatients();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadPatients();
        }

        private void btnAddVisit_Click(object sender, EventArgs e)
        {
            Models.visit module = new Models.visit();
            module.lblId.Text = pid;
            module.ShowDialog();
            loadPatients();
        }

        private void btnViewVisit_Click(object sender, EventArgs e)
        {
            Models.Patient_visits module = new Models.Patient_visits(this);
            module.ShowDialog();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAddVisit.Enabled = true;
            btnViewVisit.Enabled = true;
            pid = dgvPatients.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
    }
}
