using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class DataManager
    {
        private readonly SqlConnection connection;
        private SqlCommand command;
        //private SqlDataAdapter adapter;

        public DataManager(string link)
        {
            connection = new SqlConnection(link);
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public void CreateTable(string table)
        {
            connection.Open();
            switch (table.ToLower())
            {
                case "hall":
                    command = new SqlCommand("" +
                    "CREATE TABLE Hall(" +
                    "Id int primary key identity(1, 1), " +
                    "Type varchar(10), " +
                    "Number int," +
                    "ThreeD bit" +
                    ")" +
                    "", connection);
                    break;
                case "seat":
                    command = new SqlCommand("" +
                    "CREATE TABLE Seat(" +
                    "Id int primary key identity(1, 1), " +
                    "HallId int, " +
                    "Row int, " +
                    "Number int, " +
                    "Busy bit, " +
                    "FOREIGN KEY (HallId) REFERENCES Hall(Id)" +
                    ")" +
                    "", connection);
                    break;
                case "movie":
                    command = new SqlCommand("" +
                    "CREATE TABLE Movie(" +
                    "Id int primary key identity(1, 1), " +
                    "Name varchar(MAX)," +
                    "Category varchar(MAX), " +
                    "Language varchar(MAX), " +
                    "Subtitles bit, " +
                    "Duration int" +
                    ")" +
                    "", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("" +
                    "CREATE TABLE Showtime(" +
                    "Id int primary key identity(1, 1), " +
                    "MovieId int, " +
                    "Date datetime, " +
                    "HallId int, " +
                    "FOREIGN KEY (MovieId) REFERENCES Movie(Id)," +
                    "FOREIGN KEY (HallId) REFERENCES Hall(Id)" +
                    ")" +
                    "", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("" +
                    "CREATE TABLE Ticket(" +
                    "Id int primary key identity(1, 1), " +
                    "Price decimal(7, 2)," +
                    "ShowtimeId int," +
                    "SeatId int," +
                    "FOREIGN KEY (ShowtimeId) REFERENCES Showtime(Id)," +
                    "FOREIGN KEY (SeatId) REFERENCES Seat(Id)" +
                    ")" +
                    "", connection);
                    break;
            }
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DropTable(string table)
        {
            connection.Open();
            switch (table.ToLower())
            {
                case "hall":
                    command = new SqlCommand("DROP TABLE Hall", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("DROP TABLE Showtime", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("DROP TABLE Ticket", connection);
                    break;
                case "movie":
                    command = new SqlCommand("DROP TABLE Movie", connection);
                    break;
                case "seat":
                    command = new SqlCommand("DROP TABLE Seat", connection);
                    break;
            }
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ClearData(string table)
        {
            connection.Open();
            switch (table.ToLower())
            {
                case "hall":
                    command = new SqlCommand("DELETE FROM Hall", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("DELETE FROM Showtime", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("DELETE FROM Ticket", connection);
                    break;
                case "movie":
                    command = new SqlCommand("DELETE FROM Movie", connection);
                    break;
                case "seat":
                    command = new SqlCommand("DELETE FROM Seat", connection);
                    break;
            }
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ClearData(string table, int id)
        {
            connection.Open();
            switch (table.ToLower())
            {
                case "hall":
                    command = new SqlCommand("DELETE FROM Hall WHERE Id = @id", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("DELETE FROM Showtime WHERE Id = @id", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("DELETE FROM Ticket WHERE Id = @id", connection);
                    break;
                case "movie":
                    command = new SqlCommand("DELETE FROM Movie WHERE Id = @id", connection);
                    break;
                case "seat":
                    command = new SqlCommand("DELETE FROM Seat WHERE Id = @id", connection);
                    break;
            }
            command.Parameters.AddWithValue("@Id", id);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<int> GetIds(string table)
        {
            connection.Open();
            List<int> ids = new List<int>();
            switch (table)
            {
                case "hall":
                    command = new SqlCommand("SELECT Id FROM Hall", connection);
                    break;
                case "seat":
                    command = new SqlCommand("SELECT Id FROM Seat", connection);
                    break;
                case "movie":
                    command = new SqlCommand("SELECT Id FROM Movie", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("SELECT Id FROM Showtime", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("SELECT Id FROM Ticket", connection);
                    break;
            }

            command.CommandType = CommandType.Text;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ids.Add(Convert.ToInt32(reader["Id"].ToString()));
                }
            }
            connection.Close();
            return ids;
        }

        public string GetSpecificRow(string table, int sign, int id)
        {
            connection.Open();
            string row = "";
            switch (table)
            {
                case "hall":
                    command = new SqlCommand("SELECT * FROM Hall WHERE Id = @id", connection);
                    break;
                case "seat":
                    command = new SqlCommand("SELECT * FROM Seat WHERE Id = @id", connection);
                    break;
                case "movie":
                    command = new SqlCommand("SELECT * FROM Movie WHERE Id = @id", connection);
                    break;
                case "showtime":
                    command = new SqlCommand("SELECT * FROM Showtime WHERE Id = @id", connection);
                    break;
                case "ticket":
                    command = new SqlCommand("SELECT * FROM Ticket WHERE Id = @id", connection);
                    break;
            }
            command.Parameters.AddWithValue("@sign", sign);
            command.Parameters.AddWithValue("@id", id);
            command.CommandType = CommandType.Text;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    row = reader.GetValue(sign).ToString();
                }
            }
            connection.Close();
            return row;
        }

        public void InsertData(string first, int second, bool third)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Hall(Type, Number, ThreeD) VALUES(" +
                        "@type, @number, @threeD" +
                        ")", connection);
            command.Parameters.AddWithValue("@type", first);
            command.Parameters.AddWithValue("@number", second);
            command.Parameters.AddWithValue("@threeD", third);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void InsertData(string first, string second, string third, bool fourth, int fifth)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Movie(Name, Category, Language, Subtitles, Duration) VALUES(" +
                        "@Name, @category, @language, @subtitles, @duration" +
                        ")", connection);
            command.Parameters.AddWithValue("@Name", first);
            command.Parameters.AddWithValue("@category", second);
            command.Parameters.AddWithValue("@language", third);
            command.Parameters.AddWithValue("@subtitles", fourth);
            command.Parameters.AddWithValue("@duration", fifth);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void InsertData(int first, int second, int third, bool fourth)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Seat(HallId, Row, Number, Busy) VALUES(" +
                        "@hallId, @row, @number, @busy" +
                        ")", connection);
            command.Parameters.AddWithValue("@hallId", first);
            command.Parameters.AddWithValue("@row", second);
            command.Parameters.AddWithValue("@number", third);
            command.Parameters.AddWithValue("@busy", fourth);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void InsertData(int first, DateTime second, int third)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Showtime(MovieId, Date, HallId) VALUES(" +
                        "@movieId, @date, @hallId" +
                        ")", connection);
            command.Parameters.AddWithValue("@movieId", first);
            command.Parameters.AddWithValue("@date", second);
            command.Parameters.AddWithValue("@hallId", third);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void InsertData(double first, int second, int third)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Ticket(Price, ShowtimeId, SeatId) VALUES(" +
                        "@price, @showtimeId, @seatId" +
                        ")", connection);
            command.Parameters.AddWithValue("@price", first);
            command.Parameters.AddWithValue("@showtimeId", second);
            command.Parameters.AddWithValue("@seatId", third);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
