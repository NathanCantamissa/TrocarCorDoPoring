using Microsoft.AspNetCore.Mvc;
using TrocarCorDoPoring.Models;
using TrocarCorDoPoring.Servicos.Interfaces;

namespace TrocarCorDoPoring.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ILightsOutSolver _desligaPoringService;

        public HomeController(ILightsOutSolver desligaPoringService, ILogger<HomeController> logger)
        {
            _desligaPoringService = desligaPoringService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() => View(new PuzzleInput());

        [HttpPost]
        public IActionResult Index(PuzzleInput input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var solution = _desligaPoringService.Resolver(input.Verdes);
            return View("Solution", solution);
        }
    }
}
