namespace CrudDapper.Dto
{
    public class CriarUsuarioDto
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string CPF { get; set; }
        public double Salario { get; set; }
        public string Senha { get; set; }
        public bool Situacao { get; set; } //1 : ativo, 0 : Inativo
    }
}
