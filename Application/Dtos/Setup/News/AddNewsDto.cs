
namespace Application.Dtos.News
{
    public class AddNewsDto
    {
        public DateTime ActivateFrom { get; set; }
        public DateTime ActivateTo { get; set; }
        public string NewsTextAr { get; set; }
        public string NewsTextEn { get; set; }
    }
}
