using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace csharp_project
{
    public partial class NewComboBox : ComboBox
    {
        public int columnPadding = 5;
        private DataGridView DGV;
        public float[] columnWidths = { 80, 100, 100, 100, 150, 150, 80 };  //项宽
        public String[] columnNames = { "航班号", "航空公司", "航线", "机型", "起飞机场", "降落机场", "日期" }; //项名称
        public int valueMemberColumnIndex = 0;       //valueMember属性列所在的索引
        private DataTable dataTable = new DataTable("Student");
        public string valueText = "";
        public DataTable pubtable = new DataTable();
        public int box = 0;   //box用于存储combobox数据行数，用于绘制
        public NewComboBox(DataGridView dgv)
        {
            this.DGV = dgv;
            InitializeComponent();
            this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.DrawMode = DrawMode.OwnerDrawVariable;//手动绘制所有元素
            this.DropDownHeight = 350;   //自己设置下拉框最长到多长显示滚动条
            this.IntegralHeight = false;
            this.ItemHeight = 23;
            InitItems();
        }
       /* public NewComboBox(string valuetext)
        {
            this.valueText = valuetext;
            InitializeComponent();
            this.DropDownHeight =350;   //自己设置下拉框最长到多长显示滚动条
            this.IntegralHeight = false;
            this.ItemHeight = 30;

            InitItems();
        }
       //重写选中事件，不处理标题
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndex != 0)
            {
                this.Text = this.SelectedText.ToString().Replace("航班号", "");
            }
            else
            {
                this.Text = "";
               // this.valueText = "";

            }
        }*/


        //根据传入的Title_id号查询对应的航班信息，构造DataTable对象
        private DataTable CreateDataTable(string s)
        {
            SqlConnection conn = new SqlConnection("Data Source=B238\\SQLEXPRESS;Initial Catalog=Flight;Uid=sa;Pwd=123;");
            SqlDataAdapter dapt2;
            DataSet ds = new DataSet();
            string sql2;
            //combobox数据源为flight2
            sql2 = "select title_id as 航班号,company as 航空公司,line as 航线,plane_type as 机型,take_off as 起飞机场,land as 降落机场,date as 日期  from flight2 where title_id like '" + s.Replace("'", "''") + "%'";
            if (s == null || s == "")
            {
                sql2 = "select title_id as 航班号,company as 航空公司,line as 航线,plane_type as 机型,take_off as 起飞机场,land as 降落机场,date as 日期 from flight2"; 
            }

            dapt2 = new SqlDataAdapter(sql2, conn);
            dapt2.Fill(ds, "table1");
            DataTable dtable = new DataTable();
            //第一行的标题名称
            dtable.Columns.Add("航班号", typeof(System.String));
            dtable.Columns.Add("航空公司", typeof(System.String));
            dtable.Columns.Add("航线", typeof(System.String));
            dtable.Columns.Add("机型", typeof(System.String));
            dtable.Columns.Add("起飞机场", typeof(System.String));
            dtable.Columns.Add("降落机场", typeof(System.String));
            dtable.Columns.Add("日期", typeof(System.String));
            dtable = ds.Tables["table1"];
            box = dtable.Rows.Count+1;//为box赋值，box记录combobox中需要绘制的行数
            return dtable;
        }
        //获取相应的数据table

        //初始化相应的数据和属性
        public void InitItems()
        {
            dataTable = CreateDataTable(this.valueText);
            pubtable = dataTable;
            this.DataSource = dataTable;
            this.DisplayMember = "航班号";
            this.ValueMember = "航班号";

            if ((this.DataSource != null) && (!string.IsNullOrEmpty(this.DisplayMember)))
            {

                if (!string.IsNullOrEmpty(this.Text))
                {

                    DataView dv = dataTable.DefaultView;
                    dv.RowFilter = string.Format("航班号 like '{0}%'", this.valueText);
                  
                    DataTable NewDt = dv.ToTable("newTableName");
                    DataTable copyTable = new DataTable();
                    copyTable.Columns.Add("航班号", Type.GetType("System.String"));
                    copyTable.Columns.Add("航空公司", Type.GetType("System.String"));
                    copyTable.Columns.Add("航线", Type.GetType("System.String"));
                    copyTable.Columns.Add("机型", Type.GetType("System.String"));
                    copyTable.Columns.Add("起飞机场", Type.GetType("System.String"));
                    copyTable.Columns.Add("降落机场", Type.GetType("System.String"));
                    copyTable.Columns.Add("日期", Type.GetType("System.String"));
                    copyTable.Rows.Add(new object[] { "航班号", "航空公司", "航线", "机型", "起飞机场", "降落机场", "日期" });

                    for (int i = 0; i < NewDt.Rows.Count; i++)
                    {
                        copyTable.Rows.Add(new object[] { NewDt.Rows[i][0].ToString() + "\t", NewDt.Rows[i][1].ToString().Length > 20 ? NewDt.Rows[i][1].ToString().Substring(0, 20) : NewDt.Rows[i][1].ToString() + "\t", NewDt.Rows[i][2].ToString() + "\t", NewDt.Rows[i][3].ToString() + "\t", NewDt.Rows[i][4].ToString() + "\t", NewDt.Rows[i][5].ToString().Length > 20 ? NewDt.Rows[i][5].ToString().Substring(0, 20) : NewDt.Rows[i][5].ToString() + "\t", NewDt.Rows[i][6].ToString().Length >= 10 ? NewDt.Rows[i][6].ToString().Substring(0, 10) : NewDt.Rows[i][6].ToString() + "\t" });
                    }
                    this.DataSource = copyTable;
                    pubtable = copyTable;
                }
                else
                {
                    this.DataSource = dataTable;
                    pubtable = dataTable;
                }
                this.DropDownWidth = (int)CalculateTotalWidth();//计算下拉框的总宽度 
                if (this.Items.Count > 0)
                {
                    this.SelectedIndex = -1;
                }
            }
        }

        //计算下拉框的总宽度
        private float CalculateTotalWidth()
        {
            columnPadding = 5;
            float totalWidth = 0;
            foreach (int width in columnWidths)
            {
                totalWidth += (width + columnPadding);
            }
            return totalWidth + SystemInformation.VerticalScrollBarWidth;
        }

        //重写绘制方法，自定义绘制item
        protected override void OnDrawItem(DrawItemEventArgs e)
        {

            base.OnDrawItem(e);
            Rectangle boundsRect = e.Bounds;//获取绘制项边界的矩形
            boundsRect.Height = 23;
            e = new DrawItemEventArgs(e.Graphics, e.Font, boundsRect, e.Index, e.State, e.ForeColor, e.BackColor);


            if (DesignMode)
            {
                return;
            }

            if (e.State == DrawItemState.Selected)
            {
                boundsRect.Y = e.Index * 23;
                e.Graphics.FillRectangle(Brushes.Bisque, boundsRect);
            }
            else 
            {
                boundsRect.Y = e.Index * 23;
                e.Graphics.FillRectangle(Brushes.White, boundsRect);
            }

            //第一行为表头，设置颜色
            if (e.Index == 0)
            {
                for (int i = 0; i < columnNames.Length; i++)
                {
                    SolidBrush brush = new SolidBrush(Color.FromArgb(30, 123, 189));  //定义画刷
                    Rectangle rectColor = new Rectangle(boundsRect.Location, new Size(boundsRect.Width, boundsRect.Height));
                    e.Graphics.FillRectangle(brush, rectColor);   //填充颜色
                }
            }



            int lastRight = 0;
            using (Pen linePen = new Pen(SystemColors.GrayText))
            {
                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    if (columnNames.Length == 0)
                    {
                        e.Graphics.DrawString(Convert.ToString(Items[e.Index]), Font, brush, boundsRect);
                    }
                    else
                    {
                        //循环各列
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], columnNames[i]));
                            boundsRect.X = lastRight;//列的左边位置
                            boundsRect.Width = (int)columnWidths[i]+ columnPadding;//列的宽度
                            lastRight = boundsRect.Right;
                            //垂直居中与水平居中
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;
                           



                            if (i == valueMemberColumnIndex)//如果是valuemember
                            {
                                boundsRect.Y = e.Index * 23;
                                using (Font font = new Font("微软雅黑", 13))
                                {
                                    //绘制项的内容，即文字部分
                                   e.Graphics.DrawString(item, font, brush, boundsRect, sf);
                                }
                            }
                            else
                            {
                                //绘制项的内容
                                e.Graphics.DrawString(item, new Font("微软雅黑", 13), brush, boundsRect, sf);
                            }

                            //绘制各项间的竖线
                            if (i < columnNames.Length - 1)
                            {
                                e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);
                            }
                        }
                          //e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);   
                    }

                    //绘制各行间横线
                    int hh = 0;   //起点高度为0
                    using (SolidBrush brush2 = new SolidBrush(ForeColor))
                    {
                        if (this.Items.Count == 0)
                        {
                            e.Graphics.DrawString(Convert.ToString(Items[e.Index]), Font, brush2, boundsRect);
                        }
                        else
                        {
                            //循环各列
                            for (int i = 0; i < this.Items.Count; i++)
                            {
                                boundsRect.X = 0;
                                int h = 23;   //第一次绘制的高度
                                hh += h;
                                for (int j = 0; j < box+1; j++)  //以box记录行数
                                {
                                    if (i < box -1)
                                    {
                                        if (i < box - 1)
                                        {
                                            e.Graphics.DrawLine(linePen, boundsRect.Left, hh, (int)CalculateTotalWidth(), hh);
                                        }
                                    }
                                }
                                //e.Graphics.DrawLine(linePen, boundsRect.Left, boundsRect.Bottom, (int)CalculateTotalWidth(), boundsRect.Bottom);
                            }

                        }
                    }
                   e.DrawFocusRectangle();
                }
            }


        }
        protected string strTemp;
        //重写按键点击事件
        protected override void OnKeyUp(KeyEventArgs e)
        {
            strTemp = this.Text;
            this.valueText = this.Text;
            InitItems();
            this.DroppedDown = true;
            this.Text = strTemp;
            if (this.Text != null && this.Text != string.Empty)
            {
                // 获得光标位置
                this.Text = this.valueText;
                this.SelectionStart = this.Text.Length;
            }

            this.Cursor = Cursors.Default;
            base.OnKeyUp(e);
        }
    }
}
