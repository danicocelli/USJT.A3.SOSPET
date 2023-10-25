using PROJETO.A3.USJT.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_RECURSO")]
    public class Recurso
    {
        [Key]
        [Column("ID_RECURSO")]
        public String? RecursoId { get; set; }

        [Column("NOME_RECURSO")]
        public String? NomeRecurso { get; set; }

        [Column("CATEGORIA_RECURSO")]
        public CategoriaRecurso Categoria { get; set; }

        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

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
