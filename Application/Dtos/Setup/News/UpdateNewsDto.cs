
namespace Application.Dtos.News
{
    public class UpdateNewsDto
    {
        public int Id { get; set; }
        public DateTime ActivateFrom { get; set; }
        public DateTime ActivateTo { get; set; }
        public string NewsTextAr { get; set; }
        public string NewsTextEn { get; set; }
        public bool IsActive { get; set; }
    }
}
