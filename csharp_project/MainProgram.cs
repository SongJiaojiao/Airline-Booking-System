using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.Drawing.Color;

namespace csharp_project
{
    public partial class MainProgram : Form
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

        public MainProgram()
        {
            InitializeComponent();
            mycombox = new NewComboBox(this.dataGridView1);
            this.dataGridView1.DataError += delegate (object sender, DataGridViewDataErrorEventArgs e) { };
        }

        private void Form3_lzy_Load(object sender, EventArgs e)
        {
        
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
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 14, FontStyle.Bold);

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            string sqlSentence = "select * from flight2 where title_id='" + strKey.Replace("\t", "") + "'";

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
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.dataGridView1.Columns["title_id"].Index && this.dataGridView1.CurrentCell.RowIndex == this.dataGridView1.Rows.Count - 1)
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

        //删除当前行

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> oSQLList = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                oSQLList.Add("delete from flight where title_id='" + this.dataGridView1.SelectedRows[0].Cells[8].Value.ToString() + "'");
            }

            bool blResult = odb.BatchExecute(oSQLList);
            if (blResult)
            {
                MessageBox.Show("删除信息成功！", "提示");
                SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
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
                MessageBox.Show("删除信息失败，请检查！", "提示");
            }
        }


        //添加记录
        private void button2_Click(object sender, EventArgs e)
        {
           
             List<string> oSQLList = new List<string>();
            for (int i = 0; i <= 6; i++)
            {
                Console.WriteLine("yo=" + this.dataGridView1.CurrentRow.Cells[i].Value);
            }
       

           oSQLList.Add("insert into  flight values('" +this.dataGridView1.CurrentRow.Cells[0].Value.ToString() + "''" + this.dataGridView1.CurrentRow.Cells[1].Value.ToString () + "''" + this.dataGridView1.CurrentRow.Cells[2].Value.ToString() + "''" + this.dataGridView1.CurrentRow.Cells[3].Value.ToString() + "''" + this.dataGridView1.CurrentRow.Cells[4].Value.ToString() + "''" + this.dataGridView1.CurrentRow.Cells[5].Value.ToString() + "''" +this.dataGridView1.CurrentRow.Cells[6].Value + "'");
            //oSQLList.Add("insert into  flight values(1,2,3,4,5,6,7)");


              bool blResult = odb.BatchExecute(oSQLList);
              if (blResult)
              {
                  MessageBox.Show("添加成功！", "提示");
                  SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
                  string sqlSentence = "select *,title_id as ID from flight";
                  SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                  DataTable result = new DataTable();   //直接使用datatable
                  adapter.Fill(result);
                  connectionToDatabase.Close();
                  bsTitle = new BindingSource();
                  bsTitle.DataSource = result;
                  this.dataGridView1.DataSource = bsTitle;
                  this.dataGridView1.Columns[7].Visible = false;
              }
              else
              {
                  MessageBox.Show("删除信息失败，请检查！", "提示");
              }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            List<string> oSQLList = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                oSQLList.Add("delete from flight where title_id='" + this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString() + "'");
            }

            bool blResult = odb.BatchExecute(oSQLList);
            if (blResult)
            {
                MessageBox.Show("删除信息成功！", "提示");
                SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
                string sqlSentence = "select *,title_id as ID from flight";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                DataTable result = new DataTable();
                adapter.Fill(result);
                connectionToDatabase.Close();
                bsTitle = new BindingSource();   //重新绑定
                bsTitle.DataSource = result;
                this.dataGridView1.DataSource = bsTitle;
                this.dataGridView1.Columns[7].Visible = false;
                this.mycombox.Hide();
            }
            else
            {
                MessageBox.Show("删除信息失败，请检查！", "提示");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            string sqlsentence = "select * from flight";
            Boolean flag = false;
            List<string> oSQLList = new List<string>();
            Console.WriteLine(this.dataGridView1.Rows.Count);
            //记录当前行的数据
            string a = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string b = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string c = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();
            string d = this.dataGridView1.CurrentRow.Cells[3].Value.ToString();
            string ee = this.dataGridView1.CurrentRow.Cells[4].Value.ToString();
            string f = this.dataGridView1.CurrentRow.Cells[5].Value.ToString();
            string g = this.dataGridView1.CurrentRow.Cells[6].Value.ToString();

            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlsentence, conn);
            SqlDataReader Dr;
            Dr = cmd.ExecuteReader();

            while (Dr.Read())
            {
                if (a == Dr["title_id"].ToString())
                {
                    flag = true;   //若读到相同值则标记
                }

            }
            Dr.Close();
            if (flag)
            {
                MessageBox.Show("已有相同值存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {   //将数据添加到表flight
                oSQLList.Add("insert into  flight values('" + a + "','" + b + "','" + c + "','" + d + "','" + ee + "','" + f + "','" + g + "')");
                bool blResult = odb.BatchExecute(oSQLList);
                if (blResult)
                {
                    MessageBox.Show("添加成功！", "提示");
                    SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
                    string sqlSentence = "select *,title_id as ID from flight";
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    connectionToDatabase.Close();
                    bsTitle = new BindingSource();   //重新绑定
                    bsTitle.DataSource = result;
                    this.dataGridView1.DataSource = bsTitle;
                    this.dataGridView1.Columns[7].Visible = false;
                }
                else
                {
                    MessageBox.Show("删除信息失败，请检查！", "提示");
                }
                mycombox.Visible = false;        //将combobox暂时隐藏
            }

            /*   oSQLList.Add("insert into  flight values('"+a+ "','" + b + "','" + c + "','" + d + "','" + ee + "','" + f + "','" + g + "')");


                bool blResult = odb.BatchExecute(oSQLList);
                if (blResult)
                {
                    MessageBox.Show("添加成功！", "提示");
                    SqlConnection connectionToDatabase = new SqlConnection("Data Source = B238\\SQLEXPRESS; Initial Catalog = Flight; Uid = sa; Pwd = 123; ");
                    string sqlSentence = "select *,title_id as ID from flight";
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                    DataTable result = new DataTable();   //直接使用datatable
                    adapter.Fill(result);
                    connectionToDatabase.Close();
                    bsTitle = new BindingSource();
                    bsTitle.DataSource = result;
                    this.dataGridView1.DataSource = bsTitle;
                    this.dataGridView1.Columns[7].Visible = false;
                }
                else
                {
                    MessageBox.Show("删除信息失败，请检查！", "提示");
                }*/
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login back = new Login();
            this.Hide();
            back.Show();
        }
    }
}


        /*  protected void onSelectionChangeCommitted(object sender, EventArgs e)
          {
              string id = this.mycombox.SelectedText;
              DataRow[] dr = this.mycombox.pubtable.Select("title_id='" + id + "'"); 
              foreach (DataRow d in dr)
              {

                  //  MessageBox.Show(id);
              }
              this.dataGridView1.CurrentCell.Value = id;
          }*/

        //处理dataGridView1的CellValueChanged事件，当点击backspace按钮时触发事件
        /* private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
         {
             Console.WriteLine("==========> dataGridView_cellValueChanged");
             if (e.ColumnIndex == 1 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1 && this.dataGridView1.CurrentRow.Cells[e.ColumnIndex] != null)
             {
                 try
                 {
                     /*dataGridView1.BeginEdit(true);
                     SendKeys.Send(" ");               
                     //“System.StackOverflowException”类型的未经处理的异常在 mscorlib.dll 中发生
                    SendKeys.Send("{BKSPT}");
                     dataGridView1.EndEdit(); */
        //EndEdit(); 写在最后输入没问题了，但是添加不了新行

        /* dataGridView1.BeginEdit(true);
         SendKeys.Send(" ");
         dataGridView1.EndEdit();
         SendKeys.Send("{BKSP}");       */


        // }
        // catch { }
        // }
        //  if (e.ColumnIndex == 1 && e.RowIndex >= 0 && e.RowIndex == dataGridView1.Rows.Count - 1 && this.dataGridView1.CurrentRow.Cells[e.ColumnIndex] != null)
        //  {
        //try
        //   {
        /*dataGridView1.BeginEdit(true);
        SendKeys.Send(" ");               
        //“System.StackOverflowException”类型的未经处理的异常在 mscorlib.dll 中发生
       SendKeys.Send("{BKSPT}");
        dataGridView1.EndEdit(); */
        //EndEdit(); 写在最后输入没问题了，但是添加不了新行

        /* dataGridView1.BeginEdit(true);
         SendKeys.Send(" ");
         dataGridView1.EndEdit();
         SendKeys.Send("{BKSP}");       */
        /* dataGridView1.BeginEdit(true);
         SendKeys.Send(" ");
         dataGridView1.EndEdit();
         SendKeys.Send("{BKSP}");

     }
     catch { }
 }

/* else if (e.ColumnIndex ==7 && e.RowIndex >= 0 && e.RowIndex <= dataGridView1.Rows.Count - 1 && this.dataGridView1.CurrentRow.Cells[e.ColumnIndex] != null)
 {
     for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
     {
         strChongfu += "$" + dataGridView1.Rows[i].Cells[7].Value + "$";
     }
     if (dataGridView1.CurrentCell.Value.ToString().Trim() != "" && strChongfu.IndexOf("$" + dataGridView1.CurrentCell.Value.ToString() + "$") >= 0)
     {
         MessageBox.Show("相同编号的信息在列表中已经存在！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        // return;
     }
 }*/
    //}


        //撤销修改
       /* private void button6_Click_1(object sender, EventArgs e)
        {
            /*try
            {
                ds.Tables[0].RejectChanges();
           
                SqlConnection connectionToDatabase = new SqlConnection(@"server=2012-20140123sp\sqlexpress;user id=sa;password=zr199406;database=publish");
                //SqlConnection connectionToDatabase = new SqlConnection(@"server=YAO-PC;user id=nanan;password=123;database=publish");
                string sqlSentence = "select *,title_id as ID from dbo.newtitles";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlSentence, connectionToDatabase);
                DataTable result = new DataTable();
                adapter.Fill(result);
                connectionToDatabase.Close();
                bsTitle = new BindingSource();
                bsTitle.DataSource = result;
                this.dataGridView1.DataSource = bsTitle;
                this.dataGridView1.Columns[7].Visible = false;
                MessageBox.Show("撤销操作成功！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("回滚失败！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
       // }





    

