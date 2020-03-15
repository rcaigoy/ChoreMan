using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;

using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _ChoreList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public int ChoreListTypeId { get; set; }
        public string ChoreListTypeName { get; set; }

        public List<_ChoreUser> ChoreUsers { get; set; }
        public List<_Chore> Chores { get; set; }
        public List<_RotationInterval> RotationIntervals { get; set; }

        public _ChoreList() { }

        public _ChoreList(ChoreList Value)
        {
            TinyMapper.Bind<ChoreList, _ChoreList>(config =>
            {
                config.Bind(x => x.ChoreUsers, y => y.ChoreUsers);
                config.Bind(x => x.Chores, y => y.Chores);
                config.Bind(x => x.RotationIntervals, y => y.RotationIntervals);
            });

            TinyMapper.Map<ChoreList, _ChoreList>(Value, this);

            this.ChoreListTypeName = Value.ChoreListType.ChoreListType1;
        }
    }
}