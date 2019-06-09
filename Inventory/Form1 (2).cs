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
    public partial class Form1 : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lb\source\repos\Inventory\Inventory\Database1.mdf;Integrated Security=True");
        register reg = new register();
        home home = new home();
        public static string adminName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            /*
             con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM masterList where name LIKE'%" + tbSearch.Text + "%'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lblSection.Text = (dr["Section"].ToString());
                lblName.Text = (dr["Name"].ToString());
                lblId.Text = (dr["Id"].ToString());
                lblCity.Text = (dr["Address"].ToString());
                lblGender.Text = (dr["Gender"].ToString());
                byte[] img = (byte[])(dr["Image"]);
                MemoryStream mstream = new MemoryStream(img);
                pictureBox.Image = System.Drawing.Image.FromStream(mstream);
            }
            con.Close();
             */
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogIn.PerformClick();
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            con.Open();
            string username = tbName.Text;
            string password = tbPass.Text;
            SqlCommand cmd = new SqlCommand("select username,password from [dbo].[Table] where username ='" + tbName.Text + "'and password ='" + tbPass.Text + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                adminName = tbName.Text;
                this.Hide();
                home.Show();
            }
            else
            {
                MessageBox.Show("Invalid Login please check username and password","error!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            con.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            reg.Show();
            this.Hide();
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            label3.ForeColor = System.Drawing.Color.SkyBlue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = System.Drawing.Color.SteelBlue;
        }
    }
}
