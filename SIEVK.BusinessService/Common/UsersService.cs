using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using SIEVK.BusinessData.Common;
using SIEVK.BusinessData;
using System.Data;

namespace SIEVK.BusinessService.Common
{
    public class UsersService
    {
        Mapper map = new Mapper();
        UsersDAO dao = new UsersDAO();

        public Users GetUser (String userName, Boolean? isActive)
        {
            DataTable dt = dao.GetList(userName, isActive);
            Users usr = null;
            if (dt.Rows.Count > 0)
            {
                usr = map.BindData<Users>(dt);
            }
            return usr;
        }

        public List<Users> GetAllUsers()
        {
            DataTable dt = dao.GetList();
            List<Users> lst = map.BindDataList<Users>(dt);
            return lst;
        }

        public void UpdateDataUsers(Users user)
        {
            dao.Update(user);
        }

        public void InsertDataUsers(Users user)
        {
            
            dao.Insert(user);
        }

        public void DeleteDataUsers(Users user)
        {
            dao.Delete(user);
        }

        public Dictionary<string, string> ValidateData(Users user, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(user.UserGroupID.ToString()))
            {
                dic.Add("ErrUserGroupID", "User Group harus diisi");
            }

            if (String.IsNullOrEmpty(user.UserName))
            {
                dic.Add("ErrUserName", "User Name harus diisi");
            }

            if (String.IsNullOrEmpty(user.FirstName))
            {
                dic.Add("ErrFirstName", "First Name harus diisi");
            }

            if (String.IsNullOrEmpty(user.IsActive.ToString()))
            {
                dic.Add("ErrStatusActive", "Status Aktif harus diisi");
            }

            if (isCreate)
            {
                if (String.IsNullOrEmpty(user.Password))
                {
                    dic.Add("ErrPassword", "Password harus diisi");
                }

                DataTable dt = dao.GetList(user.UserName, null);
                if (dt.Rows.Count> 0)
                {
                    dic.Add("ErrUserNameExist", "User Name sudah ada");
                }
            }

            return dic;
        }

    }
}
