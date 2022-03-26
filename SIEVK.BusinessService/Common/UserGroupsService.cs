using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using SIEVK.BusinessData.Common;
using System.Data;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Common
{
    public class UserGroupsService
    {
       

        Mapper map = new Mapper();
        UserGroupsDAO dao = new UserGroupsDAO();

        public UserGroups GetUserGroup(int id)
        {
            DataTable dt = dao.GetList(null, id);
            UserGroups userGroups = null;
            if (dt.Rows.Count > 0)
            {
                userGroups = map.BindData<UserGroups>(dt);
            }
            return userGroups;
        }

        public List<UserGroups> GetAllUserGroups(bool? isEnabled = null)
        {
            DataTable dt = dao.GetList(isEnabled);
            List<UserGroups> lst = map.BindDataList<UserGroups>(dt);
            return lst;
        }

        public void UpdateDataUserGroups(UserGroups userGroup)
        {
            dao.Update(userGroup);
        }

        public void InsertDataUserGroups(UserGroups userGroup)
        {
            dao.Insert(userGroup);
        }

        public void DeleteDataUserGroups(UserGroups userGroup)
        {
            dao.Delete(userGroup);
        }

        public Dictionary<string, string> ValidateData(UserGroups userGroups, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(userGroups.GroupName))
            {
                dic.Add("ErrGroupName", "Group Name harus diisi");
            }

            if (String.IsNullOrEmpty(userGroups.IsEnabled.ToString()))
            {
                dic.Add("ErrStatusActive", "Status Aktif harus diisi");
            }

            DataTable dt = dao.GetList(true, null, userGroups.GroupName);
            if (isCreate)
            {
                if (dt.Rows.Count > 0)
                {
                    dic.Add("ErrUserNameExist", "User Group Name sudah ada");
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["ID"]) != userGroups.ID)
                    {
                        dic.Add("ErrUserNameExist", "User Group Name sudah ada");
                    }
                }
            }

            return dic;
        }

    }
}
