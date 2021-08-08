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
            var p = new Models.ProgramManager(inputWord, a);

            if (a.initialIsValid())
            {
                if (p.inputWord == "empty")
                {
                    ViewData["Accepted"] = finalStates.Contains(initialState);
                    ViewData["stringToEval"] = finalStates.Contains(initialState);
                    return PartialView("Tree", new Tree(initialState));

                }
                if (p.inputWordIsValid())
                {

                    try
                    {
                        ViewData["stringToEval"] = p.determineAcceptance().Item1;
                        ViewData["Accepted"] = p.determineAcceptance().Item2;
                        ViewData["inputWord"] = inputWord;
                        var (s, d) = p.activateAut();
                        var tree = p.generateTree(0, d, s, initialState);
                        tree.setConnectives();
                        //return View("Index", tree);
                        return PartialView("Tree", tree);
                    }

                    catch
                    {
                        ViewData["Error"] = "Your Input is invalid, please check again. Make sure you have followed the instructions.";
                        //return View("Index");
                        return PartialView("Tree");
                    }
                }

                else
                {
                    ViewData["Error"] = "Your Input is invalid. The input word must contain only letters from the alphabet.";
                    //return View("Index");
                    return PartialView("Tree");

                }
            }

            else
            {
                ViewData["Error"] = "Your Input is invalid. The initial state must be contained in the transition function.";
                //return View("Index");
                return PartialView("Tree");

            }
        }


        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Tutorial()
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
