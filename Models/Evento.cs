using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_EVENTO")]
    public class Evento : IUserDateLog
    {
        [Key]
        [Column("ID_EVENTO")]
        public String? EventoId { get; set; }

        [Required]
        [Column("TITULO")]
        public String Titulo { get; set; }

        [Required]
        [Column("SUBTITULO")]
        public String? DescBreve { get; set; }

        [Column("DATA_EVENTO")]
        public DateTime DataEvento { get; set; }

        [Required]
        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

        [Column("DIRETORIO_IMG")]
        public String? ImageDirectory { get; set; }

        [Column("ENDERECO")]
        public String? Endereco { get; set;}

        [Required]
        [Column("PUBLICAR_PAG")]
        public Boolean Publicar { get; set; }

        [Required]
        [Column("PROPIO")]
        public Boolean Proprio { get; set; }

        [Required]
        [Column("ID_VOLUNTARIO")]
        public String VoluntarioId { get; set; }

        [ForeignKey("VoluntarioId")]
        public Voluntario Voluntario { get; set; }

        [Column("USUARIO_INCLUSAO")]
        public String? UsuarioInclusao { get; set; }

        [Column("DATA_INCLUSAO")]
        public DateTimeOffset DataInclusao { get; set; }

        [Column("USUARIO_ALTERACAO")]
        public String? UsuarioAlteracao { get; set; }

        [Column("DATA_ALTERACAO")]
        public DateTimeOffset DataAlteracao { get; set; }
    }
}
