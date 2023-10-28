using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PROJETO.A3.USJT.Models
{
    [Table("SIS_USUARIO")]
    public class Usuario : IUserDateLog
    {
        [Key]
        [Column("ID_USUARIO")]
        public String UsuarioId { get; set; }

        [Column("USUARIO")]
        public String Username { get; set; }

        [Column("SENHA")]
        public String Senha { get; set; }

        [Column("ATIVO")]
        [DefaultValue("S")]
        public String Ativo { get; set; }

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
