namespace PROJETO.A3.USJT.Models
{
    public interface IUserDateLog
    {
        String? UsuarioInclusao { get; set; }
        DateTimeOffset DataInclusao { get; set; }
        String? UsuarioAlteracao { get; set; }
        DateTimeOffset DataAlteracao { get; set; }
    }
}
