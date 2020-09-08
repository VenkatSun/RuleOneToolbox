using AutoMapper;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RuleOneToolbox.CompanyFinancialsClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Rep = RuleOneToolbox.Repository.DataManager;


namespace RuleOneToolbox.CompanyFinancials
{
    class Program
    {
        public static IConfigurationRoot _objConfiguration;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Create service collection to read the data from Appsetting.json file to get the connection string
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            _log.Info("Satrted BalanceSheet Job");
            SaveBalanceSheetData();

        }

        /// <summary>
        /// 
        /// </summary>
        private static void SaveBalanceSheetData()
        {
            try
            {
                Rep.CompanyDetailsDataManager manger = new Rep.CompanyDetailsDataManager(_objConfiguration);
                var statementTypes = Helper.GetSectionArrayValues("StatementType");
                var dataTypes = Helper.GetSectionArrayValues("DataType");
                _log.Info("Fetching Company Financial Availability List");
                var objCompanyDtls = manger.GetCompanyDetails();
                DateTime dtSatartDate = DateTime.Now.AddMonths(-1);
                string sEndDate = dtSatartDate.Month.ToString() + "/" + dtSatartDate.Year.ToString();
                string sStratDate = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                foreach (string sStatementType in statementTypes)
                {
                    foreach (var objCompanyDtl in objCompanyDtls)
                    {
                        foreach (string sDataType in dataTypes)
                        {
                            SaveBalanceSheetData(objCompanyDtl.ExchangeId, objCompanyDtl.Symbol, sDataType,
                                sStratDate, sEndDate, sStatementType);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.Error("Error occuured while saving balance sheet data");
                _log.Error(ex);
            }
        }

        private void SaveCompanyDetails()
        {
            try
            {
                Rep.CompanyDetailsDataManager manger = new Rep.CompanyDetailsDataManager(_objConfiguration);
                var sData = CompanyDetails.GetCompanyFinancialAvailabilityList("NYS");
                var sCompanyDtls = $"{ sData.CompanyFinancialAvailabilityEntityList }";
                var objCompanyDtls = JsonConvert.DeserializeObject<List<RuleOneToolbox.DTO.Models.CompanyDetail>>(sCompanyDtls);
                manger.BulkInsert(objCompanyDtls.ToArray());
            }
            catch (Exception ex)
            {
                _log.Error("Error occuured while saving company details data");
                _log.Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sExchangeId"></param>
        /// <param name="sIdentifier"></param>
        /// <param name="sDataType"></param>
        /// <param name="sStartDate"></param>
        /// <param name="sEndDate"></param>
        /// <param name="statementType"></param>
        private static void SaveBalanceSheetData(string sExchangeId, string sIdentifier, string sDataType,
            string sStartDate, string sEndDate, string statementType)
        {
            Rep.BalanceSheetManager manger = new Rep.BalanceSheetManager(_objConfiguration);
            string sIdentifierType = "Symbol";
            _log.Info("Fetching balance sheet data for " + sExchangeId);
            var data = BalanceSheet.GetBalanceSheet(sExchangeId, sIdentifierType, sIdentifier,
                sDataType, sStartDate, sEndDate, statementType);
            var balanceSheetEntity = $"{data.BalanceSheetEntityList}";
            if (balanceSheetEntity != null && balanceSheetEntity.Trim() != "")
            {
                RuleOneToolbox.DTO.Models.BalanceSheet ohjBalanceSheet = new DTO.Models.BalanceSheet();
                ohjBalanceSheet.BalanceSheetKey = @$"{data.GeneralInfo.Symbol}"
                    + ($"{ data.BalanceSheetEntityList[0].PeriodEndingDate}").Replace("-", "") +
                    statementType + $"{ data.BalanceSheetEntityList[0].DataType }";
                ohjBalanceSheet.BalanceSheetValue = balanceSheetEntity;
                ohjBalanceSheet.DataType = $"{ data.BalanceSheetEntityList[0].DataType}";
                ohjBalanceSheet.ExchangeId = sExchangeId;
                ohjBalanceSheet.Interim = $"{ data.BalanceSheetEntityList[0].Interim}";
                ohjBalanceSheet.Symbol = sIdentifier;
                ohjBalanceSheet.PeriodEndingDate = $"{ data.BalanceSheetEntityList[0].PeriodEndingDate}";
                ohjBalanceSheet.Symbol = $"{ data.BalanceSheetEntityList[0].Symbol}";
                manger.InsertUpdateBalanceSheet(ohjBalanceSheet);
                _log.Info("Inserted/updated balance sheet data into database for " + sExchangeId);
            }
            else
            {
                _log.Warn(string.Format("No {0} balance sheet data found for {1}, {2}", statementType,sExchangeId, sIdentifier));
            }
        }
        /// <summary>
        /// // Create service collection to read the data from Appsetting.json file to get the connection string
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            // Build configuration
            _objConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            IMapper mapper = mapperConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
            serviceCollection.AddTransient<Rep.BalanceSheetManager>();
            serviceCollection.AddTransient<Rep.CashFlowManager>();
            serviceCollection.AddSingleton<IConfigurationRoot>(_objConfiguration);
            
        }
    }

}
