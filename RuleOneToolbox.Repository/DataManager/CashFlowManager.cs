using AutoMapper;
using RuleOneToolbox.DTO.DTO;
using RuleOneToolbox.DTO.Models;
using RuleOneToolboxDbContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RuleOneToolbox.Repository.DataManager
{
    public class CashFlowManager
    {
        private readonly RuleOneToolboxDbContext _objDbContext;
        private readonly IMapper _objMapper;
        public CashFlowManager(RuleOneToolboxDbContext dbContext, IMapper mapper)
        {
            _objDbContext = dbContext;
            _objMapper = mapper;
        }

        public CashFlowManager(Microsoft.Extensions.Configuration.IConfigurationRoot configuration)
        {
            _objDbContext = new RuleOneToolboxDbContext(configuration);
        }
        /// <summary>
        /// Clear the DbContext object from memory
        /// </summary>
        public void Dispose()
        {
            _objDbContext?.Dispose();
        }
        /// <summary>
        /// Connecting with database to get cash flow data and  using mapper to convert the modal object to DTO Object
        /// </summary>
        /// <returns></returns>
        public async Task<List<CashFlowDetailsDto>> GetAllCashFlowDetails()
        {
            try
            {
                var cashUnitOfWork = new UnitOfWork<RuleOneToolboxDbContext>(_objDbContext);
                var cashRepository = cashUnitOfWork.GetReadOnlyRepositoryAsync<CashFlow>();
                var cashFlowDetails = cashRepository.GetListAsync().Result.Items;
                return _objMapper.Map<List<CashFlowDetailsDto>>(cashFlowDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
