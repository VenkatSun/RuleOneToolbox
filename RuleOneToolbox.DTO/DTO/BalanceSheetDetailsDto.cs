using System;
using System.Collections.Generic;
using System.Text;

namespace RuleOneToolbox.DTO.DTO
{
    public class BalanceSheetDetailsDto
    {
        public long ID { get; set; }

        public string BalanceSheetKey { get; set; }

        public string ExchangeId { get; set; }

        public string Symbol { get; set; }


        public string PeriodEndingDate { get; set; }


        public string DataType { get; set; }


        public string Interim { get; set; }


        public string BalanceSheetValue { get; set; }

    }
}
