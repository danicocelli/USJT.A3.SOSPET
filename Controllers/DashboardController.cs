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
            //Função chamada  ao acessar a tela: 
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            var animais = _context.Animal.ToList();

            var counterQuery = LoadChartCounterDashboard(animais);
            var chart15DaysQuery = LoadChart15Days();
            var recurso = LoadRecursosChart();
            var efetivoOng = LoadChartEfetivoOng();
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
                ProximosEventos = eventos.ProximosEventos
               
            };
            ViewBag.ProximosEventos = dashboardData.ProximosEventos;
            return dashboardData != null ?
                         View(dashboardData) :
                         Problem("Não foi possível carregar a DashBoard");
        }

        //Contador de Pets Doados e Acolhidos e Gráfico de Pizza, a query é obtida via parâmetro do construtor
        private static Dashboard LoadChartCounterDashboard(List<Animal> animais)
        {

            return new Dashboard()
            {
                //Contagem conforme condições
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

            //Busca por recursos zerados:
            var contagemPorZerado = recursos
                .Where(x => x.Situacao == SituacaoRecurso.Zerado) //Condição
                .GroupBy(x => x.Categoria) // Agrupamento pela Categoria
                .Select(g => new { Categoria = g.Key, Count = g.Count() }) //Obtenção dos dados
                .ToDictionary(x => x.Categoria, x => x.Count); //Função que aplica os dados em um dicionário

            //Busca por recursos críticos:
            var contagemPorCritico = recursos
                .Where(x => x.Situacao == SituacaoRecurso.Crítico)
                .GroupBy(x => x.Categoria)
                .Select(g => new { Categoria = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Categoria, x => x.Count);

            //Busca por recursos baixos:
            var contagemPorBaixo = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Baixo)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);

            //Busca por recursos Oks:
            var contagemPorOk = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Ok)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);

            //Busca por recursos completos:
            var contagemPorCompleto = recursos
               .Where(x => x.Situacao == SituacaoRecurso.Completo)
               .GroupBy(x => x.Categoria)
               .Select(g => new { Categoria = g.Key, Count = g.Count() })
               .ToDictionary(x => x.Categoria, x => x.Count);

            //Chamada da classe de atribuição de dados:
            var dash = new Dashboard()
            {
                //Busca das informações nos dicionários; Caso não encontre é aplicado 0 a fim de não impactar no gráfico:
                Zerados = categorias.Select(categoria => contagemPorZerado.ContainsKey(categoria) ? contagemPorZerado[categoria] : 0).ToList(),
                Criticos = categorias.Select(categoria => contagemPorCritico.ContainsKey(categoria) ? contagemPorCritico[categoria] : 0).ToList(),
                Baixos = categorias.Select(categoria => contagemPorBaixo.ContainsKey(categoria) ? contagemPorBaixo[categoria] : 0).ToList(),
                Oks = categorias.Select(categoria => contagemPorOk.ContainsKey(categoria) ? contagemPorOk[categoria] : 0).ToList(),
                Completos = categorias.Select(categoria => contagemPorCompleto.ContainsKey(categoria) ? contagemPorCompleto[categoria] : 0).ToList()
            };

            return dash;
        }

        //Gráfico dos últimos 15 dias:
        private Dashboard LoadChart15Days()
        {
            //Obtemos a data inicial e final para passarmos para consulta do banco de dados.
            var startDate = DateTimeOffset.Now.AddDays(-15);
            var endDate = DateTimeOffset.Now;

            //Criamos listas para armazenar os últimos 15 dias.
            List<string> last15Days = new List<string>();
            List<DateTimeOffset> dateRange = new List<DateTimeOffset>();


            //Atribuição do range de dias de hoje, menos 15 dias:
            for (int i = -15; i < 0; i++)
            {
                var date = DateTimeOffset.Now.AddDays(i);
                dateRange.Add(date);
                last15Days.Add(date.ToString("dd/MM/yyyy"));
            }

            //Aqui buscamos no banco de dados através do Entity Framework com uso de lambdas os dados a serem populados no range:
            //Obtenção dos animais acolhidos com data e quantidade acolhida na data:
            var resultadoAcolhidos = _context.Animal
                .Where(d => d.DataAcolhimento >= startDate && d.DataAcolhimento <= endDate)
                .OrderByDescending(d => d.DataAcolhimento)
                .GroupBy(d => d.DataAcolhimento)
                .Select(g => new
                    {
                        Data = g.Key,
                        QtdAcolhida = g.Count(d => d.DataAcolhimento.HasValue)
                      })
                .ToList();

            //Obtenção dos animais doados com data e quantidade doado na data:
            var resultadoDoados = _context.Animal
                .Where(d => d.DataDoacao >= startDate && d.DataDoacao <= endDate)
                .OrderByDescending(d => d.DataDoacao)
                .GroupBy(d => d.DataDoacao)
                .Select(g => new
                {
                    Data = g.Key,
                    QtdDoada = g.Count(d => d.DataDoacao.HasValue)
                })
                .ToList();

            //Chamada da classe de disposição de dados:
            var dash = new Dashboard()
            {
                Acolhidos15DaysChart = new List<int>(),
                Doados15DaysChart = new List<int>(),
                Datas15DaysChart = last15Days
            };


            //Loop do range de datas e aplicação das datas encontradas no range:
            foreach (var date in dateRange)
            {
                var dataAcolhido = resultadoAcolhidos.Where(x => x.Data?.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).FirstOrDefault();
                var dataDoado = resultadoDoados.Where(x => x.Data?.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).FirstOrDefault();

                if (dataAcolhido != null) dash.Acolhidos15DaysChart.Add(dataAcolhido.QtdAcolhida);
                else dash.Acolhidos15DaysChart.Add(0);
                

                if (dataDoado != null) dash.Doados15DaysChart.Add(dataDoado.QtdDoada);
                else dash.Doados15DaysChart.Add(0);
            }
          
            //Atribiução do range para o eixo de datas do gráfico:
            dash.Datas15DaysChart = last15Days;

            return dash;
        }
        
        //Gráfico do voluntários ativos
        private Dashboard LoadChartEfetivoOng()
        {
           //Busca de todos os voluntários:
           var efetivoOng = _context.Voluntario.ToList();

            //Obtém a quantidade total de voluntários:
           decimal total = efetivoOng.Count();

            //Obtém a quantidade de ativos, sem realizar uma busca nova. Apenas reaproveitando da consulta feita anteriormente:
           decimal ativos = efetivoOng.Where(x => SituacaoVoluntario.Ativo.Equals(x.Situacao)).Count();

            //Cálculo do percentual:
            var percentOng = (ativos / total) * 100;

            //Chamada da classe de disposição de dados e atribuição:
            var dash = new Dashboard()
            {
                EfetivoOng = (int)percentOng
            };
            return dash;
        }

        //Próximos Eventos
        private Dashboard LodEventos()
        {
            //Obtenção dos próximos eventos com a condição que a data do evento seja maior ou igual a hoje:
            var eventos = _context.Evento.Where(x => x.DataEvento >= DateTime.Now).ToList();

            //instancia da lista de eventos que não é nula
            var listEventos = new List<Evento>();
            //atribuição com condição:
            if (eventos != null) eventos.ForEach(x => listEventos.Add(x));

            //Chamada da classe de disposição de dados:
            var dash = new Dashboard();
            dash.ProximosEventos = listEventos;
            return dash;
        }
             
    }

}

