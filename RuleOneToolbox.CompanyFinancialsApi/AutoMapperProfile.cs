using AutoMapper;
using RuleOneToolbox.DTO.DTO;
using RuleOneToolbox.DTO.Models;

namespace RuleOneToolbox.CompanyFinancialsApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<BalanceSheet, BalanceSheetDetailsDto>();
            CreateMap<CompanyDetail, CompanyDetailsDto>();
            CreateMap<CashFlow, CashFlowDetailsDto>();

        }
    }
}