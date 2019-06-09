using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Inventory
{
    public partial class itemEntry : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        string item, added;
        public itemEntry()
        {
            InitializeComponent();
        }
        public void zxc()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [dbo].[History] values('" + lblUser.Text + "', '" + added + "','" + lblDate.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void itemEntry_Load(object sender, EventArgs e)
        {
            lblUser.Text = Form1.adminName;
            lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        private void lblStocks_Click(object sender, EventArgs e)
        {
            stock s = new stock();
            s.Show();
            this.Hide();
        }

        private void lblCashier_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.Show();
            this.Hide();
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {

            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            itemEntry ie = new itemEntry();
            ie.Show();
            this.Hide();
        }

        private void lblUser_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible != true)
            {
                groupBox1.Visible = true;
            }
            else if (groupBox1.Visible == true)
            {
                groupBox1.Visible = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tbContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCashier_Click_1(object sender, EventArgs e)
        {

            home h = new home();
            h.Show();
            this.Hide();
        }

        private void lblHistory_Click(object sender, EventArgs e)
        {
            history h = new history();
            h.Show();
            this.Hide();
        }

        private void lblLogOut_Click_1(object sender, EventArgs e)
        {

            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void lblStocks_Click_1(object sender, EventArgs e)
        {
            stock s = new stock();
            s.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int number;
            bool result = Int32.TryParse(tbPrice.Text, out number);
            if (result)
            {
                int z = int.Parse(tbPrice.Text);
                if (z <= 0)
                {
                    MessageBox.Show("The price value entered is not applicable", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    try
                    {
                        double prc = Convert.ToDouble(tbPrice.Text);
                        int stk = int.Parse(tbStock.Text);
                        item = tbBrand.Text + " - " + tbName.Text;
                        DialogResult res = MessageBox.Show("are you sure you want to add " + item + "", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (res == DialogResult.OK)
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into [dbo].[Items] values('" + item + "', '" + tbQuan.Text + "','" + prc + "', '"+ stk+"')";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            added = "Added new item "+ item+ " " + tbQuan.Text + " " + prc + " " + stk + "";
                            zxc();
                        }
                        else
                        {

                        }
                    }
                    catch
                    {
                        MessageBox.Show("plese make sure the price value has '.00' in it and that the amount of stock is a numeric value");
                    }

                }
            }
            else
            {
                MessageBox.Show("The price value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

         }
    }
}
