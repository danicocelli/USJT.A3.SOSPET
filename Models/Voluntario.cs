using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_VOLUNTARIO")]
    public class Voluntario
    {
        
        [Column("NOME")]
        public String Nome { get; set; }

        [Column("EMAIL")]
        public String Email { get; set; }

        [Column("TELEFONE")]
        public String Telefone { get; set; }

        [Column("DESCRICAO")]
        public String Descricao { get; set; }
    }
}
