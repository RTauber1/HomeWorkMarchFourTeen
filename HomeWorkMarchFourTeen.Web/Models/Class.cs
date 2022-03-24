using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HomeWorkMarchFourTeen.Data;
using HomeWorkMarchFourTeen.Web;



namespace HomeWorkMarchFourTeen.Web.Models
{
    public class IndexViewModel
    {
        public List<Contributors> ListOfContributors { get; set; }
    }
    public class IndexViewModelSimcha
    {
        public List<Simcha> ListOfSimcha { get; set; }
    }
    public class ContributionsViewModel
    {
        public List<ContributorsForTheSimcha> contributorsForTheSimchas { get; set; }
        public string Name { get; set; }
    }
    public class HistoryViewModel
    {
        public List<History> ListOfHistory { get; set; }
        public string Name { get; set; }
        public decimal Balance {get;set;}
    }
}
