using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkMarchFourTeen.Data
{
    public class Contributors
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public DateTime DateCreated { get; set; }
        public bool ShouldAlwaysBeIncluded { get; set; }
        public decimal? Balance { get; set; }
    }
    public class Simcha
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public int? ContributorsCountToThisOne { get; set; }
        public int? ContributorsCountTotal { get; set; }
        public decimal Total { get; set; }
    }
    public class Deposits
    {
        public int? Id { get; set; }
        public int? ContributorsId { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime? Date { get; set; }
    }
    public class ContributorsForTheSimcha
    {
        public int ContributorId { get; set; }
        public bool Contribute { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool AlwaysInclude { get; set; }
        public decimal Amount { get; set; }
        public int SimchaId { get; set; }
        public bool shouldInclude { get; set; }
    }
    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string columnName)
        {
            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }
    }
    public class History
    {
        public string Auction { get; set; }
        public DateTime Date{get;set;}
        public decimal Amount { get; set; }
    }
}
