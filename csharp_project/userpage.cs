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
    public partial class userpage : Form
    {
        DBOper odb = new DBOper();
        private string strSql = "";
        SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
        SqlDataAdapter dapt1;
        SqlDataAdapter dapt2;
        BindingSource bsTitle;
        NewComboBox mycombox = null;
        DataTable result = new DataTable();   //数据集

        //声明一个全局的DataSet
        DataSet ds = new DataSet();
        //声明一个全局的SqlDataAdapter
        SqlDataAdapter da = new SqlDataAdapter();
        //声明一个全局的SQLParameter
        SqlParameter param = new SqlParameter();

        string sql1;
        string sql2;
        static public int boxRow;

        public userpage()
        {
            InitializeComponent();
            mycombox = new NewComboBox(this.dataGridView1);
            this.dataGridView1.DataError += delegate (object sender, DataGridViewDataErrorEventArgs e) { };
        }

        private void Form3_lzy_Load(object sender, EventArgs e)
        {

            comboBox1.Text = "全部";
            comboBox1.Items.Add("南方航空");
            comboBox1.Items.Add("东方航空");
            comboBox1.Items.Add("中国国航");
            comboBox1.Items.Add("厦门航空");
            comboBox1.Items.Add("深圳航空");

            SqlConnection connectionToDatabase = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            string sqlSentence = "select *,title_id as ID from flight";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);

            adapter.Fill(ds);
            connectionToDatabase.Close();
            bsTitle = new BindingSource();
            bsTitle.DataSource = ds.Tables[0];
            this.dataGridView1.DataSource = bsTitle;
            mycombox.DropDownStyle = ComboBoxStyle.DropDown;
            mycombox.FlatStyle = FlatStyle.Standard;
            mycombox.Visible = false;

            //this.dataGridView1.AllowUserToAddRows = false;
            //this.mycombox.DrawItem +=
            //    new DrawItemEventHandler(ComboBox1_DrawItem);
            //this.mycombox.MeasureItem +=
            //   new MeasureItemEventHandler(ComboBox1_MeasureItem);
            this.mycombox.TextChanged += mycombox_TextChanged;
            this.dataGridView1.Controls.Add(mycombox);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 123, 189);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 13, FontStyle.Bold);

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Columns[1].HeaderCell.Value = "航线";    //aaa
            this.dataGridView1.Columns[1].HeaderCell.Value = "航空公司";
            this.dataGridView1.Columns[2].HeaderCell.Value = "航线";
            this.dataGridView1.Columns[3].HeaderCell.Value = "机型";
            this.dataGridView1.Columns[4].HeaderCell.Value = "起飞机场";
            this.dataGridView1.Columns[5].HeaderCell.Value = "降落机场";
            this.dataGridView1.Columns[6].HeaderCell.Value = "日期";
            this.dataGridView1.Columns[7].Visible = false;
            for (int m = 0; m < dataGridView1.Columns.Count; m++)
            {
                dataGridView1.Columns[m].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //绑定combobox的数据源
        void mycombox_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView1.CurrentCell.Value = ((NewComboBox)sender).Text;
            FindValue(((NewComboBox)sender).Text);
        }


        string strChongfu = "";

        //combobox的数据源
        private void FindValue(string strKey)
        {

            SqlConnection connectionToDatabase = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            string sqlSentence = "select * from flight where title_id='" + strKey.Replace("\t", "") + "'";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
            DataTable result = new DataTable();


            adapter.Fill(result);
            boxRow = result.Rows.Count;
            Console.WriteLine("boxRow=" + boxRow);

            connectionToDatabase.Close();
            //datagridview1显示选中的combobox
            if (result.Rows.Count > 0)
            {
                dataGridView1.CurrentRow.Cells[0].Value = result.Rows[0]["title_id"].ToString();
                dataGridView1.CurrentRow.Cells[1].Value = result.Rows[0]["company"].ToString();
                dataGridView1.CurrentRow.Cells[2].Value = result.Rows[0]["line"].ToString();
                dataGridView1.CurrentRow.Cells[3].Value = result.Rows[0]["plane_type"].ToString();
                dataGridView1.CurrentRow.Cells[4].Value = result.Rows[0]["take_off"].ToString();
                dataGridView1.CurrentRow.Cells[5].Value = result.Rows[0]["land"].ToString();
                dataGridView1.CurrentRow.Cells[6].Value = result.Rows[0]["date"].ToString();
                dataGridView1.CurrentRow.Cells[7].Value = result.Rows[0]["title_id"].ToString();
            }
            /*else 
            {
                connectionToDatabase = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
                sqlSentence = "select * from flight where title_id='" + strKey.Replace("\t", "") + "'";
                adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                DataTable result2 = new DataTable();
              adapter.Fill(result2);
                connectionToDatabase.Close();
                if (result2.Rows.Count > 0)
                {
                    dataGridView1.CurrentRow.Cells[0].Value = result2.Rows[0]["title_id"].ToString();
                    dataGridView1.CurrentRow.Cells[1].Value = result2.Rows[0]["company"].ToString();
                    dataGridView1.CurrentRow.Cells[2].Value = result2.Rows[0]["line"].ToString();
                    dataGridView1.CurrentRow.Cells[3].Value = result2.Rows[0]["plane_type"].ToString();
                    dataGridView1.CurrentRow.Cells[4].Value = result2.Rows[0]["take_off"].ToString();
                    dataGridView1.CurrentRow.Cells[5].Value = result2.Rows[0]["land"].ToString();
                    dataGridView1.CurrentRow.Cells[6].Value = result2.Rows[0]["date"].ToString();
                    dataGridView1.CurrentRow.Cells[7].Value = result2.Rows[0]["title_id"].ToString();
                }
            }*/

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                strChongfu += "$" + dataGridView1.Rows[i].Cells[7].Value + "$";
                if (i != dataGridView1.CurrentRow.Index)
                {

                    dataGridView1.Rows[i].Cells[0].Value = dataGridView1.Rows[i].Cells[7].Value;
                }
            }
        }

        //判断是否是数字
        public static bool IsNum(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    continue;
                }
                if (!Char.IsNumber(str, i))
                    return false;
            }
            if (str.Length == 0)
            {
                return false;
            }
            return true;
        }



        //批量添加多条数据功能
        private void button4_Click(object sender, EventArgs e)
        {

            /* List<string> oSQLList = new List<string>();
             oSQLList.Add("delete from flight ");
             for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
             {
                 //string strTitleID = dataGridView1.Rows[i].Cells[7].Value.ToString();

                 //DataTable dtCunzai = odb.ExecuteSQL("select * from newtitles where title_id='" + strTitleID + "' ");
                 //if (dtCunzai != null && dtCunzai.Rows.Count == 1)
                 //{
                 //   oSQLList.Add("update dbo.newtitles set title='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "',type='" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "',price=" + float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) + ",ytd_sales=" + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) + ",notes='" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "',pubdate='" + DateTime.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()) + "' where title_id='" + strTitleID + "'");
                 //    // MessageBox.Show("第" + (i + 1).ToString() + "行相同ID信息的图书已经存在，将做更细操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 //}
                 //else
                 //{
                     oSQLList.Add("insert into flight values('" + dataGridView1.Rows[i].Cells[7].Value.ToString() + "', '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "'," + float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) + "," + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) + ",'" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + DateTime.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()) + "')");
                // }

             }

             bool blResult = odb.BatchExecute(oSQLList);
             if (blResult)
             {
                 MessageBox.Show("添加信息保存成功！", "提示");
               SqlConnection connectionToDatabase = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");

                 string sqlSentence = "select *,title_id as ID from flight";
                 SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                 DataTable result = new DataTable();   
                 adapter.Fill(result);
                 connectionToDatabase.Close();
                 bsTitle = new BindingSource();
                 bsTitle.DataSource = result;
                 this.dataGridView1.DataSource = bsTitle;
                 this.dataGridView1.Columns[7].Visible = false;
             }
             else
             {
                 MessageBox.Show("添加信息保存失败，请检查！", "提示");
             }*/


        }




        //点击title_id列时，combobox的下拉框
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("=========> dataGridView_cellEnter");
            // for(int i=0; i <=7; i++){
            // Console.WriteLine("yo=" + this.dataGridView1.CurrentRow.Cells[i].Value);
            // }


            //try
            //{
            //判断当前列是否为title_id列，不是就不处理
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.dataGridView1.Columns["title_id"].Index && this.dataGridView1.CurrentCell.RowIndex == this.dataGridView1.Rows.Count)
            {
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(this.dataGridView1.CurrentCell.ColumnIndex, this.dataGridView1.CurrentCell.RowIndex, false);
                string strCell = this.dataGridView1.CurrentCell.Value.ToString();
                this.mycombox.valueText = strCell.Trim();
                this.mycombox.InitItems();
                this.mycombox.Text = strCell.Trim();
                this.mycombox.Left = rect.Left;
                this.mycombox.Width = rect.Width;
                this.mycombox.Height = 1000;
                this.mycombox.Top = rect.Top;
                this.mycombox.Visible = true;
                this.mycombox.Focus();
            }
            //}
            //catch
            //{
            //    MessageBox.Show("错误！111", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string company = comboBox1.Text.Trim();
            string dep = textBox1.Text.Trim();
            string dest = textBox2.Text.Trim();
            string line = dep + "-" + dest;

            SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
            string sqlsentence = "select * from flight where company='" + company + "' and line='" + line + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlsentence, connectionToDatabase);
            DataTable result = new DataTable();
            adapter.Fill(result);
            connectionToDatabase.Close();
            bsTitle = new BindingSource();
            bsTitle.DataSource = result;
            this.dataGridView1.DataSource = bsTitle;



            // MessageBox.Show("用户名或密码输入错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string company = comboBox1.Text.Trim();
            string dep = textBox1.Text.Trim();
            string dest = textBox2.Text.Trim();
            string line = dep + "-" + dest;

            SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
            if (company == "全部")
            {
                if (line != "-") {
                    string sqls = "select * from flight where line='" + line + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(sqls, connectionToDatabase);
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    connectionToDatabase.Close();
                    bsTitle = new BindingSource();
                    bsTitle.DataSource = result;
                    this.dataGridView1.DataSource = bsTitle;
                }
                else
                    MessageBox.Show("请输入公司或者往返地！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (line == "-") {
                    string sqls = "select * from flight where company='" + company + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(sqls, connectionToDatabase);
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    connectionToDatabase.Close();
                    bsTitle = new BindingSource();
                    bsTitle.DataSource = result;
                    this.dataGridView1.DataSource = bsTitle;
                }

                else
                {
                    string sqlsentence = "select * from flight where company='" + company + "' and line='" + line + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlsentence, connectionToDatabase);
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    connectionToDatabase.Close();
                    bsTitle = new BindingSource();
                    bsTitle.DataSource = result;
                    this.dataGridView1.DataSource = bsTitle;
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login back = new Login();
            this.Hide();
            back.Show();
        }
    } }


      
       

       



       

