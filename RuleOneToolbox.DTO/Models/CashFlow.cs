using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RuleOneToolbox.DTO.Models
{

        [Table("CashFlow", Schema = "RuleOneToolBox")]
        public class CashFlow
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long CashFlowId { get; set; }

            [Required]
            [MaxLength(50)]
            public string CashFlowKey { get; set; }

            [Required]
            [MaxLength(5000)]
            public string CashFlowValue { get; set; }


        }
}
