using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.APBD;
using SIEVK.BusinessService.APBD;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class APBDController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private APBDRealisasiKinerjaAPBDService kinSvc = new APBDRealisasiKinerjaAPBDService();
        private APBDRealisasiCapaianKinerjaProgramAPBDService kinProgSvc = new APBDRealisasiCapaianKinerjaProgramAPBDService();
        private APBDRealisasiCapaianProgramService realCapSvc = new APBDRealisasiCapaianProgramService();

        //Balon, 20200210
        private APBDKegUnitService kegUnitSVC = new APBDKegUnitService();
        //Balon, 20200501
        private APBDCatatanLaporanRealisasiPemdaService clrSvcPemda = new APBDCatatanLaporanRealisasiPemdaService();
        private APBDCatatanLaporanRealisasiSKPDService clrSvcSKPD = new APBDCatatanLaporanRealisasiSKPDService();

        //Balon, 20210808
        private APBDKinKegUnitService kinKegUnitSVC = new APBDKinKegUnitService();
        #endregion

        #region Realisasi Kinerja APBD
        public ActionResult RealisasiKinerjaAPBD(string msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "", String idKeg = "", String nuKeg_nmKeg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.idKeg = idKeg;
            ViewBag.nuKeg_nmKeg = nuKeg_nmKeg;

            if (unitKey != "" && kdTahap != "" && kdTahun != "" && idKeg != "" && idPrgrm != "")
            {
                ViewBag.IsBack = true;
            }

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                if (idPrgrm != "")
                {
                    List<Program> prgrm = genSvc.GetProgramAPBD(unitKey, kdTahun, idPrgrm, kdTahap);
                    if (prgrm.Count > 0) { ViewBag.prgrmName = prgrm[0].KDPRGRM + " - " + prgrm[0].NMPRGRM; }
                }
            }
            else//info terakhir dr kang lukman default per unit di users
            {
                String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
                String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
                unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();
                
                if (kdUnit != "" && nmUnit != "")
                {
                    ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
                    ViewBag.unitKey = unitKey;
                }
            }
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

            GenerateTahapanCombo(kdTahap);
            GenerateTahunAnggaranCombo(kdTahun);
            //ViewBag.Info = msg;
            return View();
        }

        public ActionResult RealisasiKinerjaAPBDCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string idKeg = "")
        {
            APBDKinerja realisasi = new APBDKinerja();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                ViewBag.idPrgrm = idPrgrm;
                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHAP = kdTahap;
                realisasi.KDTAHUN = kdTahun;
                realisasi.IDKEG = idKeg;

                GenerateTriwulanCombo(realisasi.UNITKEY, realisasi.KDTAHAP, realisasi.KDTAHUN, realisasi.IDKEG);
                return View("RealisasiKinerjaAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaAPBDCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDKinerja kin = new APBDKinerja();
            try
            {
                kin = BindDataKinerjaAPBDToObject(param);
                kinSvc.CreateDataRealisasi(kin);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiKinerjaAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = idPrgrm, idKeg = kin.IDKEG });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTriwulanCombo(kin.UNITKEY, kin.KDTAHAP, kin.KDTAHUN, kin.IDKEG, kin.KDTRWN);
                return View("RealisasiKinerjaAPBDEdit", kin);
            }
        }

        public ActionResult RealisasiKinerjaAPBDEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string idKeg, string kdTrwn)
        {
            APBDKinerja realisasi = new APBDKinerja();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = kinSvc.GetKinerjaAPBDRealisasi(unitKey, kdTahap, kdTahun, idKeg, kdTrwn);
                ViewBag.idPrgrm = idPrgrm;
                ViewBag.Method = "Edit";
                return View("RealisasiKinerjaAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaAPBDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDKinerja kin = new APBDKinerja();
            try
            {
                kin = BindDataKinerjaAPBDToObject(param);
                kinSvc.UpdateDataRealisasi(kin);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiKinerjaAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = idPrgrm, idKeg = kin.IDKEG });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTriwulanCombo(kin.UNITKEY, kin.KDTAHAP, kin.KDTAHUN, kin.IDKEG, kin.KDTRWN);
                return View("RealisasiKinerjaAPBDEdit", kin);
            }
        }

        //Balon, 20200209
        public ActionResult KegUnitAPBDEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string idKeg, string kdTrwn)
        {
            APBDKinerja kegUnit = new APBDKinerja();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                kegUnit = kegUnitSVC.GetKinerjaAPBDById(unitKey, kdTahap, kdTahun, idKeg);
                //kegUnit.UNITKEY = unitKey;
                //kegUnit.KDTAHAP = kdTahap;
                //kegUnit.KDTAHUN = kdTahun;
                //kegUnit.IDPRGRM = idPrgrm;
                //kegUnit.IDKEG = idKeg;

                ViewBag.idPrgrm = idPrgrm;
                ViewBag.Method = "Edit";
                return View("KegUnitAPBDEdit", kegUnit);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        [HttpPost]
        public ActionResult KegUnitAPBDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDKinerja kin = new APBDKinerja();
            try
            {
                kin = BindDataKegUnitAPBDToObject(param);
                kegUnitSVC.UpdateKegUnit(kin);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiKinerjaAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = idPrgrm, idKeg = kin.IDKEG });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTriwulanCombo(kin.UNITKEY, kin.KDTAHAP, kin.KDTAHUN, kin.IDKEG, kin.KDTRWN);
                return View("KegUnitAPBDEdit", kin);
            }
        }
        #endregion Realisasi Kinerja APBD

        #region Realisasi Capaian Program APBD
        public ActionResult RealisasiCapaianProgramAPBD(string msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                if (idPrgrm != "")
                {
                    List<Program> prgrm = genSvc.GetProgramAPBD(unitKey, kdTahun, idPrgrm, kdTahap);
                    if (prgrm.Count > 0) { ViewBag.prgrmName = prgrm[0].KDPRGRM + " - " + prgrm[0].NMPRGRM; }
                }
            }
            else//info terakhir dr kang lukman default per unit di users
            {
                String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
                String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
                unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();

                if (kdUnit != "" && nmUnit != "")
                {
                    ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
                    ViewBag.unitKey = unitKey;
                }
            }
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

            GenerateTahapanCombo(kdTahap);
            GenerateTahunAnggaranCombo(kdTahun);
            ViewBag.Info = msg;
            return View();
        }

        public ActionResult RealisasiCapaianProgramAPBDCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            APBDRealisasiCapaianProgram realisasi = new APBDRealisasiCapaianProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                ViewBag.idPrgrm = idPrgrm;

                realisasi.KDTAHAP = kdTahap;
                realisasi.KDTAHUN = kdTahun;

                return View("RealisasiCapaianProgramAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianProgramAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiCapaianProgramAPBDCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDRealisasiCapaianProgram realisasi = new APBDRealisasiCapaianProgram();
            try
            {
                realisasi = BindDataRealisasiCapaianProgramAPBDToObject(param);
                realCapSvc.CreateDataRealisasi(realisasi);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiCapaianProgramAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = realisasi.UNITKEY, kdTahap = realisasi.KDTAHAP, kdTahun = realisasi.KDTAHUN, idPrgrm = idPrgrm });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiCapaianProgramAPBDEdit", realisasi);
            }
        }

        public ActionResult RealisasiCapaianProgramAPBDEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            APBDRealisasiCapaianProgram realisasi = new APBDRealisasiCapaianProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = realCapSvc.GetRealisasiCapaianProgramAPBD(unitKey, kdTahap, kdTahun, idPrgrm);
                ViewBag.idPrgrm = idPrgrm;
                ViewBag.Method = "Edit";
                return View("RealisasiCapaianProgramAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianProgramAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiCapaianProgramAPBDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDRealisasiCapaianProgram realisasi = new APBDRealisasiCapaianProgram();
            try
            {
                realisasi = BindDataRealisasiCapaianProgramAPBDToObject(param);
                realCapSvc.UpdateDataRealisasi(realisasi);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiCapaianProgramAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = realisasi.UNITKEY, kdTahap = realisasi.KDTAHAP, kdTahun = realisasi.KDTAHUN, idPrgrm = idPrgrm });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiCapaianProgramAPBDEdit", realisasi);
            }
        }
        #endregion Realisasi Capaian Program APBD

        #region Realisasi Capaian Kinerja Program APBD

        public ActionResult RealisasiCapaianKinerjaProgramAPBD(string msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "", String Kinpgrapbdkey = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.Kinpgrapbdkey = Kinpgrapbdkey;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                if (idPrgrm != "")
                {
                    List<Program> prgrm = genSvc.GetProgramAPBD(unitKey, kdTahun, idPrgrm, kdTahap);
                    if (prgrm.Count > 0) { ViewBag.prgrmName = prgrm[0].KDPRGRM + " - " + prgrm[0].NMPRGRM; }
                }
            }
            else//info terakhir dr kang lukman default per unit di users
            {
                String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
                String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
                unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();

                if (kdUnit != "" && nmUnit != "")
                {
                    ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
                    ViewBag.unitKey = unitKey;
                }
            }
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

            GenerateTahapanCombo(kdTahap);
            GenerateTahunAnggaranCombo(kdTahun);
            ViewBag.Info = msg;
            return View();
        }

        public ActionResult RealisasiCapaianKinerjaProgramCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string Kinpgrapbdkey = "", String target = "")
        {
            APBDCapaianKinerjaProgram realisasi = new APBDCapaianKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";

                realisasi.KDTAHAP = kdTahap;
                realisasi.KDTAHUN = kdTahun;
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KINPGRAPBDKEY = Kinpgrapbdkey;
                realisasi.UNITKEY = unitKey;
                realisasi.REALTAR1 = "";
                realisasi.REALTAR2 = "";
                realisasi.REALTAR3 = "";
                realisasi.REALTAR4 = "";
                realisasi.REALTARAKHIR = "";
                realisasi.TARGET = target;
                return View("RealisasiCapaianKinerjaProgramAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianKinerjaProgramAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiCapaianKinerjaProgramCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCapaianKinerjaProgram kin = new APBDCapaianKinerjaProgram();
            try
            {
                kin = BindDataKinerjaProgramAPBDToObject(param);
                kinProgSvc.CreateDataRealisasi(kin);
                return RedirectToAction("RealisasiCapaianKinerjaProgramAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = kin.IDPRGRM, Kinpgrapbdkey = kin.KINPGRAPBDKEY });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaAPBDEdit", kin);
            }
        }

        public ActionResult RealisasiCapaianKinerjaProgramEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string Kinpgrapbdkey)
        {
            APBDCapaianKinerjaProgram realisasi = new APBDCapaianKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = kinProgSvc.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, idPrgrm, Kinpgrapbdkey);
                ViewBag.idPrgrm = idPrgrm;
                ViewBag.Method = "Edit";
                return View("RealisasiCapaianKinerjaProgramAPBDEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianKinerjaProgramAPBD");
            }
        }

        [HttpPost]
        public ActionResult RealisasiCapaianKinerjaProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCapaianKinerjaProgram kin = new APBDCapaianKinerjaProgram();
            try
            {
                kin = BindDataKinerjaProgramAPBDToObject(param);
                kinProgSvc.UpdateDataRealisasi(kin);
                return RedirectToAction("RealisasiCapaianKinerjaProgramAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = kin.IDPRGRM, Kinpgrapbdkey = kin.KINPGRAPBDKEY });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaAPBDEdit", kin);
            }
        }

        #endregion Realisasi Kinerja APBD

        #region Catatan Laporan Realisasi Pemda
        public ActionResult CatatanLaporanRealisasiPemda(string msg = "", string kdTahun = "", string kdTriwulan = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.kdTriwulan = kdTriwulan;
            ViewBag.Info = msg;

            GenerateTahunAnggaranCombo(kdTahun);
            GenerateTriwulanCombo(kdTriwulan);
            return View();
        }

        public ActionResult CatatanLaporanRealisasiPemdaCreate(string kdTahun, string kdTriwulan)
        {
            //if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            //if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            //APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            //return CreateInputViewCatatanPemda(catatan, "true");

            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi realisasi = new APBDCatatanLaporanRealisasi();
            try
            {
                ViewBag.Method = "Tambah";

                realisasi.KDTAHUN = kdTahun;
                realisasi.KDTRWN = kdTriwulan;

                //return View("CatatanLaporanRealisasiPemdaEdit", realisasi);
                return CreateInputViewCatatanPemda(realisasi, "true");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTriwulan);
                return View("CatatanLaporanRealisasiPemda");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiPemdaCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatanPemda(param, true);
                if (ValidateDataCatatanPemda(catatan, true))
                {
                    clrSvcPemda.InsertData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasiPemda", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatanPemda(catatan, "true");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanPemda(catatan, "true");
            }
        }


        public ActionResult CatatanLaporanRealisasiPemdaEdit(string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }


                catatan = clrSvcPemda.GetCatatanLaporanRealisasi(kdTahun, kdTriwulan, kdFAKTOR);
                return CreateInputViewCatatanPemda(catatan, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanPemda(catatan, "false");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiPemdaEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatanPemda(param, false);
                if (ValidateDataCatatanPemda(catatan, false))
                {
                    clrSvcPemda.UpdateData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasiPemda", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatanPemda(catatan, "false");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanPemda(catatan, "false");
            }
        }

        // Catatan Laporan Realisasi
        public JsonResult DeleteCatatanLaporanRealisasiPemda(string kdtahun = "", string kdTriwulan = "", string kdFaktor = "")
        {
            try
            {
                clrSvcPemda.DeleteData(kdtahun, kdTriwulan, kdFaktor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataCatatanLaporanRealisasiPemda(string kdTahun = "", string kdTriwulan = "")
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
                List<APBDCatatanLaporanRealisasi> lst = new List<APBDCatatanLaporanRealisasi>();
                lst = clrSvcPemda.GetCatatanLaporanRealisasiList(kdTahun, kdTriwulan);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.NMCTT.ToLower().Contains(searchValue) ||
                        m.KDFAKTOR.ToLower().Contains(searchValue)
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
                GenerateTriwulanCombo(kdTriwulan);
                return View("CatatanLaporanRealisasiPemda");
            }
        }

        private ActionResult CreateInputViewCatatanPemda(APBDCatatanLaporanRealisasi clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
            GenerateTriwulanCombo(clr.KDTRWN == null ? string.Empty : clr.KDTRWN);
            GenerateJFaktorCombo(clr.KDFAKTOR == null ? string.Empty : clr.KDFAKTOR);
            ViewBag.isCreate = isCreate;
            return View("CatatanLaporanRealisasiPemdaEdit", clr);
        }

        public bool ValidateDataCatatanPemda(APBDCatatanLaporanRealisasi clr, bool isCreate)
        {
            Dictionary<string, string> dic = clrSvcPemda.ValidateDataCatatan(clr, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        private APBDCatatanLaporanRealisasi BindDataToObjectCatatanPemda(FormCollection param, bool isCreate)
        {
            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            catatan.KDTAHUN = param["KDTAHUN"].ToString();
            catatan.KDTRWN = param["KDTRWN"].ToString();
            if (isCreate)
            {
                catatan.KDFAKTOR = param["JFAKTORCombo"].ToString();
            }
            else
            {
                catatan.KDFAKTOR = param["KDFAKTOR"].ToString();
            }


            catatan.NMCTT = param["NMCTT"].ToString();
            catatan.ISICTT = param["ISICTT"].ToString();

            return catatan;
        }

        #endregion Catatan Laporan Realisasi Pemda

        #region Catatan Laporan Realisasi SKPD
        public ActionResult CatatanLaporanRealisasiSKPD(string msg = "", String unitKey = "", string kdTahun = "", string kdTriwulan = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdUnit = unitKey;
            ViewBag.kdTahun = kdTahun;
            ViewBag.kdTriwulan = kdTriwulan;
            ViewBag.Info = msg;

            //if (unitKey != "")
            //{
            //    List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
            //    if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }
            //}
            //else//info terakhir dr kang lukman default per unit di users
            //{
            //    String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
            //    String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
            //    unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();

            //    if (kdUnit != "" && nmUnit != "")
            //    {
            //        ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
            //        ViewBag.kdUnit = unitKey;
            //    }
            //}
            //ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

            GenerateUnitCombo(unitKey);
            GenerateTahunAnggaranCombo(kdTahun);
            GenerateTriwulanCombo(kdTriwulan);

            ViewBag.Info = msg;
            return View();
        }

        public ActionResult CatatanLaporanRealisasiSKPDCreate(string unitKey, string kdTahun, string kdTriwulan)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi realisasi = new APBDCatatanLaporanRealisasi();
            try
            {

                ViewBag.Method = "Tambah";

                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHUN = kdTahun;
                realisasi.KDTRWN = kdTriwulan;

                //return View("CatatanLaporanRealisasiSKPDEdit", realisasi);
                return CreateInputViewCatatanSKPD(realisasi, "true");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahunAnggaranCombo(kdTahun);
                GenerateTriwulanCombo(kdTriwulan);
                return View("CatatanLaporanRealisasiSKPD");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiSKPDCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatanSKPD(param, true);
                if (ValidateDataCatatanSKPD(catatan, true))
                {
                    clrSvcSKPD.InsertData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasiSKPD", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = catatan.UNITKEY, kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatanSKPD(catatan, "true");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanSKPD(catatan, "true");
            }
        }

        public ActionResult CatatanLaporanRealisasiSKPDEdit(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }


                catatan = clrSvcSKPD.GetCatatanLaporanRealisasi(unitKey, kdTahun, kdTriwulan, kdFAKTOR);
                return CreateInputViewCatatanSKPD(catatan, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanSKPD(catatan, "false");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiSKPDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatanSKPD(param, false);
                if (ValidateDataCatatanSKPD(catatan, false))
                {
                    clrSvcSKPD.UpdateData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasiSKPD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = catatan.UNITKEY, kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatanSKPD(catatan, "false");
                }

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatanSKPD(catatan, "false");
            }
        }

        // Catatan Laporan Realisasi
        public JsonResult DeleteCatatanLaporanRealisasiSKPD(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            try
            {
                clrSvcSKPD.DeleteData(unitKey, kdTahun, kdTriwulan, kdFAKTOR);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataCatatanLaporanRealisasiSKPD(string unitKey = "", string kdTahun = "", string kdTriwulan = "")
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
                List<APBDCatatanLaporanRealisasi> lst = new List<APBDCatatanLaporanRealisasi>();
                lst = clrSvcSKPD.GetCatatanLaporanRealisasiList(unitKey, kdTahun, kdTriwulan);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.NMCTT.ToLower().Contains(searchValue) ||
                        m.KDFAKTOR.ToLower().Contains(searchValue)
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
                GenerateTriwulanCombo(kdTriwulan);
                return View("CatatanLaporanRealisasiSKPD");
            }
        }

        private ActionResult CreateInputViewCatatanSKPD(APBDCatatanLaporanRealisasi clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
            GenerateTriwulanCombo(clr.KDTRWN == null ? string.Empty : clr.KDTRWN);
            GenerateJFaktorCombo(clr.KDFAKTOR == null ? string.Empty : clr.KDFAKTOR);
            ViewBag.isCreate = isCreate;
            return View("CatatanLaporanRealisasiSKPDEdit", clr);
        }

        public bool ValidateDataCatatanSKPD(APBDCatatanLaporanRealisasi clr, bool isCreate)
        {
            Dictionary<string, string> dic = clrSvcSKPD.ValidateDataCatatan(clr, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        private APBDCatatanLaporanRealisasi BindDataToObjectCatatanSKPD(FormCollection param, bool isCreate)
        {
            APBDCatatanLaporanRealisasi catatan = new APBDCatatanLaporanRealisasi();

            catatan.UNITKEY = param["UNITKEY"].ToString();
            catatan.KDTAHUN = param["KDTAHUN"].ToString();
            catatan.KDTRWN = param["KDTRWN"].ToString();
            if (isCreate)
            {
                catatan.KDFAKTOR = param["JFAKTORCombo"].ToString();
            }
            else
            {
                catatan.KDFAKTOR = param["KDFAKTOR"].ToString();
            }

            //if (isCreate)
            //{
            //    catatan.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //    catatan.KDTRWN = param["TriwulanCombo"].ToString();
            //}
            //else
            //{
            //    catatan.KDTAHUN = param["KDTAHUN"].ToString();
            //    catatan.KDTRWN = param["KDTRWN"].ToString();
            //}

            //catatan.KDFAKTOR = param["KDFAKTOR"].ToString();
            catatan.NMCTT = param["NMCTT"].ToString();
            catatan.ISICTT = param["ISICTT"].ToString();

            return catatan;
        }
        #endregion Catatan Laporan Realisasi SKPD

        #region Kinerja Keg Unit APBD
        public ActionResult KinerjaKegiatanUnitAPBD(string msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idKeg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idKeg = idKeg;

            if (unitKey != "" && kdTahap != "" && kdTahun != "" && idKeg != "")
            {
                ViewBag.IsBack = true;
            }

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }
            }
            else//info terakhir dr kang lukman default per unit di users
            {
                String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
                String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
                unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();

                if (kdUnit != "" && nmUnit != "")
                {
                    ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
                    ViewBag.unitKey = unitKey;
                }
            }
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

            GenerateTahapanCombo(kdTahap);
            GenerateTahunAnggaranCombo(kdTahun);
            //ViewBag.Info = msg;
            return View();
        }

        public ActionResult KinerjaKegiatanUnitAPBDCreate(string unitKey, string kdTahap, string kdTahun, string idKeg, string idPrgrm, string nuKeg_nmKeg = "")
        {
            APBDKinKegUnit kinKegUnit = new APBDKinKegUnit();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                kinKegUnit.UNITKEY = unitKey;
                kinKegUnit.KDTAHAP = kdTahap;
                kinKegUnit.KDTAHUN = kdTahun;
                kinKegUnit.IDKEG = idKeg;
                kinKegUnit.IDPRGRM = idPrgrm;
                ViewBag.idPrgrm = idPrgrm;
                ViewBag.nuKeg_nmKeg = nuKeg_nmKeg;

                return View("KinerjaKegiatanUnitAPBDEdit", kinKegUnit);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        [HttpPost]
        public ActionResult KinerjaKegiatanUnitAPBDCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDKinKegUnit kin = new APBDKinKegUnit();
            try
            {
                kin = BindDataKinKegUnitAPBDToObject(param);
                kinKegUnitSVC.Create(kin);
                var idPrgrm = param["IDPRGRM"].ToString();
                var nuKeg_nmKeg = param["nuKeg_nmKeg"].ToString();
                return RedirectToAction("RealisasiKinerjaAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = idPrgrm, idKeg = kin.IDKEG, nuKeg_nmKeg = nuKeg_nmKeg });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("KinerjaKegiatanUnitAPBDEdit", kin);
            }
        }

        public ActionResult KinerjaKegiatanUnitAPBDEdit(string unitKey, string kdTahap, string kdTahun, string idKeg, string noUrkin, string idPrgrm, string nuKeg_nmKeg = "")
        {
            APBDKinKegUnit kinKegUnit = new APBDKinKegUnit();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                kinKegUnit = kinKegUnitSVC.GetKinKegUnitbyId(unitKey, kdTahap, kdTahun, idKeg, noUrkin);
                ViewBag.Method = "Edit";
                ViewBag.idPrgrm = idPrgrm;

                ViewBag.nuKeg_nmKeg = nuKeg_nmKeg;

                return View("KinerjaKegiatanUnitAPBDEdit", kinKegUnit);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        [HttpPost]
        public ActionResult KinerjaKegiatanUnitAPBDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            APBDKinKegUnit kin = new APBDKinKegUnit();
            try
            {
                kin = BindDataKinKegUnitAPBDToObject(param);
                kinKegUnitSVC.Update(kin);
                var idPrgrm = param["IDPRGRM"].ToString();
                var nuKeg_nmKeg = param["nuKeg_nmKeg"].ToString();
                return RedirectToAction("RealisasiKinerjaAPBD", "APBD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kin.UNITKEY, kdTahap = kin.KDTAHAP, kdTahun = kin.KDTAHUN, idPrgrm = idPrgrm, idKeg = kin.IDKEG, nuKeg_nmKeg = nuKeg_nmKeg });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("KinerjaKegiatanUnitAPBDEdit", kin);
            }
        }
        #endregion

        #region private function
        private Dictionary<string,string> TriwulanDictionary()
        {
            var dict = new Dictionary<string, string>();
            var list = genSvc.GetTriwulan();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDTRWN.Trim(), item.NMTRWN.Trim());
                }
            }
            return dict;
        }

        public ActionResult GetJFaktor()
        {
            var list = genSvc.GetJFaktor();
            return Json(list, JsonRequestBehavior.AllowGet);
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

        private bool GenerateJFaktorCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();
            var list = genSvc.GetJFaktor();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDFAKTOR, item.URFAKTOR);
                }
            }
            ViewBag.JFaktorCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
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

        private bool GenerateTriwulanCombo(string unitKey, string kdTahap, string kdTahun, string idKeg, string selected_id = "")
        {
            // get list of  triwulan
            var dict = TriwulanDictionary();

            // get saved triwulan data and remove from list
            var list = kinSvc.GetKinerjaAPBDRealisasiList(unitKey, kdTahap, kdTahun, idKeg);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Remove(item.KDTRWN);
                }
            }

            ViewBag.TriwulanCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
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

        private bool GenerateTahunAnggaranCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahunAnggaran();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (selected_id == "")
                    {
                        if (item.NMTAHUN == DateTime.Now.Year.ToString())
                        {
                            selected_id = item.KDTAHUN;
                        }
                    }
                    dict.Add(item.KDTAHUN, item.NMTAHUN);
                }
            }
            //if (selected_id == "")
            //{
            //    TahunAnggaran x = list.OrderByDescending(z => Convert.ToInt32(z.NMTAHUN)).FirstOrDefault();
            //    selected_id = x.KDTAHUN;
            //}
            ViewBag.TahunAnggaranCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
            return dict.Count > 0;
        }

        public ActionResult GetUnitOrganization()
        {
            GeneralService gsvc = new GeneralService();
            var list = gsvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgramAPBD(string unitKey,string kdTahun, string kdTahap)
        {
            var list = genSvc.GetProgramAPBD(unitKey, kdTahun, null, kdTahap);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKinerjaProgramAPBD(string unitKey, string kdTahun, string kdTahap)
        {
            var list = kinProgSvc.GetKinerjaProgramProgramList(unitKey, kdTahap, kdTahun);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //Balon, 20200209
        private APBDKinerja BindDataKegUnitAPBDToObject(FormCollection param)
        {
            APBDKinerja kin = new APBDKinerja();
            kin.UNITKEY = param["UNITKEY"].ToString();
            kin.KDTAHAP = param["KDTAHAP"].ToString();
            kin.KDTAHUN = param["KDTAHUN"].ToString();
            kin.IDKEG = param["IDKEG"].ToString();
            kin.IDPRGRM = param["IDPRGRM"].ToString();

            kin.URKIN = param["URKIN"].ToString();
            kin.TARGET = param["TARGET"].ToString();
            kin.PAGU = Convert.ToDecimal(param["PAGU"].ToString().Replace(".", ""));

            kin.ANGKAS1 = Convert.ToDecimal(param["ANGKAS1"].ToString().Replace(".", ""));
            kin.ANGKAS2 = Convert.ToDecimal(param["ANGKAS2"].ToString().Replace(".", ""));
            kin.ANGKAS3 = Convert.ToDecimal(param["ANGKAS3"].ToString().Replace(".", ""));
            kin.ANGKAS4 = Convert.ToDecimal(param["ANGKAS4"].ToString().Replace(".", ""));
            
            kin.LOKASI = param["LOKASI"].ToString();
            kin.CTT = param["CTT"].ToString();
            kin.KET = param["KET"].ToString();

            return kin;
        }

        private APBDKinerja BindDataKinerjaAPBDToObject(FormCollection param)
        {
            APBDKinerja kin = new APBDKinerja();
            kin.UNITKEY = param["UNITKEY"].ToString();
            kin.KDTAHAP = param["KDTAHAP"].ToString();
            kin.KDTAHUN = param["KDTAHUN"].ToString();
            kin.IDKEG = param["IDKEG"].ToString();
            kin.KDTRWN = param["TriwulanCombo"].ToString();

            kin.REALNUM = Convert.ToDecimal(param["REALNUM"].ToString().Replace(" ", "").Replace(",", ""));
            kin.REALFISIK = Convert.ToDecimal(param["REALFISIK"].ToString().Replace(",", ""));
            kin.REALSTR = Convert.ToDecimal(param["REALSTR"].ToString().Replace(",", ""));
            kin.REALSTR1 = Convert.ToDecimal(param["REALSTR1"].ToString().Replace(",", ""));
            kin.SATUAN = param["SATUAN"].ToString();
            kin.SATUAN1 = param["SATUAN1"].ToString();
            kin.MASALAH = param["MASALAH"].ToString();
            kin.SOLUSI = param["SOLUSI"].ToString();
            kin.KET = string.Empty;
            
            return kin;
        }

        private APBDCapaianKinerjaProgram BindDataKinerjaProgramAPBDToObject(FormCollection param)
        {
            APBDCapaianKinerjaProgram kin = new APBDCapaianKinerjaProgram();
            kin.UNITKEY = param["UNITKEY"].ToString();
            kin.KDTAHAP = param["KDTAHAP"].ToString();
            kin.KDTAHUN = param["KDTAHUN"].ToString();
            kin.IDPRGRM = param["IDPRGRM"].ToString();
            kin.KINPGRAPBDKEY = param["KINPGRAPBDKEY"].ToString();
            kin.TARGET = param["TARGET"].ToString();
            kin.REALTAR1 = param["REALTAR1"].ToString();
            kin.REALTAR2 = param["REALTAR2"].ToString();
            kin.REALTAR3 = param["REALTAR3"].ToString();
            kin.REALTAR4 = param["REALTAR4"].ToString();
            kin.REALTARAKHIR = param["REALTARAKHIR"].ToString();
            kin.KET = param["KET"].ToString();
            kin.CTT = param["CTT"].ToString();
            return kin;
        }

        private APBDRealisasiCapaianProgram BindDataRealisasiCapaianProgramAPBDToObject(FormCollection param)
        {
            APBDRealisasiCapaianProgram kin = new APBDRealisasiCapaianProgram();
            kin.UNITKEY = param["UNITKEY"].ToString();
            kin.KDTAHAP = param["KDTAHAP"].ToString();
            kin.KDTAHUN = param["KDTAHUN"].ToString();
            kin.IDPRGRM = param["IDPRGRM"].ToString();

            kin.REAL1 = param["REAL1"].ToString().Replace(".", "");
            kin.REAL2 = param["REAL2"].ToString().Replace(".", "");
            kin.REAL3 = param["REAL3"].ToString().Replace(".", "");
            kin.REAL4 = param["REAL4"].ToString().Replace(".", "");
            kin.REALAKHIR = param["REALAKHIR"].ToString().Replace(".", "");
            kin.KET = param["KET"].ToString();
            kin.CTT = param["CTT"].ToString();
            return kin;
        }

        //Balon, 202108008
        private APBDKinKegUnit BindDataKinKegUnitAPBDToObject(FormCollection param)
        {            
            APBDKinKegUnit kin = new APBDKinKegUnit();

            kin.UNITKEY = String.IsNullOrEmpty(param["UNITKEY"].ToString()) ? String.Empty : param["UNITKEY"].ToString();
            kin.KDTAHAP = String.IsNullOrEmpty(param["KDTAHAP"].ToString()) ? String.Empty : param["KDTAHAP"].ToString();
            kin.KDTAHUN = String.IsNullOrEmpty(param["KDTAHUN"].ToString()) ? String.Empty : param["KDTAHUN"].ToString();
            kin.IDKEG = String.IsNullOrEmpty(param["IDKEG"].ToString()) ? String.Empty : param["IDKEG"].ToString();

            kin.NOURKIN = String.IsNullOrEmpty(param["NOURKIN"].ToString()) ? String.Empty : param["NOURKIN"].ToString();
            kin.URKIN = String.IsNullOrEmpty(param["URKIN"].ToString()) ? String.Empty : param["URKIN"].ToString();
            kin.TARGET = String.IsNullOrEmpty(param["TARGET"].ToString()) ? String.Empty : param["TARGET"].ToString();
            kin.REALISASI = String.IsNullOrEmpty(param["REALISASI"].ToString()) ? String.Empty : param["REALISASI"].ToString();
            kin.KET = String.IsNullOrEmpty(param["KET"].ToString()) ? String.Empty : param["KET"].ToString();

            return kin;
        }

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }
        #endregion

        #region Json Result

        // Realisasi Kinerja APBD
        public JsonResult DeleteRealisasiKinerjaAPBD(string unitkey = "", string kdTahap = "", string kdtahun = "", string idkeg = "", string kdTrwn = "")
        {
            try
            {
                kinSvc.DeleteDataRealisasi(unitkey, kdTahap, kdtahun, idkeg, kdTrwn);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCapaianKinerjaProgramRealisasiAPBD(string unitkey = "", string kdTahap = "", string kdTahun = "", string idprgrm = "", string kinpgrapbdkey = "")
        {
            try
            {
                kinProgSvc.DeleteDataRealisasi(unitkey, kdTahap, kdTahun, idprgrm, kinpgrapbdkey);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiKinerjaAPBD(string unitKey = "", string kdTahap = "", string kdTahun = "", string idPrgrm = "")
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
                List<APBDKinerja> APBDKinerja = new List<APBDKinerja>();
                if (unitKey != "" && idPrgrm != "") //supaya pas first load page diawal load data
                {
                    APBDKinerja = kinSvc.GetKinerjaAPBD(unitKey, kdTahap, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    APBDKinerja = APBDKinerja.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    APBDKinerja = APBDKinerja.Where(m => m.NUKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = APBDKinerja.Count();
                //Paging     
                var data = APBDKinerja.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaAPBDDetail(string unitKey = "", string kdTahap = "", string kdTahun = "", string idKeg = "")
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

                // Getting all data    
                List<APBDKinerja> APBDKinerja = new List<APBDKinerja>();
                //Temp
                //unitKey = "4_        ";
                //kdTahap = "21";
                //kdTahun = "5 ";
                //idKeg = "1_";
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    APBDKinerja = kinSvc.GetKinerjaAPBDRealisasiList(unitKey, kdTahap, kdTahun, idKeg);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    APBDKinerja = APBDKinerja.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    APBDKinerja = APBDKinerja.Where(m => m.NMKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = APBDKinerja.Count();
                //Paging     
                var data = APBDKinerja.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }

        public ActionResult LoadDataCapaianKinerjaProgramAPBD(string unitKey = "", string kdTahap = "", string kdTahun = "", string idPrgrm = "")
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
                List<APBDCapaianKinerjaProgram> APBDKinerja = new List<APBDCapaianKinerjaProgram>();
                if (unitKey != "" && idPrgrm != "") //supaya pas first load page diawal load data
                {
                    APBDKinerja = kinProgSvc.GetKinerjaProgramCapaianKinerjaList(unitKey, kdTahap, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    APBDKinerja = APBDKinerja.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    APBDKinerja = APBDKinerja.Where(m => m.NMPRGRM.ToLower().Contains(searchValue) ||
                        m.URKIN.ToLower().Contains(searchValue) || m.TARGET.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = APBDKinerja.Count();
                //Paging     
                var data = APBDKinerja.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianKinerjaProgramAPBD");
            }
        }

        public ActionResult LoadDataCapaianKinerjaProgramAPBDDetail(string unitKey = "", string kdTahap = "", string kdTahun = "", string idPrgrm = "", string Kinpgrapbdkey = "")
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

                // Getting all data    
                List<APBDCapaianKinerjaProgram> APBDKinerja = new List<APBDCapaianKinerjaProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    APBDKinerja = kinProgSvc.GetKinerjaProgramRealisasiList(unitKey, kdTahap, kdTahun, idPrgrm, Kinpgrapbdkey);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    APBDKinerja = APBDKinerja.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //total number of rows count     
                recordsTotal = APBDKinerja.Count();
                //Paging     
                var data = APBDKinerja.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianKinerjaProgramAPBD");
            }
        }

        // Realisasi Capaian Program APBD
        public JsonResult DeleteRealisasiCapaianProgramAPBD(string unitkey = "", string kdTahap = "", string kdtahun = "", string idprgrm = "")
        {
            try
            {
                realCapSvc.DeleteDataRealisasi(unitkey, kdTahap, kdtahun, idprgrm);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiCapaianProgramAPBD(string unitKey = "", string kdTahap = "", string kdTahun = "", string idPrgrm = "")
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
                List<APBDRealisasiCapaianProgram> obj = new List<APBDRealisasiCapaianProgram>();
                if (unitKey != "" && idPrgrm != "") //supaya pas first load page diawal load data
                {
                    obj = realCapSvc.GetRealisasiCapaianProgramAPBDList(unitKey, kdTahap, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    obj = obj.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    obj = obj.Where(m => m.NMPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = obj.Count();
                //Paging     
                var data = obj.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianProgramAPBD");
            }
        }

        public ActionResult LoadDataRealisasiCapaianProgramAPBDDetail(string unitKey = "", string kdTahap = "", string kdTahun = "", string idPrgrm = "")
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

                // Getting all data    
                List<APBDRealisasiCapaianProgram> obj = new List<APBDRealisasiCapaianProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    obj = realCapSvc.GetRealisasiCapaianProgramAPBDList(unitKey, kdTahap, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    obj = obj.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    obj = obj.Where(m => m.NMPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = obj.Count();
                //Paging     
                var data = obj.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiCapaianProgramAPBD");
            }
        }


        //Balon,20210808 : KinKegUnit
        // Realisasi Kinerja APBD
        public JsonResult DeleteKinKegUnitAPBD(string unitkey = "", string kdTahap = "", string kdtahun = "", string idkeg = "", string  noUrkin = "")
        {
            try
            {
                kinKegUnitSVC.Delete(unitkey, kdTahap, kdtahun, idkeg, noUrkin);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataKinKegUnitAPBD(string unitKey = "", string kdTahap = "", string kdTahun = "", string idKeg = "")
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

                // Getting all data    
                List<APBDKinKegUnit> obj = new List<APBDKinKegUnit>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    obj = kinKegUnitSVC.GetKinKegUnitList(unitKey, kdTahap, kdTahun, idKeg);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    obj = obj.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    obj = obj.Where(m => m.NOURKIN.ToLower().Contains(searchValue) ||
                        m.URKIN.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = obj.Count();
                //Paging     
                var data = obj.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaAPBD");
            }
        }
        

        #endregion

    }
}