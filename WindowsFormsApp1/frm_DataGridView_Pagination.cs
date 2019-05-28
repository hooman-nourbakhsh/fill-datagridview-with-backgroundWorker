using DataBase_Connection.SQL;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frm_DataGridView_Pagination : Form
    {
        private int _pageSize = 10;
        private int _currentPageIndex;
        private int _totalPage;
        private DataSet _ds;

        public frm_DataGridView_Pagination()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          //  timer1.Enabled = true;
            Stopwatch sw = new Stopwatch();
            sw.Start();
          
            _ds = ClassDbSql.ReturnDataSet("select * from table_person");
            CalculateTotalPage();
            dataGridView1.DataSource = GetCurrentRecord(1);
            sw.Stop();
            label1.Text = sw.Elapsed.ToString();
        }

        private void CalculateTotalPage()
        {
            int rowCount = _ds.Tables[0].Rows.Count;
            _totalPage = rowCount / _pageSize - 1;
            if (rowCount % _pageSize > 0)
            {
                _totalPage += 1;
            }
        }

        private DataTable GetCurrentRecord(int page)
        {
            DataTable dt;
            if (page == 0)
            {
                dt = ClassDbSql.ReturnDataTable("select top " + _pageSize + " * from table_person");
            }
            else
            {
                int prePagelimit = page * _pageSize;
                dt = ClassDbSql.ReturnDataTable("select top " + _pageSize +
                                                " * from table_person where id not in(select top " + prePagelimit +
                                                " id from table_person)");
            }

            try
            {
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _currentPageIndex = 0;
            dataGridView1.DataSource = GetCurrentRecord(_currentPageIndex);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                dataGridView1.DataSource = GetCurrentRecord(_currentPageIndex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < _totalPage)
            {
                _currentPageIndex++;
                dataGridView1.DataSource = GetCurrentRecord(_currentPageIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _currentPageIndex = _totalPage;
            dataGridView1.DataSource = GetCurrentRecord(_currentPageIndex);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var con = _pageSize * _currentPageIndex + 1;
            foreach (DataGridViewRow r in dataGridView1.Rows)

                r.Cells["row"].Value = (r.Index + con).ToString();
        }
    }
}