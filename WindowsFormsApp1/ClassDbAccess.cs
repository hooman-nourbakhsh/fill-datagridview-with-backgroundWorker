using System.Data;
using System.Data.OleDb;

namespace DataBase_Connection.OLEDB
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