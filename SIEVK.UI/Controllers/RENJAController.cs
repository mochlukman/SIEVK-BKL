using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.RENJA;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.RENJA;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class RENJAController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private RENJAEvaluasiBijakService evaBijakSvc = new RENJAEvaluasiBijakService();
        private RENJAEvaluasiSimpulService evaSimpulSvc = new RENJAEvaluasiSimpulService();
        private RENJAEvaluasiKendaliService evaKendaliSvc = new RENJAEvaluasiKendaliService();
        #endregion private Variable Declaration



        #region Evaluasi Bijak
        public ActionResult EvaluasiBijak(string msg = "", string kdTahun = "", string kdUnit = "", string nomor = "", string kdTahap = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.kdUnit = kdUnit;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;
            GenerateUnitCombo(kdUnit);
            GenerateTahunAnggaranCombo(kdTahun);
            return View();
        }

        public ActionResult EvaluasiBijakCreate(string kdTahun, string kdUnit)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiBijak evaBijak = new RENJAEvaluasiBijak();
            evaBijak.KDTAHUN = kdTahun;
            evaBijak.UNITKEY = kdUnit;
            return CreateInputEvaluasiBijak(evaBijak, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiBijakCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiBijak evaBijak = new RENJAEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, true);
                if (ValidateDataEvaluasiBijak(evaBijak, true))
                {
                    evaBijakSvc.InsertData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RENJA", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = evaBijak.KDTAHUN, kdUnit = evaBijak.UNITKEY, nomor = evaBijak.NOMOR });
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


        public ActionResult EvaluasiBijakEdit(string kdTahun, string kdUnit, string nomor)
        {
            RENJAEvaluasiBijak evaBijak = new RENJAEvaluasiBijak();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaBijak = evaBijakSvc.GetEvaluasiBijak(kdTahun, kdUnit, nomor);
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

            RENJAEvaluasiBijak evaBijak = new RENJAEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, false);
                if (ValidateDataEvaluasiBijak(evaBijak, false))
                {
                    evaBijakSvc.UpdateData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RENJA", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = evaBijak.KDTAHUN, kdUnit = evaBijak.UNITKEY, nomor = evaBijak.NOMOR });
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

        public JsonResult DeleteEvaluasiBijak(string kdtahun = "", string kdUnit = "", string nomor = "")
        {
            try
            {
                evaBijakSvc.DeleteData(kdtahun, kdUnit, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiBijak(string kdTahun = "", string kdUnit = "", string nomor = "", string kdTahap = "")
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
                List<RENJAEvaluasiBijak> lst = new List<RENJAEvaluasiBijak>();
                lst = evaBijakSvc.GetEvaluasiBijakList(kdTahun, kdUnit);

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

                GenerateTahunAnggaranCombo(kdTahun);
                GenerateUnitCombo(kdUnit);
                GenerateTahapanCombo(kdTahap);
                return View("EvaluasiBijak");
            }
        }
        #endregion Evaluasi Bijak

        #region Evaluasi Simpul
        public ActionResult EvaluasiSimpul(string msg = "", string kdTahun = "", string kdUnit = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.kdUnit = kdUnit;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            GenerateUnitCombo(kdUnit);
            GenerateTahunAnggaranCombo(kdTahun);
            return View();
        }

        public ActionResult EvaluasiSimpulCreate(string kdTahun, string kdUnit)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiSimpul evaSimpul = new RENJAEvaluasiSimpul();
            evaSimpul.KDTAHUN = kdTahun;
            evaSimpul.UNITKEY = kdUnit;
            return CreateInputEvaluasiSimpul(evaSimpul, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiSimpul evaSimpul = new RENJAEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, true);
                if (ValidateDataEvaluasiSimpul(evaSimpul, true))
                {
                    evaSimpulSvc.InsertData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RENJA", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = evaSimpul.KDTAHUN, kdUnit = evaSimpul.UNITKEY, nomor = evaSimpul.NOMOR });
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


        public ActionResult EvaluasiSimpulEdit(string kdTahun, string kdUnit, string nomor)
        {
            RENJAEvaluasiSimpul evaSimpul = new RENJAEvaluasiSimpul();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaSimpul = evaSimpulSvc.GetEvaluasiSimpul(kdTahun, kdUnit, nomor);
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

            RENJAEvaluasiSimpul evaSimpul = new RENJAEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, false);
                if (ValidateDataEvaluasiSimpul(evaSimpul, false))
                {
                    evaSimpulSvc.UpdateData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RENJA", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = evaSimpul.KDTAHUN, kdUnit = evaSimpul.UNITKEY, nomor = evaSimpul.NOMOR });
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

        public JsonResult DeleteEvaluasiSimpul(string kdtahun = "", string kdUnit = "", string nomor = "")
        {
            try
            {
                evaSimpulSvc.DeleteData(kdtahun, kdUnit, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiSimpul(string kdTahun = "", string kdUnit = "", string nomor = "")
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
                List<RENJAEvaluasiSimpul> lst = new List<RENJAEvaluasiSimpul>();
                lst = evaSimpulSvc.GetEvaluasiSimpulList(kdTahun, kdUnit);

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
                GenerateTahunAnggaranCombo(kdTahun);
                return View("EvaluasiSimpul");
            }
        }
        #endregion Evaluasi Simpul

        #region Evaluasi Kendali
        public ActionResult EvaluasiKendali(string msg = "", string kdTahun = "", string kdUnit = "", string nomor = "", string kdTahap = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.kdUnit = kdUnit;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;
            ViewBag.kdTahap = kdTahap;

            GenerateTahapanCombo(kdTahap);
            GenerateUnitCombo(kdUnit);
            GenerateTahunAnggaranCombo(kdTahun);
            return View();
        }

        public ActionResult EvaluasiKendaliCreate(string kdTahun, string kdUnit, string kdTahap)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiKendali evaKendali = new RENJAEvaluasiKendali();
            evaKendali.KDTAHUN = kdTahun;
            evaKendali.UNITKEY = kdUnit;
            evaKendali.KDTAHAP = kdTahap;
            return CreateInputEvaluasiKendali(evaKendali, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiKendaliCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiKendali evaKendali = new RENJAEvaluasiKendali();
            try
            {
                evaKendali = BindDataToObjectEvaluasiKendali(param, true);
                if (ValidateDataEvaluasiKendali(evaKendali, true))
                {
                    evaKendaliSvc.InsertData(evaKendali);
                    return RedirectToAction("EvaluasiKendali", "RENJA", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = evaKendali.KDTAHUN, kdUnit = evaKendali.UNITKEY, nomor = evaKendali.NOMOR, kdTahap = evaKendali.KDTAHAP });
                }
                else
                {
                    return CreateInputEvaluasiKendali(evaKendali, "true");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiKendali(evaKendali, "true");
            }
        }


        public ActionResult EvaluasiKendaliEdit(string kdTahun, string kdUnit, string nomor, string kdTahap)
        {
            RENJAEvaluasiKendali evaKendali = new RENJAEvaluasiKendali();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaKendali = evaKendaliSvc.GetEvaluasiKendali(kdTahun, kdUnit, nomor, kdTahap);
                return CreateInputEvaluasiKendali(evaKendali, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiKendali(evaKendali, "false");
            }
        }

        [HttpPost]
        public ActionResult EvaluasiKendaliEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RENJAEvaluasiKendali evaKendali = new RENJAEvaluasiKendali();
            try
            {
                evaKendali = BindDataToObjectEvaluasiKendali(param, false);
                if (ValidateDataEvaluasiKendali(evaKendali, false))
                {
                    evaKendaliSvc.UpdateData(evaKendali);
                    return RedirectToAction("EvaluasiKendali", "RENJA", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = evaKendali.KDTAHUN, kdUnit = evaKendali.UNITKEY, nomor = evaKendali.NOMOR, kdTahap = evaKendali.KDTAHAP });
                }
                else
                {
                    return CreateInputEvaluasiKendali(evaKendali, "false");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputEvaluasiKendali(evaKendali, "false");
            }
        }

        public JsonResult DeleteEvaluasiKendali(string kdtahun = "", string kdUnit = "", string nomor = "", string kdTahap = "")
        {
            try
            {
                evaKendaliSvc.DeleteData(kdtahun, kdUnit, nomor, kdTahap);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiKendali(string kdTahun = "", string kdUnit = "", string nomor = "", string kdTahap = "")
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
                List<RENJAEvaluasiKendali> lst = new List<RENJAEvaluasiKendali>();
                lst = evaKendaliSvc.GetEvaluasiKendaliList(kdTahun, kdUnit, kdTahap);

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

                GenerateTahunAnggaranCombo(kdTahun);
                GenerateUnitCombo(kdUnit);
                return View("EvaluasiKendali");
            }
        }
        #endregion Evaluasi Kendali

        #region Private Function

        private ActionResult CreateInputEvaluasiBijak(RENJAEvaluasiBijak clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
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

        private ActionResult CreateInputEvaluasiSimpul(RENJAEvaluasiSimpul clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
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

        private ActionResult CreateInputEvaluasiKendali(RENJAEvaluasiKendali clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
            GenerateUnitCombo(clr.UNITKEY == null ? string.Empty : clr.UNITKEY);
            GenerateTahapanCombo(clr.KDTAHAP == null ? string.Empty : clr.KDTAHAP);
            ViewBag.isCreate = isCreate;
            if (isCreate == "true")
            {
                ViewBag.Method = "Tambah";
            }
            else
            {
                ViewBag.Method = "Edit";
            }

            return View("EvaluasiKendaliEdit", clr);
        }

        public bool ValidateDataEvaluasiBijak(RENJAEvaluasiBijak clr, bool isCreate)
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

        public bool ValidateDataEvaluasiSimpul(RENJAEvaluasiSimpul clr, bool isCreate)
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

        public bool ValidateDataEvaluasiKendali(RENJAEvaluasiKendali clr, bool isCreate)
        {
            Dictionary<string, string> dic = evaKendaliSvc.ValidateData(clr, isCreate);
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

            /*
            List<DaftUnit> newList = new List<DaftUnit>();
            foreach(DaftUnit dafUnit in list)
            {
                newList.Add()
            }
            */

            ViewBag.UnitCombo = new SelectList(list, "UNITKEY", "KDNMUNITFULL", unitkey.Trim());

            if (unitkey != "")
            {
                DaftUnit result = list.Find(x => x.UNITKEY == unitkey.Trim());

                ViewBag.flexdatalist = result.KDUNIT + " " + result.NMUNIT;
            }

            return list.Count > 0;
        }

        private bool GenerateTahunAnggaranCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahunAnggaran();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.NMTAHUN == DateTime.Now.Year.ToString())
                    {
                        selected_id = item.KDTAHUN;
                    }
                    dict.Add(item.KDTAHUN, item.NMTAHUN);
                }
            }
            if (selected_id == "")
            {
                TahunAnggaran x = list.OrderByDescending(z => Convert.ToInt32(z.NMTAHUN)).FirstOrDefault();
                selected_id = x.KDTAHUN;
            }
            ViewBag.TahunAnggaranCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        private bool GenerateTahapanCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahapan("2");
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDTAHAP, item.NMTAHAP);
                }
            }

            ViewBag.TahapanCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }        

        private RENJAEvaluasiBijak BindDataToObjectEvaluasiBijak(FormCollection param, bool isCreate)
        {
            RENJAEvaluasiBijak evaBijak = new RENJAEvaluasiBijak();
            //if (isCreate)
            //{
            //    evaBijak.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
                evaBijak.KDTAHUN = param["KDTAHUN"].ToString();
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

        private RENJAEvaluasiSimpul BindDataToObjectEvaluasiSimpul(FormCollection param, bool isCreate)
        {
            RENJAEvaluasiSimpul evaSimpul = new RENJAEvaluasiSimpul();
            //if (isCreate)
            //{
            //    evaSimpul.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
            evaSimpul.KDTAHUN = param["KDTAHUN"].ToString();
            evaSimpul.UNITKEY = param["UNITKEY"].ToString();
            //}

            evaSimpul.NOMOR = param["NOMOR"].ToString();
            evaSimpul.ASPEK = param["ASPEK"].ToString();
            evaSimpul.PENJELASAN = param["PENJELASAN"].ToString();
            evaSimpul.KET = param["KET"].ToString();

            return evaSimpul;
        }

        private RENJAEvaluasiKendali BindDataToObjectEvaluasiKendali(FormCollection param, bool isCreate)
        {
            RENJAEvaluasiKendali evaKendali = new RENJAEvaluasiKendali();
            //if (isCreate)
            //{
            //    evaKendali.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
            evaKendali.KDTAHUN = param["KDTAHUN"].ToString();
            evaKendali.UNITKEY = param["UNITKEY"].ToString();
            evaKendali.KDTAHAP = param["KDTAHAP"].ToString();
            //}

            evaKendali.NOMOR = param["NOMOR"].ToString();
            evaKendali.URAIAN = param["URAIAN"].ToString();

            evaKendali.TARGET_RENJA = param["TARGET_RENJA"].ToString();
            evaKendali.TARGET_RKA = param["TARGET_RKA"].ToString();
            evaKendali.PAGU_RKA = Convert.ToDecimal(param["PAGU_RKA"].ToString());  
            evaKendali.PAGU_RENJA = Convert.ToDecimal(param["PAGU_RENJA"].ToString());

            evaKendali.TYPE = param["TYPE"].ToString();
            evaKendali.KESESUAIAN = int.Parse(param["KESESUAIAN"].ToString());
            evaKendali.MASALAH = param["MASALAH"].ToString();
            evaKendali.SOLUSI = param["SOLUSI"].ToString();
            evaKendali.HASIL = param["HASIL"].ToString();

            return evaKendali;
        }
        #endregion



    }
}