using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;

using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _RotationInterval
    {
        public int Id { get; set; }
        public int ChoreListId { get; set; }
        public System.DateTime StartDate { get; set; }
        public int IntervalTypeId { get; set; }
        public int IntervalNumber { get; set; }
        public bool IsActive { get; set; }

        public _RotationInterval(){}

        public _RotationInterval(RotationInterval Value)
        {
            try
            {
                TinyMapper.Bind<RotationInterval, _RotationInterval>();
                TinyMapper.Map<RotationInterval, _RotationInterval>(Value, this);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}