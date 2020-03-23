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

        public int StatusId { get; set; }
        public string ChoreListStatusName { get; set; }

        public List<_ChoreUser> ChoreUsers { get; set; }
        public List<_Chore> Chores { get; set; }
        public List<_RotationInterval> RotationIntervals { get; set; }

        public _ChoreList() { }

        public _ChoreList(ChoreList Value)
        {
            //recreate chorelist sub lists
            Value.ChoreUsers = Value.ChoreUsers.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToList();
            Value.Chores = Value.Chores.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToList();
            Value.RotationIntervals = Value.RotationIntervals.Where(x => x.IsActive).ToList();

            TinyMapper.Bind<ChoreList, _ChoreList>(config =>
            {
                config.Bind(x => x.ChoreUsers, x => x.ChoreUsers);
                config.Bind(x => x.Chores, x => x.Chores);
                config.Bind(x => x.RotationIntervals, x => x.RotationIntervals);
            });

            TinyMapper.Map<ChoreList, _ChoreList>(Value, this);
            if (Value.ChoreListStatu != null)
                ChoreListStatusName = Value.ChoreListStatu.ChoreListStatusName;

        }
    }
}