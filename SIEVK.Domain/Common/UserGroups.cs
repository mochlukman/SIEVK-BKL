using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.Common
{
    public class UserGroups
    {
        public int ID
        { get; set; }

        public String GroupName
        { get; set; }

        public String IsActiveText
        { get; set; }

        public bool IsEnabled
        { get; set; }

        public bool IsDeleted
        { get; set; }

        public String CreatedBy
        { get; set; }

        public DateTime CreatedTime
        { get; set; }

        public String LastUpdateBy
        { get; set; }

        public DateTime LastUpdateTime
        { get; set; }
    }
}
