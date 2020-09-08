using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RuleOneToolbox.DTO;
using RuleOneToolbox.Repository.DataManager;

namespace RuleOneToolbox.CompanyFinancialsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceSheetController : ControllerBase
    {
        private readonly BalanceSheetManager _objBalanceSheetManager;
        public BalanceSheetController(BalanceSheetManager balanceSheetManager)
        {
            _objBalanceSheetManager = balanceSheetManager;
        }
        /// <summary>
        /// This method we return the all records from Balancesheet table
        /// </summary>
        [HttpGet]
        public async Task<IActionResult>  GetAllBalanceSheet()
        {
            try
            {
                var balanceSheetDetails= await _objBalanceSheetManager.GetAllBalanceSheetDetails();
                if (balanceSheetDetails != null && balanceSheetDetails.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(
                    new
                    {
                        isError = false,
                        errorMessage = string.Empty,
                        errorCode = 200,
                        model = balanceSheetDetails
                    });
                    return Ok(result);
                }
                   
                else
                {
                    var result = JsonConvert.SerializeObject(
                    new
                    {
                        isError = false,
                        errorMessage = "No Records Found",
                        errorCode = 404,
                        model = balanceSheetDetails
                    });
                    return Ok(result);
                }
            }
            catch (RuleOneToolBoxCustomException ex)
            {
                // TO DO writing the exception in log File /DB
                return StatusCode(500, "Internal Server Error "+ ex.ToString());
            }
        }
    }
}
