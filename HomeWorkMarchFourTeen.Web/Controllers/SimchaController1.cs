using HomeWorkMarchFourTeen.Data;
using HomeWorkMarchFourTeen.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWorkMarchFourTeen.Web.Controllers
{
    public class SimchaController : Controller
    {
        string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ALlMyProjects;Integrated Security=true;";
        public IActionResult Index()
        {
            DataBaseForSimcha dataBase = new DataBaseForSimcha(_connectionString);
            IndexViewModelSimcha indexViewModelSimcha = new IndexViewModelSimcha();
            indexViewModelSimcha.ListOfSimcha=dataBase.CreateSimchaHomePage();
            return View(indexViewModelSimcha);
        }
        public IActionResult newSimcha(Simcha simcha)
        {
            DataBaseForSimcha dataBase = new DataBaseForSimcha(_connectionString);
            dataBase.AddSimcha(simcha);
            return Redirect("/Simcha");
        }
        public IActionResult contributions(int simchaid)
        {
            DataBaseForSimcha dataBase = new DataBaseForSimcha(_connectionString);
            ContributionsViewModel contributionsViewModel = new ContributionsViewModel();
            contributionsViewModel.contributorsForTheSimchas = dataBase.GetContributorsNameAndBalance(simchaid);
            contributionsViewModel.Name = dataBase.NameOfSimcha(simchaid);
            return View(contributionsViewModel);
        }
        public IActionResult Update(List<ContributorsForTheSimcha> ContributorsForTheSimcha)
        {
            DataBaseForSimcha dataBase = new DataBaseForSimcha(_connectionString);
            dataBase.UpdateContributersForASimcha(ContributorsForTheSimcha, ContributorsForTheSimcha[0].SimchaId);
            return Redirect("/Simcha");
        }
    }
}

