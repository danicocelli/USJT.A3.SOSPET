using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PROJETO.A3.USJT.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }

        public String? NomeAnimal { get; set; }

        [DefaultValue(0)]
        public int Idade { get; set; }

        [DefaultValue("S")]
        public String Disponivel { get; set; }

        public String CategoriaAnimalId { get; set; }
        
        public CategoriaAnimal CategoriaAnimal { get; set; }


    }
}
