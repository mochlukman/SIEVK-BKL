
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.RPJPD;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.RPJPD;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class RPJPDController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private RPJPDEvaluasiBijakService evaBijakSvc = new RPJPDEvaluasiBijakService();
        private RPJPDEvaluasiSimpulService evaSimpulSvc = new RPJPDEvaluasiSimpulService();
        #endregion

        #region Evaluasi Bijak
        public ActionResult EvaluasiBijak(string msg = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            return View();
        }

        public ActionResult EvaluasiBijakCreate()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJPDEvaluasiBijak evaBijak = new RPJPDEvaluasiBijak();
            return CreateInputEvaluasiBijak(evaBijak, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiBijakCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJPDEvaluasiBijak evaBijak = new RPJPDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, true);
                if (ValidateDataEvaluasiBijak(evaBijak, true))
                {
                    evaBijakSvc.InsertData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RPJPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), nomor = evaBijak.NOMOR });
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


        public ActionResult EvaluasiBijakEdit(string nomor)
        {
            RPJPDEvaluasiBijak evaBijak = new RPJPDEvaluasiBijak();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaBijak = evaBijakSvc.GetEvaluasiBijak(nomor);
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

            RPJPDEvaluasiBijak evaBijak = new RPJPDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, false);
                if (ValidateDataEvaluasiBijak(evaBijak, false))
                {
                    evaBijakSvc.UpdateData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RPJPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), nomor = evaBijak.NOMOR });
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

        public JsonResult DeleteEvaluasiBijak(string nomor = "")
        {
            try
            {
                evaBijakSvc.DeleteData(nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiBijak(string nomor = "")
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
                List<RPJPDEvaluasiBijak> lst = new List<RPJPDEvaluasiBijak>();
                lst = evaBijakSvc.GetEvaluasiBijakList();

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

                return View("EvaluasiBijak");
            }
        }

        public bool ValidateDataEvaluasiBijak(RPJPDEvaluasiBijak clr, bool isCreate)
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

        private RPJPDEvaluasiBijak BindDataToObjectEvaluasiBijak(FormCollection param, bool isCreate)
        {
            RPJPDEvaluasiBijak evaBijak = new RPJPDEvaluasiBijak();

            evaBijak.NOMOR = param["NOMOR"].ToString();
            evaBijak.URAIAN = param["URAIAN"].ToString();
            evaBijak.KESESUAIAN = int.Parse(param["KESESUAIAN"].ToString());
            evaBijak.MASALAH = param["MASALAH"].ToString();
            evaBijak.SOLUSI = param["SOLUSI"].ToString();
            evaBijak.KET = param["KET"].ToString();

            return evaBijak;
        }

        private ActionResult CreateInputEvaluasiBijak(RPJPDEvaluasiBijak clr, string isCreate)
        {
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


        #endregion Evaluasi Bijak

        #region Evaluasi Simpul
        public ActionResult EvaluasiSimpul(string msg = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            return View();
        }

        public ActionResult EvaluasiSimpulCreate()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJPDEvaluasiSimpul evaSimpul = new RPJPDEvaluasiSimpul();
            return CreateInputEvaluasiSimpul(evaSimpul, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJPDEvaluasiSimpul evaSimpul = new RPJPDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, true);
                if (ValidateDataEvaluasiSimpul(evaSimpul, true))
                {
                    evaSimpulSvc.InsertData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RPJPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), nomor = evaSimpul.NOMOR });
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


        public ActionResult EvaluasiSimpulEdit(string nomor)
        {
            RPJPDEvaluasiSimpul evaSimpul = new RPJPDEvaluasiSimpul();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaSimpul = evaSimpulSvc.GetEvaluasiSimpul(nomor);
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

            RPJPDEvaluasiSimpul evaSimpul = new RPJPDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, false);
                if (ValidateDataEvaluasiSimpul(evaSimpul, false))
                {
                    evaSimpulSvc.UpdateData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RPJPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), nomor = evaSimpul.NOMOR });
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

        public JsonResult DeleteEvaluasiSimpul(string nomor = "")
        {
            try
            {
                evaSimpulSvc.DeleteData(nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiSimpul(string nomor = "")
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
                List<RPJPDEvaluasiSimpul> lst = new List<RPJPDEvaluasiSimpul>();
                lst = evaSimpulSvc.GetEvaluasiSimpulList();

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

                return View("EvaluasiSimpul");
            }
        }

        public bool ValidateDataEvaluasiSimpul(RPJPDEvaluasiSimpul clr, bool isCreate)
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

        private RPJPDEvaluasiSimpul BindDataToObjectEvaluasiSimpul(FormCollection param, bool isCreate)
        {
            RPJPDEvaluasiSimpul evaSimpul = new RPJPDEvaluasiSimpul();

            evaSimpul.NOMOR = param["NOMOR"].ToString();
            evaSimpul.ASPEK = param["ASPEK"].ToString();
            evaSimpul.PENJELASAN = param["PENJELASAN"].ToString();
            evaSimpul.KET = param["KET"].ToString();

            return evaSimpul;
        }

        private ActionResult CreateInputEvaluasiSimpul(RPJPDEvaluasiSimpul clr, string isCreate)
        {
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


        #endregion Evaluasi Simpul

        #region Private Function

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }

        private void SetTahuntoViewBag()
        {
            List<TahunAnggaran> tahuns = genSvc.GetTahunAnggaran();
            String tahun1 = "-", tahun2 = "-", tahun3 = "-", tahun4 = "-", tahun5 = "-", tahun6 = "-";
            foreach (TahunAnggaran thn in tahuns)
            {
                switch (thn.KDTAHUN)
                {
                    case "1":
                        tahun1 = thn.NMTAHUN;
                        break;
                    case "2":
                        tahun2 = thn.NMTAHUN;
                        break;
                    case "3":
                        tahun3 = thn.NMTAHUN;
                        break;
                    case "4":
                        tahun4 = thn.NMTAHUN;
                        break;
                    case "5":
                        tahun5 = thn.NMTAHUN;
                        break;
                    case "6":
                        tahun6 = thn.NMTAHUN;
                        break;
                }

            }

            ViewBag.Tahun1 = tahun1;
            ViewBag.Tahun2 = tahun2;
            ViewBag.Tahun3 = tahun3;
            ViewBag.Tahun4 = tahun4;
            ViewBag.Tahun5 = tahun5;
            ViewBag.Tahun6 = tahun6;
        }

        

        #endregion
    }
}