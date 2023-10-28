using PROJETO.A3.USJT.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_RECURSO")]
    public class Recurso : IUserDateLog
    {
        [Key]
        [Column("ID_RECURSO")]
        public String? RecursoId { get; set; }

        [Column("NOME_RECURSO")]
        public String? NomeRecurso { get; set; }

        [Column("CATEGORIA_RECURSO")]
        public CategoriaRecurso Categoria { get; set; }

        [Column("SITUACAO_RECURSO")]
        public SituacaoRecurso Situacao { get; set; }

        [Required]
        [Column("DATA_RECEBIMENTO")]
        public DateTimeOffset DataRecebimento { get; set; }

        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

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
