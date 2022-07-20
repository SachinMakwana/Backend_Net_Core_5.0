namespace BACKEND_HTML_DOT_NET.Models
{
    public class ServiceResponse<T>
    {
        public T data { get; set; }
        public string status_code { get; set; }
        public string message { get; set; }
    }
}
