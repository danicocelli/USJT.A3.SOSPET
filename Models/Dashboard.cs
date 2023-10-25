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
    }

}

