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
            Dashboard dash = LoadDashboard(animais);

            return dash != null ?
                         View(dash) :
                         Problem("Não foi possível carregar a DashBoard");
        }

        private static Dashboard LoadDashboard(List<Animal> animais)
        {
            return new Dashboard()
            {
                PetsOng = animais.Count(x => "S".Equals(x.Disponivel)),
                PetsDoados = animais.Count(x => "N".Equals(x.Disponivel)),
                CachorrosContagem = animais.Count(x => CategoriaAnimal.Cachorro.Equals(x.CategoriaAnimal)),
                GatosContagem = animais.Count(x => CategoriaAnimal.Gato.Equals(x.CategoriaAnimal))
            };
        }



    }
}
