using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.Common;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Common
{
    public class MenuService
    {
        Mapper map = new Mapper();
        MenuDAO dao = new MenuDAO();

        public string GetMenu(int userGroupId, string baseUrl)
        {
            string strHtml = string.Empty;

            MenuDAO m = new MenuDAO();
            DataTable dtParent = m.GetParentMenu(userGroupId);
            List<Menu> listParent = null;
            if (dtParent.Rows.Count > 0)
            {
                listParent = map.BindDataList<Menu>(dtParent);

                int n = 1;
                foreach (Menu parentMenu in listParent)
                {
                    string isOpenNewTab = string.Empty;
                    DataTable dtFristChild = m.GetChildMenu(userGroupId, parentMenu.ID);
                    List<Menu> firstChild = null;                    
                    if (dtFristChild.Rows.Count > 0)
                    {
                        // create parent
                        string parentMenuIconColor = "background-color:  "+ parentMenu.IconColor;
                        //strHtml += @"<li class=""dropdown"" style=""" + parentMenuIconColor + "<a href=""javascript:;"" class=""dropdown-toggle"" data-toggle=""dropdown""><i class=" + parentMenu.IconClass + "></i><span>" + parentMenu.NavigationLabel + "</span>";
                        strHtml += @"<li class=""dropdown"" style='" + parentMenuIconColor + "'><a href='javascript:;' class='dropdown-toggle' data-toggle='dropdown'><i class=" + parentMenu.IconClass + "></i><span>" + parentMenu.NavigationLabel + "</span>";
                        strHtml += @" <b class=""caret""></b></a>";
                        strHtml += @"<ul class=""dropdown-menu multi-level"" role=""menu"" aria-labelledby=""dropdownMenu"">";

                        firstChild = map.BindDataList<Menu>(dtFristChild);
                        foreach (Menu firstChildMenu in firstChild)
                        {                            
                            // get second child
                            List<Menu> secondChild = null;
                            DataTable dtSecondChild = m.GetChildMenu(userGroupId, firstChildMenu.ID);
                            if (dtSecondChild.Rows.Count > 0)
                            {
                                // first child if it have second child
                                // create second child parent
                                strHtml += @"<li class=""dropdown-submenu"">";
                                strHtml += @"<a href=""javascript:;""><span>" + firstChildMenu.NavigationLabel + "</span></a>";
                                strHtml += @"<ul class=""dropdown-menu"">";


                                secondChild = map.BindDataList<Menu>(dtSecondChild);
                                foreach (Menu secondChildMenu in secondChild)
                                {
                                    // Check IsOpenNewTab
                                    isOpenNewTab = (secondChildMenu.IsOpenNewTab) ? "target='_blank'" : string.Empty;

                                    // create second child
                                    strHtml += @"<li><a href=" + baseUrl + secondChildMenu.URL + " " + isOpenNewTab + ">" + secondChildMenu.NavigationLabel + "</a></li>";

                                }
                                strHtml += @"</ul></li>";
                            }
                            else
                            {
                                // first child if no second child
                                // Check IsOpenNewTab
                                isOpenNewTab = (firstChildMenu.IsOpenNewTab) ? "target='_blank'" : string.Empty;

                                // create first child
                                strHtml += @"<li><a href=" + baseUrl + firstChildMenu.URL + " " + isOpenNewTab + ">" + firstChildMenu.NavigationLabel + "</a></li>";

                            }
                            
                        }

                        strHtml += @"</ul></li>";
                    }
                    else
                    {
                        // Check IsOpenNewTab
                        isOpenNewTab = (parentMenu.IsOpenNewTab) ? "target='_blank'" : string.Empty;

                        // set parent without child menu
                        strHtml += n == 1 ? @"<li class=""active"">" : @"<li>";
                        strHtml += @"<a href=" + baseUrl + parentMenu.URL + " " + isOpenNewTab + "><i class=" + parentMenu.IconClass + "></i><span>" + parentMenu.NavigationLabel + "</span> </a> </li>";
                    }
                    n++;
                }
            }


            return strHtml;
        }

        public List<Menu> GetAllMenu()
        {
            MenuDAO mn = new MenuDAO();
            DataTable dt = mn.GetList(null);
            List<Menu> lst = map.BindDataList<Menu>(dt);
            return lst;
        }

        public void DeleteMenu(Menu menu)
        {
            dao.Delete(menu);
        }

        public Menu GetMenu(int ID)
        {
            DataTable dt = dao.GetList(ID);
            Menu menu = null;
            if (dt.Rows.Count > 0)
            {
                menu = map.BindData<Menu>(dt);
            }
            return menu;
        }

    }
}
