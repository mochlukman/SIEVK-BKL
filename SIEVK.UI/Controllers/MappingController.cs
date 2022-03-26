using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.Mapping;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.Mapping;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class MappingController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private MappingReferenceService mapSvc = new MappingReferenceService();
        private MappingKegiatan13_90Service mappingKegiatan13_90Service = new MappingKegiatan13_90Service();
        private MappingKegiatan90_50Service mappingKegiatan90_50Service = new MappingKegiatan90_50Service();
        #endregion


        #region Mapping Kegiatan 13_90
        public ActionResult MappingKegiatan13_90(String msg = "", String idPrgrm = "", String idKeg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.idKeg = idKeg;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.Info = msg;

            List<MPGRM> prg = mapSvc.GetMPGRMList(idPrgrm);
            if (idPrgrm != "" && prg.Count > 0)
            {
                ViewBag.programName = prg[0].NUPRGRM + " - " + prg[0].NMPRGRM;
            }

            List<MKEGIATAN> keg = mapSvc.GetMKEGIATANList(idPrgrm, idKeg);
            if (keg.Count > 0)
            {
                ViewBag.kegiatanName = keg[0].NUKEG + " - " + keg[0].NMKEG;
            }

            //SetTahuntoViewBag();
            return View();
        }

        public ActionResult MappingKegiatan13_90Create(string idPrgrm, string idKeg)
        {
            MappingKegiatan13_90 mapKeg = new MappingKegiatan13_90();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";

                //mapKeg.IDPRGRM = idPrgrm;
                mapKeg.IDKEG = idKeg;
                mapKeg.IDKEG90 = string.Empty;
                mapKeg.KDTAHUN = string.Empty;

                //GenerateMPGRM90();
                //GenerateMKEGIATAN90();
                GenerateTahunAnggaranCombo();
                //SetTahuntoViewBag();
                return View("MappingKegiatan13_90Edit", mapKeg);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                //GenerateTahapanCombo(kdTahap);
                //SetTahuntoViewBag();
                return View("MappingKegiatan13_90");
            }
        }

        [HttpPost]
        public ActionResult MappingKegiatan13_90Create(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            MappingKegiatan13_90 mapKeg = new MappingKegiatan13_90();
            try
            {
                mapKeg = BindDataMappingKegiatan13_90ToObject(param);
                mappingKegiatan13_90Service.Create(mapKeg);
                mapKeg = mappingKegiatan13_90Service.GetKegiatanMappingByID(mapKeg.IDKEG, mapKeg.IDKEG90);
                return RedirectToAction("MappingKegiatan13_90", "Mapping", new { msg = GetValueFromResource("SUCCESS_EDIT"), idKeg = mapKeg.IDKEG, idPrgrm = mapKeg.IDPRGRM });
            }
            catch (Exception ex)
            {
                //SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan13_90", mapKeg);
            }
        }


        public ActionResult MappingKegiatan13_90Edit(string idKeg, string idKeg90, string kdTahun)
        {
            MappingKegiatan13_90 mapKeg = new MappingKegiatan13_90();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                mapKeg = mappingKegiatan13_90Service.GetKegiatanMappingByID(idKeg, idKeg90);

                ViewBag.Method = "Edit";
                ViewBag.idprgrm90 = mapKeg.IDPRGRM90;
                ViewBag.programName90 = mapKeg.NUPRGRM90 +' '+ mapKeg.NMPRGRM90;
                ViewBag.idKeg90 = mapKeg.IDKEG90;
                ViewBag.kegiatanName90 = mapKeg.NUKEG90 + ' ' + mapKeg.NMKEG90;


                //GenerateMPGRM90();
                //GenerateMKEGIATAN90();
                GenerateTahunAnggaranCombo(kdTahun);

                //SetTahuntoViewBag();
                return View("MappingKegiatan13_90Edit", mapKeg);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                //GenerateTahapanCombo(kdTahap);
                //SetTahuntoViewBag();
                return View("MappingKegiatan13_90Edit");
            }
        }

        [HttpPost]
        public ActionResult MappingKegiatan13_90Edit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            MappingKegiatan13_90 mapKeg = new MappingKegiatan13_90();
            try
            {
                mapKeg = BindDataMappingKegiatan13_90ToObject(param);
                mappingKegiatan13_90Service.Update(mapKeg);
                return RedirectToAction("MappingKegiatan13_90", "Mapping", new { msg = GetValueFromResource("SUCCESS_EDIT"), idKeg = mapKeg.IDKEG, idPrgrm = mapKeg.IDPRGRM });
            }
            catch (Exception ex)
            {
                //SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan13_90Edit", mapKeg);
            }
        }

        private MappingKegiatan13_90 BindDataMappingKegiatan13_90ToObject(FormCollection param)
        {
            MappingKegiatan13_90 mapKeg = new MappingKegiatan13_90();
            mapKeg.IDKEG = param["IDKEG"].ToString();
            
            //mapKeg.IDKEG90 = param["IDKEG90"].ToString();
            mapKeg.IDKEG90 = param["IDKegiatan90Selected"].ToString();
            mapKeg.IDKEG90_Prev = param["IDKEG90_Prev"].ToString();
            //mapKeg.KDTAHUN = param["KDTAHUN"].ToString();
            mapKeg.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            return mapKeg;
        }



        #endregion

        #region Mapping Kegiatan 90_50
        public ActionResult MappingKegiatan90_50(String msg = "", String idPrgrm90 = "", String idKeg90 = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.idKeg90 = idKeg90;
            ViewBag.idPrgrm90 = idPrgrm90;
            ViewBag.Info = msg;

            List<MPGRM90> prg = mapSvc.GetMPGRM90List(idPrgrm90);
            if (idPrgrm90 != "" && prg.Count > 0)
            {
                ViewBag.programName90 = prg[0].NUPRGRM90 + " " + prg[0].NMPRGRM90;
            }

            List<MKEGIATAN90> keg = mapSvc.GetMKEGIATAN90List(idPrgrm90, idKeg90);
            if (keg.Count > 0)
            {
                ViewBag.kegiatanName90 = keg[0].NUKEG90 + " " + keg[0].NMKEG90;
            }

            //SetTahuntoViewBag();
            return View();
        }

        public ActionResult MappingKegiatan90_50Create(string idPrgrm90, string idKeg90)
        {
            MappingKegiatan90_50 mapKeg = new MappingKegiatan90_50();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";

                //mapKeg.IDPRGRM90 = idPrgrm90;
                mapKeg.IDKEG90 = idKeg90;
                mapKeg.IDKEG050 = string.Empty;
                mapKeg.KDTAHUN = string.Empty;

                //GenerateMPGRM90();
                //GenerateMKEGIATAN90();
                GenerateTahunAnggaranCombo();
                //SetTahuntoViewBag();
                return View("MappingKegiatan90_50Edit", mapKeg);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                //GenerateTahapanCombo(kdTahap);
                //SetTahuntoViewBag();
                return View("MappingKegiatan90_50");
            }
        }

        [HttpPost]
        public ActionResult MappingKegiatan90_50Create(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            MappingKegiatan90_50 mapKeg = new MappingKegiatan90_50();
            try
            {
                mapKeg = BindDataMappingKegiatan90_50ToObject(param);
                mappingKegiatan90_50Service.Create(mapKeg);
                mapKeg = mappingKegiatan90_50Service.GetKegiatanMappingByID(mapKeg.IDKEG90, mapKeg.IDKEG050);
                return RedirectToAction("MappingKegiatan90_50", "Mapping", new { msg = GetValueFromResource("SUCCESS_EDIT"), idKeg90 = mapKeg.IDKEG90, idPrgrm90 = mapKeg.IDPRGRM90 });
            }
            catch (Exception ex)
            {
                //SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan90_50", mapKeg);
            }
        }


        public ActionResult MappingKegiatan90_50Edit(string idKeg90, string idKeg050, string kdTahun)
        {
            MappingKegiatan90_50 mapKeg = new MappingKegiatan90_50();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                mapKeg = mappingKegiatan90_50Service.GetKegiatanMappingByID(idKeg90, idKeg050);

                ViewBag.Method = "Edit";
                ViewBag.idprgrm50 = mapKeg.IDPRGRM050;
                ViewBag.programName50 = mapKeg.NUPRGRM050 + ' ' + mapKeg.NMPRGRM050;
                ViewBag.idKeg50 = mapKeg.IDKEG050;
                ViewBag.kegiatanName50 = mapKeg.NUKEG050 + ' ' + mapKeg.NMKEG050;


                //GenerateMPGRM90();
                //GenerateMKEGIATAN90();
                GenerateTahunAnggaranCombo(kdTahun);

                //SetTahuntoViewBag();
                return View("MappingKegiatan90_50Edit", mapKeg);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                //GenerateTahapanCombo(kdTahap);
                //SetTahuntoViewBag();
                return View("MappingKegiatan90_50Edit");
            }
        }

        [HttpPost]
        public ActionResult MappingKegiatan90_50Edit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            MappingKegiatan90_50 mapKeg = new MappingKegiatan90_50();
            try
            {
                mapKeg = BindDataMappingKegiatan90_50ToObject(param);
                mappingKegiatan90_50Service.Update(mapKeg);
                return RedirectToAction("MappingKegiatan90_50", "Mapping", new { msg = GetValueFromResource("SUCCESS_EDIT"), idKeg90 = mapKeg.IDKEG90, idPrgrm90 = mapKeg.IDPRGRM90 });
            }
            catch (Exception ex)
            {
                //SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan90_50Edit", mapKeg);
            }
        }

        private MappingKegiatan90_50 BindDataMappingKegiatan90_50ToObject(FormCollection param)
        {
            MappingKegiatan90_50 mapKeg = new MappingKegiatan90_50();
            mapKeg.IDKEG90 = param["IDKEG90"].ToString();

            //mapKeg.IDKEG050 = param["IDKEG050"].ToString();
            mapKeg.IDKEG050 = param["IDKegiatan50Selected"].ToString();
            mapKeg.IDKEG050_Prev = param["IDKEG050_Prev"].ToString();
            //mapKeg.KDTAHUN = param["KDTAHUN"].ToString();
            mapKeg.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            return mapKeg;
        }



        #endregion

        #region Private Function

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }


        private bool GenerateTahunAnggaranCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahunAnggaran();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
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

        private bool GenerateMPGRM90(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = mapSvc.GetMPGRM90List();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.NUPRGRM90, item.NMPRGRM90);
                }
            }

            ViewBag.MPGRM90Combo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        private bool GenerateMKEGIATAN90(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = mapSvc.GetMKEGIATAN90List();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.NUKEG90, item.NMKEG90);
                }
            }

            ViewBag.MKEGIATAN90Combo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        //private bool GenerateMPGRM90(string selected_id = "")
        //{
        //    var dict = new Dictionary<string, string>();

        //    var list = mapSvc.GetMPGRM90List();
        //    if (list.Count > 0)
        //    {
        //        foreach (var item in list)
        //        {
        //            dict.Add(item.NUPRGRM90, item.NMPRGRM90);
        //        }
        //    }

        //    ViewBag.TahunAnggaranCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
        //    return dict.Count > 0;
        //}

        //private bool GenerateMPGRM90(string selected_id = "")
        //{
        //    var dict = new Dictionary<string, string>();

        //    var list = mapSvc.GetMPGRM90List();
        //    if (list.Count > 0)
        //    {
        //        foreach (var item in list)
        //        {
        //            dict.Add(item.NUPRGRM90, item.NMPRGRM90);
        //        }
        //    }

        //    ViewBag.TahunAnggaranCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
        //    return dict.Count > 0;
        //}
        #endregion

        #region JSON Result

        [HttpPost]
        public JsonResult DeleteMappingKegiatan13_90(string idKeg, string idKEG90)
        {
            try
            {
                mappingKegiatan13_90Service.Delete(idKeg, idKEG90);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteMappingKegiatan90_50(string idKEG90, string idKEG050)
        {
            try
            {
                mappingKegiatan90_50Service.Delete(idKEG90, idKEG050);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult LoadDataMappingKegiatan13_90(string idKeg)
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
                List<MappingKegiatan13_90> mapKegs = new List<MappingKegiatan13_90>();
                mapKegs = mappingKegiatan13_90Service.GetKegiatanMappingList(idKeg);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "IDKEG")
                {
                    mapKegs = mapKegs.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    mapKegs = mapKegs.Where(m => m.NUKEG90.ToLower().Contains(searchValue) ||
                        m.NMKEG90.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = mapKegs.Count();
                //Paging     
                var data = mapKegs.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan13_90");
            }
        }

        public ActionResult LoadDataMappingKegiatan90_50(string idKeg90)
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
                if (pageSize == -1) { pageSize = 50; }
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                List<MappingKegiatan90_50> mapKegs = new List<MappingKegiatan90_50>();
                mapKegs = mappingKegiatan90_50Service.GetKegiatanMappingList(idKeg90);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "IDKeg90")
                {
                    mapKegs = mapKegs.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    mapKegs = mapKegs.Where(m => m.NUKEG050.ToLower().Contains(searchValue) ||
                        m.NMKEG050.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = mapKegs.Count();
                //Paging     
                var data = mapKegs.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("MappingKegiatan90_50");
            }
        }

        public ActionResult GetMPGRM()
        {
            var list = mapSvc.GetMPGRMList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMKEGIATAN(string idPgrm = "")
        {
            var list = mapSvc.GetMKEGIATANList(idPgrm);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMPGRM90()
        {
            var list = mapSvc.GetMPGRM90List();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMKEGIATAN90(string idPgrm90 = "")
        {
            var list = mapSvc.GetMKEGIATAN90List(idPgrm90);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMPGRM50()
        {
            var list = mapSvc.GetMPGRM50List();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMKEGIATAN50(string idPgrm50 = "")
        {
            var list = mapSvc.GetMKEGIATAN50List(idPgrm50);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}