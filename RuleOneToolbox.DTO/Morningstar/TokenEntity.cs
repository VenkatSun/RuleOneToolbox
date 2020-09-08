using System;
using System.Collections.Generic;
using System.Text;

namespace RuleOneToolbox.DTO.Morningstar
{
    public class TokenEntity
    {
        public bool IsSuccess { set; get; }

        public string Token { set; get; }

        private DateTime _expireDate = DateTime.MinValue;

        public DateTime expireDate
        {
            set { _expireDate = value; }
            get { return _expireDate; }
        }
    }
}
