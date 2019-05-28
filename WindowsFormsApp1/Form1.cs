using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dt = (DateTime.Now);
            label1.Text = p.GetDayOfMonth(dt).ToString();
            label2.Text = p.GetDayOfWeek(dt).ToString();
            label3.Text = p.GetDayOfYear(dt).ToString();
            label4.Text = p.GetDaysInMonth(p.GetYear(dt), p.GetMonth(dt)).ToString();
            label5.Text = p.GetDaysInYear(p.GetYear(dt)).ToString();
            label6.Text = p.GetEra(dt).ToString();
            label7.Text = p.GetLeapMonth(p.GetYear(dt)).ToString();
            label8.Text = p.IsLeapYear(p.GetYear(dt)).ToString();
            label9.Text = p.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday).ToString();
            label10.Text = p.GetYear(DateTime.Now).ToString() + "/" + p.GetMonth(DateTime.Now).ToString("0#") + "/" + p.GetDayOfMonth(DateTime.Now).ToString("0#");



        }
    }
}
