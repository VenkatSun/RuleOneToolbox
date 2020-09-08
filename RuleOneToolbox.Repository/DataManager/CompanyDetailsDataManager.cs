using log4net;
using RuleOneToolbox.DTO.Models;
using RuleOneToolboxDbContext;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RuleOneToolbox.Repository.DataManager
{
    public class CompanyDetailsDataManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RuleOneToolboxDbContext _objDbContext;
        public CompanyDetailsDataManager(RuleOneToolboxDbContext dbContext)
        {
            _objDbContext = dbContext;

        }

        public CompanyDetailsDataManager(Microsoft.Extensions.Configuration.IConfigurationRoot configuration)
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
        /// Insert or update company details to database 
        /// </summary>
        /// <param name="objCompanyDetails"></param>
        public void InsertUpdateCompanyDetails(CompanyDetail objCompanyDetails)
        {
            try
            {
                using var uow = new UnitOfWork<RuleOneToolboxDbContext>(_objDbContext);
                var repo = uow.GetRepository<CompanyDetail>();
                var objResult = repo.SingleOrDefault(x => x.CompanyName == objCompanyDetails.CompanyName);
                if (objResult != null)
                {
                    objResult.CompanyName = objCompanyDetails.CompanyName;
                    objResult.CIK = objCompanyDetails.CIK;
                    objResult.ExchangeId = objCompanyDetails.ExchangeId;
                    objResult.Symbol = objCompanyDetails.Symbol;
                    repo.Update(objCompanyDetails);
                    uow.Commit();

                }
                else
                {
                    repo.Insert(objCompanyDetails);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                _log.Error("", ex);
            }
        }

        public void BulkInsert(params CompanyDetail[] objCompanyDetails)
        {
            try
            {
                using var uow = new UnitOfWork<RuleOneToolboxDbContext>(_objDbContext);
                var repo = uow.GetRepository<CompanyDetail>();
                repo.Insert(objCompanyDetails);
                uow.Commit();
            }
            catch (Exception ex)
            {
                _log.Error("Error", ex);
            }
        }
        /// <summary>
        /// Get Company Details 
        /// </summary>
        /// <param name="objCompanyDetails"></param>
        public IList<CompanyDetail> GetCompanyDetails()
        {
            IList<CompanyDetail> objCompanyDetails = new List<CompanyDetail>();
            try
            {
                var uow = new UnitOfWork<RuleOneToolboxDbContext>(_objDbContext);
                var repo = uow.GetReadOnlyRepository<CompanyDetail>();
                objCompanyDetails = repo.GetList().Items;
            }
            catch (Exception ex)
            {
                _log.Error("Error", ex);
            }
            return objCompanyDetails;
        }
    }
}
