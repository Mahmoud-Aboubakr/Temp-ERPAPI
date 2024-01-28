using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.HR.Setup;
using Application.Specifications.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class EmployeeController : CommonBaseController
	{

		public EmployeeController(IUnitOfWork uof, IMapper mapper, 
            IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) 
            : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler , options)
		{
		}

		//// GET: api/Employee
		[HttpGet("GetEmployees")]
		public async Task<IResponseModel> GetEmployees([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Employee> EmployeeRepository = uof.GetRepository<Employee>();
            using (ISpecification<Employee> specifications = new EmployeeSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadEmployeeDto> Employees = mapper.Map<IEnumerable<ReadEmployeeDto>>(await EmployeeRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(Employees, paginationParams.PageNumber, paginationParams.PageSize, await EmployeeRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
			
		}
		[HttpPost("AddEmployee")]
		public async Task<IResponseModel> AddEmployee([FromBody] CreateEmployeeDto CreateEmployee)
		{
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(CreateEmployee, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<Employee> EmployeesRepository = uof.GetRepository<Employee>();

			Employee newEmployee = mapper.Map<Employee>(CreateEmployee);

            newEmployee.IsActive = true;
            newEmployee.CreateDate = DateTime.Now;

			var newId = await EmployeesRepository.InsertWithIdAsync(newEmployee);
            // add employee files 
            foreach(var empolyeeFile in CreateEmployee.EmployeeFiles)
            {
                empolyeeFile.EmployeeId = newId;
                IGenericRepository<HRFile> HRFileRepository = uof.GetRepository<HRFile>();
                var FindFile = await HRFileRepository.GetByIdAsync(empolyeeFile.HRFileId,cts.Token);

                if (FindFile != null)
                {
                    IGenericRepository<EmployeeFiles> EmployeeFilessRepository = uof.GetRepository<EmployeeFiles>();

                    EmployeeFiles newEmployeeFiles = mapper.Map<EmployeeFiles>(empolyeeFile);

                    newEmployeeFiles.IsActive = true;
                    newEmployeeFiles.CreateDate = DateTime.Now;

                    await EmployeeFilessRepository.InsertAsync(newEmployeeFiles, cts.Token);
                   
                }
            }

            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

        // PUT: api/Employee/5
        //[HttpPut("UpdateEmployee/{id}")]
        //public async Task<IResponseModel> UpdateEmployee(int id, [FromBody] UpdateEmployeesDto Employees)
        //{
        //	if (!ModelState.IsValid)
        //		return responseModelHandler.GetResponseModel(Employees, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        //	if (id != Employees.Id)
        //		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        //	IGenericRepository<Employee> EmployeeRepository = uof.GetRepository<Employee>();

        //	Employee updateEmployee = mapper.Map<UpdateEmployeesDto, Employee>(Employees);
        //	updateEmployee.EditDate = DateTime.Now;

        //	EmployeeRepository.Update(updateEmployee);
        //	await uof.Commit();

        //	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        //}

        // POST: api/Employee
        // DELETE: api/Employee/5
        //[HttpDelete("DeleteEmployee/{id}")]
        //public async Task<IResponseModel> DeleteEmployee([FromRoute] int id)
        //{
        //	IGenericRepository<Employee> EmployeeRepository = uof.GetRepository<Employee>();

        //	Employee currentEmployees = await EmployeeRepository.GetByIdAsync(id);

        //	if (currentEmployees is null)
        //		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        //	currentEmployees.DeleteDate = DateTime.Now;
        //	EmployeeRepository.SoftDelete(currentEmployees);
        //	await uof.Commit();

        //	return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        //}
    }
}