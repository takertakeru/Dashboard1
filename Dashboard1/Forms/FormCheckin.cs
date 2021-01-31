using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Dashboard1
{
    public partial class FormCheckin : Form
    {
        public FormCheckin()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=TAKERU\SQLEXPRESS;Initial Catalog=KSPool;Integrated Security=True");
        public int cottageID;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormCheckin_Load(object sender, EventArgs e)
        {
            GetCheckInRecord();
        }

        private void GetCheckInRecord()
        {
            
            SqlCommand cmd = new SqlCommand("Select * from checkInTable", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            CheckInRecordDataGridView.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(IsValid())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO checkInTable VALUES (@customerName, @customerContactNumber)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@customerContactNumber", txtMobile.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Customer is successfully saved in the database", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetCheckInRecord();
                ResetFormControls();

            }
        }

        private bool IsValid()
        {
            if(txtCustomerName.Text == string.Empty)
            {
                MessageBox.Show("Customer Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            cottageID = 0;
            txtCustomerName.Clear();
            txtMobile.Clear();

            txtCustomerName.Focus();
        }

        private void CheckInRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cottageID = Convert.ToInt32(CheckInRecordDataGridView.SelectedRows[0].Cells[0].Value);
            txtCustomerName.Text = CheckInRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtMobile.Text = CheckInRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(cottageID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE checkInTable SET customerName = @customerName, customerContactNumber = @customerContactNumber WHERE cottageID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@customerContactNumber", txtMobile.Text);
                cmd.Parameters.AddWithValue("ID", this.cottageID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Information is updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetCheckInRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please select a customer to update his information", "Select Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(cottageID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM checkInTable WHERE cottageID = @ID", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("ID", this.cottageID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer is deleted from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetCheckInRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please select a customer to delete", "Select Customer to Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
