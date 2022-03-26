using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.Dashboard;
using SIEVK.Domain.Common;
using SIEVK.Domain.Dashboard;
using System.Data;

namespace SIEVK.Service.Controllers
{
    public class DashboardController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private DashboardService dashSvc = new DashboardService();
        #endregion

        #region Grafik Penyerapan Anggaran
        public ActionResult GrafikPenyerapanAnggaran(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }
                ViewBag.kdTahap = kdTahap;
                ViewBag.kdTahun = kdTahun;
                ViewBag.kdTrwln = kdTrwln;
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTrwln);
                return View();
            }
            catch(Exception ex)
            {
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTrwln);
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("GrafikPenyerapanAnggaran");
            }
        }

        #endregion

        #region Grafik Penyerapan Anggaran
        public ActionResult GrafikPenyerapanFisik(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }
                ViewBag.kdTahap = kdTahap;
                ViewBag.kdTahun = kdTahun;
                ViewBag.kdTrwln = kdTrwln;
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTrwln);
                return View();
            }
            catch (Exception ex)
            {
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTrwln);
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("GrafikPenyerapanFisik");
            }
        }
        #endregion

        #region Private function
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

        private bool GenerateTriwulanCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();
            var list = genSvc.GetTriwulan();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDTRWN, item.NMTRWN);
                }
            }
            ViewBag.TriwulanCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        [HttpPost]
        public JsonResult JSONGrafikPenyerapanAnggaran(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("NMUNIT", System.Type.GetType("System.String"));
            dt.Columns.Add("REALISASI", System.Type.GetType("System.Double"));

            List<GrafikPenyerapan> list = dashSvc.GetPenyerapanAnggaran(kdTahap, kdTahun, kdTrwln);

            foreach (var v in list)
            {
                //foo = null; // WRONG, foo is not editable
                //foo.name = "John";  // RIGHT, foo properties are editable
                DataRow dr = dt.NewRow();
                dr["NMUNIT"] = v.NMUNIT;
                dr["REALISASI"] = v.REALISASI;
                dt.Rows.Add(dr);
            }

            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashboardPenyerapanFisikData(String kdTriwulan, String kdTahun, String kdTahap)
        {
            // Getting data    
            List<GrafikPenyerapan> _list = new List<GrafikPenyerapan>();
            _list = dashSvc.PenyerapanFisik(kdTahap, kdTahun, kdTriwulan);


            return Json(data: _list, behavior: JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSortData(String kdTriwulan, String kdTahun, String kdTahap, string sort = "ASC")
        {
            // Getting data    
            List<GrafikPenyerapan> _list = new List<GrafikPenyerapan>();
            List<GrafikPenyerapan> _listSort = null;
            _list = dashSvc.PenyerapanFisik(kdTahap, kdTahun, kdTriwulan);
            if (sort == "ASC")
            {
                _listSort = _list.OrderBy(z => Convert.ToInt32(z.REALISASI)).ToList();
            }
            else
            {
                _listSort = _list.OrderByDescending(z => Convert.ToInt32(z.REALISASI)).ToList();
            }

            return Json(data: _listSort, behavior: JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}