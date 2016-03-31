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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            TextBoxEx ll= new TextBoxEx();
            ll.backGroundText = "请输入密码";
            


        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin admin = new admin();
            this.Hide();
            admin.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            conn.Open();
            string sqlsentence = "select * from userinfo";
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            Boolean flag = false;
            SqlCommand cmd = new SqlCommand(sqlsentence, conn);
            SqlDataReader Dr;
            Dr = cmd.ExecuteReader();
            while (Dr.Read())   //当表中还有值时
            {
                if (username == Dr["name"].ToString() && password == Dr["pwd"].ToString())
                {
                    flag = true;   //若有相同值则标记
                }

            }
            Dr.Close();
            if (flag)
            {
                userpage user = new userpage();
                this.Hide();
                user.Show();
            }
            else
                MessageBox.Show("用户名或密码输入错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //进入管理员登陆界面
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            admin admin = new admin();
            this.Hide();
            admin.Show();
        }
    }
}
