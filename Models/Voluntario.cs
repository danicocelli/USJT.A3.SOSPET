using PROJETO.A3.USJT.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_VOLUNTARIO")]
    public class Voluntario : IUserDateLog
    {
        [Key]
        [Column("ID_VOLUNTARIO")]
        public String VoluntarioId { get; set; }

        [Required]
        [Column("NOME")]
        public String? Nome { get; set; }

        [Column("SOBRENOME")]
        public String? Sobrenome { get; set; }

        [Column("CARGO")]
        public String? Cargo { get; set; }

        [Required]
        [Column("EMAIL")]
        public String? Email { get; set; }

        [Required]
        [Column("TELEFONE")]
        public String? Telefone { get; set; }

        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

        [Column("ENDERECO")]
        public String? Endereco { get; set; }

        [Required]
        [Column("SITUACAO")]
        public SituacaoVoluntario Situacao {get; set;}

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
