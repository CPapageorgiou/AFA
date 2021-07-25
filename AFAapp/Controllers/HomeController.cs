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
        public IActionResult Index(Tree t)
        {
            return View(t);
        }


        [HttpPost]
        public IActionResult Tree1()
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

        //Dictionary<(string, char), string>
        [HttpPost]
        public Dictionary<string, string> Blah (Dictionary<string,string> transitionFun)
        {
            return transitionFun; 
        }

        [HttpPost]
        public IActionResult Tree5 (string inputWord, string initialState, List<char> letters, List<string> states, List<string> formulas, List<bool> isFinal
            )
        {
            int count = 0;
            int statesCount = 0;
            var finalStates = new List<string>();
            var transitionFun = new Dictionary<(string, char), string> ();

            foreach (string state in states)
            {
                if (isFinal[statesCount])
                {
                    finalStates.Add(state);
                }
                statesCount += 1;
                foreach (char letter in letters)
                {
                    
                    transitionFun[(state, letter)] = formulas[count];
                    count += 1;
                }
            }

            AFA a = new AFA(initialState, transitionFun, finalStates);
            if (a.initialIsValid())
            {
                var p = new Models.ProgramManager();
                ViewData["Accepted"] = p.determineAcceptance(inputWord, a);
                ViewData["inputWord"] = inputWord;
                var (s, d) = p.activateAut(inputWord, a);
                var tree = p.generateTree(0, d, s);
                tree.setConnectives();
            }
            return View("Index",tree);
            //return PartialView("Tree",tree);
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
