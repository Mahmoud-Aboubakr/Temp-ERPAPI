using Application.Contracts.Handlers;
using Application.Contracts.Models;
using Application.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Application.Helpers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MessageHandler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetMessageValueAr(string key)
        {
            var rootPath = _hostingEnvironment.ContentRootPath; 

            var fullPathAr = Path.Combine(rootPath, "../Api/appMessageAr.json"); 

            var jsonDataAr = File.ReadAllText(fullPathAr);
            if (string.IsNullOrWhiteSpace(jsonDataAr))
                return null;
            var dataAr = JsonConvert.DeserializeObject<List<AppMessage>>(jsonDataAr);
            var resultAr = dataAr.Where(x => x.Key == key).Select(x => x.Value).ToList();
            return resultAr[0].ToString();
        }

        public string GetMessageValueEn(string key)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;

            var fullPathEn = Path.Combine(rootPath, "../Api/appMessageEn.json");

            var jsonDataEn = File.ReadAllText(fullPathEn);
            if (string.IsNullOrWhiteSpace(jsonDataEn))
                return null;

            var dataEn = JsonConvert.DeserializeObject<List<AppMessage>>(jsonDataEn);
            var resultEn = dataEn.Where(x => x.Key == key).Select(x => x.Value).ToList();
            return resultEn[0].ToString();
        }
    }
}
