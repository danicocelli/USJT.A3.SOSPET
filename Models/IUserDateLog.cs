using PROJETO.A3.USJT.Utils;

namespace PROJETO.A3.USJT.Models
{
    //Interface de auditoria
    public interface IUserDateLog
    {
        String? UsuarioInclusao { get; set; }
        DateTimeOffset DataInclusao { get; set; }
        String? UsuarioAlteracao { get; set; }
        DateTimeOffset DataAlteracao { get; set; }

    }
}
    