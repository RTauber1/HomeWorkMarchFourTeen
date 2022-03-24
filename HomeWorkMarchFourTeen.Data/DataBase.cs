using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeWorkMarchFourTeen.Data;

namespace HomeWorkMarchFourTeen.Data
{
    public class DataBase
    {
        private string _connectionString;

        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddContributors(Contributors contributor, decimal Deposit, DateTime Date)
        {
            AddToContributePart(contributor);
            AddTheDepositPart(GetTheMostRecentContributerId(), Deposit, Date);
        }
        public void UpdateContributors(Contributors contributor, DateTime Date)
        {
            UpdateContributePart(contributor);
            UpdateDepositPart(contributor.Id, Date);
        }
        public void AddToContributePart(Contributors contributor)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributors VALUES (@FirstName, @LastName, @Cell, @ShouldAlwaysBeIncluded)";
            cmd.Parameters.AddWithValue("@FirstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@Cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@ShouldAlwaysBeIncluded", contributor.ShouldAlwaysBeIncluded);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public int GetTheMostRecentContributerId()
        {
            int Id=0;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 Id FROM Contributors ORDER BY Id DESC";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Id = (int)reader["Id"];
            }
            connection.Close();
            connection.Dispose();
            return Id;
        }
        public void AddTheDepositPart(int Id, decimal Deposit, DateTime Date)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits VALUES(@Id, @Deposit, @Date)";
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Deposit", Deposit);
            cmd.Parameters.AddWithValue("@Date", Date);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public void UpdateContributePart(Contributors contributor)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Contributors SET FirstName=@FirstName, LastName=@LastName, Cell=@Cell, ShouldAlwaysBeIncluded=@ShouldAlwaysBeIncluded
                                Where Id=@Id";
            cmd.Parameters.AddWithValue("@FirstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@Cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@ShouldAlwaysBeIncluded", contributor.ShouldAlwaysBeIncluded);
            cmd.Parameters.AddWithValue("@Id", contributor.Id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public void UpdateDepositPart(int Id, DateTime Date)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Deposits SET DateDeposit=@Date
                                Where Id=1 
                                AND ContributorsId=@Id";
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Date", Date);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public List<Contributors> GetContributors()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Select * from Contributors";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Contributors> contributorsList = new List<Contributors>();
            while (reader.Read())
            {
                contributorsList.Add(new Contributors
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Cell = (string)reader["Cell"],
                    DateCreated= getDateCreated((int)reader["Id"]),
                    ShouldAlwaysBeIncluded = (bool)reader["ShouldAlwaysBeIncluded"],
                    Balance = getDeposit((int)reader["Id"]) - getAmount((int)reader["Id"])
                });
            }
            connection.Close();
            connection.Dispose();
            return contributorsList;
        }
        public decimal getAmount(int ConributerId)
        {
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"Select ISNULL(SUM(amount),0) As Amount from ConributersFofTheSimcha
                                    where ContributorsId = @ID";
                cmd.Parameters.AddWithValue("@Id", ConributerId);
                connection.Open();
                decimal var = (decimal)cmd.ExecuteScalar();
                connection.Close();
                connection.Dispose();
                return var;
            }
        }
        public decimal getDeposit(int ConributerId)
        {
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"Select ISNULL(SUM(d.Deposit),0) As Deposit from Contributors c
                                    LEFT JOIN Deposits d
                                    on d.ContributorsId =c.Id
								    where c.Id=@Id
								    GROUP BY(c.Id)";
                cmd.Parameters.AddWithValue("@Id", ConributerId);
                connection.Open();
                decimal var = (decimal)cmd.ExecuteScalar();
                connection.Close();
                connection.Dispose();
                return var;
            }
        }
        public DateTime getDateCreated(int ConributerId)
        {
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"Select Top 1 DateDeposit from Deposits
                                    where ContributorsId=@Id";
                cmd.Parameters.AddWithValue("@Id", ConributerId);
                connection.Open();
                DateTime var = (DateTime)cmd.ExecuteScalar();
                connection.Close();
                connection.Dispose();
                return var;
            }
        }
        public List<History> GetHistory(int ContributerId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Select s.Name,s.Date, cfts.Amount from ConributersFofTheSimcha cfts
                                JOIN Simcha s
                                on s.Id=cfts.SimchaId
                                JOIN Contributors c
                                on cfts.ContributorsId =c.Id
								where c.Id=@Id";
            cmd.Parameters.AddWithValue("@Id", ContributerId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<History> listOfHistory = new List<History>();
            while (reader.Read())
            {
                listOfHistory.Add(new History
                {
                    Auction = $"Contribution for the {(string)reader["name"]}",
                    Date = (DateTime)reader["Date"],
                    Amount = (decimal)reader["Amount"]
                });
            }
            connection.Close();
            connection.Dispose();
            connection = new SqlConnection(_connectionString);
            cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from Deposits where ContributorsId=@Id";
            cmd.Parameters.AddWithValue("@Id", ContributerId);
            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listOfHistory.Add(new History
                {
                    Auction = "Deposit",
                    Date = (DateTime)reader["DateDeposit"],
                    Amount = (decimal)reader["Deposit"]
                });
            }
            connection.Close();
            connection.Dispose();
            listOfHistory.Sort((h1,h2) => DateTime.Compare(h1.Date,h2.Date));
            return listOfHistory;
        }


    }
}
