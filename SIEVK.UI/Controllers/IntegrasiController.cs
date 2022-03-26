using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;

namespace SIEVK.Service.Controllers
{
    public class IntegrasiController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        #endregion

        #region Transfer Tahapan RPJM
        public ActionResult TransferTahapanRPJM(string tahapan = "", bool isTransfer = false, bool isDeleteInsert = false)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.isTransfer = isTransfer;
            ViewBag.isDeleteInsert = isDeleteInsert;
            GenerateTahapanCombo("0", tahapan);
            return View();
        }
        #endregion Transfer Tahapan RPJM

        #region Transfer Tahapan RKPD
        public ActionResult TransferTahapanRKPD(string tahapan = "", bool isTransfer = false, bool isDeleteInsert = false)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.isTransfer = isTransfer;
            ViewBag.isDeleteInsert = isDeleteInsert;
            GenerateTahapanCombo("1", tahapan);
            return View();
        }
        #endregion Transfer Tahapan RKPD

        #region Terima Data APBD
        public ActionResult TerimaDataAPBD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunCombo();
            GenerateData();
            return View();
        }
        #endregion Transfer Data APBD

        #region Terima Data Realisasi
        public ActionResult TerimaDataRealisasi()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunCombo();
            GenerateData();
            return View();
        }
        #endregion Terima Data Realisasi

        #region Sinkronisasi Data Master
        public ActionResult SinkronisasiDataMaster()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunCombo();
            return View();
        }
        #endregion Sinkronisasi Data Master

        #region Private Function
        private bool GenerateData(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Data1", "Data 1");
            dict.Add("Data2", "Data 2");

            ViewBag.DataCombo = new SelectList(dict, "Key", "Value", selected_id);
            return dict.Count > 0;
        }

        private bool GenerateTahunCombo(string selected_id = "")
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
            ViewBag.TahunCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        private bool GenerateTahapanCombo(string key, string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahapan(key);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDTAHAP, item.NMTAHAP);
                }
            }

            ViewBag.TahapanCombo = new SelectList(dict, "Key", "Value", selected_id);
            return dict.Count > 0;
        }
        #endregion Private Function

        #region Public Function
        public ActionResult GetUnitOrganization()
        {
            var list = genSvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion Public Function

    }
}