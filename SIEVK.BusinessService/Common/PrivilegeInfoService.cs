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
    public class PrivilegeInfoService
    {
        Mapper map = new Mapper();
        PrivilegeInfoDAO dao = new PrivilegeInfoDAO();
              
        public List<PrivilegeInfo> GetPrivilegeInfo(int userGroupsId)
        {            
            DataTable dt = dao.GetList(userGroupsId);
            List<PrivilegeInfo> lst = map.BindDataList<PrivilegeInfo>(dt);
            return lst;
        }

        public List<PrivilegeInfo> GetNotSelectedMenu(int userGroupsId)
        {
            DataTable dt = dao.GetNotSeletedMenu(userGroupsId);
            List<PrivilegeInfo> lst = map.BindDataList<PrivilegeInfo>(dt);
            return lst;
        }

        public void InsertPrivilegeInfo(PrivilegeInfo privilegeInfo)
        {
            dao.Insert(privilegeInfo);
        }

        public void UpdatePrivilegeInfo(List<PrivilegeInfo> newList, int userGroupsID)
        {
            var oldList = map.BindDataList<PrivilegeInfo>(dao.GetList(userGroupsID));
            if (newList.Count > 0)
            {
                
                foreach (var newPrivilegeInfo in newList)
                {
                    if (newPrivilegeInfo.ID == 0){
                        // insert id = 0
                        dao.Insert(newPrivilegeInfo);
                    }else{     
                        // update or delete data
                        if (oldList.Count > 0)
                        {
                            int index = -1;                            
                            for (int i = 0; i < oldList.Count; i++ )
                            {
                                if (oldList[i].ID == newPrivilegeInfo.ID)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            if (index > -1)
                            {
                                oldList.RemoveAt(index);
                            }
                        }
                        else
                        {
                            dao.Insert(newPrivilegeInfo);
                        }
                    }                    
                }

                // delete data
                if (oldList.Count > 0)
                {                    
                    foreach (var oldData in oldList)
                    {
                        oldData.IsDeleted = true;
                        dao.Delete(oldData);
                    }
                }

            }           
        }

        public void DeletePrivilegeInfo(PrivilegeInfo privilegeInfo)
        {
            dao.Delete(privilegeInfo);
        }

        public Dictionary<string, string> ValidateData(List<PrivilegeInfo> newList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (newList.Count > 0)
            {
                for (int i = 0; i < newList.Count; i++)
                {
                    var privilegeInfo = new PrivilegeInfo();

                    if (String.IsNullOrEmpty(privilegeInfo.MenuID.ToString()))
                    {
                        dic.Add("ErrMenuID" + i, "Menu di row "+ i +" harus diisi");
                    }

                    if (String.IsNullOrEmpty(privilegeInfo.UserGroupsID.ToString()))
                    {
                        dic.Add("ErrUserGroupsID" + i, "User Group di row " + i + " harus diisi");
                    }
                }
            }            

            return dic;
        }

    }
}
