using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [NotMapped]
    public class Dashboard
    {
        public Int32? PetsOng { get; set; }

        public Int32? PetsDoados { get; set; }

        public Int32? PetsAcolhidos { get; set; }

        #region Count Pizza Pizza
        public Int32? GatosContagem { get; set; }

        public Int32? CachorrosContagem { get; set; }
        #endregion Count Pizza

        #region 15 Days Chart
        public List<Int32>? Acolhidos15DaysChart { get; set; }

        public List<Int32>? Doados15DaysChart { get; set; }

        public List<String>? Datas15DaysChart { get; set; }
        #endregion 15 Days Chart


        #region Recurso Chart
        public List<Int32>? Zerados { get; set; }

        public List<Int32>? Criticos { get; set; }

        public List<Int32>? Baixos { get; set; }

        public List<Int32>? Oks { get; set; }

        public List<Int32>? Completos { get; set; }

        #endregion RecursoChart

        #region Quantidade Efetivo Ong

        public int EfetivoOng { get; set; }

        #endregion Quantidade Efetivo Ong

        #region Contadores Novos
        public int NovosVoluntarios { get; set; }

        public int NovosPetsA { get; set; }
        
        public int NovosPetsD { get; set; }

        public int NovosRecursos { get; set; }

        #endregion Contadores Novos

        #region Eventos 
        public List<Evento> ProximosEventos { get; set; }
        #endregion Eventos
    }

}

