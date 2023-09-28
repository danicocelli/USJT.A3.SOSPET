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
        [Column("ID_ANIMAL")]
        public int AnimalId { get; set; }

        [Required]
        [Column("NOME")]
        public String NomeAnimal { get; set; }

        [Required]
        [Column("DATA_NASC")]
        public DateTimeOffset DataNascimento { get; set; }

        [Required]
        [Column("DATA_ACOLHIMENTO")]
        public DateTimeOffset DataAcolhimento { get; set; }

        [Required]
        [Column("DISPONIVEL")]
        [DefaultValue("S")]
        public String Disponivel { get; set; }

        [Column("DESCRICAO")]
        public String? Descricao { get; set; }

        [Column("LOCAL_ENCONTRO")]
        public String? LocalEncontro { get; set; }

        [Column("OBS_SAUDE")]
        public String? ObservacaoSaude { get; set; }

        [Column("ID_VOLUNTARIO")]
        public String? VoluntarioResponsavelId { get; set; }

        [IgnoreDataMember]
        public String  Voluntario { get; set; }

        [Column("COR")]
        public String? Cor { get; set; }

        [Column("DIR_IMAGEM")]
        public String? DiretorioImagem { get; set; }

        #region enumModel

        [Column("CATEGORIA")]
        public CategoriaAnimal CategoriaAnimal { get; set; }

        [Column("GENERO")]
        public Genero Genero { get; set; }

        #endregion enumModel
    }
}
