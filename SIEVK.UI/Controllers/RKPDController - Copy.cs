using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.RKPD;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.RKPD;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Pemda.Bengkalis.Controllers
{
    public class RKPDController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private RKPDRealisasiProgramService prgrmSvc = new RKPDRealisasiProgramService();
        private RKPDRealisasiKinerjaProgramService kinPrgrmSvc = new RKPDRealisasiKinerjaProgramService();
        private RKPDRealisasiKinerjaKegiatanService kinKegSvc = new RKPDRealisasiKinerjaKegiatanService();
        private RKPDEvaluasiBijakService evaBijakSvc = new RKPDEvaluasiBijakService();
        private RKPDEvaluasiSimpulService evaSimpulSvc = new RKPDEvaluasiSimpulService();

        private RKPDCatatanLaporanRealisasiPemdaService clrSvcPemda = new RKPDCatatanLaporanRealisasiPemdaService();
        private RKPDCatatanLaporanRealisasiSKPDService clrSvcSKPD = new RKPDCatatanLaporanRealisasiSKPDService();

        #endregion private Variable Declaration

        #region Realisasi Program
        public ActionResult RealisasiProgram(String msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;

			string unit= System.Web.HttpContext.Current.Session["UnitKey"].ToString();


			if ((unit == "") || (unit == null))

			{
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }
            }
            else//info terakhir dr kang lukman default per unit di users
            {
                    unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();
                    String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
                    String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();

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

        public ActionResult RealisasiProgramCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            RKPDProgram realisasi = new RKPDProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KDTAHUN = kdTahun;

                return View("RealisasiProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiProgramCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDProgram rkpd = new RKPDProgram();
            try
            {
                rkpd = BindDataProgramToObject(param);
                prgrmSvc.CreateDataRealisasi(rkpd);
                return RedirectToAction("RealisasiProgram", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = rkpd.UNITKEY, kdTahap = rkpd.KDTAHAP, idPrgrm = rkpd.IDPRGRM, kdTahun = rkpd.KDTAHUN });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgramEdit", rkpd);
            }
        }

        public ActionResult RealisasiProgramEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            RKPDProgram realisasi = new RKPDProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = prgrmSvc.GetProgramRealisasi(unitKey, kdTahap, kdTahun, idPrgrm);
                ViewBag.Method = "Edit";
                return View("RealisasiProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDProgram rkpd = new RKPDProgram();
            try
            {
                rkpd = BindDataProgramToObject(param);
                prgrmSvc.UpdateDataRealisasi(rkpd);
                return RedirectToAction("RealisasiProgram", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rkpd.UNITKEY, kdTahap = rkpd.KDTAHAP, kdTahun = rkpd.KDTAHUN, idPrgrm = rkpd.IDPRGRM });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgramEdit", rkpd);
            }
        }
        #endregion Realisasi Program

        #region Realisasi Kinerja Program
        public ActionResult RealisasiKinerjaProgram(String msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "", String kinPgRKPDKey = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.kinPgRKPDKey = kinPgRKPDKey;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                if (idPrgrm != "")
                {
                    List<Program> prgrm = genSvc.GetProgramRKPD(unitKey, idPrgrm);
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

        public ActionResult RealisasiKinerjaProgramCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string kinPgRKPDKey = "")
        {
            RKPDKinerjaProgram realisasi = new RKPDKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KDTAHUN = kdTahun;
                realisasi.KINPGRKPDKEY = kinPgRKPDKey;

                return View("RealisasiKinerjaProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaProgramCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDKinerjaProgram kinPrgrm = new RKPDKinerjaProgram();
            try
            {
                kinPrgrm = BindDataKinerjaProgramToObject(param);
                kinPrgrmSvc.CreateDataRealisasi(kinPrgrm);
                return RedirectToAction("RealisasiKinerjaProgram", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = kinPrgrm.UNITKEY, kdTahap = kinPrgrm.KDTAHAP, idPrgrm = kinPrgrm.IDPRGRM, kdTahun = kinPrgrm.KDTAHUN, kinPgRKPDKey = kinPrgrm.KINPGRKPDKEY });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgramEdit", kinPrgrm);
            }
        }

        public ActionResult RealisasiKinerjaProgramEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string kinPgRKPDKey)
        {
            RKPDKinerjaProgram realisasi = new RKPDKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = kinPrgrmSvc.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, idPrgrm, kinPgRKPDKey);
                ViewBag.Method = "Edit";
                return View("RealisasiKinerjaProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDKinerjaProgram kinPrgrm = new RKPDKinerjaProgram();
            try
            {
                kinPrgrm = BindDataKinerjaProgramToObject(param);
                kinPrgrmSvc.UpdateDataRealisasi(kinPrgrm);
                return RedirectToAction("RealisasiKinerjaProgram", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kinPrgrm.UNITKEY, kdTahap = kinPrgrm.KDTAHAP, kdTahun = kinPrgrm.KDTAHUN, idPrgrm = kinPrgrm.IDPRGRM, kinPgRKPDKey = kinPrgrm.KINPGRKPDKEY });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgramEdit", kinPrgrm);
            }
        }
        #endregion Realisasi Kinerja Program

        #region Realisasi Kinerja Kegiatan
        public ActionResult RealisasiKinerjaKegiatan(String msg = "", String unitKey = "", String kdTahap = "", String kdTahun = "", String idPrgrm = "", String idKeg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.kdTahun = kdTahun;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.idKeg = idKeg;
            ViewBag.Info = msg;

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
                    List<Program> prgrm = genSvc.GetProgramRKPD(unitKey, idPrgrm);
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
            return View();
        }

        public ActionResult RealisasiKinerjaKegiatanCreate(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string idKeg = "")
        {
            RKPDKinerjaKegiatan realisasi = new RKPDKinerjaKegiatan();
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
                return View("RealisasiKinerjaKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaKegiatanCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDKinerjaKegiatan kinKeg = new RKPDKinerjaKegiatan();
            try
            {
                kinKeg = BindDataKinerjaKegiatanToObject(param);
                kinKegSvc.CreateDataRealisasi(kinKeg);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiKinerjaKegiatan", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = kinKeg.UNITKEY, kdTahap = kinKeg.KDTAHAP, kdTahun = kinKeg.KDTAHUN, idPrgrm = idPrgrm, idKeg = kinKeg.IDKEG });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTriwulanCombo(kinKeg.UNITKEY, kinKeg.KDTAHAP, kinKeg.KDTAHUN, kinKeg.IDKEG, kinKeg.KDTRWN);
                return View("RealisasiKinerjaKegiatanEdit", kinKeg);
            }
        }

        public ActionResult RealisasiKinerjaKegiatanEdit(string unitKey, string kdTahap, string kdTahun, string idPrgrm, string idKeg, string kdTrwn)
        {
            RKPDKinerjaKegiatan realisasi = new RKPDKinerjaKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                realisasi = kinKegSvc.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, kdTahun, idKeg,kdTrwn);
                ViewBag.idPrgrm = idPrgrm;
                ViewBag.Method = "Edit";
                return View("RealisasiKinerjaKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahap);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaKegiatanEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDKinerjaKegiatan kinKeg = new RKPDKinerjaKegiatan();
            try
            {
                kinKeg = BindDataKinerjaKegiatanToObject(param);
                kinKegSvc.UpdateDataRealisasi(kinKeg);
                var idPrgrm = param["IDPRGRM"].ToString();
                return RedirectToAction("RealisasiKinerjaKegiatan", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = kinKeg.UNITKEY, kdTahap = kinKeg.KDTAHAP, kdTahun = kinKeg.KDTAHUN, idPrgrm = idPrgrm, idKeg = kinKeg.IDKEG });
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTriwulanCombo(kinKeg.UNITKEY, kinKeg.KDTAHAP, kinKeg.KDTAHUN, kinKeg.IDKEG, kinKeg.KDTRWN);
                return View("RealisasiKinerjaKegiatanEdit", kinKeg);
            }
        }
        #endregion Realisasi Kinerja Kegiatan

        #region Catatan Laporan Realisasi
        public ActionResult CatatanLaporanRealisasi(string msg = "", string kdTahun = "", string kdTriwulan = "")
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

        public ActionResult CatatanLaporanRealisasiCreate()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDCatatanLaporanRealisasi catatan = new RKPDCatatanLaporanRealisasi();
            return CreateInputViewCatatan(catatan, "true");
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDCatatanLaporanRealisasi catatan = new RKPDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatan(param, true);
                if (ValidateDataCatatan(catatan, true))
                {                    
                    clrSvc.InsertData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasi", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatan(catatan, "true");
                }
                
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatan(catatan, "true");
            }
        }


        public ActionResult CatatanLaporanRealisasiEdit(string kdTahun, string kdTriwulan)
        {
            RKPDCatatanLaporanRealisasi catatan = new RKPDCatatanLaporanRealisasi();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                catatan = clrSvc.GetCatatanLaporanRealisasi(kdTahun, kdTriwulan);
                return CreateInputViewCatatan(catatan, "false");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatan(catatan, "false");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanRealisasiEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDCatatanLaporanRealisasi catatan = new RKPDCatatanLaporanRealisasi();
            try
            {
                catatan = BindDataToObjectCatatan(param,false);
                if (ValidateDataCatatan(catatan, false))
                {
                    clrSvc.UpdateData(catatan);
                    return RedirectToAction("CatatanLaporanRealisasi", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = catatan.KDTAHUN, kdTriwulan = catatan.KDTRWN });
                }
                else
                {
                    return CreateInputViewCatatan(catatan, "false");
                }                
                
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputViewCatatan(catatan, "false");
            }
        }
        #endregion Catatan Laporan Realisasi

        #region Evaluasi Bijak
        public ActionResult EvaluasiBijak(string msg = "", string kdTahun = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            GenerateTahunAnggaranCombo(kdTahun);
            return View();
        }

        public ActionResult EvaluasiBijakCreate(string kdTahun)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDEvaluasiBijak evaBijak = new RKPDEvaluasiBijak();
            return CreateInputEvaluasiBijak(evaBijak, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiBijakCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDEvaluasiBijak evaBijak = new RKPDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, true);
                if (ValidateDataEvaluasiBijak(evaBijak, true))
                {
                    evaBijakSvc.InsertData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = evaBijak.KDTAHUN, nomor = evaBijak.NOMOR });
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


        public ActionResult EvaluasiBijakEdit(string kdTahun, string nomor)
        {
            RKPDEvaluasiBijak evaBijak = new RKPDEvaluasiBijak();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaBijak = evaBijakSvc.GetEvaluasiBijak(kdTahun, nomor);
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

            RKPDEvaluasiBijak evaBijak = new RKPDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, false);
                if (ValidateDataEvaluasiBijak(evaBijak, false))
                {
                    evaBijakSvc.UpdateData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = evaBijak.KDTAHUN, nomor = evaBijak.NOMOR });
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

        public JsonResult DeleteEvaluasiBijak(string kdtahun = "", string nomor = "")
        {
            try
            {
                evaBijakSvc.DeleteData(kdtahun, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiBijak(string kdTahun = "", string nomor = "")
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
                List<RKPDEvaluasiBijak> lst = new List<RKPDEvaluasiBijak>();
                lst = evaBijakSvc.GetEvaluasiBijakList(kdTahun);

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
                return View("EvaluasiBijak");
            }
        }
        #endregion Evaluasi Bijak

        #region Evaluasi Simpul
        public ActionResult EvaluasiSimpul(string msg = "", string kdTahun = "", string nomor = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.kdTahun = kdTahun;
            ViewBag.nomor = nomor;
            ViewBag.Info = msg;

            GenerateTahunAnggaranCombo(kdTahun);
            return View();
        }

        public ActionResult EvaluasiSimpulCreate(string kdTahun)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDEvaluasiSimpul evaSimpul = new RKPDEvaluasiSimpul();
            return CreateInputEvaluasiSimpul(evaSimpul, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RKPDEvaluasiSimpul evaSimpul = new RKPDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, true);
                if (ValidateDataEvaluasiSimpul(evaSimpul, true))
                {
                    evaSimpulSvc.InsertData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RKPD", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdTahun = evaSimpul.KDTAHUN, nomor = evaSimpul.NOMOR });
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


        public ActionResult EvaluasiSimpulEdit(string kdTahun, string nomor)
        {
            RKPDEvaluasiSimpul evaSimpul = new RKPDEvaluasiSimpul();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                evaSimpul = evaSimpulSvc.GetEvaluasiSimpul(kdTahun, nomor);
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

            RKPDEvaluasiSimpul evaSimpul = new RKPDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, false);
                if (ValidateDataEvaluasiSimpul(evaSimpul, false))
                {
                    evaSimpulSvc.UpdateData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RKPD", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdTahun = evaSimpul.KDTAHUN, nomor = evaSimpul.NOMOR });
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

        public JsonResult DeleteEvaluasiSimpul(string kdtahun = "", string nomor = "")
        {
            try
            {
                evaSimpulSvc.DeleteData(kdtahun, nomor);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataEvaluasiSimpul(string kdTahun = "", string nomor = "")
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
                List<RKPDEvaluasiSimpul> lst = new List<RKPDEvaluasiSimpul>();
                lst = evaSimpulSvc.GetEvaluasiSimpulList(kdTahun);

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
                return View("EvaluasiSimpul");
            }
        }
        #endregion Evaluasi Simpul

        #region Private Function
        private Dictionary<string, string> TriwulanDictionary()
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

        private bool GenerateTriwulanCombo(string unitKey, string kdTahap, string kdTahun, string idKeg, string selected_id = "")
        {
            // get list of  triwulan
            var dict = TriwulanDictionary();

            // get saved triwulan data and remove from list            
            var list = kinKegSvc.GetKinerjaKegiatanRealisasiList(unitKey, kdTahap, kdTahun, idKeg);
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

        private ActionResult CreateInputViewCatatan(RKPDCatatanLaporanRealisasi clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
            GenerateTriwulanCombo(clr.KDTRWN == null ? string.Empty : clr.KDTRWN);
            ViewBag.isCreate = isCreate;
            return View("CatatanLaporanRealisasiInput", clr);
        }

        private ActionResult CreateInputEvaluasiBijak(RKPDEvaluasiBijak clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
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

        private ActionResult CreateInputEvaluasiSimpul(RKPDEvaluasiSimpul clr, string isCreate)
        {
            GenerateTahunAnggaranCombo(clr.KDTAHUN == null ? string.Empty : clr.KDTAHUN);
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

        public bool ValidateDataCatatan(RKPDCatatanLaporanRealisasi clr, bool isCreate)
        {
            Dictionary<string, string> dic = clrSvc.ValidateDataCatatan(clr, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        public bool ValidateDataEvaluasiBijak(RKPDEvaluasiBijak clr, bool isCreate)
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

        public bool ValidateDataEvaluasiSimpul(RKPDEvaluasiSimpul clr, bool isCreate)
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

        private bool GenerateTahapanCombo(string selected_id = "")
        {            
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahapan("1");
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dict.Add(item.KDTAHAP,item.NMTAHAP);
                }
            }
            
            ViewBag.TahapanCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
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

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Pemda.Bengkalis.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }

        private RKPDProgram BindDataProgramToObject(FormCollection param)
        {
            RKPDProgram rkpd = new RKPDProgram();
            rkpd.UNITKEY = param["UNITKEY"].ToString();
            rkpd.KDTAHAP = param["KDTAHAP"].ToString();
            rkpd.KDTAHUN = param["KDTAHUN"].ToString();
            rkpd.IDPRGRM = param["IDPRGRM"].ToString();

            rkpd.REAL1 = Convert.ToDecimal(param["REAL1"].ToString().Replace(".", ""));
            rkpd.REAL2 = Convert.ToDecimal(param["REAL2"].ToString().Replace(".", ""));
            rkpd.REAL3 = Convert.ToDecimal(param["REAL3"].ToString().Replace(".", ""));
            rkpd.REAL4 = Convert.ToDecimal(param["REAL4"].ToString().Replace(".", ""));
            rkpd.REALAKHIR = Convert.ToDecimal(param["REALAKHIR"].ToString().Replace(".", ""));
            rkpd.KET = param["KET"].ToString();
            rkpd.CTT = param["CTT"].ToString();
            return rkpd;
        }

        private RKPDKinerjaProgram BindDataKinerjaProgramToObject(FormCollection param)
        {
            RKPDKinerjaProgram kinPrgrm = new RKPDKinerjaProgram();
            kinPrgrm.UNITKEY = param["UNITKEY"].ToString();
            kinPrgrm.KDTAHAP = param["KDTAHAP"].ToString();
            kinPrgrm.KDTAHUN = param["KDTAHUN"].ToString();
            kinPrgrm.IDPRGRM = param["IDPRGRM"].ToString();
            kinPrgrm.KINPGRKPDKEY = param["KINPGRKPDKEY"].ToString();

            kinPrgrm.REALTAR1 = param["REALTAR1"].ToString();
            kinPrgrm.REALTAR2 = param["REALTAR2"].ToString();
            kinPrgrm.REALTAR3 = param["REALTAR3"].ToString();
            kinPrgrm.REALTAR4 = param["REALTAR4"].ToString();
            kinPrgrm.REALTARAKHIR = param["REALTARAKHIR"].ToString();
            kinPrgrm.KET = param["KET"].ToString();
            kinPrgrm.CTT = param["CTT"].ToString();
            return kinPrgrm;
        }

        private RKPDKinerjaKegiatan BindDataKinerjaKegiatanToObject(FormCollection param)
        {
            RKPDKinerjaKegiatan kinKeg = new RKPDKinerjaKegiatan();
            kinKeg.UNITKEY = param["UNITKEY"].ToString();
            kinKeg.KDTAHAP = param["KDTAHAP"].ToString();
            kinKeg.KDTAHUN = param["KDTAHUN"].ToString();
            kinKeg.IDKEG = param["IDKEG"].ToString();
            kinKeg.KDTRWN = param["TriwulanCombo"].ToString();

            kinKeg.REALNUM = Convert.ToDecimal(param["REALNUM"].ToString().Replace(".", ""));
            kinKeg.REALFISIK = Convert.ToDecimal(param["REALFISIK"].ToString().Replace(".", ""));
            kinKeg.REALSTR = Convert.ToDecimal(param["REALSTR"].ToString().Replace(".", ""));
            kinKeg.SATUAN = param["SATUAN"].ToString();
            kinKeg.MASALAH = param["MASALAH"].ToString();
            kinKeg.SOLUSI = param["SOLUSI"].ToString();
            kinKeg.KET = string.Empty;
            kinKeg.CTT = string.Empty;
            return kinKeg;
        }

        private RKPDCatatanLaporanRealisasi BindDataToObjectCatatan(FormCollection param, bool isCreate)
        {
            RKPDCatatanLaporanRealisasi catatan = new RKPDCatatanLaporanRealisasi();
            if (isCreate){ 
                catatan.KDTAHUN = param["TahunAnggaranCombo"].ToString();
                catatan.KDTRWN = param["TriwulanCombo"].ToString();
            }
            else { 
                catatan.KDTAHUN = param["KDTAHUN"].ToString();
                catatan.KDTRWN = param["KDTRWN"].ToString();
            }           
            
            catatan.NOCTT = param["NOCTT"].ToString();
            catatan.NMCTT = param["NMCTT"].ToString();
            catatan.ISICTT = param["ISICTT"].ToString();
            
            return catatan;
        }

        private RKPDEvaluasiBijak BindDataToObjectEvaluasiBijak(FormCollection param, bool isCreate)
        {
            RKPDEvaluasiBijak evaBijak = new RKPDEvaluasiBijak();
            //if (isCreate)
            //{
            //    evaBijak.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
            evaBijak.KDTAHUN = param["KDTAHUN"].ToString();
            //}

            evaBijak.NOMOR = param["NOMOR"].ToString();
            evaBijak.URAIAN = param["URAIAN"].ToString();
            evaBijak.KESESUAIAN = int.Parse(param["KESESUAIAN"].ToString());
            evaBijak.MASALAH = param["MASALAH"].ToString();
            evaBijak.SOLUSI = param["SOLUSI"].ToString();
            evaBijak.KET = param["KET"].ToString();

            return evaBijak;
        }

        private RKPDEvaluasiSimpul BindDataToObjectEvaluasiSimpul(FormCollection param, bool isCreate)
        {
            RKPDEvaluasiSimpul evaSimpul = new RKPDEvaluasiSimpul();
            //if (isCreate)
            //{
            //    evaSimpul.KDTAHUN = param["TahunAnggaranCombo"].ToString();
            //}
            //else
            //{
            evaSimpul.KDTAHUN = param["KDTAHUN"].ToString();
            //}

            evaSimpul.NOMOR = param["NOMOR"].ToString();
            evaSimpul.ASPEK = param["ASPEK"].ToString();
            evaSimpul.PENJELASAN = param["PENJELASAN"].ToString();
            evaSimpul.KET = param["KET"].ToString();

            return evaSimpul;
        }

        #endregion

        #region Json Result
        public ActionResult GetUnitOrganization()
        {
            var list = genSvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgramRKPD(string unitKey)
        {
            var list = genSvc.GetProgramRKPD(unitKey);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Realisasi Program
        [HttpPost]
        public JsonResult DeleteRealisasiProgram(string unitkey = "", string kdtahapan = "", string kdtahun = "", string idprgrm = "")
        {
            try
            {
                prgrmSvc.DeleteDataRealisasi(unitkey, kdtahapan, kdtahun, idprgrm);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiProgram(string unitKey = "", string kdTahapan = "", string kdTahun = "")
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
                List<RKPDProgram> rkpdProgram = new List<RKPDProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rkpdProgram = prgrmSvc.GetProgram(unitKey, kdTahapan, kdTahun);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdProgram = rkpdProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdProgram = rkpdProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdProgram.Count();
                //Paging     
                var data = rkpdProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiProgram");
            }
        }

        public ActionResult LoadDataRealisasiProgramDetail(string unitKey = "", string kdTahapan = "", string kdTahun = "", string idPrgrm = "")
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
                List<RKPDProgram> rkpdProgram = new List<RKPDProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rkpdProgram = prgrmSvc.GetProgramRealisasiList(unitKey, kdTahapan, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdProgram = rkpdProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdProgram = rkpdProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdProgram.Count();
                //Paging     
                var data = rkpdProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiProgram");
            }
        }
        
        // Realisasi Kinerja Program
        public JsonResult DeleteRealisasiKinerjaProgram(string unitkey = "", string kdtahapan = "", string kdtahun = "", string idprgrm = "", string kinPgRKPDKey = "")
        {
            try
            {
                kinPrgrmSvc.DeleteDataRealisasi(unitkey, kdtahapan, kdtahun, idprgrm, kinPgRKPDKey);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiKinerjaProgram(string unitKey = "", string kdTahapan = "", string kdTahun = "", string idPrgrm = "")
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
                List<RKPDKinerjaProgram> rkpdKinerjaProgram = new List<RKPDKinerjaProgram>();
                if (unitKey != "" && idPrgrm != "") //supaya pas first load page diawal load data
                {
                    rkpdKinerjaProgram = kinPrgrmSvc.GetKinerjaProgram(unitKey, kdTahapan, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdKinerjaProgram = rkpdKinerjaProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdKinerjaProgram = rkpdKinerjaProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdKinerjaProgram.Count();
                //Paging     
                var data = rkpdKinerjaProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaProgram");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaProgramDetail(string unitKey = "", string kdTahapan = "", string kdTahun = "", string idPrgrm = "", string kinPgRKPDKey = "")
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
                List<RKPDKinerjaProgram> rkpdKinerjaProgram = new List<RKPDKinerjaProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rkpdKinerjaProgram = kinPrgrmSvc.GetKinerjaProgramRealisasiList(unitKey, kdTahapan, kdTahun, idPrgrm, kinPgRKPDKey);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdKinerjaProgram = rkpdKinerjaProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdKinerjaProgram = rkpdKinerjaProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdKinerjaProgram.Count();
                //Paging     
                var data = rkpdKinerjaProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaProgram");
            }
        }

        // Realisasi Kinerja Kegiatan
        public JsonResult DeleteRealisasiKinerjaKegiatan(string unitkey = "", string kdtahapan = "", string kdtahun = "", string idkeg = "", string kdTrwn = "")
        {
            try
            {
                kinKegSvc.DeleteDataRealisasi(unitkey, kdtahapan, kdtahun, idkeg,kdTrwn);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiKinerjaKegiatan(string unitKey = "", string kdTahapan = "", string kdTahun = "", string idPrgrm = "")
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
                List<RKPDKinerjaKegiatan> rkpdKinerjaKegiatan = new List<RKPDKinerjaKegiatan>();
                if (unitKey != "" && idPrgrm != "") //supaya pas first load page diawal load data
                {
                    rkpdKinerjaKegiatan = kinKegSvc.GetKinerjaKegiatan(unitKey, kdTahapan, kdTahun, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdKinerjaKegiatan = rkpdKinerjaKegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdKinerjaKegiatan = rkpdKinerjaKegiatan.Where(m => m.NMKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdKinerjaKegiatan.Count();
                //Paging     
                var data = rkpdKinerjaKegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaKegiatanDetail(string unitKey = "", string kdTahapan = "", string kdTahun = "", string idKeg = "")
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
                List<RKPDKinerjaKegiatan> rkpdKinerjaKegiatan = new List<RKPDKinerjaKegiatan>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rkpdKinerjaKegiatan = kinKegSvc.GetKinerjaKegiatanRealisasiList(unitKey, kdTahapan, kdTahun, idKeg);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rkpdKinerjaKegiatan = rkpdKinerjaKegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rkpdKinerjaKegiatan = rkpdKinerjaKegiatan.Where(m => m.NMKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rkpdKinerjaKegiatan.Count();
                //Paging     
                var data = rkpdKinerjaKegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);

                GenerateTahapanCombo(kdTahapan);
                GenerateTahunAnggaranCombo(kdTahun);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        // Catatan Laporan Realisasi
        public JsonResult DeleteCatatanLaporanRealisasi(string kdtahun = "", string kdTriwulan = "")
        {
            try
            {
                clrSvc.DeleteData(kdtahun, kdTriwulan);
                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataCatatanLaporanRealisasi(string kdTahun = "", string kdTriwulan = "")
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
                List<RKPDCatatanLaporanRealisasi> lst = new List<RKPDCatatanLaporanRealisasi>();
                lst = clrSvc.GetCatatanLaporanRealisasiList(kdTahun, kdTriwulan);
                
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.NMCTT.ToLower().Contains(searchValue) ||
                        m.NOCTT.ToLower().Contains(searchValue)
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
                return View("CatatanLaporanRealisasi");
            }
        }

        #endregion

    }
}