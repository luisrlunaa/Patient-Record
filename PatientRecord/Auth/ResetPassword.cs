using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Patient_Record.Auth
{
    public partial class ResetPassword : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Patients Management System";
        public string email;

        public ResetPassword()
        {
            InitializeComponent();
            loadClinic();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text != txtComNewPass.Text)
            {
                MessageBox.Show("The password you typed do not match. Type the password for this account in both text boxes.", "Add User Wizard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    if (MessageBox.Show("Reset password?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        dbcon.executeQuery("UPDATE tbUsers SET password = '" + txtNewPass.Text + "'WHERE email = '" + txtEmail.Text + "'");
                        MessageBox.Show("Password has been successfully reset", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
            Application.Restart();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
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
        private void ResetPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
