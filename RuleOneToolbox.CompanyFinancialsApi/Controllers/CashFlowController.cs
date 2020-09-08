using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RuleOneToolbox.Repository.DataManager;

namespace RuleOneToolbox.CompanyFinancialsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashFlowController : ControllerBase
    {
        private readonly CashFlowManager _objCashFlowManager;
        public CashFlowController(CashFlowManager cashFlowManager)
        {
            _objCashFlowManager = cashFlowManager;
        }
        /// <summary>
        /// This method we return the all records from CashFlow table
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCashFlow()
        {
            try
            {
                var cashFlowDetails = await _objCashFlowManager.GetAllCashFlowDetails();
                if (cashFlowDetails != null && cashFlowDetails.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(
                    new
                    {
                        isError = false,
                        errorMessage = string.Empty,
                        errorCode = 200,
                        model = cashFlowDetails
                    });
                    return Ok(result);
                }
                else{

                    var result = JsonConvert.SerializeObject(
                    new
                    {
                        isError = false,
                        errorMessage = "No Records Found",
                        errorCode = 404,
                        model = cashFlowDetails
                    });
                    return Ok(result);
                }
            }
            catch (RuleOneToolBoxCustomException ex)
            {
                // TO DO writing the exception in log File /DB
                return StatusCode(500, "Internal Server Error " + ex.ToString());
            }
        }
    }
}
