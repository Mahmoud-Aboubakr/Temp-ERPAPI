using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Models;
using Application.Specifications.HR.Setup;
using Application.Specifications.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Common
{

    public class FileController : CommonBaseController
	{
        public IWebHostEnvironment _environment { get; set; }

        public FileController(IWebHostEnvironment environment, IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler, 
            IOptions<CustomServiceConfiguration> options) : base(uof, mapper, accessor, responseModelHandler,
                paginatedModelHandler, options)
		{
            _environment = environment;
        }
        [HttpPost("UploadImage")]
        public async Task<IResponseModel> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            //var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine("uploads","images", uniqueFileName);
            var savePath = Path.Combine("wwwroot", filePath); 
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var model = new UploadFileModel { FilePath = filePath };
            return responseModelHandler.GetResponseModel(model, "FILE_UPLOADED", StatusCodes.Status201Created, Lang);
        }
        [HttpPost("UploadFile")]
        public async Task<IResponseModel> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            //var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine("uploads","files", uniqueFileName);
            var savePath = Path.Combine("wwwroot", filePath);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var model = new UploadFileModel { FilePath = filePath };
            return responseModelHandler.GetResponseModel(model, "FILE_UPLOADED", StatusCodes.Status201Created, Lang);
        }
    }
}