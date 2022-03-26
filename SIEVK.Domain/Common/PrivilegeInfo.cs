using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.Common
{
    public class PrivilegeInfo
    {
        public int ID
        { get; set; }

        public int UserGroupsID
        { get; set; }

        public int MenuID
        { get; set; }

        public string NavigationLabel
        { get; set; }

        public string Description
        { get; set; }

        public bool IsOpenNewTab
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
