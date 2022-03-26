using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.RENSTRA;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.RENSTRA;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class RENSTRAController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private RENSTRAEvaluasiBijakService evaBijakSvc = new RENSTRAEvaluasiBijakService();
        private RENSTRAEvaluasiSimpulService evaSimpulSvc = new RENSTRAEvaluasiSimpulService();
        #endregion private Variable Declaration

        

        #region Evaluasi Bijak
        public ActionResult EvaluasiBijak(string msg = "", string kdUnit = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdUnit = kdUnit;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            GenerateUnitCombo(kdUnit);
            return View();
        }

        public ActionResult EvaluasiBijakCreate(string kdUnit)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiBijak evaBijak = new RENSTRAEvaluasiBijak();
            evaBijak.UNITKEY = kdUnit;
            return CreateInputEvaluasiBijak(evaBijak, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiBijakCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiBijak evaBijak = new RENSTRAEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, true);
                if (ValidateDataEvaluasiBijak(evaBijak, true))
                {
                    evaBijakSvc.InsertData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RENSTRA", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdUnit = evaBijak.UNITKEY, nomor = evaBijak.NOMOR });
                }
                else
                {
                    return CreateInputEvaluasiBijak(evaBijak, "true");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiBijak(evaBijak, "true");
            }
        }


        public ActionResult EvaluasiBijakEdit(string kdUnit, string nomor)
        {
            RENSTRAEvaluasiBijak evaBijak = new RENSTRAEvaluasiBijak();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaBijak = evaBijakSvc.GetEvaluasiBijak(kdUnit, nomor);
                return CreateInputEvaluasiBijak(evaBijak, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiBijak(evaBijak, "false");
            }
        }

        [HttpPost]
        public ActionResult EvaluasiBijakEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiBijak evaBijak = new RENSTRAEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, false);
                if (ValidateDataEvaluasiBijak(evaBijak, false))
                {
                    evaBijakSvc.UpdateData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RENSTRA", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdUnit = evaBijak.UNITKEY, nomor = evaBijak.NOMOR });
                }
                else
                {
                    return CreateInputEvaluasiBijak(evaBijak, "false");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiBijak(evaBijak, "false");
            }
        }

        public JsonResult DeleteEvaluasiBijak(string kdUnit = "", string nomor = "")
        {
            try
            {
                evaBijakSvc.DeleteData(kdUnit, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiBijak(string kdUnit = "", string nomor = "")
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

                // Getting all data    
                List<RENSTRAEvaluasiBijak> lst = new List<RENSTRAEvaluasiBijak>();
                lst = evaBijakSvc.GetEvaluasiBijakList(kdUnit);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.NOMOR.ToLower().Contains(searchValue) ||
                        m.NOMOR.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = lst.Count();
                //Paging     
                var data = lst.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateUnitCombo(kdUnit);
                return View("EvaluasiBijak");
            }
        }
        #endregion Evaluasi Bijak

        #region Evaluasi Simpul
        public ActionResult EvaluasiSimpul(string msg = "", string kdUnit = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdUnit = kdUnit;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            GenerateUnitCombo(kdUnit);
            return View();
        }

        public ActionResult EvaluasiSimpulCreate(string kdUnit)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiSimpul evaSimpul = new RENSTRAEvaluasiSimpul();
            evaSimpul.UNITKEY = kdUnit;
            return CreateInputEvaluasiSimpul(evaSimpul, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiSimpul evaSimpul = new RENSTRAEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, true);
                if (ValidateDataEvaluasiSimpul(evaSimpul, true))
                {
                    evaSimpulSvc.InsertData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RENSTRA", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdUnit = evaSimpul.UNITKEY, nomor = evaSimpul.NOMOR });
                }
                else
                {
                    return CreateInputEvaluasiSimpul(evaSimpul, "true");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiSimpul(evaSimpul, "true");
            }
        }


        public ActionResult EvaluasiSimpulEdit(string kdUnit, string nomor)
        {
            RENSTRAEvaluasiSimpul evaSimpul = new RENSTRAEvaluasiSimpul();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaSimpul = evaSimpulSvc.GetEvaluasiSimpul(kdUnit, nomor);
                return CreateInputEvaluasiSimpul(evaSimpul, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiSimpul(evaSimpul, "false");
            }
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENSTRAEvaluasiSimpul evaSimpul = new RENSTRAEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, false);
                if (ValidateDataEvaluasiSimpul(evaSimpul, false))
                {
                    evaSimpulSvc.UpdateData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RENSTRA", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdUnit = evaSimpul.UNITKEY, nomor = evaSimpul.NOMOR });
                }
                else
                {
                    return CreateInputEvaluasiSimpul(evaSimpul, "false");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiSimpul(evaSimpul, "false");
            }
        }

        public JsonResult DeleteEvaluasiSimpul(string kdUnit = "", string nomor = "")
        {
            try
            {
                evaSimpulSvc.DeleteData(kdUnit, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiSimpul(string kdUnit = "", string nomor = "")
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

                // Getting all data    
                List<RENSTRAEvaluasiSimpul> lst = new List<RENSTRAEvaluasiSimpul>();
                lst = evaSimpulSvc.GetEvaluasiSimpulList(kdUnit);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.NOMOR.ToLower().Contains(searchValue) ||
                        m.NOMOR.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = lst.Count();
                //Paging     
                var data = lst.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateUnitCombo(kdUnit);
                return View("EvaluasiSimpul");
            }
        }
        #endregion Evaluasi Simpul

        #region Private Function
        
        private ActionResult CreateInputEvaluasiBijak(RENSTRAEvaluasiBijak clr, string isCreate)
        {
            GenerateUnitCombo(clr.UNITKEY == null ? string.Empty : clr.UNITKEY);
            ViewBag.isCreate = isCreate;
            if (isCreate == "true")
            {
                ViewBag.Method = "Tambah";
            }
            else
            {
                ViewBag.Method = "Edit";
            }

            return View("EvaluasiBijakEdit", clr);
        }

        private ActionResult CreateInputEvaluasiSimpul(RENSTRAEvaluasiSimpul clr, string isCreate)
        {
            GenerateUnitCombo(clr.UNITKEY == null ? string.Empty : clr.UNITKEY);
            ViewBag.isCreate = isCreate;
            if (isCreate == "true")
            {
                ViewBag.Method = "Tambah";
            }
            else
            {
                ViewBag.Method = "Edit";
            }

            return View("EvaluasiSimpulEdit", clr);
        }

        public bool ValidateDataEvaluasiBijak(RENSTRAEvaluasiBijak clr, bool isCreate)
        {
            Dictionary<string, string> dic = evaBijakSvc.ValidateData(clr, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        public bool ValidateDataEvaluasiSimpul(RENSTRAEvaluasiSimpul clr, bool isCreate)
        {
            Dictionary<string, string> dic = evaSimpulSvc.ValidateData(clr, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        public ActionResult GetUnitOrganization()
        {
            GeneralService gsvc = new GeneralService();
            var list = gsvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
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
            if (unitkey != "")
            {
                DaftUnit result = list.Find(x => x.UNITKEY == unitkey.Trim());

                ViewBag.flexdatalist = result.KDUNIT + " " + result.NMUNIT;
            }
            return list.Count > 0;
        }

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }        

        private RENSTRAEvaluasiBijak BindDataToObjectEvaluasiBijak(FormCollection param, bool isCreate)
        {
            RENSTRAEvaluasiBijak evaBijak = new RENSTRAEvaluasiBijak();
            //if (isCreate)
            //{
            //    evaBijak.kdUnit = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
                evaBijak.UNITKEY = param["UNITKEY"].ToString();
            //}

            evaBijak.NOMOR = param["NOMOR"].ToString();
            evaBijak.URAIAN = param["URAIAN"].ToString();
            evaBijak.KESESUAIAN = int.Parse(param["KESESUAIAN"].ToString());
            evaBijak.MASALAH = param["MASALAH"].ToString();
            evaBijak.SOLUSI = param["SOLUSI"].ToString();
            evaBijak.KET = param["KET"].ToString();

            return evaBijak;
        }

        private RENSTRAEvaluasiSimpul BindDataToObjectEvaluasiSimpul(FormCollection param, bool isCreate)
        {
            RENSTRAEvaluasiSimpul evaSimpul = new RENSTRAEvaluasiSimpul();
            //if (isCreate)
            //{
            //    evaSimpul.kdUnit = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
            evaSimpul.UNITKEY = param["UNITKEY"].ToString();
            //}

            evaSimpul.NOMOR = param["NOMOR"].ToString();
            evaSimpul.ASPEK = param["ASPEK"].ToString();
            evaSimpul.PENJELASAN = param["PENJELASAN"].ToString();
            evaSimpul.KET = param["KET"].ToString();

            return evaSimpul;
        }

        #endregion

   

    }
}