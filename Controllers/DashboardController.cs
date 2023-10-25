using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PROJETO.A3.USJT.Models;
using PROJETO.A3.USJT.Models.Enums;


namespace PROJETO.A3.USJT.Controllers
{
    public class DashboardController : Controller
    {
        private readonly dbSOSPET _context;

        private readonly Animal _animalRepository;


        public DashboardController(dbSOSPET context)
        {
            _context = context;
        }



        // GET: Dashboard
        public async Task<IActionResult> Index()
        {

            var animais = _context.Animal.ToList();

            var counterQuery = LoadChartCounterDashboard(animais);
            var chart15DaysQuery = LoadChart15Days();
            var dashboardData = new Dashboard()
            {
                PetsOng = counterQuery.PetsOng,
                PetsDoados = counterQuery.PetsDoados,
                CachorrosContagem = counterQuery.CachorrosContagem,
                GatosContagem = counterQuery.GatosContagem,
                Acolhidos15DaysChart = chart15DaysQuery.Acolhidos15DaysChart,
                Doados15DaysChart = chart15DaysQuery.Doados15DaysChart,
                Datas15DaysChart = chart15DaysQuery.Datas15DaysChart

            };
               
            return dashboardData != null ?
                         View(dashboardData) :
                         Problem("Não foi possível carregar a DashBoard");
        }

        private static Dashboard LoadChartCounterDashboard(List<Animal> animais)
        {

            return new Dashboard()
            {
                PetsOng = animais.Count(x => "S".Equals(x.Disponivel)),
                PetsDoados = animais.Count(x => "N".Equals(x.Disponivel)),
                CachorrosContagem = animais.Count(x => CategoriaAnimal.Cachorro.Equals(x.CategoriaAnimal)),
                GatosContagem = animais.Count(x => CategoriaAnimal.Gato.Equals(x.CategoriaAnimal)),


            };
        }

        private Dashboard LoadChart15Days()
        {
            var startDate = DateTimeOffset.Now.AddDays(-15);
            var endDate = DateTimeOffset.Now;

            var countAcolhidosPerDay = _context.Animal
                    .Where(x => x.DataAcolhimento >= startDate && x.DataAcolhimento <= endDate)
                    .GroupBy(x => x.DataAcolhimento.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .Select(x => x.Count)
                    .ToList();


            var countDoadosPerDay = _context.Animal
                    .Where(x => x.DataDoacao >= startDate && x.DataDoacao <= endDate)
                    .GroupBy(x => x.DataDoacao.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .Select(x => x.Count) 
                    .ToList();


            var daysWithRecordsDate = _context.Animal
                .Where(x => x.DataAcolhimento >= startDate && x.DataAcolhimento <= endDate)
                .Select(x => x.DataAcolhimento)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var daysWithRecords = new List<string>();
            daysWithRecordsDate.ForEach(x => daysWithRecords.Add(x.ToString("dd/MM/yyyy")));


            var dash = new Dashboard()
            {
                Acolhidos15DaysChart = countAcolhidosPerDay,
                Doados15DaysChart = countDoadosPerDay,
                Datas15DaysChart = daysWithRecords
            };
            
            return dash;
    }
}

}

