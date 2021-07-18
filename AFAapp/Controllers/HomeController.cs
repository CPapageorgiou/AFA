using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AFAapp.Models;

namespace AFAapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Tree()
        {
            var p = new Models.ProgramManager();

            var (s, d) = p.activateAut("bba", Global.a1);
            var tree = p.generateTree(0, d, s);
            tree.setConnectives();
            return View(tree);

        }


        [HttpPost]
        public IActionResult Tree2(AFA a)
        {
            var p = new Models.ProgramManager();

            var (s, d) = p.activateAut("bba", a);
            var tree = p.generateTree(0, d, s);
            tree.setConnectives();
            return View(tree);

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
