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
using System.IO;

namespace Inventory
{
    public partial class itemEntry : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        string item, added;
        byte[] imageBT = null;
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
            MessageBox.Show("item entry successful!", "new item entered", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picPath = dlg.FileName.ToString();
                tbImagePath.Text = picPath;
                pictureBox1.ImageLocation = picPath;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In this page you can register new items to sell. \n Remember that the Brand name, Item name/flavor and price fields cannot be empty", "What is this?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            float number;
            bool result = float.TryParse(tbPrice.Text, out number);
            if (tbBrand.Text == null || tbName.Text == null || tbPrice.Text == null)
            {
                MessageBox.Show("Please fill up brand, item name/flavor, price fields", "Fill up required fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (result)
                {
                    float z = float.Parse(tbPrice.Text);
                    if (z <= 0)
                    {
                        MessageBox.Show("The price value entered is not applicable", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {

                        float prc = float.Parse(tbPrice.Text);
                        int stk = int.Parse(tbStock.Text);
                        item = tbBrand.Text + " - " + tbName.Text;
                        DialogResult res = MessageBox.Show("are you sure you want to add " + item + "", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (res == DialogResult.OK)
                        {
                            if (tbImagePath.Text != "")
                            {
                                FileStream fstream = new FileStream(this.tbImagePath.Text, FileMode.Open, FileAccess.Read);
                                BinaryReader br = new BinaryReader(fstream);
                                imageBT = br.ReadBytes((int)fstream.Length);
                                con.Open();
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "insert into [dbo].[Items] values('" + item + "', '" + tbQuan.Text + "','" + prc + "', '" + stk + "',@IMG)";

                                cmd.Parameters.Add(new SqlParameter("@IMG", imageBT));

                                cmd.ExecuteNonQuery();
                                con.Close();
                                added = "Added new item " + item + " " + tbQuan.Text + " " + prc + " " + stk + "";
                                zxc();
                                pictureBox1.Image = null;
                                tbImagePath.Text = null;
                                tbBrand.Text = null;
                                tbName.Text = null;
                                tbPrice.Text = null;
                                tbQuan.Text = null;
                                tbStock.Text = null;
                            }
                            else
                            {
                                con.Open();
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "insert into [dbo].[Items] values('" + item + "', '" + tbQuan.Text + "','" + prc + "', '" + stk + "', null)";

                                cmd.ExecuteNonQuery();
                                con.Close();
                                added = "Added new item " + item + " " + tbQuan.Text + " " + prc + " " + stk + "";
                                zxc();
                                pictureBox1.Image = null;
                                tbImagePath.Text = null;
                                tbBrand.Text = null;
                                tbName.Text = null;
                                tbPrice.Text = null;
                                tbQuan.Text = null;
                                tbStock.Text = null;
                            }
                        }
                        else
                        {

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
}
