﻿using System;
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
            command = new SqlCommand("DELETE FROM @table", connection);
            command.Parameters.AddWithValue("@table", table);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ClearData(string table, int id)
        {
            connection.Open();
            command = new SqlCommand("DELETE FROM @table WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@table", table);
            command.Parameters.AddWithValue("@Id", id);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<int> GetIds(string table)
        {
            connection.Open();
            List<int> ids = new List<int>();
            command = new SqlCommand("SELECT Id FROM @table", connection);
            command.Parameters.AddWithValue("@table", table);
            using (var reader = command.ExecuteReader())
            {
                ids.Add(Convert.ToInt32(reader["Id"].ToString()));
            }
            connection.Close();
            return ids;
        }

        public string GetType(string table, int id)
        {
            connection.Open();
            string type = "";
            command = new SqlCommand("SELECT Type FROM @table WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@table", table);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                type = reader["Id"].ToString();
            }
            connection.Close();
            return type;
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
        public void InsertData(int first, int second, int third, bool fourth)
        {
            connection.Open();
            command = new SqlCommand("INSERT INTO Movie(Category, Language, Subtitles, Duration) VALUES(" +
                        "@category, @language, @subtitles, @duration" +
                        ")", connection);
            command.Parameters.AddWithValue("@category", first);
            command.Parameters.AddWithValue("@language", second);
            command.Parameters.AddWithValue("@subtitles", third);
            command.Parameters.AddWithValue("@duration", fourth);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void InsertData(string first, string second, bool third, int fourth)
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
        public void InsertData(int first, int second, int third)
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
