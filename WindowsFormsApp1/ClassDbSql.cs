#region Description

// <summary>
//*This class file is for you after defining the database connection string
//The CRUD core operations are easy to perform (insert, read, edit, and delete information).

//You can also use the methods
//(DataReader, Datatable,Dataset, ....)
//Use for various operations.

//Database Backup and Restore operations are performed with specific methods.

//Checking the activation of the sql service is done using the CheckEngine method.
//Using the appropriate namespace, you can also use the sqlserver for the Access database and use the functions of each one.
//*/
//=======================================================================================
//*این فایل کلاس برای شما پس از تعریف رشته اتصال دیتابیس
//عملیات های اصلی CRUD (درج ،خواندن ،ویرایش و حذف اطلاعات) را به  راحتی انجام میدهد.

//همچنین می توانید از متد های 
//(DataReader,Datatable,Dataset,....)
//جهت عملیات های مختلف استفاده کنید.

//عملیات پشتیبان گیری و بازیابی بانک اطلاعاتی با متد های مشخص انجام میشود.

//چک کردن فعال بودن سرویس sql با استفاده از متد CheckEngine انجام می شود.

//همچنین با استفاده از namespace مربوطه می توانید هم برای دیتابیس اکسس هم sqlserver کد بزنید و از توابع هر کدام استفاده کنید.
//*/

