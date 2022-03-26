using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class UsersDAO
    {
        public DataTable GetList(String userName = null, Boolean? isActive = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserName", SqlDbType.VarChar, userName);
                db.AddParameter("IsActive", SqlDbType.Bit, isActive);
                dt = db.ExecuteDataTable("[sp_UsersList]");
            }
            return dt;
        }

        public void Insert(Users user)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UserGroupID", SqlDbType.Int, user.UserGroupID);
                db.AddParameter("UnitKey", SqlDbType.VarChar, user.UnitKey);
                db.AddParameter("UserName", SqlDbType.VarChar, user.UserName);
                db.AddParameter("Password", SqlDbType.VarChar, user.Password);
                db.AddParameter("FirstName", SqlDbType.VarChar, user.FirstName);
                db.AddParameter("LastName", SqlDbType.VarChar, user.LastName);
                db.AddParameter("Email", SqlDbType.VarChar, user.Email);
                db.AddParameter("CreatedBy", SqlDbType.VarChar, user.CreatedBy);
                db.ExecuteNonQuery("[sp_UsersInsert]");
            }
        }

        public void Update(Users user)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, user.ID);
                db.AddParameter("UserGroupID", SqlDbType.Int, user.UserGroupID);
                db.AddParameter("UnitKey", SqlDbType.VarChar, user.UnitKey);
                db.AddParameter("UserName", SqlDbType.VarChar, user.UserName);
                db.AddParameter("Password", SqlDbType.VarChar, user.Password);
                db.AddParameter("FirstName", SqlDbType.VarChar, user.FirstName);
                db.AddParameter("LastName", SqlDbType.VarChar, user.LastName);
                db.AddParameter("Email", SqlDbType.VarChar, user.Email);
                db.AddParameter("IsActive", SqlDbType.Bit, user.IsActive);
                db.AddParameter("IsDeleted", SqlDbType.Bit, user.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, user.LastUpdateBy);
                db.ExecuteNonQuery("sp_UsersUpdate");
            }
        }

        public void Delete(Users user)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ID", SqlDbType.Int, user.ID);
                db.AddParameter("IsDeleted", SqlDbType.Bit, user.IsDeleted);
                db.AddParameter("LastUpdateBy", SqlDbType.VarChar, user.LastUpdateBy);
                db.ExecuteNonQuery("sp_UsersUpdate");
            }
        }

    }
}
