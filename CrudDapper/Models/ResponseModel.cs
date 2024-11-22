namespace CrudDapper.Models
{
    public class ResponseModel<T>
    {
        public T? Dados { get; set; }
        public string Mensagen { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
