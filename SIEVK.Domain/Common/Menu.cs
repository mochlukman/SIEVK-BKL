using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.Common
{
    public class Menu
    {
        public int ID
        { get; set; }

        public int ParentID
        { get; set; }

        public String ParentName
        { get; set; }

        public String NavigationLabel
        { get; set; }

        public String IconClass
        { get; set; }

        public String IconColor
        { get; set; }

        public bool IsOpenNewTab
        { get; set; }

        public String Description
        { get; set; }

        public String URL
        { get; set; }

        public int Sequence
        { get; set; }

        public bool IsShow
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
