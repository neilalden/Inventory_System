using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace Inventory
{
    public partial class register : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        public register()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void register_Load(object sender, EventArgs e)
        {

        }

        private void register_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnReg.PerformClick();
            }
        }
        

        private void btnReg_Click(object sender, EventArgs e)
        {
            if (tbPass.Text != tbConfirm.Text)
            {
                MessageBox.Show("Please confirm your password correctly","Confirm password",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
            if (tbAdminU.Text == null || tbPass.Text == null)
            {
                MessageBox.Show("Please verify your registration to an admin", "Verify registration", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (tbName.Text != null && tbPass.Text != null && tbConfirm.Text != null && tbContact.Text !=null && tbAdminU.Text != null && tbAdminP.Text != null)
            {
                con.Open();
                string username = tbName.Text;
                string password = tbPass.Text;
                SqlCommand cmd = new SqlCommand("select username,password from [dbo].[Table] where username ='" + tbAdminU.Text + "'and password ='" + tbAdminP.Text + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into [dbo].[Table] values('" + tbName.Text + "', '" + tbConfirm.Text + "','" + tbContact.Text + "')";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New admin registered successfully","Registration successful",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                    tbName.Text = null;
                    tbPass.Text = null;
                    tbConfirm.Text = null;
                    tbContact.Text = null;
                    tbAdminU.Text = null;
                    tbAdminP.Text = null;
                }
                else
                {
                    MessageBox.Show("Invalid admin credentials","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                con.Close();
            }

        }

        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To register a new admin, an existing admin should verify the registration first by entering his credentials.", "Verify", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 z = new Form1();
            z.Show();
        }

        private void label9_MouseHover(object sender, EventArgs e)
        {
            label9.ForeColor = System.Drawing.Color.White;
            label9.BackColor = System.Drawing.Color.Black;

        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.ForeColor = System.Drawing.Color.Black;
            label9.BackColor = System.Drawing.Color.Empty;
        }
    }
}
