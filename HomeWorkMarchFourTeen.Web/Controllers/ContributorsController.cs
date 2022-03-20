using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWorkMarchFourTeen.Web.Controllers
{
    public class ContributorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
