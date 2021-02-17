using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class DataManager
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;

        public DataManager()
        {
            connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\opilane\Source\Repos\Cinema\Cinema\MyDB.mdf; Integrated Security = True");
        }

        public void CreateTable(string table)
        {
            connection.Open();
            command = new SqlCommand("" +
                "CREATE TABLE Hall(" +
                "Id primary key int identity(1, 1), " +
                "Type varchar(10), " +
                "Number int," +
                "3D bit" +
                ")" +
                "", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DropTable(string table)
        {

        }

        public void ClearData(string table)
        {

        }
    }
}
