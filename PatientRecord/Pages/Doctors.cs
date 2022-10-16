using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Patient_Record.Pages
{
    public partial class Doctors : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Patient Management System";
        public Doctors()
        {
            InitializeComponent();
            loadDoctor();
            cbGender.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Models.Doctor doctor = new Models.Doctor(this);
            doctor.ShowDialog();
        }

        #region method
        public void loadDoctor()
        {
            string sql = "";
            string gender = cbGender.Text;
            if (gender == "Gender (All)")
                sql = "SELECT d.id, name, gender, specialty, phone, email, datejoined, COUNT(distinct V.pid) AS Patient, count(V.did) AS visits FROM tbDoctors AS D LEFT JOIN tbVisits AS V ON D.id=V.did WHERE CONCAT(name, specialty)LIKE '%" + txtSearch.Text + "%' GROUP BY d.id, name, gender, specialty, phone, email, datejoined";
            else sql = "SELECT d.id, name, gender, specialty, phone, email, datejoined, COUNT(distinct V.pid) AS Patient, count(V.did) AS visits FROM tbDoctors AS D LEFT JOIN tbVisits AS V ON D.id=V.did WHERE CONCAT(name, specialty)LIKE '%" + txtSearch.Text + "%' and gender='" + gender + "' GROUP BY d.id, name, gender, specialty, phone, email, datejoined";
            try
            {
                int i = 0;
                dgvDoctor.Rows.Clear();
                cm = new SqlCommand(sql, dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvDoctor.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), DateTime.Parse(dr[6].ToString()).ToShortDateString(), dr[7].ToString(), dr[8].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method

        private void dgvDoctor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvDoctor.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to to sent doctor data to the module 
                Models.Doctor module = new Models.Doctor(this);
                module.loadDoctor(dgvDoctor.Rows[e.RowIndex].Cells[1].Value.ToString());
                module.ShowDialog();
            }
            else if (colName == "Delete") // if you want to delete the record to click the delete icon on the datagridview
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("DELETE FROM tbDoctors WHERE id LIKE'" + dgvDoctor.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.open();
                        cm.ExecuteNonQuery();
                        dbcon.close();
                        MessageBox.Show("Record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadDoctor();
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
            loadDoctor();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDoctor();
        }
    }
}
