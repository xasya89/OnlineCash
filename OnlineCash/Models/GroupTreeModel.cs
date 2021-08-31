using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class GroupTreeModel
    {
        public int Id { get; set; }
        public GroupTreeTypes Type { get; set; }
        public string Name { get; set; }
        public List<Good> Goods { get; set; } = new List<Good>();
        public List<GroupTreeModel> Childs { get; set; } = new List<GroupTreeModel>();
    }
    public enum GroupTreeTypes
    {
        Group,
        Supplier
    }
}
