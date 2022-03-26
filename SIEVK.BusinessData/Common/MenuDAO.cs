using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class MenuDAO
    {
        public DataTable GetParentMenu(int userGruopID)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupID", SqlDbType.Int, userGruopID);
                dt = db.ExecuteDataTable("sp_GetParentMenu");
            }
            return dt;
        }

        public DataTable GetChildMenu(int userGruopID, int parentID)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupID", SqlDbType.Int, userGruopID);
                db.AddParameter("ParentID", SqlDbType.Int, parentID);
                dt = db.ExecuteDataTable("sp_GetChildMenu");
            }
            return dt;
        }

        public DataTable CheckPrivilege(string userName, string url)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserName", SqlDbType.Text, userName);
                db.AddParameter("Url", SqlDbType.Text, url);
                dt = db.ExecuteDataTable("sp_CheckPrivilege");
            }
            return dt;
        }

        public void Delete(Menu menu)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, menu.ID);
                db.AddParameter("IsDeleted", SqlDbType.Bit, menu.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, menu.LastUpdateBy);
                db.ExecuteNonQuery("sp_MenuUpdate");
            }
        }

        public DataTable GetList(int? id = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, id);
                dt = db.ExecuteDataTable("sp_MenuList");
            }
            return dt;
        }        
    }
}
