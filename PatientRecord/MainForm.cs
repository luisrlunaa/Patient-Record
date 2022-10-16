using MaterialSkin.Controls;
using System;
using System.Windows.Forms;


namespace Patient_Record
{
    public partial class MainForm : MaterialForm
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Restart();
        }



        // create a function any form to the panelChild on the mainform

        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnPatient.Width;
            panelSlide.Left = btnPatient.Left;
            openChildForm(new Pages.Patients());
        }



        private void btnVisit_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnVisit.Width;
            panelSlide.Left = btnVisit.Left;
            openChildForm(new Pages.Visits());
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnDoctors.Width;
            panelSlide.Left = btnDoctors.Left;
            openChildForm(new Pages.Doctors());
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnUsers.Width;
            panelSlide.Left = btnUsers.Left;
            openChildForm(new Pages.Users());
        }

        private void btnClinic_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnClinic.Width;
            panelSlide.Left = btnClinic.Left;
            Models.Clinic module = new Models.Clinic();
            module.ShowDialog();
        }

        private void btnDash_Click(object sender, EventArgs e)
        {
            panelSlide.Width = btnDash.Width;
            panelSlide.Left = btnDash.Left;
            openChildForm(new Pages.Dashboards());

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            btnDash.PerformClick();

        }
    }
}
