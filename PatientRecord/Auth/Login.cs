﻿using MaterialSkin.Controls;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Patient_Record.Auth
{
    public partial class Login : MaterialForm
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Patients Management System";
        public Login()
        {
            InitializeComponent();
            loadClinic();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("SELECT name FROM tbUsers WHERE email ='" + txtEmail.Text + "' AND password ='" + txtPassword.Text + "'", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    MessageBox.Show("WELCOME " + dr["name"].ToString() + " | ", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    MainForm main = new MainForm();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid email or password", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                dbcon.close();
                dr.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtEmail.Clear();
            txtPassword.Clear();
        }

        bool passOn = true;
        private void txtPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (passOn)
            {
                txtPassword.TrailingIcon = Properties.Resources.eye_20px;
                txtPassword.Password = false;
                passOn = false;
            }
            else
            {
                txtPassword.TrailingIcon = Properties.Resources.hide_20px;
                txtPassword.Password = true;
                passOn = true;
            }

        }

        public void loadClinic()
        {
            try
            {
                dbcon.open();
                cm = new SqlCommand("SELECT * FROM tbClinic", dbcon.connect());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    byte[] img = dr[0] as byte[];
                    MemoryStream ms = new MemoryStream(img);

                    picLogo.Image = Image.FromStream(ms);
                    lblName.Text = dr[1].ToString().ToUpper();
                }
                dr.Close();
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }

        }
    }
}
