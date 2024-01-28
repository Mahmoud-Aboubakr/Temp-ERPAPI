using Application.Contracts.Handlers;
using Application.Contracts.IServices;
using Application.Contracts.Persistence;
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Responses;
using AutoMapper;
using Domain.Entities.HR.Setup;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class IdentitificationTypeService : IIdentitificationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseModelHandler _responseModelHandler;
        private readonly IPaginatedModelHandler _paginatedModelHandler;

        public IdentitificationTypeService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IResponseModelHandler responseModelHandler, 
            IPaginatedModelHandler paginatedModelHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseModelHandler = responseModelHandler;
            _paginatedModelHandler = paginatedModelHandler;
        }

        public async Task<IdentityType> AddIdentityTypeAsync(IdentitificationTypeCreationRequestDto request)
        {
            var identityType = _mapper.Map<IdentityType>(request);
            identityType.IsActive = true;
            identityType.CreateDate = DateTime.UtcNow;

            await _unitOfWork.GetRepository<IdentityType>().InsertAsync(identityType);
            await _unitOfWork.Commit();

            return identityType;
        }

        public async Task<IdentityType> UpdateIdentityTypeAsync(IdentitificationTypeUpdateRequestDto request)
        {
            var oldIdentityType = await _unitOfWork.GetRepository<IdentityType>().GetByIdAsync(request.Id);
            if (oldIdentityType is null) throw new Exception($"{request.Id} not found");

            oldIdentityType = _mapper.Map<IdentityType>(request);
            oldIdentityType.EditDate = DateTime.UtcNow; 

            _unitOfWork.GetRepository<IdentityType>().Update(oldIdentityType);
            await _unitOfWork.Commit();

            return oldIdentityType;
        }

        public async Task<IdentitificationTypeResponseDto> GetIdentityTypeByIdAsync(int id)
        {
            var identityType = await _unitOfWork.GetRepository<IdentityType>().GetByIdAsync(id);
            if (identityType is null || identityType.IsDeleted) throw new Exception($"{id} not found");

            var data = _mapper.Map<IdentitificationTypeResponseDto>(identityType);

            return data;
        }

        public async Task DeleteIdentityTypeByIdAsync(int id)
        {
            var identityType = await _unitOfWork.GetRepository<IdentityType>().GetByIdAsync(id, new CancellationToken());
            if (identityType is null) throw new Exception($"{id} not found");

            identityType.DeleteDate = DateTime.UtcNow;
            _unitOfWork.GetRepository<IdentityType>().SoftDelete(identityType);

            await _unitOfWork.Commit();
        }

        public async Task<IReadOnlyList<IdentitificationTypeResponseDto>> GetAllIdentityTypesAsync()
        {
            var identityTypes = await _unitOfWork.GetRepository<IdentityType>().GetAllAsync();

            var data = _mapper.Map<IReadOnlyList<IdentitificationTypeResponseDto>>(identityTypes);

            return data;
        }
    }
}
