using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_EVENTO")]
    public class Evento
    {
        [Key]
        [Column("ID_EVENTO")]
        public String EventoId { get; set; }

        [Column("TITULO")]
        public String Titulo { get; set; }

        [Column("SUBTITULO")]
        public String DescBreve { get; set; }

        [Column("DATA_EVENTO")]
        public DateTime DataEvento { get; set; }

        [Column("DESCRICAO")]
        public String Descricao { get; set; }

        [Column("DIRETORIO_IMG")]
        public String ImageDirectory { get; set; }

        [Column("DATA_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Column("ENDERECO")]
        public String Endereco { get; set;}
    }
}
