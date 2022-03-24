using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkMarchFourTeen.Data
{
    public class DataBaseForSimcha
    {
        private string _connectionString;

        public DataBaseForSimcha(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddSimcha(Simcha simcha)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Simcha VALUES (@Name, @Date)";
            cmd.Parameters.AddWithValue("@Name", simcha.Name);
            cmd.Parameters.AddWithValue("@Date", simcha.Date);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public int ContributorsCountToThisOne(int Id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM Contributors c
                                JOIN ConributersFofTheSimcha cfts
                                on c.Id = cfts.ContributorsId
                                JOIN Simcha s
                                on s.Id=cfts.SimchaId
                                WHERE s.Id=@Id";
            cmd.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            int var= (int)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return var;
        }
        public int ContributorsCountTotal()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM Contributors";
            connection.Open();
            int var = (int)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return var;
        }
        public decimal GetTotalContributed(int Id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(Amount),0) FROM ConributersFofTheSimcha
                                where SimchaId=@Id";
            cmd.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            decimal var = (decimal)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return var;
        }
        public List<Simcha> CreateSimchaHomePage()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Select * from Simcha";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Simcha> simchaList = new List<Simcha>();
            while (reader.Read())
            {
                int contributorsCountToThisOne = 0;
                int contributorsCountTotal = 0;
                if (ContributorsCountToThisOne((int)reader["Id"]) != null)
                {
                    contributorsCountToThisOne = ContributorsCountToThisOne((int)reader["Id"]);
                }
                if (ContributorsCountTotal() != null)
                {
                    contributorsCountTotal = ContributorsCountTotal();
                }
                simchaList.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    ContributorsCountToThisOne = contributorsCountToThisOne,
                    ContributorsCountTotal = contributorsCountTotal,
                    Total = GetTotalContributed((int)reader["Id"])
                }); ;
                
            }
            connection.Close();
            connection.Dispose();
            return simchaList;
        }
        public List<string> AreTheyContributing(int simchaId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT c.FirstName, c.LastName FROM Contributors c
                                JOIN ConributersFofTheSimcha cfts
                                on c.Id = cfts.ContributorsId
                                JOIN Simcha s
                                on s.Id=cfts.SimchaId
                                Where s.Id=@simchaId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> peopleContributed = new List<string>();
            while (reader.Read())
            {
                peopleContributed.Add($"{(string)reader["FirstName"]} {(string)reader["LastName"]}");

            }
            connection.Close();
            connection.Dispose();
            return peopleContributed;
        }
        public decimal GetAmount(int simchaId, int contributionId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(Amount,0) As Amount FROM ConributersFofTheSimcha WHERE SimchaId=@simchaId AND ContributorsId = @contributionId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            cmd.Parameters.AddWithValue("@contributionId", contributionId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            decimal? var=0;
            while (reader.Read())
            {
                var = (reader.GetOrNull<decimal?>("Amount"));
                if (reader.GetOrNull<decimal?>("Amount")==null)
                {
                    var = 0;
                }
            }
            //decimal var = ((decimal)cmd.ExecuteScalar() != null)? (decimal)cmd.ExecuteScalar():0;
            //decimal var = ((decimal)cmd.ExecuteScalar());
            connection.Close();
            connection.Dispose();
            return (decimal)var;
        }
        public List<ContributorsForTheSimcha> GetContributorsNameAndBalance(int Id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Select c.id ,ISNULL(SUM(d.Deposit),0) AS Deposit, ISNULL(SUM(cfts.Amount),0) AS Amount from Contributors c
                                LEFT JOIN ConributersFofTheSimcha cfts
                                on c.Id = cfts.ContributorsId
                                LEFT JOIN Simcha s
                                on s.Id=cfts.SimchaId
                                LEFT JOIN Deposits d
                                on d.ContributorsId =c.Id
								group by c.id";
            cmd.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<ContributorsForTheSimcha> contributorsList = new List<ContributorsForTheSimcha>();
            while (reader.Read())
            {
                contributorsList.Add(new ContributorsForTheSimcha
                {
                    ContributorId = (int)reader["Id"],
                    Balance = (decimal)reader["Deposit"] - (decimal)reader["Amount"],
                    //Amount= (decimal)reader["Amount"]
                    //SimchaId = Id
                });
            }
            connection.Close();
            connection.Dispose();
            return GetContributorsNameAndBalancehalf(Id, contributorsList);
        }
        public List<ContributorsForTheSimcha> GetContributorsNameAndBalancehalf(int Id, List<ContributorsForTheSimcha> contributorsForTheSimcha)
        {
            List<ContributorsForTheSimcha> contributorsList = new List<ContributorsForTheSimcha>();
            foreach (ContributorsForTheSimcha c in contributorsForTheSimcha)
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"Select top 1 c.*,ISNULL(d.Deposit,0) AS Deposit, d.DateDeposit, ISNULL(cfts.Amount,0) AS Amount, cfts.DateContibuted from Contributors c
                                LEFT JOIN ConributersFofTheSimcha cfts
                                on c.Id = cfts.ContributorsId
                                LEFT JOIN Simcha s
                                on s.Id=cfts.SimchaId
                                LEFT JOIN Deposits d
                                on d.ContributorsId =c.Id
								Where c.id=@Id";
                cmd.Parameters.AddWithValue("@Id", c.ContributorId);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    contributorsList.Add(new ContributorsForTheSimcha
                    {
                        ContributorId = (int)reader["Id"],
                        Contribute = AreTheyContributing(Id).Contains($"{(string)reader["FirstName"]} {(string)reader["LastName"]}"),
                        Name = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}",
                        AlwaysInclude = (bool)reader["ShouldAlwaysBeIncluded"],
                        Balance = c.Balance,
                        Amount = GetAmount(Id, ((int)reader["Id"])),
                        //Amount=c.Amount,
                        SimchaId = Id
                    });
                }
                connection.Close();
                connection.Dispose();
            }
            return contributorsList;
        }
        public string NameOfSimcha(int SimchaId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Select Name FROM Simcha
                                WHERE Id=@SimchaId";
            cmd.Parameters.AddWithValue("@SimchaId", SimchaId);
            connection.Open();
            string var = (string)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return var;
        }
        public void UpdateContributersForASimcha(List<ContributorsForTheSimcha> contributorsForTheSimchas, int simchaId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Delete from ConributersFofTheSimcha
                                WHERE SimchaId=@Id";
            cmd.Parameters.AddWithValue("@Id", simchaId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
            foreach (ContributorsForTheSimcha c in contributorsForTheSimchas)
            {
                if (c.shouldInclude)
                {
                    SqlConnection connection2 = new SqlConnection(_connectionString);
                    SqlCommand cmd2 = connection2.CreateCommand();
                    cmd2.CommandText = @"INSERT INTO ConributersFofTheSimcha(ContributorsId, SimchaId, Amount)
                                        VALUES(@ContributorsId,@SimchaId,@Amount)";
                    cmd2.Parameters.AddWithValue("@ContributorsId", c.ContributorId);
                    cmd2.Parameters.AddWithValue("@SimchaId", c.SimchaId);
                    cmd2.Parameters.AddWithValue("@Amount", c.Amount);
                    connection2.Open();
                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    connection2.Close();
                    connection2.Dispose();
                }
            }
        }

    }
}
