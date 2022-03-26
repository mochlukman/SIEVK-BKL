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
    public class UsersController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private UsersService uSvc = new UsersService();
        private GeneralService genSvc = new GeneralService();
        #endregion

        public ActionResult LoadDataUsers()
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

                // Getting all Customer data    
                List<Users> usersData = uSvc.GetAllUsers();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    usersData = usersData.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    usersData = usersData.Where(m => m.UserName.ToLower().Contains(searchValue) ||
                        m.FirstName.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = usersData.Count();
                //Paging     
                var data = usersData.Skip(skip).Take(pageSize).ToList();
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

        public ActionResult Edit(String userName)
        {
            Users user = new Users();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                user = uSvc.GetUser(userName, null);
                return CreateInputView(user);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(user);
            }
        }

        public ActionResult ChangePassword(String userName, bool afterReload = false)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }
            Users user = new Users();
            try
            {
                if (userName == "loginuser") //from Layout
                {
                    userName = secSvc.GetUserLogin();
                    ViewBag.isLoginUser = "true";
                }
                else { ViewBag.isLoginUser = "false"; }

                if (afterReload == true)
                {
                    ViewBag.Info = GetValueFromResource("SUCCESS_EDIT");
                    ViewBag.isLoginUser = "true";
                }

                user = uSvc.GetUser(userName, null);
                user.Password = secSvc.Decrypt(user.Password);
                return View("ChangePassword", user);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("ChangePassword", user);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Users user = new Users();
            String userName = param["UserName"].ToString();
            String password = param["Password"].ToString();
            Boolean isLoginUser = Convert.ToBoolean(param["isLoginUser"].ToString());
            try
            {
                user = uSvc.GetUser(userName, null);
                user.Password = secSvc.Encrypt(password);

                if (ValidateData(user, false))
                {
                    uSvc.UpdateDataUsers(user);
                    if (!isLoginUser)
                    {
                        return RedirectToAction("Index", "Users", new { msg = GetValueFromResource("SUCCESS_EDIT") });
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword", "Users", new { userName = userName, afterReload= true });
                    }
                }
                else
                {
                    user.Password = password;
                    return View("Input", user);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("ChangePassword", user);
            }
        }

        public ActionResult Detail(string userName)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Users user = new Users();
            try
            {
                user = uSvc.GetUser(userName, null);
                return View(user);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return RedirectToAction("Index");
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

            Users user = new Users();
            try
            {
                user = BindDataToObject(param, false);
                if (ValidateData(user, false))
                {
                    uSvc.UpdateDataUsers(user);
                    return RedirectToAction("Index", "Users", new { msg = GetValueFromResource("SUCCESS_EDIT") });
                }
                else
                {
                    return CreateInputView(user);
                }
            }
            catch(Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(user);
            }
        }

        
        public ActionResult Create()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Users user = new Users();
            return CreateInputView(user);
        }

        [HttpPost]
        public ActionResult Create(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Users user = new Users();
            try
            {
                user = BindDataToObject(param, true);
                if (ValidateData(user, true))
                {
                    String passwordencr = secSvc.Encrypt(user.Password);
                    user.Password = passwordencr;
                    uSvc.InsertDataUsers(user);
                    return RedirectToAction("Index", "Users", new { msg = GetValueFromResource("SUCCESS_INPUT") });
                }
                else
                {
                    return CreateInputView(user);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(user);
            }
        }

        public bool ValidateData(Users user, bool isCreate)
        {
            Dictionary<string, string> dic = uSvc.ValidateData(user, isCreate);
            foreach(KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        private ActionResult CreateInputView(Users user)
        {
            GenerateUserGroupsCombo(user.UserGroupID.ToString());
            GenerateUnitCombo(user.UnitKey);
            GenerateStatusAktifCombo(Convert.ToInt32(user.IsActive).ToString());
            if (String.IsNullOrEmpty(user.ID.ToString()) || user.ID == 0)
            {
                ViewBag.isEdit = false;
            }
            else
            {
                ViewBag.isEdit = true;
            }
            return View("Input", user);
        }

        private bool GenerateUserGroupsCombo(string selected_id = "")
        {
            if (selected_id == "0")
            {
                selected_id = "";
            }
            UserGroupsService svc = new UserGroupsService();
            List<UserGroups> list = svc.GetAllUserGroups(null);
            list = list.OrderBy(x => x.GroupName).ToList();
            ViewBag.UserGroupsCombo = new SelectList(list, "Id", "GroupName", selected_id);
            return list.Count > 0;
        }

        private bool GenerateUnitCombo(string unitkey = null)
        {
            if (unitkey == null)
            {
                unitkey = "";
            }
            GeneralService svc = new GeneralService();
            List<DaftUnit> list = svc.GetUnitOrganization(null, "3"); //yg kode levelnya 3 confirm kang Lukman 7/4/2019
            list = list.OrderBy(x => x.KDNMUNITFULL).ToList();
            ViewBag.UnitCombo = new SelectList(list, "UNITKEY", "KDNMUNITFULL", unitkey.Trim());
            return list.Count > 0;
        }

        private bool GenerateStatusAktifCombo(string selected_id = "")
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
            ViewBag.UserStatusAktifCombo = new SelectList(dict, "Key", "Value", selected_id);
            return dict.Count > 0;
        }

         
        private Users BindDataToObject(FormCollection param, bool isCreate)
        {
            String username = secSvc.GetUserLogin();
            Users user = new Users();
            user.ID = Convert.ToInt32(param["ID"].ToString());
            user.UserGroupID = Convert.ToInt32(param["UserGroupsCombo"].ToString());
            user.UnitKey = param["UnitCombo"].ToString();
            user.UserName = param["UserName"].ToString();
            user.FirstName = param["FirstName"].ToString();
            user.LastName = param["LastName"].ToString();
            user.IsActive = Convert.ToBoolean(param["UserStatusAktifCombo"].ToString());
            user.Email = param["Email"].ToString();
            user.CreatedBy = username;
            user.LastUpdateBy = username;
            if (isCreate)
            {
                user.Password = param["Password"].ToString();
            }
            return user;
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            try
            {
                Users user = new Users();
                user.ID = ID;
                user.IsDeleted = true;
                user.LastUpdateBy = secSvc.GetUserLogin();

                uSvc.DeleteDataUsers(user);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

    }
}