using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace csharp_project
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            conn.Open();
            string sqlsentence = "select * from admin";
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            Boolean flag = false;
            SqlCommand cmd = new SqlCommand(sqlsentence, conn);
            SqlDataReader Dr;
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                if (username == Dr["name"].ToString() && password == Dr["pwd"].ToString())
                {
                    flag = true;
                }

            }
            Dr.Close();
            if (flag)
            {
                MainProgram mainProgram = new MainProgram();
                this.Hide();
                mainProgram.Show();
            }
            else
                MessageBox.Show("用户名或密码输入错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();

       
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            conn.Open();
            string sqlsentence = "select * from admin";
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            Boolean flag = false;
            SqlCommand cmd = new SqlCommand(sqlsentence, conn);
            SqlDataReader Dr;
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                if (username == Dr["name"].ToString() && password == Dr["pwd"].ToString())
                {
                    flag = true;
                }

            }
            Dr.Close();
            if (flag)
            {
                MainProgram mainProgram = new MainProgram();
                this.Hide();
                mainProgram.Show();
            }
            else
                MessageBox.Show("用户名或密码输入错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }
    }
    
}