// (Signature (HooMaN) Version 2.0)
// </summary>
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    #region Class SQL
    namespace SQL
    {
        public class ClassDbSql
        {
            #region propertys
            private static SqlCommand _com;
            private static SqlDataAdapter _dataAdapter;
            private static DataSet _ds;
            private static DataTable _dt;
            private static SqlDataReader _dr;
            #endregion

            #region Connection
            private static string _dbName = "dbtest";//Enter the database name is here
            //private static string _startupPath = Application.StartupPath + "\\" + _dbName;

            //private static SqlConnection con = new SqlConnection(@"Data Source=.;AttachDbFilename=" + StartupPath + ".mdf; Integrated Security = True");//method 1
            private static readonly SqlConnection Connection = new SqlConnection(@"server=.;database=" + _dbName + "; trusted_connection=true");//method 2
            #endregion

            #region Methods ConnectionState
            public static int IsConnectionOk()
            {
                var cmd = new SqlCommand("SELECT 1", Connection);
                Connection.Open();
                int i = (int)cmd.ExecuteScalar();
                return i;
            }
            private static SqlConnection OpenConnection()
            {
                if (Connection.State == ConnectionState.Closed || Connection.State == ConnectionState.Broken)
                {
                    Connection.Open();
                }
                return Connection;
            }

            private static void CloseConnection()
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            #endregion

            #region Methods Execute
            /// <summary>
            /// نمونه کد برای این بخش 
            /// اولین آرگومان دستور کوئری شما بصورت رشته
            /// دومین آرگومان پارامتر های می باشد مثل
            /// ClassDB_SQL.ExecuteNonQuery("insert into NameTable (NameFild) values(@a)", new Dictionary<string, object>()
            ///{ {"a","your value" } });
            /// </summary>
            /// <param name="sqlQuery"></param>
            /// <param name="paramter"></param>
            /// <returns></returns>
            public static bool ExecuteNonQuery(string sqlQuery, Dictionary<string, object> paramter)
            {
                _com = new SqlCommand();
                _com.CommandText = sqlQuery;
                _com.Connection = OpenConnection();
                foreach (var param in paramter)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _com.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            public static SqlCommand ExecuteScalar(string sqlQuery, Dictionary<string, object> paramter)
            {
                _com = new SqlCommand();
                _com.CommandText = sqlQuery;
                _com.Connection = OpenConnection();
                foreach (var param in paramter)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _com.ExecuteScalar();
                return _com;
            }
            public static SqlDataReader ExecuteReader(string sqlQuery, Dictionary<string, object> paramter)
            {
                _com = new SqlCommand();
                _com.CommandText = sqlQuery;
                _com.Connection = OpenConnection();
                foreach (var param in paramter)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _dr = _com.ExecuteReader();
                return _dr;
            }
            #endregion

            #region DataTable and DataSet
            public static DataTable ReturnDataTable(string sqlQuery)
            {
                _dt = new DataTable();
                _com = new SqlCommand();
                _dataAdapter = new SqlDataAdapter(sqlQuery, OpenConnection());
                _dt.Reset();
                _dataAdapter.Fill(_dt);
                return _dt;
            }
            /// <summary>
            ///نمونه کد
            ///  DataSet ds = ClassDB_SQL.returnDataSet("select * from mytable");
            /// dataGridView1.DataSource = ds.Tables[0];
            /// </summary>
            /// <param name="sqlQuery"></param>
            /// <returns></returns>
            public static DataSet ReturnDataSet(string sqlQuery)
            {
                _ds = new DataSet();
                _com = new SqlCommand();
                _dataAdapter = new SqlDataAdapter(sqlQuery, OpenConnection());
                _ds.Reset();
                _dataAdapter.Fill(_ds);
                return _ds;
            }
            #endregion

            #region Methods Backup and Restore Database
            public static void BackUp_DB(string nameDataBase)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.OverwritePrompt = true;
                sfd.Filter = @"SQL BackUp FIles ALL Files (*.*) |*.*| (*.Bak)|*.Bak";
                sfd.DefaultExt = "Bak";
                sfd.FilterIndex = 1;
                sfd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                sfd.Title = @"BackUp SQL Files";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    OpenConnection();
                    ExecuteNonQuery(@"BACKUP DATABASE [" + nameDataBase + "] TO  DISK='" + sfd.FileName + "'", new Dictionary<string, object>());
                    CloseConnection();
                }
            }
            public void Restore_DB(string nameDataBase)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = @"SQL BackUp FIles ALL Files (*.*) |*.*| (*.Bak)|*.Bak";
                ofd.FilterIndex = 1;
                ofd.Title = @"BackUp SQL Files";
                ofd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenConnection();
                    ExecuteNonQuery(@"Alter DATABASE [" + nameDataBase + "] SET SINGLE_USER with ROLLBACK IMMEDIATE " + "USE master " + " RESTORE DATABASE [" + nameDataBase + "] FROM DISK =N'" + ofd.FileName + "' with RECOVERY,REPLACE", new Dictionary<string, object>());
                    CloseConnection();
                }
            }
            #endregion

            #region Method Check Run Service SQL Server
            /// <summary>
            /// servce name : "MSSQLSERVER"
            /// </summary>
            /// <param name="servicename"></param>
            /// <returns></returns>
            public static string CheckEngine(string servicename)
            {
                ///Step 1 = add reference ServiceProcess

                ///step 2 = add using System.ServiceProcess;

                ///step 3 = Uncomment all code

                //ServiceController sc = new ServiceController(SERVICENAME);

                //switch (sc.Status)
                //{
                //    case ServiceControllerStatus.Running:
                //        return "Running";
                //    case ServiceControllerStatus.Stopped:
                //        return "Stopped";
                //    case ServiceControllerStatus.Paused:
                //        return "Paused";
                //    case ServiceControllerStatus.StopPending:
                //        return "Stopping";
                //    case ServiceControllerStatus.StartPending:
                //        return "Starting";

                //    default:
                //        return "Status Changing";
                //}

                return null; // Then Uncomment the code above to delete this line code
            }
            #endregion
        }
    }
    #endregion

    #region Class Access
    namespace OLEDB
    {
        class ClassDbAccess
        {
            #region propertys

            private static OleDbCommand _com;
            private static OleDbDataAdapter _da;
            private static DataSet _ds;
            private static DataTable _dt;
            private static OleDbDataReader _dr;
            #endregion

            #region Connection
            private static string _dbName = "MyDB";//Enter the database name is here
            private static OleDbConnection _con = new OleDbConnection("provider=microsoft.jet.oledb.4.0; data source=" + _dbName + ".mdb");
            #endregion

            #region Methods Connection State
            public static OleDbConnection OpenConnection()
            {
                if (_con.State == ConnectionState.Closed || _con.State == ConnectionState.Broken)
                    _con.Open();
                return _con;
            }
            public static OleDbConnection CloseConnection()
            {
                if (_con.State == ConnectionState.Open)
                    _con.Close();
                return _con;
            }
            #endregion

            #region Methods Execute
            public static bool RunQuery(string sqlQuery)
            {
                string command = sqlQuery;
                _com = new OleDbCommand();
                _com.Connection = OpenConnection();
                _com.CommandText = command;
                _com.ExecuteNonQuery();
                return true;
            }
            public static OleDbDataReader ReturnDataReader(string sqlQuery)
            {
                string command = sqlQuery;
                _com = _con.CreateCommand();
                _com.CommandText = command;
                _dr = _com.ExecuteReader();
                return _dr;
            }
            #endregion

            #region DataTable and Dataset
            public static DataTable ReturnDataTable(string sqlQuery)
            {
                _da = new OleDbDataAdapter();
                _com = new OleDbCommand();
                string command = sqlQuery;
                _dt = new DataTable();
                _com.Connection = OpenConnection();
                _com.CommandText = command;
                _da.SelectCommand = _com;
                _da.Fill(_dt);
                return _dt;
            }
            public static DataSet ReturnDataSet(string sqlQuery)
            {
                _com = new OleDbCommand();
                string command = sqlQuery;
                _ds = new DataSet();
                OpenConnection();
                _com = _con.CreateCommand();
                _da = new OleDbDataAdapter(command, OpenConnection());
                _ds.Reset();
                _da.Fill(_ds);
                return _ds;
            }
            #endregion
        }
    }
    #endregion
}