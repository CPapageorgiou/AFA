using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        // Displays the home page view before submitting inout.
        public IActionResult Index()
        {
            return View();
        }


        // Displays the Tree partial view after submitting input. It also checks whether the input is valid.
        [HttpPost]
        public IActionResult Tree(string inputWord, string initialState, List<char> letters, List<string> states, List<string> formulas, List<bool> isFinal
            )
        {
            int count = 0;
            int statesCount = 0;
            var finalStates = new List<string>();
            var transitionFun = new Dictionary<(string, char), string>();

            states = states.Select(state => state.ToLower()).ToList();
            formulas = formulas.Select(formula => formula.ToLower()).ToList();
            letters = letters.Select(letter => Char.ToLower(letter)).ToList();
            inputWord = inputWord.ToLower();
            initialState = initialState.ToLower();

            if (states.Distinct().Count() == states.Count() && letters.Distinct().Count() == letters.Count())
            {

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
                            var (s, d) = p.generateComputation();
                            var tree = p.generateTree(0, d, s, initialState);
                            tree.setConnectives();
                            return PartialView("Tree", tree);
                        }

                        catch
                        {
                            ViewData["Error"] = "Your Input is invalid. Please make sure that you have followed the instructions.";
                            return PartialView("Tree");
                        }
                    }

                    else
                    {
                        ViewData["Error"] = "Your Input is invalid. The input word must contain only letters from the alphabet.";
                        return PartialView("Tree");
                    }
                }

                else
                {
                    ViewData["Error"] = "Your Input is invalid. The initial state must be contained in the transition function.";
                    return PartialView("Tree");
                }
            }

            else
            {
                ViewData["Error"] = "Your Input is invalid. Each state and each letter must be contained only once in the transition function.";
                return PartialView("Tree");
            }
        }

        // Displays the view for the tutorial page.
        public IActionResult Tutorial()
        {
            return View();
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
