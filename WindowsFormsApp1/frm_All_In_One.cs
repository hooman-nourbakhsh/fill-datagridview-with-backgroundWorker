using DataBase_Connection.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frm_All_In_One : Form
    {
        private int _pageSize = 100;
        private int _pageIndex = 1;
        private int _totalPage;
        private DataTable _dt;
        private int _rowCount;
        private int _con;
        private const string NameTable = "table_user";

        public frm_All_In_One()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var q = ClassDbSql.ExecuteScalar("select count(*) from table_user", new Dictionary<string, object>());

            _rowCount = ((int)q.ExecuteScalar());

            _totalPage = _rowCount / _pageSize;
            if (_rowCount % _pageSize > 0)
            {
                _totalPage += 1;
            }
            sw.Stop();
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action(() => label1.Text = @"TimeLoad:  " + sw.Elapsed.ToString()));
            }
        }

        private object GetCurrentRecord(int page)
        {
            if (page == 1)
            {
                _dt = ClassDbSql.ReturnDataTable("select top " + _pageSize + " * from " + NameTable);
            }
            else
            {
                var prePagelimit = (page - 1) * _pageSize;
                _dt = ClassDbSql.ReturnDataTable("select top " + _pageSize + " * from " + NameTable + " where id not in(select top " + prePagelimit + " id from " + NameTable + ")");
            }
            try
            {
                dataGridView1.DataSource = _dt;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return _dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _pageIndex = 1;
            dataGridView1.DataSource = GetCurrentRecord(_pageIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex--;
                dataGridView1.DataSource = GetCurrentRecord(_pageIndex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_pageIndex < _totalPage)
            {
                _pageIndex++;
                dataGridView1.DataSource = GetCurrentRecord(_pageIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _pageIndex = _totalPage;
            dataGridView1.DataSource = GetCurrentRecord(_pageIndex);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridView1.DataSource = GetCurrentRecord(1);
           // timer1.Stop();
            cp_Loading.Visible = false;
            label2.Text = _pageIndex + @" of " + _totalPage;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            backgroundWorker1.RunWorkerAsync();
            cp_Loading.Visible = true;
          //  timer1.Enabled = true;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            _con = _pageSize * (_pageIndex - 1);
            foreach (DataGridViewRow r in dataGridView1.Rows)

                r.Cells["row"].Value = ((r.Index + 1) + _con).ToString();
        }
    }
}