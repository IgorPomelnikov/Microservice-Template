﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.EntityFrameworkCore.UnitOfWork;
using Calabonga.EntityFrameworkCore.UnitOfWork.Framework.Factories;
using Calabonga.Microservice.IdentityModule.Data;
using Calabonga.Microservice.IdentityModule.Entities;
using Calabonga.Microservice.IdentityModule.Web.Infrastructure.ViewModels.LogViewModels;
using Calabonga.OperationResultsCore;
using Microsoft.Extensions.Logging;

namespace Calabonga.Microservice.IdentityModule.Web.Infrastructure.Engine.ViewModelFactories
{
    /// <summary>
    /// ViewModel factory for <see cref="Log"/>
    /// </summary>
    public class LogViewModelFactory: ViewModelFactory<Log, LogCreateViewModel, LogUpdateViewModel>
    {
        private readonly IUnitOfWork<ApplicationDbContext, ApplicationUser, ApplicationRole> _context;
        private readonly IMapper _mapper;

        public LogViewModelFactory(
            IUnitOfWork<ApplicationDbContext, ApplicationUser, ApplicationRole> context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public override Task<OperationResult<LogCreateViewModel>> GenerateForCreateAsync()
        {
             var operation = OperationResult.CreateResult<LogCreateViewModel>();
             operation.Result = new LogCreateViewModel()
             {
                 CreatedAt = DateTime.Now,
                 Level = LogLevel.Information.ToString(),
                 Logger = "Demo purposes only",
                 Message = $"This is demo message {DateTime.Now}"
             };
             return Task.FromResult(operation);
        }

        /// <inheritdoc />
        public override async Task<OperationResult<LogUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<LogUpdateViewModel>();
            var entity = await _context.GetRepository<Log>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<LogUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for Log entity. Please see Additional information in DataObject").AddData(new { Identifier = id });

            return operation;
        }
    }
}
