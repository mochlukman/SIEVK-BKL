using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;

namespace SIEVK.Service.Controllers
{
    public class LoginController : Controller
    {
        #region private Variable Declaration
        private UsersService uSvc = new UsersService();
        private SecurityService secSvc = new SecurityService();
        #endregion

        // GET: /Login/
        public ActionResult Index(String msg = "")
        {
            TempData["ErrorMsg"] = msg;
            return View();
        }

        public ActionResult SessionEnd()
        {
            String msg = "Session anda telah berakhir. Silakan log in kembali";
            return RedirectToAction("Index", "Login", new { msg = msg });
        }

        [HttpPost]
        public ActionResult Logon(FormCollection param)
        {
            var msg = string.Empty;

            var username = param["username"];
            var password = param["password"];

            if (string.IsNullOrEmpty(username))
            {
                msg = "Username is required";
            }
            else
            {
                Users user = uSvc.GetUser(username, true);
				
                if (user != null)
                {
                    if (secSvc.IsPasswordMatch(user, password))
                    {
                        Session["Username"] = user.UserName;
                        Session["FirstName"] = user.FirstName;
                        Session["UserGroupID"] = user.UserGroupID;
                        Session["UnitKey"] = ""; 
                        Session["KdUnit"] = ""; 
                        Session["NmUnit"] = "";
                        if ((user.UnitKey != "") && (user.UnitKey != null))
                        {
                            Session["UnitKey"] = user.UnitKey;
                            Session["KdUnit"] = user.KdUnit;
                            Session["NmUnit"] = user.NmUnit;
                        }

                        var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));//Request.Url.GetLeftPart(UriPartial.Authority) + "/";
                        MenuService lm = new MenuService();
                        Session["Menu"] = lm.GetMenu(user.UserGroupID, baseUrl);

                        return RedirectToAction("Home", "General");
                    }
                    msg = "Incorrect Password";
                }
                else
                {
                    msg = "Username " + username + " doesn't exist";
                }
             }

            return RedirectToAction("Index", "Login", new { msg = msg});
        }

        public ActionResult Logout()
        {
            Session.Clear();
            System.Web.HttpContext.Current.Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }

    }
}