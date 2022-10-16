﻿using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Patient_Record.Models
{
    public partial class visit : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Patients Management System";

        bool check = false;
        bool update = false;
        public visit()
        {
            InitializeComponent();
            doctorList();
        }


        public void doctorList()
        {
            cbDoctor.DataSource = dbcon.getTable("SELECT * FROM tbDoctors");
            cbDoctor.DisplayMember = "name";
            cbDoctor.ValueMember = "id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (update)
                    {
                        if (MessageBox.Show("Are you sure you want to update this visit info?", "Info Updating", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            cm = new SqlCommand("UPDATE tbVisits SET date=@date, did=@did, reason=@reason, weight=@weight, systolic=@systolic, diastolic=@diastolic, notes=@notes WHERE id=" + lblId.Text + "", dbcon.connect());
                            cm.Parameters.AddWithValue("@date", dtDate.Value);
                            cm.Parameters.AddWithValue("@did", cbDoctor.SelectedValue);
                            cm.Parameters.AddWithValue("@reason", txtReason.Text);
                            cm.Parameters.AddWithValue("@weight", txtWeight.Text);
                            cm.Parameters.AddWithValue("@systolic", txtSystolic.Text);
                            cm.Parameters.AddWithValue("@diastolic", txtDiastolic.Text);
                            cm.Parameters.AddWithValue("@notes", txtNote.Text);

                            dbcon.open();// to open connection
                            cm.ExecuteNonQuery();
                            dbcon.close();// to close connection
                            MessageBox.Show("Visit info has been successfully updated!", title);

                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to register this Visit?", "Visit Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            cm = new SqlCommand("INSERT INTO tbVisits (date, pid, did, reason, weight, temperature, systolic, diastolic, notes, createAt) VALUES (@date, @pid, @did, @reason, @weight, @temperature, @systolic, @diastolic, @notes, @createAt)", dbcon.connect());
                            cm.Parameters.AddWithValue("@date", dtDate.Value);
                            cm.Parameters.AddWithValue("@pid", lblId.Text);
                            cm.Parameters.AddWithValue("@did", cbDoctor.SelectedValue);
                            cm.Parameters.AddWithValue("@reason", txtReason.Text);
                            cm.Parameters.AddWithValue("@weight", txtWeight.Text);
                            cm.Parameters.AddWithValue("@temperature", txtTemp.Text);
                            cm.Parameters.AddWithValue("@systolic", txtSystolic.Text);
                            cm.Parameters.AddWithValue("@diastolic", txtDiastolic.Text);
                            cm.Parameters.AddWithValue("@notes", txtNote.Text);
                            cm.Parameters.AddWithValue("@createAt", dtCreate.Value);


                            dbcon.open();// to open connection
                            cm.ExecuteNonQuery();
                            dbcon.close();// to close connection
                            MessageBox.Show("Visit info has been successfully registered!", title);
                            Clear();//to clear data field, after data inserted into the database                            


                        }
                    }
                    this.Dispose();
                    //user.loadUser();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            update = false;
        }

        #region method
        public void Clear()
        {
            txtDiastolic.Clear();
            txtNote.Clear();
            txtReason.Clear();
            txtSystolic.Clear();
            txtTemp.Clear();
            txtWeight.Clear();

            cbDoctor.SelectedIndex = 0;
            dtDate.Value = DateTime.Now;
        }

        public void checkField()
        {
            if (txtReason.Text == "" || txtTemp.Text == "" || txtWeight.Text == "" || txtSystolic.Text == "")
            {
                MessageBox.Show("Required data Field!", "Warning");
                return; // return to the data field and form
            }

            check = true;
        }

        public void loadVisits(string str)
        {
            try
            {
                cm = new SqlCommand("SELECT * FROM tbVisits WHERE id=" + str + "", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {

                    lblId.Text = str;
                    dtDate.Text = dr[1].ToString();
                    cbDoctor.SelectedValue = dr[3].ToString();
                    txtReason.Text = dr[4].ToString();
                    txtWeight.Text = dr[5].ToString();
                    txtTemp.Text = dr[6].ToString();
                    txtSystolic.Text = dr[7].ToString();
                    txtDiastolic.Text = dr[8].ToString();
                    txtNote.Text = dr[9].ToString();
                    dtCreate.Enabled = false;
                    update = true;
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method
    }
}
