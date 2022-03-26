using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessService.Common
{
    public class SecurityService
    {
        public string Encrypt(String input)
        {
            Encoding encoding = System.Text.Encoding.Unicode;
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public string Decrypt(String hexInput)
        {
            Encoding encoding = System.Text.Encoding.Unicode;
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        public Boolean CekPrivilege(Controller controller)
        {
            var url = string.Empty;

            string actionName = controller.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            url = controllerName + "/" + actionName + "/";
            string userName = controller.HttpContext.Session["Username"].ToString();

            // title page
            controller.ViewBag.Title = AddSpacesToSentence(actionName, true) + " " + AddSpacesToSentence(controllerName, true);

            MenuDAO m = new MenuDAO();
            DataTable dt = m.CheckPrivilege(userName, url);
            return dt.Rows.Count > 0 ? true : false;
        }

        public string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public bool IsLogin()
        {
            var sess = System.Web.HttpContext.Current.Session["Username"];
            return sess != null;
        }

        public String GetUserLogin()
        {
            return System.Web.HttpContext.Current.Session["Username"].ToString();
        }

        public Boolean HasUnitKey()
        {
            String unitkey =  System.Web.HttpContext.Current.Session["UnitKey"].ToString();
            if (unitkey != "")
            {
                return true;
            }
            return false;
        }

        public bool IsPasswordMatch(Users user, String password)
        {
            SecurityService sec = new SecurityService();
            password = sec.Encrypt(password);
            if (user.Password == password)
            {
                return true;
            }
            return false;
        }

    }
}
