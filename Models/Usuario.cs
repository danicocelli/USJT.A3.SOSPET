using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public String UsuarioId { get; set; }

        [Column("NOME")]
        public String Nome { get; set; }

        [Column("EMAIL")]
        public String Email { get; set; }

        [Column("USUARIO")]
        public String Username { get; set; }

        [Column("SENHA")]
        public String Senha { get; set; }

        [Column("ATIVO")]
        [DefaultValue("S")]
        public String Ativo { get; set; }
    }
}
