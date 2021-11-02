using doee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using doee.Data;
using System.Linq;
using System.Threading.Tasks;
using doee.Models.doeeViewModels;
//using Microsoft.Extensions.Logging;

namespace doee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DoeeContext _context;

        public HomeController(ILogger<HomeController> logger, DoeeContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<ActionResult> About()
        {
            IQueryable<DataRegistroGroup> data =
                from student in _context.Instituicoes
                group student by student.DataRegistro into dateGroup
                select new DataRegistroGroup()
                {
                    InscricaoDate = dateGroup.Key,
                    Count = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Index()
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
