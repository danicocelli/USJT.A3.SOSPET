using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using NuGet.Protocol.Core.Types;
using PROJETO.A3.USJT.Models;
using PROJETO.A3.USJT.Models.Enums;
using PROJETO.A3.USJT.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            var animais = _context.Animal.ToList();

            var counterQuery = LoadChartCounterDashboard(animais);
            var chart15DaysQuery = LoadChart15Days();
            var recurso = LoadRecursosChart();
            var efetivoOng = LoadChartEfetivoOng();
            var novosDados = LoadNovoPeriodo();
            var eventos = LodEventos();
            var dashboardData = new Dashboard()
            {
                PetsOng = counterQuery.PetsOng,
                PetsDoados = counterQuery.PetsDoados,
                CachorrosContagem = counterQuery.CachorrosContagem,
                GatosContagem = counterQuery.GatosContagem,
                Acolhidos15DaysChart = chart15DaysQuery.Acolhidos15DaysChart,
                Doados15DaysChart = chart15DaysQuery.Doados15DaysChart,
                Datas15DaysChart = chart15DaysQuery.Datas15DaysChart,
                Zerados = recurso.Zerados,
                Criticos = recurso.Criticos,
                Baixos = recurso.Baixos,
                Oks = recurso.Oks,
                Completos = recurso.Completos,
                EfetivoOng = efetivoOng.EfetivoOng,
                NovosRecursos = novosDados.NovosRecursos,
                NovosPetsA = novosDados.NovosPetsA,
                NovosPetsD = novosDados.NovosPetsD,
                NovosVoluntarios = novosDados.NovosVoluntarios,
                ProximosEventos =eventos.ProximosEventos
               
            };
            ViewBag.ProximosEventos = dashboardData.ProximosEventos;
            return dashboardData != null ?
                         View(dashboardData) :
                         Problem("Não foi possível carregar a DashBoard");
        }

        private static Dashboard LoadChartCounterDashboard(List<Animal> animais)
        {

            return new Dashboard()
            {
                PetsOng = animais.Count(x => x.SituacaoAnimal != SituacaoAnimal.Adotado),
                PetsDoados = animais.Count(x => x.SituacaoAnimal == SituacaoAnimal.Adotado),
                CachorrosContagem = animais.Count(x => CategoriaAnimal.Cachorro.Equals(x.CategoriaAnimal)),
                GatosContagem = animais.Count(x => CategoriaAnimal.Gato.Equals(x.CategoriaAnimal))
            };
        }

        private Dashboard LoadRecursosChart()
        {
            var categorias = Enum.GetValues(typeof(CategoriaRecurso)).Cast<CategoriaRecurso>();
            var recursos = _context.Recurso.ToList();

            var contagemPorZerado = recursos
                .Where(x => x.Situacao == SituacaoRecurso.Zerado)
                .GroupBy(x => x.Categoria)
                .Select(g => new { Categoria = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Categoria, x => x.Count);

            var contagemPorCritico = recursos
                .Where(x => x.Situacao == SituacaoRecurso.Crítico)
                .GroupBy(x => x.Categoria)
                .Select(g => new { Categoria = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Categoria, x => x.Count);

            var contagemPorBaixo = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Baixo)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);

            var contagemPorOk = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Ok)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);

            var contagemPorCompleto = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Completo)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);


            var dash = new Dashboard()
            {
                Zerados = categorias.Select(categoria => contagemPorZerado.ContainsKey(categoria) ? contagemPorZerado[categoria] : 0).ToList(),
                Criticos = categorias.Select(categoria => contagemPorCritico.ContainsKey(categoria) ? contagemPorCritico[categoria] : 0).ToList(),
                Baixos = categorias.Select(categoria => contagemPorBaixo.ContainsKey(categoria) ? contagemPorBaixo[categoria] : 0).ToList(),
                Oks = categorias.Select(categoria => contagemPorOk.ContainsKey(categoria) ? contagemPorOk[categoria] : 0).ToList(),
                Completos = categorias.Select(categoria => contagemPorCompleto.ContainsKey(categoria) ? contagemPorCompleto[categoria] : 0).ToList()
            };

            return dash;
        }

        private Dashboard LoadChart15Days()
        {
            List<string> last15Days = new List<string>();

            var startDate = DateTimeOffset.Now.AddDays(-15);
            var endDate = DateTimeOffset.Now;

            for (int i = -15; i <= 0; i++)
            {
                var date = DateTimeOffset.Now.AddDays(i);
                last15Days.Add(date.ToString("dd/MM/yyyy"));
            }

            var dateRange = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                .Select(offset => startDate.AddDays(offset).Date);

            var countAcolhidosPerDay = dateRange
                .GroupJoin(
                    _context.Animal
                        .Where(x => x.DataAcolhimento >= startDate && x.DataAcolhimento <= endDate)
                        .GroupBy(x => x.DataAcolhimento.Date)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Count()

                        }),
                    date => date,
                    animalGroup => animalGroup.Date,
                    (date, animalGroup) => animalGroup.DefaultIfEmpty(new { Date = date, Count = 0 })
                )
                .Select(group => group.Sum(g => g.Count))
                .ToList();

            var countDoadosPerDay = dateRange
                .GroupJoin(
                    _context.Animal
                        .Where(x => x.DataDoacao >= startDate && x.DataAcolhimento <= endDate)
                        .GroupBy(x => x.DataAcolhimento.Date)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Count()

                        }),
                    date => date,
                    animalGroup => animalGroup.Date,
                    (date, animalGroup) => animalGroup.DefaultIfEmpty(new { Date = date, Count = 0 })
                )
                .Select(group => group.Sum(g => g.Count))
                .ToList();

            var dash = new Dashboard()
            {
                Acolhidos15DaysChart = countAcolhidosPerDay,
                Doados15DaysChart = countDoadosPerDay,
                Datas15DaysChart = last15Days
            };

            return dash;
        }
        
        private Dashboard LoadChartEfetivoOng()
        {
           var efetivoOng = _context.Voluntario.ToList();

           decimal total = efetivoOng.Count();
           decimal ativos = efetivoOng.Where(x => SituacaoVoluntario.Ativo.Equals(x.Situacao)).Count();


            var percentOng = (ativos / total) * 100;

            var dash = new Dashboard()
            {
                EfetivoOng = (int)percentOng
            };
            return dash;
        }

        private Dashboard LoadNovoPeriodo()
        {
            var novosVoluntarios = _context.Voluntario.Where(x => x.DataInclusao >= DateTime.Now.AddMonths(-1)).Count();
            var novosAcolhidos = _context.Animal.Where(x => x.DataAcolhimento >= DateTime.Now.AddDays(-7)).Count();
            var novosDoados = _context.Animal.Where(x => x.DataDoacao >= DateTime.Now.AddDays(-7)).Count();
            var novosRecursos = _context.Recurso.Where(x => x.DataInclusao >= DateTime.Now.AddMonths(-1)).Count();

            var dash = new Dashboard()
            {
                NovosVoluntarios = novosVoluntarios,
                NovosPetsA = novosAcolhidos,
                NovosPetsD = novosDoados,
                NovosRecursos = novosRecursos
            };
            return dash;
        }

        private Dashboard LodEventos()
        {
            var eventos = _context.Evento.Where(x => x.DataEvento >= DateTime.Now).ToList();

            var listEventos = new List<Evento>();

            var dash = new Dashboard();
            if(eventos != null) eventos.ForEach(x =>listEventos.Add(x));

            dash.ProximosEventos = listEventos;
            return dash;
        }
             
    }

}

