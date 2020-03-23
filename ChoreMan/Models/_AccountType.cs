using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//project
using ChoreMan.Entities;

//external
using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _AccountType
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public decimal BasePricePerMonth { get; set; }
        public int UserLimit { get; set; }
        public int ChoreListLimit { get; set;}
        public int RotationLimit { get; set; }

        public _AccountType() { }

        public _AccountType(AccountType Value)
        {
            try
            {
                TinyMapper.Bind<AccountType, _AccountType>();
                TinyMapper.Map<AccountType, _AccountType>(Value, this);

                this.BasePricePerMonth = decimal.Round(BasePricePerMonth, 2);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}