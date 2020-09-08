using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace RuleOneToolbox.DTO.Models
{
    [Table("BalanceSheet", Schema="RuleOneToolBox")]
    public class BalanceSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string BalanceSheetKey { get; set; }

        [Required]
        [MaxLength(5)]
        public string ExchangeId { get; set; }

        [Required]
        [MaxLength(5)]
        public string Symbol { get; set; }

        [Required]
        [MaxLength(10)]
        public string PeriodEndingDate { get; set; }

        [Required]
        [MaxLength(5)]
        public string DataType { get; set; }

        [Required]
        [MaxLength(5)]
        public string Interim { get; set; }

        [Required]
        [MaxLength(5000)]
        public string BalanceSheetValue { get; set; }

    }
}
