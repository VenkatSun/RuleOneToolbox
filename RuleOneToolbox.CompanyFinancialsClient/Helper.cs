using Newtonsoft.Json.Linq;
using RuleOneToolbox.DTO.Morningstar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Configuration;
using log4net;
using System.Reflection;

namespace RuleOneToolbox.CompanyFinancialsClient
{
    public class Helper
    {
        private static string _sToken;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string LoginURL = "https://equityapi.morningstar.com/WSLogin/Login.asmx/Login?email={0}&password={1}";

        /// <summary>
        /// Method get Morningstrat web service Token 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static TokenEntity Login(string email, string password)
        {
            _log.Info("Trying to login");
            TokenEntity tokenEntity = null;
            try
            {
                string sURL = string.Format(LoginURL, email, password);
                string loginResult = GetXMLResultByURL(sURL);
                if (string.IsNullOrEmpty(loginResult))
                    return null;
                tokenEntity = GetTokenEntity(loginResult);
            }
            catch (Exception ex)
            {
                _log.Error("Error while login : " + ex.Message);
            }
            return tokenEntity;
        }

        /// <summary>
        /// Get Morningstar service response in XML format
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetXMLResultByURL(string url)
        {
            
            HttpWebRequest request = null;
            Stream responseStream = null;
            StreamReader outSr = null;
            HttpWebResponse response = null;
            string sRet = null;

            try
            {
                string sXML = null;
                request = (HttpWebRequest)WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                outSr = new StreamReader(responseStream);

                sXML = outSr.ReadToEnd();
                if (sXML != null)
                {
                    sRet = sXML;
                }
            }
            catch (Exception ex)
            {
                _log.Error("ERROR on this url=" + url);
                _log.Error(ex);
            }
            finally
            {
                if (request != null)
                    request.Abort();
                if (response != null)
                    response.Close();
                if (outSr != null)
                    outSr.Close();
                if (responseStream != null)
                    responseStream.Close();
            }

            return sRet;
        }

        /// <summary>
        /// Get Morningstar service response in Json format
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static dynamic GetJSONResultByURL(string url)
        {
            dynamic result = null;
            TokenEntity tokenEntity = null;
            try
            {
                var sUName = GeAppSettings("UserName");
                var sPassword = GeAppSettings("Password");
                if (_sToken == null || _sToken == "")
                {
                    tokenEntity = Login(sUName, sPassword);
                    if (tokenEntity.IsSuccess)
                    {
                        _sToken = tokenEntity.Token;
                        _log.Info("Successfully logged in");
                    }
                    else
                        _log.Error(tokenEntity.Token);

                }
                using (var client = new HttpClient())
                {
                    var content = client.GetStringAsync(url + "&responseType=Json&Token=" + _sToken).Result;
                    result = JObject.Parse(content);
                    if ($"{result.MessageInfo.MessageCode}" == "40011")
                    {
                        _log.Info("Token is expired. Fetching fresh token");
                        tokenEntity = Login(sUName, sPassword);
                        if (tokenEntity.IsSuccess)
                        {
                            _sToken = tokenEntity.Token;
                            content = client.GetStringAsync(url + "&responseType=Json&Token=" + _sToken).Result;
                            result = JObject.Parse(content);
                        }
                        else
                            _log.Error(tokenEntity.Token);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// Get section array values of appsetting.json
        /// </summary>
        /// <param name="sSectionname"></param>
        /// <returns></returns>
        public static string[] GetSectionArrayValues(string sSectionname)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            return builder.Build().GetSection(sSectionname).Get<string[]>(); ;
        }

        /// <summary>
        /// Get Token entirty from login result string
        /// </summary>
        /// <param name="loginResult"></param>
        /// <returns></returns>
        private static TokenEntity GetTokenEntity(string sLoginResult)
        {
            TokenEntity token = null;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(System.Text.RegularExpressions.Regex.Replace(sLoginResult, @"(xmlns:?[^=]*=[""][^""]*[""])", "", RegexOptions.IgnoreCase | RegexOptions.Multiline));
                token = new TokenEntity();
                token.IsSuccess = doc.SelectSingleNode(@"//TokenEntity/IsSuccess").InnerText == "true" ? true : false;
                token.Token = doc.SelectSingleNode(@"//TokenEntity/Token").InnerText;
                token.expireDate = DateTime.ParseExact(doc.SelectSingleNode(@"//TokenEntity/expireDate").InnerText, "yyyy-MM-ddTHH:mm:ss.fffffffZ", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                _log.Error("Failed to get token entity : " + ex.Message);
            }
            return token;
        }

        /// <summary>
        /// Get app setting value from appsetting.json
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GeAppSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            return builder.Build().GetSection("AppSettings").GetSection(key).Value;
        }



    }
}
