using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace RuleOneToolbox.DTO.Models
{
    [Table("CompanyDetails", Schema = "RuleOneToolBox")]
    public class CompanyDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(10)]
        public string ExchangeId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; }

        [Required]
        [MaxLength(10)]
        public string CIK { get; set; }

        
    }
}
