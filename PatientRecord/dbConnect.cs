using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Patient_Record
{
    //to get connection string between application and database   
    class dbConnect
    {
        SqlCommand cm = new SqlCommand();
        private SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=Patient;Integrated Security=True; encrypt=false; Connect Timeout=30");
        public SqlConnection connect()
        {
            return cn;
        }

        public void open()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
        }

        public void close()
        {
            if (cn.State == System.Data.ConnectionState.Open)
                cn.Close();
        }

        public DataTable getTable(string str)
        {
            cm = new SqlCommand(str, connect());
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public void executeQuery(string sql)
        {
            try
            {
                open();
                cm = new SqlCommand(sql, connect());
                cm.ExecuteNonQuery();
                close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Patient Record Management System");
            }
        }

        public double ExtractData(string sql)
        {
            cn.Open();
            cm = new SqlCommand(sql, cn);
            int data = int.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return data;

        }
    }
}
