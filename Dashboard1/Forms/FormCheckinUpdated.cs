using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard1.Forms
{
    public partial class FormCheckinUpdated : Form
    {
        public FormCheckinUpdated()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=TAKERU\SQLEXPRESS;Initial Catalog=KSPool;Integrated Security=True");

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(IsValid())
            {
                SqlConnection con = new SqlConnection(@"Data Source=TAKERU\SQLEXPRESS;Initial Catalog=KSPool;Integrated Security=True");
                con.Open();
                SqlCommand command = new SqlCommand("insert into KS_checkInTable values('" + int.Parse(textBoxCottageID.Text) + "', '" + textBoxCustomerName.Text + "', '" + textBoxContactNumber.Text + "', '" + comboBoxCottageType.Text + "', '" + textBoxAdult.Text + "', '" + textBoxChild.Text + "', getdate(), getdate())", con);
                command.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Customer is successfully saved in the database", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                BindData();
                ResetFormControls();
            }
        }

        private bool IsValid()
        {
            if(textBoxCustomerName.Text == string.Empty)
            {
                MessageBox.Show("Customer Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBoxCottageID.Text == string.Empty)
            {
                MessageBox.Show("Cottage ID is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBoxContactNumber.Text == string.Empty)
            {
                MessageBox.Show("Contact Number is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBoxAdult.Text == string.Empty)
            {
                MessageBox.Show("Number of Adult is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBoxChild.Text == string.Empty)
            {
                MessageBox.Show("Number of Child is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(comboBoxCottageType.Text == string.Empty)
            {
                MessageBox.Show("Cottage Type is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }



        void BindData()
        {
            SqlCommand command = new SqlCommand("select * from KS_checkInTable", con);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dataGridCheckInList.DataSource = dt;
        }

        private void FormCheckinUpdated_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxCottageID.Text != "")
            {
                con.Open();
                SqlCommand command = new SqlCommand("update KS_checkInTable set CustomerName = '" + textBoxCustomerName.Text + "', ContactNumber = '" + textBoxContactNumber.Text + "', CottageType = '" + comboBoxCottageType.Text + "', Adult = '" + textBoxAdult.Text + "', Child = '" + textBoxChild.Text + "', UpdateDate = '" + DateTime.Parse(dateTimePicker1.Text) + "' where CottageID = '" + int.Parse(textBoxCottageID.Text) + "'", con);
                command.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Information is updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                BindData();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please select a customer to update his information", "Select Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(textBoxCottageID.Text != "")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("Delete KS_checkInTable where CottageID = '" + int.Parse(textBoxCottageID.Text) + "'", con);
                    command.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Customer Information is deleted from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindData();
                    ResetFormControls();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer information to delete", "Select Customer to Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridCheckInList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxCottageID.Text = dataGridCheckInList.SelectedRows[0].Cells[0].Value.ToString();
            textBoxCustomerName.Text = dataGridCheckInList.SelectedRows[0].Cells[1].Value.ToString();
            textBoxContactNumber.Text = dataGridCheckInList.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxCottageType.Text = dataGridCheckInList.SelectedRows[0].Cells[3].Value.ToString();
            textBoxAdult.Text = dataGridCheckInList.SelectedRows[0].Cells[4].Value.ToString();
            textBoxChild.Text = dataGridCheckInList.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            textBoxCottageID.Clear();
            textBoxCustomerName.Clear();
            textBoxContactNumber.Clear();
            comboBoxCottageType.SelectedIndex = -1;
            textBoxAdult.Clear();
            textBoxChild.Clear();

            textBoxCustomerName.Focus();

        }
    }
}
