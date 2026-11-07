using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrackerEmployeeHours.Models;
using TrackerEmployeeHours.Services;

namespace TrackerEmployeeHours.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimeDataService _timeDataService;
        private readonly PieChartService _pieChartService;


        public HomeController(ILogger<HomeController> logger, TimeDataService timeDataService, PieChartService pieChartService)
        {
            _logger = logger;
            _timeDataService = timeDataService;
            _pieChartService = pieChartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var employees = await _timeDataService.GetEmployeeHoursAsync();
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to load employee data: {ErrorType}",
                    ex.GetType().Name);

                ViewBag.ErrorMessage = "Unable to load employee data. Please check your API key and connection.";

                return View(new List<EmployeeHoursSum>());
            }
        }

        public async Task<IActionResult> PieChart()
        {
            var employees = await _timeDataService.GetEmployeeHoursAsync();

            byte[] chartImage = _pieChartService.GenerateChart(employees);

            return File(chartImage, "image/png");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
