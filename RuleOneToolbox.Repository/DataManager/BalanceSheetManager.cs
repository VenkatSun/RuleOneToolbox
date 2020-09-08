using AutoMapper;
using log4net;
using Newtonsoft.Json.Linq;
using RuleOneToolbox.DTO.DTO;
using RuleOneToolbox.DTO.Models;
using RuleOneToolboxDbContext;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuleOneToolbox.Repository.DataManager
{
    public class BalanceSheetManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RuleOneToolboxDbContext objDbContext;
        private readonly IMapper _objMapper;
        public BalanceSheetManager(RuleOneToolboxDbContext dbContext, IMapper mapper)
        {
            objDbContext = dbContext;
            _objMapper = mapper;
        }

        public BalanceSheetManager(Microsoft.Extensions.Configuration.IConfigurationRoot configuration)
        {
            objDbContext = new RuleOneToolboxDbContext(configuration);
        }
        /// <summary>
        /// Clear the DbContext object from memory
        /// </summary>
        public void Dispose()
        {
            objDbContext?.Dispose();
        }

        /// <summary>
        /// Insert or Update Balance Sheet to data base
        /// </summary>
        /// <param name="balanceSheet"></param>
        public void InsertUpdateBalanceSheet(BalanceSheet balanceSheet)
        {
            try
            {
                using var uow = new UnitOfWork<RuleOneToolboxDbContext>(objDbContext);
                var repo = uow.GetRepository<BalanceSheet>();
                var balSheet = repo.SingleOrDefault(x => x.BalanceSheetKey == balanceSheet.BalanceSheetKey);
                if (balSheet != null)
                {
                    //Check for any modification in balancesheet
                    bool isBalancSheetModified = !JToken.DeepEquals(balSheet.BalanceSheetValue, balanceSheet.BalanceSheetValue);
                    if (isBalancSheetModified)
                    {
                        balSheet.BalanceSheetValue = balanceSheet.BalanceSheetValue;
                        repo.Update(balSheet);
                        uow.Commit();
                        _log.Info(string.Format("Successfully inserted  balance sheet data for {0}", balanceSheet.Symbol));
                    }
                }
                else
                {
                    repo.Insert(balanceSheet);
                    uow.Commit();
                    _log.Info(string.Format("Successfully updated  balance sheet data for {0}", balanceSheet.Symbol));
                }
            }
            catch(Exception ex)
            {
                _log.Error(string.Format("failed to insert/updated  balance sheet data for {0}", balanceSheet.Symbol));
                _log.Error("Error", ex);
            }
        }
        /// <summary>
        /// Connecting with database to get Balance sheet data and  using mapper to convert the modal object to DTO Object
        /// </summary>
        /// <returns></returns>
        public async Task<List<BalanceSheetDetailsDto>> GetAllBalanceSheetDetails()
        {
            try
            {
                var balanceSheetUnitOfWork = new UnitOfWork<RuleOneToolboxDbContext>(objDbContext);
                var balanceSheetrepository = balanceSheetUnitOfWork.GetReadOnlyRepositoryAsync<BalanceSheet>();
                var balanceSheetDetails = await balanceSheetrepository.GetListAsync();
                return _objMapper.Map<List<BalanceSheetDetailsDto>>(balanceSheetDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}
