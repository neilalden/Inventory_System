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
    public partial class stock : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        string del;
        string res;
        public stock()
        {
            InitializeComponent();
        }
        public void restock()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [dbo].[History] values('" + lblUser.Text + "', '" + res + "','" + lblDate.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("item successfuly restocked!", "item restocked?s", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public void delete()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [dbo].[History] values('" + lblUser.Text + "', '" + del + "','" + lblDate.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("item successfuly removed!", "item permanently deleted", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public void search()
        {
            dataGridView2.Visible = true;
            timer1.Enabled = true;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Items where item LIKE'%" + textBox1.Text + "%'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;

            con.Close();
        }
        public void rfresh()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Items";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.Close();
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
                else
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
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
            else
            {
                MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void stock_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.database1DataSet.Items);
            lblUser.Text = Form1.adminName;
            lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            rfresh();
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblItem.Text = null;
            lblPrice.Text = null;
            lblStockL.Text = null;
            int number;
            bool result = Int32.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number);
            if (result)
            {
                MessageBox.Show("Only select an item name", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM Items where item LIKE'%" + dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "%'";
                    cmd.ExecuteNonQuery();

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
                }
                catch
                {
                    MessageBox.Show("Unknown error, please try again", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

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
                else
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Items where item LIKE'%" + dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "%'";
                cmd.ExecuteNonQuery();

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
                dataGridView2.DataSource = null;
                dataGridView2.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                dataGridView2.Visible = false;
                timer1.Enabled = false;
            }
        }

        private void lblItem_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
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
                int number;
                bool result = Int32.TryParse(tbAmount.Text, out number);
                if (result)
                {
                    DialogResult resultt = MessageBox.Show("Are you sure you want to restock this item " + lblUser.Text + "?", "restock item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultt == DialogResult.Yes)
                    {
                        float s = float.Parse(lblStockL.Text);
                        float a = float.Parse(tbAmount.Text);
                        float t = s + a;

                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update Items SET stock = '" + t + "' where item = '" + lblItem.Text + "'";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        rfresh();
                        res = "Restocked item "+lblItem.Text+" x "+tbAmount.Text+"";
                        restock();
                    }
                }
                else
                {
                    MessageBox.Show("The amount value entered is not a numeric value", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to permanently remove this item " + lblUser.Text +"?", "remove item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Items WHERE item ='" + lblItem.Text + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                del = "Deleted item " + lblItem.Text + "";
                delete();
            }
            else
            {

            }
        }

        private void lblHistory_Click(object sender, EventArgs e)
        {
            history h = new history();
            h.Show();
            this.Hide();
        }

        private void lblStocks_Click(object sender, EventArgs e)
        {

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

        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In this page you can restock items by searching or clicking the item name in the table. Then enter an amount to restock the selected item in the database","What is this?",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
