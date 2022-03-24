using HomeWorkMarchFourTeen.Data;
using HomeWorkMarchFourTeen.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWorkMarchFourTeen.Web.Controllers
{
    public class HomeController : Controller
    {
            string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ALlMyProjects;Integrated Security=true;";
            public IActionResult Index()
            {
                return View();
            }
            public IActionResult newSimcha(Simcha simcha)
            {
                DataBaseForSimcha dataBase = new DataBaseForSimcha(_connectionString);
                dataBase.AddSimcha(simcha);
                return Redirect("/Simcha");
            }
        
    }
}
