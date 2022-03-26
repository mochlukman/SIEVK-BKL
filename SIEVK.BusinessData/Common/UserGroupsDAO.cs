using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class UserGroupsDAO
    {

        public DataTable GetList(bool? isEnabled = null, int? id = null, string groupName = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, id);
                db.AddParameter("IsEnabled", SqlDbType.Bit, isEnabled);
                db.AddParameter("GroupName", SqlDbType.VarChar, groupName);
                dt = db.ExecuteDataTable("sp_UserGroupsList");
            }
            return dt;
        }

        public void Insert(UserGroups userGroups)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("GroupName", SqlDbType.VarChar, userGroups.GroupName);
                db.AddParameter("CreatedBy", SqlDbType.VarChar, userGroups.CreatedBy);
                db.ExecuteNonQuery("[sp_UserGroupsInsert]");
            }
        }

        public void Update(UserGroups userGroups)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, userGroups.ID);
                db.AddParameter("GroupName", SqlDbType.VarChar, userGroups.GroupName);
                db.AddParameter("IsEnabled", SqlDbType.Bit, userGroups.IsEnabled);
                db.AddParameter("IsDeleted", SqlDbType.Bit, userGroups.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, userGroups.LastUpdateBy);
                db.ExecuteNonQuery("sp_UserGroupsUpdate");
            }
        }

        public void Delete(UserGroups userGroups)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, userGroups.ID);
                db.AddParameter("IsDeleted", SqlDbType.Bit, userGroups.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, userGroups.LastUpdateBy);
                db.ExecuteNonQuery("sp_UserGroupsUpdate");
            }
        }
    }
}
