using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWorkMarchFourTeen.Data;
using HomeWorkMarchFourTeen.Web.Models;

namespace HomeWorkMarchFourTeen.Web.Controllers
{
    public class ContributorsController : Controller
    {
        string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ALlMyProjects;Integrated Security=true;";
        public IActionResult Index()
        {
            DataBase dataBase = new DataBase(_connectionString);
            IndexViewModel indexViewModel = new IndexViewModel();
            indexViewModel.ListOfContributors = dataBase.GetContributors();
            return View(indexViewModel);
        }
        public IActionResult NewContributor(Contributors contributor, decimal Deposit, DateTime Date)
        {
            DataBase dataBase = new DataBase(_connectionString);
            dataBase.AddContributors(contributor, Deposit, Date);
            return Redirect("/Contributors");
        }
        public IActionResult AddDeposit(int contributorId, decimal Deposit, DateTime Date)
        {
            DataBase dataBase = new DataBase(_connectionString);
            dataBase.AddTheDepositPart(contributorId, Deposit, Date);
            return Redirect("/Contributors");
        }
        public IActionResult edit(Contributors contributor, DateTime Date)
        {
            DataBase dataBase = new DataBase(_connectionString);
            dataBase.UpdateContributors(contributor, Date);
            return Redirect("/Contributors");
        }
        public IActionResult history(int contribid, string name, decimal Balance)
        {
            DataBase dataBase = new DataBase(_connectionString);
            HistoryViewModel historyViewModel = new HistoryViewModel();
            historyViewModel.ListOfHistory= dataBase.GetHistory(contribid);
            historyViewModel.Name = name;
            historyViewModel.Balance = Balance;
            return View(historyViewModel);
        }
    }
}
