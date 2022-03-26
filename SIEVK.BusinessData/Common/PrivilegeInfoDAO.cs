using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class PrivilegeInfoDAO
    {

        public DataTable GetList(int userGroupsID)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupsID", SqlDbType.Int, userGroupsID);
                dt = db.ExecuteDataTable("sp_PrivilegeInfoList");
            }
            return dt;
        }

        public DataTable GetNotSeletedMenu(int userGroupsID)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupsID", SqlDbType.Int, userGroupsID);
                dt = db.ExecuteDataTable("sp_GetMenuNotSelected");
            }
            return dt;
        }

        public void Insert(PrivilegeInfo privilegeInfo)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupsID", SqlDbType.Int, privilegeInfo.UserGroupsID);
                db.AddParameter("MenuID", SqlDbType.Int, privilegeInfo.MenuID);
                db.AddParameter("CreatedBy", SqlDbType.VarChar, privilegeInfo.CreatedBy);
                db.ExecuteNonQuery("sp_PrivilegeInfoInsert");
            }
        }

        public void Delete(PrivilegeInfo privilegeInfo)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, privilegeInfo.ID);
                db.AddParameter("IsDeleted", SqlDbType.Bit, privilegeInfo.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, privilegeInfo.LastUpdateBy);
                db.ExecuteNonQuery("sp_PrivilegeInfoUpdate");
            }
        }
    }
}
