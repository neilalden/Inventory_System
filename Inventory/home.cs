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
    public partial class home : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        int inc = 0;
        string asd;
        public home()
        {
            InitializeComponent();
        }
        public void entry()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [dbo].[History] values('" + lblUser.Text + "', '" + asd + "','" + lblDate.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void clear()
        {
            tbAmount.Text = null;
            lblPrice.Text = null;
            lblStockL.Text = null;
            lblItem.Text = null;
            pictureBox1.Image = null;
        }
        private void home_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.database1DataSet.Items);
            // TODO: This line of code loads data into the 'database1DataSet.vapeJuice' table. You can move, or remove it, as needed.
            lblUser.Text = Form1.adminName;
            lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            dataGridView1.DataSource = null;
        }
        public void search()
        {
            dataGridView1.Visible = true;
            timer1.Enabled = true;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Items where item LIKE'%" + textBox1.Text + "%'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int sl = int.Parse(lblStockL.Text);
            int am = int.Parse(tbAmount.Text);
            DialogResult res = MessageBox.Show("Purchase "+lblItem.Text+"?  \n This action CANNOT be undone!", "Are you sure?",MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (res == DialogResult.OK)
            {
                int number;
                bool result = Int32.TryParse(tbAmount.Text, out number);
                if (lblPrice.Text == "" || lblItem.Text == "")
                {
                    MessageBox.Show("Please select an item", "no item selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (tbAmount.Text == null)
                {
                    MessageBox.Show("Please enter an amount to purchase", "no amount selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (lblStockL.Text == "0")
                {
                    MessageBox.Show("No stocks left. Please Restock!", "unable to purchase", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (sl < am)
                {
                    MessageBox.Show("the amount of items to purchase exceeds the stocks left", "unable to purchase", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (result)
                {
                    try
                    {
                        inc++;
                        Label l = new Label();
                        l.Name = "item" + inc.ToString();
                        l.Text = lblItem.Text + " x" + tbAmount.Text;
                        l.AutoSize = true;
                        flowLayoutPanel1.Controls.Add(l);

                        Label prc = new Label();
                        prc.Name = inc.ToString();
                        prc.ForeColor = System.Drawing.Color.Lime;

                        float p = float.Parse(lblPrice.Text);
                        float a = float.Parse(tbAmount.Text);
                        float t = p * a;


                        prc.Text = Convert.ToString(t);
                        prc.AutoSize = true;
                        flowLayoutPanel1.Controls.Add(prc);

                        float z = float.Parse(prc.Text);
                        float y = float.Parse(lblTotal.Text);
                        lblTotal.Text = Convert.ToString(y + z);

                        int aa = int.Parse(lblStockL.Text);
                        int bb = int.Parse(tbAmount.Text);
                        int cc = aa - bb;
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update Items SET stock = '" + cc + "' where item = '" + lblItem.Text + "'";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        asd = "Sold "+lblItem.Text+" x "+tbAmount.Text+"";
                        entry();
                        clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {

            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                dataGridView1.Visible = false;
                timer1.Enabled = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Items where item LIKE'%" + dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "%'";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblItem.Text = (dr["item"].ToString());
                    lblPrice.Text = (dr["price"].ToString());
                    lblStockL.Text = (dr["stock"].ToString());
                    if (!Convert.IsDBNull(dr["Image"]))
                    {
                        byte[] img = (byte[])(dr["Image"]);
                        MemoryStream mstream = new MemoryStream(img);
                        pictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                tbAmount.Text = "1";
                con.Close();
                timer1.Enabled = false;
                textBox1.Text = null;
                dataGridView1.DataSource = null;
                dataGridView1.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("your total is "+lblTotal.Text+"\n Sold by "+lblUser.Text, "item/s sold!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            flowLayoutPanel1.Controls.Clear();
            clear();
            lblTotal.Text = null;
        } 

        private void button3_Click(object sender, EventArgs e)
        {
            if (lblItem.Text == "")
            {
                MessageBox.Show("Please select an item", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (tbAmount.Text == "")
            {
                MessageBox.Show("Please enter an amount", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (int.Parse(tbAmount.Text) <= 0)
            {
                MessageBox.Show("The amount value entered is not applicable", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                int z = int.Parse(tbAmount.Text);
                int number;
                bool result = Int32.TryParse(tbAmount.Text, out number);
                if (result)
                {
                    int val = z - 1;
                    tbAmount.Text = val.ToString();
                }
                else if (tbAmount.Text != "")
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnInc_Click(object sender, EventArgs e)
        {
            if (lblItem.Text == "")
            {
                MessageBox.Show("Please select an item", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (tbAmount.Text == "")
            {
                MessageBox.Show("Please enter an amount", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else
            {
                int number;
                bool result = Int32.TryParse(tbAmount.Text, out number);
                if (result)
                {
                    int val = int.Parse(tbAmount.Text) + 1;
                    tbAmount.Text = val.ToString();
                }
                else if (tbAmount.Text != "")
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            int number;
            bool result = Int32.TryParse(tbAmount.Text, out number);
            if (result)
            {
                int z = int.Parse(tbAmount.Text);
                if (z <= 0)
                {
                    MessageBox.Show("The amount value entered is not applicable", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else if(tbAmount.Text != "")
            {
                MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lblStocks_Click(object sender, EventArgs e)
        {
            stock s = new stock();
            s.Show();
            this.Hide();
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {
            groupBox1.Size = new Size(166, groupBox1.Size.Height);
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

        private void lblHistory_Click(object sender, EventArgs e)
        {
            history h = new history();
            h.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In this page you can sell items by searching the item name and clicking in the search results. Once an item is selected, please enter an amount of items to purchase. \n Remember that once you have clicked the 'purchase' button the stock of the item selected is automatically reduced in the database", "What is this?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
