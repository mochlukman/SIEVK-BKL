using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class UserGroupsController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private UserGroupsService ugSvc = new UserGroupsService();
        private MenuService mnSvc = new MenuService();
        private PrivilegeInfoService piSvc = new PrivilegeInfoService();
        private GeneralService genSvc = new GeneralService();
        #endregion

        public ActionResult LoadDataUserGroups()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToLower();
                
                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all User Group data  
                List<UserGroups> userGroupsData = ugSvc.GetAllUserGroups();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    userGroupsData = userGroupsData.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userGroupsData = userGroupsData.Where(m => m.GroupName.ToLower().Contains(searchValue)).ToList();
                }

                //total number of rows count     
                recordsTotal = userGroupsData.Count();
                //Paging     
                var data = userGroupsData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Index");
            }
        }

        public ActionResult Index(String msg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.Info = msg;
            return View();
        }

        public ActionResult Detail(int id)
        {
            UserGroups userGroup = new UserGroups();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                userGroup = ugSvc.GetUserGroup(id);
                List<PrivilegeInfo> list = piSvc.GetPrivilegeInfo(userGroup.ID);
                ViewBag.List = list;

                return View(userGroup);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Index", userGroup);
            }
        }

        public ActionResult Edit(int id)
        {
            UserGroups userGroup = new UserGroups();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                userGroup = ugSvc.GetUserGroup(id);
                return CreateInputView(userGroup);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Index", userGroup);
            }
        }

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            UserGroups userGroups = new UserGroups();
            try
            {
                userGroups = BindDataToObject(param);
                if (ValidateData(userGroups, false))
                {
                    ugSvc.UpdateDataUserGroups(userGroups);
                    return RedirectToAction("Index", "UserGroups", new { msg = GetValueFromResource("SUCCESS_EDIT") });
                }
                else
                {
                    return CreateInputView(userGroups);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Input", userGroups);
            }
        }

        public ActionResult EditPrivilege(int userGroupsId, string msg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            UserGroups usergroup = ugSvc.GetUserGroup(userGroupsId);            
            List<PrivilegeInfo> list = piSvc.GetPrivilegeInfo(userGroupsId);

            ViewBag.GroupID = usergroup.ID;
            ViewBag.GroupName = usergroup.GroupName;
            ViewBag.List = list;
            ViewBag.Info = msg;
            return View("InputPrivilege");
        }

        [HttpPost]
        public ActionResult EditPrivilege(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            var paramUserGroupsID = Convert.ToInt32(param["UserGroupsID"].ToString());
            var paramUserGroupsName = param["UserGroupsName"].ToString();
            List<PrivilegeInfo> newList = new List<PrivilegeInfo>();
            try
            {
                newList = BindDataPrivilegeInfoToObject(param);
                if (ValidateDataPrivilegeInfo(newList, false))
                {
                    piSvc.UpdatePrivilegeInfo(newList, paramUserGroupsID);
                    return RedirectToAction("EditPrivilege", "UserGroups", new { userGroupsId = paramUserGroupsID, msg = GetValueFromResource("SUCCESS_EDIT") });
                }
                else
                {
                    ViewBag.GroupID = paramUserGroupsID;
                    ViewBag.GroupName = paramUserGroupsName;
                    ViewBag.List = newList;
                    return View("InputPrivilege");
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                ViewBag.GroupID = paramUserGroupsID;
                ViewBag.GroupName = paramUserGroupsName;
                ViewBag.List = newList;
                return View("EditPrivilege");
            }
        }

        public ActionResult GetNotSelectedMenu(int userGroupsId)
        {
            var list = piSvc.GetNotSelectedMenu(userGroupsId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            UserGroups userGroup = new UserGroups();
            return CreateInputView(userGroup);
        }

        [HttpPost]
        public ActionResult Create(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            UserGroups userGroup = new UserGroups();
            try
            {
                userGroup = BindDataToObject(param);
                if (ValidateData(userGroup, true))
                {
                    ugSvc.InsertDataUserGroups(userGroup);
                    return RedirectToAction("Index", "UserGroups", new { msg = GetValueFromResource("SUCCESS_INPUT") });
                }
                else
                {
                    return CreateInputView(userGroup);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Input", userGroup);
            }
        }

        public bool ValidateData(UserGroups userGroups, bool isCreate)
        {
            Dictionary<string, string> dic = ugSvc.ValidateData(userGroups, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        public bool ValidateDataPrivilegeInfo(List<PrivilegeInfo> newList, bool isCreate)
        {
            // validate duplicate
            var validateDic = ValidateDuplicate(newList);
            if (validateDic.Count > 0) {
                foreach (KeyValuePair<string, string> entry in validateDic)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                    // do something with entry.Value or entry.Key
                }
                return false; 
            }

            // validate data
            Dictionary<string, string> dic = piSvc.ValidateData(newList);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        private Dictionary<string, string> ValidateDuplicate(List<PrivilegeInfo> newList)
        {
            var errDic = new Dictionary<string, string>();
            var dtDic = new Dictionary<int, int>();
            var rowNoDic = new Dictionary<int, int>();
            for (int i = 0; i < newList.Count;i++ )
            {
                var rowNo = i + 1;

                if (dtDic.ContainsKey(newList[i].MenuID))
                {
                    // add row no duplicate data
                    rowNoDic.Add(rowNo, rowNo);
                    var duplicateRowNo = 0;
                    dtDic.TryGetValue(newList[i].MenuID, out duplicateRowNo);
                    rowNoDic.Add( duplicateRowNo, duplicateRowNo);

                    errDic.Add("ErrDuplicate" + rowNo.ToString(), string.Format(GetValueFromResource("VALIDATE_DUPLICATE_PRIVILEGE"), duplicateRowNo.ToString(),rowNo.ToString()));
                }
                else
                {
                    dtDic.Add(newList[i].MenuID, rowNo);
                }                
            }

            ViewBag.ErrRowNo = rowNoDic;
            return errDic;
        }

        private ActionResult CreateInputView(UserGroups userGroups)
        {
            GenerateStatusEnabledCombo(Convert.ToInt32(userGroups.IsEnabled).ToString());
            return View("Input", userGroups);
        }

        private bool GenerateStatusEnabledCombo(string selected_id = "")
        {
            if (selected_id == "0")
            {
                selected_id = "false";
            }
            else
            {
                selected_id = "true";
            }
            var dict = new Dictionary<string, string>();
            dict.Add("false", "Tidak Aktif");
            dict.Add("true", "Aktif");
            ViewBag.UserGroupStatusEnableCombo = new SelectList(dict, "Key", "Value", selected_id);
            return dict.Count > 0;
        }


        private UserGroups BindDataToObject(FormCollection param)
        {
            String username = secSvc.GetUserLogin();

            UserGroups userGroup = new UserGroups();
            userGroup.ID = Convert.ToInt32(param["ID"].ToString());
            userGroup.GroupName = param["GroupName"].ToString();

            var comboIsEnabled = param["UserGroupStatusEnableCombo"];
            userGroup.IsEnabled = comboIsEnabled == null ? true : Convert.ToBoolean(comboIsEnabled);
            userGroup.CreatedBy = username;
            userGroup.LastUpdateBy = username;
            return userGroup;
        }

        private List<PrivilegeInfo> BindDataPrivilegeInfoToObject(FormCollection param)
        {
            var list = new List<PrivilegeInfo>();
            String username = secSvc.GetUserLogin();

            var paramMenuID = param["MenuID"];            

            if (paramMenuID != null)
            {
                string[] id = param["ID"].Split(',');
                string[] menuId = paramMenuID.Split(',');

                if (menuId.Length > 0)
                {
                    for (int i = 0; i < menuId.Length; i++)
                    {    
                        PrivilegeInfo privilegeInfo = new PrivilegeInfo();
                        privilegeInfo.ID = Convert.ToInt32(id[i]);
                        privilegeInfo.UserGroupsID = Convert.ToInt32(param["UserGroupsID"]);
                        privilegeInfo.MenuID = Convert.ToInt32(menuId[i]);
                        privilegeInfo.CreatedBy = username;
                        privilegeInfo.LastUpdateBy = username;

                        // add value for rollback form
                        Menu menu = mnSvc.GetMenu(Convert.ToInt32(menuId[i]));
                        privilegeInfo.NavigationLabel = menu.NavigationLabel;
                        privilegeInfo.Description = menu.Description;

                        list.Add(privilegeInfo);
                    }
                }       
            }                 

            return list;
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            try
            {
                UserGroups userGroup = new UserGroups();
                userGroup.ID = ID;
                userGroup.IsDeleted = true;
                userGroup.LastUpdateBy = secSvc.GetUserLogin();

                ugSvc.DeleteDataUserGroups(userGroup);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }


    }
}