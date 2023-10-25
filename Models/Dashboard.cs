using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [NotMapped]
    public class Dashboard
    {
        public Int32? PetsOng { get; set; }

        public Int32? PetsDoados { get; set; }

        public Int32? PetsAcolhidos { get; set; }

        public Int32? GatosContagem { get; set; }

        public Int32? CachorrosContagem { get; set; }

        //public struct Data
        //{

        //}

    }
}
