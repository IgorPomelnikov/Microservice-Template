﻿using System;
using AutoMapper;
using Calabonga.AspNetCore.Controllers.Base;
using $ext_projectname$.Entities;
using $safeprojectname$.ViewModels.LogViewModels;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$.Mediator.LogsReadonly
{
    /// <summary>
    /// Request for Log by Identifier
    /// </summary>
    public class LogGetByIdRequest : OperationResultRequestBase<LogViewModel>
    {
        public LogGetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    /// <summary>
    /// Response for  Request for Log by Identifier
    /// </summary>
    public class LogGetByIdRequestHandler : OperationResultRequestHandlerBase<LogGetByIdRequest, LogViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LogGetByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<OperationResult<LogViewModel>> Handle(LogGetByIdRequest request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var operation = OperationResult.CreateResult<LogViewModel>();
            var repository = _unitOfWork.GetRepository<Log>();
            var entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            if (entityWithoutIncludes == null)
            {
                operation.AddError(new MicroserviceNotFoundException($"Entity with identifier {id} not found"));
                return operation;
            }
            operation.Result = _mapper.Map<LogViewModel>(entityWithoutIncludes);
            return operation;
        }
    }
}
