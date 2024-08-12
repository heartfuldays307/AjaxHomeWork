namespace AjaxDemo.Models
{
    public class User
    {
        public string? userName { get; set; }
        public string? userEmail { get; set; }
        public int? userAge { get; set; }
        public IFormFile? photo { get; set; }
    }
}
