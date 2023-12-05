using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using PROJETO.A3.USJT.Models.Enums;

namespace PROJETO.A3.USJT.Models
{
    [Table("CAD_ANIMAL")]
    public class Animal 
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_ANIMAL")]
        public String? AnimalId { get; set; }

        [Required]
        [Column("NOME")]
        public String? NomeAnimal { get; set; }

        [Required]
        [Column("DATA_NASCIMENTO")]
        public DateTimeOffset DataNascimento { get; set; }

        [Required]
        [Column("DATA_ACOLHIMENTO")]
        public DateTimeOffset? DataAcolhimento { get; set; }

        [Column("DATA_DOACAO")]
        public DateTimeOffset? DataDoacao { get; set; }
  
        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

        [Column("LOCAL_ENCONTRO")]
        public String? LocalEncontro { get; set; }

        [Column("OBS_SAUDE")]
        public String? ObservacaoSaude { get; set; }

        [Required]
        [Column("ID_VOLUNTARIO")]
        public String VoluntarioId { get; set; }

        [ForeignKey("VoluntarioId")]
        public Voluntario Voluntario { get; set; }

        [Column("COR")]
        public String? Cor { get; set; }

        #region enumModel

        [Required]
        [Column("CATEGORIA")]
        public CategoriaAnimal? CategoriaAnimal { get; set; }

        [Required]
        [Column("GENERO")]
        public Genero? Genero { get; set; }

        [Required]
        [Column("SITUACAO")]
        public SituacaoAnimal? SituacaoAnimal { get; set; }

        #endregion enumModel

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
