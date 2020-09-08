using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RuleOneToolbox.CompanyFinancialsClient
{
    public class CompanyDetails
    {
        private static string _serviceURL="https://equityapi.morningstar.com/WebService/GlobalMasterListsService.asmx/GetCompanyFinancialAvailabilityList?category=GetCompanyFinancialAvailabilityList&exchangeId={0}&identifier={1}&responseType={2}&identifierType={3}";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sExchangeId"></param>
        /// <param name="sIdentifier"></param>
        /// <param name="sResponseType"></param>
        /// <param name="sIdentifierType"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public static dynamic GetCompanyFinancialAvailabilityList(string sExchangeId)
        {
            dynamic result = null;
            try
            {
                string sURL = string.Format(_serviceURL, sExchangeId, sExchangeId, "Json", "ExchangeId");
                result = Helper.GetJSONResultByURL(sURL);
                
            }
            catch(Exception ex)
            {
                _log.Error("", ex);
            }
            return result;
        }
    }
}
