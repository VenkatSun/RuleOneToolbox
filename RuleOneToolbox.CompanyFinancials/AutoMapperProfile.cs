using AutoMapper;
using RuleOneToolbox.DTO.DTO;
using RuleOneToolbox.DTO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuleOneToolbox.CompanyFinancials
{
   public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<BalanceSheet, BalanceSheetDetailsDto>();
            CreateMap<CashFlow, CashFlowDetailsDto>();

        }
    }
}
