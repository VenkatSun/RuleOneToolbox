using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RuleOneToolbox.CompanyFinancialsClient
{
    public class BalanceSheet
    {
        private static string BalanceSheetURL = "https://equityapi.morningstar.com/Webservice/CompanyFinancialsService.asmx/GetBalanceSheet?exchangeId={0}&identifierType={1}&identifier={2}&statementType={3}&dataType={4}&startDate={5}&endDate={6}";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get Annual Balance Sheet data in XML format
        /// </summary>
        /// <param name="token"></param>
        /// <param name="exchangeId"></param>
        /// <param name="identifierType"></param>
        /// <param name="identifier"></param>
        /// <param name="dataType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string GetBalanceSheet_XML(string token,string exchangeId, string identifierType, string identifier, 
            string dataType, string startDate, string endDate, string statementType)
        {
            string result = null;
            try
            {
                string sURL = string.Format(BalanceSheetURL, token, exchangeId, identifierType, 
                    identifier, statementType, dataType, startDate, endDate);
                result = Helper.GetXMLResultByURL(sURL);
            }
            catch(Exception ex)
            {
                _log.Error("Error :", ex);
            }
            return result;
        }

        /// <summary>
        /// Get Annual Balance Sheet data in Json format
        /// </summary>
        /// <param name="token"></param>
        /// <param name="exchangeId"></param>
        /// <param name="identifierType"></param>
        /// <param name="identifier"></param>
        /// <param name="dataType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static dynamic GetBalanceSheet(string exchangeId, string identifierType, string identifier, 
            string dataType, string startDate, string endDate, string statementType)
        {
            string sURL = string.Format(BalanceSheetURL, exchangeId, identifierType, identifier, 
                statementType, dataType, startDate, endDate);
            return Helper.GetJSONResultByURL(sURL);
        }

    }
}
