using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.Common
{
    public class Users
    {
    
        public int ID
        { get; set; }

        public int UserGroupID
        { get; set; }

        public String UserGroupName
        { get; set; }

        public String UnitKey
        { get; set; }

        public String KdUnit
        { get; set; }

        public String NmUnit
        { get; set; }

        public String UserName
        { get; set; }

        public String Password
        { get; set; }

        public String FirstName
        { get; set; }

        public String LastName
        { get; set; }

        public String Email
        { get; set; }

        public bool IsActive
        { get; set; }

        public String IsActiveText
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
