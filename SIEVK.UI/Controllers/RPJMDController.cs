using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.Domain.RPJMD;
using SIEVK.BusinessService.Common;
using SIEVK.BusinessService.RPJMD;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class RPJMDController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        private RPJMDKinerjaProgramService rpjmdkinpgrmSvc = new RPJMDKinerjaProgramService();
        private RPJMDRealisasiProgramService rpjmdprgSvc = new RPJMDRealisasiProgramService();
        private RPJMDRealisasiKinerjaProgramService rpjmdkinerjaSvc = new RPJMDRealisasiKinerjaProgramService();
        private RPJMDRealisasiKegiatanService rpjmdkgtSvc = new RPJMDRealisasiKegiatanService();
        private RPJMDRealisasiKinerjaKegiatanService rpjmdkinkgtSvc = new RPJMDRealisasiKinerjaKegiatanService();
        private RPJMDCatatanLaporanEvaluasiService rpjmdCatSvc = new RPJMDCatatanLaporanEvaluasiService();
        private RPJMDEvaluasiBijakService evaBijakSvc = new RPJMDEvaluasiBijakService();
        private RPJMDEvaluasiSimpulService evaSimpulSvc = new RPJMDEvaluasiSimpulService();
        private RPJMDProgramService rpjmdPgrmSvc = new RPJMDProgramService();
        private RPJMDKegiatanService rpjmdKegSvc = new RPJMDKegiatanService();
        private RPJMDKinerjaKegiatanService rpjmdkinkegSvc = new RPJMDKinerjaKegiatanService();
        #endregion

        #region Realisasi Program

        public ActionResult ProgramEdit(string unitKey, string kdTahap, string idPrgrm)
        {
            RPJMDProgram program = new RPJMDProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit";

                program = rpjmdprgSvc.GetProgramPaguById(unitKey, kdTahap, idPrgrm);

                SetTahuntoViewBag();
                return View("ProgramEdit", program);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiProgram");
            }
        }

        [HttpPost]
        public ActionResult ProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDProgram rpjmd = new RPJMDProgram();
            try
            {
                rpjmd = BindDataProgramPaguToObject(param);
                rpjmdprgSvc.UpdateDataProgram(rpjmd);
                return RedirectToAction("RealisasiProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP,  idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("ProgramEdit", rpjmd);
            }
        }

        private RPJMDProgram BindDataProgramPaguToObject(FormCollection param)
        {
            RPJMDProgram rpjmd = new RPJMDProgram();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            //rpjmd.NUPRGRM = param["NUPRGRM"].ToString();
            //rpjmd.NMPRGRM = param["NMPRGRM"].ToString();
            
            rpjmd.PAGU1 = Convert.ToDecimal(param["PAGU1"].ToString().Replace(".", ""));
            rpjmd.PAGU2 = Convert.ToDecimal(param["PAGU2"].ToString().Replace(".", ""));
            rpjmd.PAGU3 = Convert.ToDecimal(param["PAGU3"].ToString().Replace(".", ""));
            rpjmd.PAGU4 = Convert.ToDecimal(param["PAGU4"].ToString().Replace(".", ""));
            rpjmd.PAGU5 = Convert.ToDecimal(param["PAGU5"].ToString().Replace(".", ""));
            rpjmd.PAGU6 = Convert.ToDecimal(param["PAGU6"].ToString().Replace(".", ""));
            rpjmd.PAGUAKHIR = Convert.ToDecimal(param["PAGUAKHIR"].ToString().Replace(".", ""));
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        public ActionResult RealisasiProgram(String unitKey = "", String kdTahap = "", String idPrgrm = "", String msg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }
            
            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.Info = msg;

            if (unitKey != "")//bila dibolehkan pke lookup
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
            SetTahuntoViewBag();
            return View();
        }

        public ActionResult RealisasiProgramCreate(string unitKey, string kdTahap, string idPrgrm)
        {
            RPJMDProgram realisasi = new RPJMDProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDPRGRM = idPrgrm;

                SetTahuntoViewBag();
                return View("RealisasiProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiProgramCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDProgram rpjmd = new RPJMDProgram();
            try
            {
                rpjmd = BindDataProgramToObject(param);
                rpjmdprgSvc.CreateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgramEdit", rpjmd);
            }
        }

        public ActionResult RealisasiProgramEdit(string unitKey, string kdTahap, string idPrgrm)
        {
            RPJMDProgram realisasi = new RPJMDProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit"; 
                realisasi = rpjmdprgSvc.GetProgramRealisasi(unitKey, kdTahap, idPrgrm);
                SetTahuntoViewBag();
                return View("RealisasiProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDProgram rpjmd = new RPJMDProgram();
            try
            {
                rpjmd = BindDataProgramToObject(param);
                rpjmdprgSvc.UpdateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgramEdit", rpjmd);
            }
        }

        private RPJMDProgram BindDataProgramToObject(FormCollection param)
        {
            RPJMDProgram rpjmd = new RPJMDProgram();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();

            rpjmd.PAGU1 = Convert.ToDecimal(param["PAGU1"].ToString());
            rpjmd.PAGU2 = Convert.ToDecimal(param["PAGU2"].ToString());
            rpjmd.PAGU3 = Convert.ToDecimal(param["PAGU3"].ToString());
            rpjmd.PAGU4 = Convert.ToDecimal(param["PAGU4"].ToString());
            rpjmd.PAGU5 = Convert.ToDecimal(param["PAGU5"].ToString());
            rpjmd.PAGU6 = Convert.ToDecimal(param["PAGU6"].ToString());
            rpjmd.PAGUAKHIR = Convert.ToDecimal(param["PAGUAKHIR"].ToString());
            rpjmd.REAL1 = Convert.ToDecimal(param["REAL1"].ToString().Replace(".", ""));
            rpjmd.REAL2 = Convert.ToDecimal(param["REAL2"].ToString().Replace(".", ""));
            rpjmd.REAL3 = Convert.ToDecimal(param["REAL3"].ToString().Replace(".", ""));
            rpjmd.REAL4 = Convert.ToDecimal(param["REAL4"].ToString().Replace(".", ""));
            rpjmd.REAL5 = Convert.ToDecimal(param["REAL5"].ToString().Replace(".", ""));
            rpjmd.REAL6 = Convert.ToDecimal(param["REAL6"].ToString().Replace(".", ""));
            rpjmd.REALAKHIR = Convert.ToDecimal(param["REALAKHIR"].ToString().Replace(".", ""));
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        #endregion

        #region Realisasi Kinerja Program
        public ActionResult RealisasiKinerjaProgram(String msg = "", String unitKey = "", String kdTahap = "", String idPrgrm = "", string kinpgrpjmkey ="")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            //unitKey = "108_";
            //kdTahap = "02";
            //idPrgrm = "1_";

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.kinpgrpjmkey = kinpgrpjmkey;
            ViewBag.Info = msg;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                List<Program> prg = genSvc.GetProgramRPJMD(unitKey, kdTahap, idPrgrm);
                if (org.Count > 0)
                {
                    ViewBag.programName = prg[0].KDPRGRM + " - " + prg[0].NMPRGRM;
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
            SetTahuntoViewBag();
            return View();
        }


        public ActionResult RealisasiKinerjaProgramCreate(string unitKey, string kdTahap, string idPrgrm, string kinpgrpjmkey = "")
        {
            RPJMDKinerjaProgram realisasi = new RPJMDKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";

                #region set empty
                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDPRGRM = idPrgrm;
                realisasi.KINPGRPJMKEY = kinpgrpjmkey;
                realisasi.REALTAR1 = string.Empty;
                realisasi.REALTAR2 = string.Empty;
                realisasi.REALTAR3 = string.Empty;
                realisasi.REALTAR4 = string.Empty;
                realisasi.REALTAR5 = string.Empty;
                realisasi.REALTAR6 = string.Empty;
                realisasi.REALTARAKHIR = string.Empty;
                realisasi.TARGET1 = "-";
                realisasi.TARGET2 = "-";
                realisasi.TARGET3 = "-";
                realisasi.TARGET4 = "-";
                realisasi.TARGET5 = "-";
                realisasi.TARGET6 = "-";
                realisasi.TARGETAKHIR = "-";
                #endregion

                SetTahuntoViewBag();
                return View("RealisasiKinerjaProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaProgramCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaProgram rpjmd = new RPJMDKinerjaProgram();
            try
            {
                rpjmd = BindDataKinerjaProgramToObject(param);
                rpjmdkinerjaSvc.CreateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiKinerjaProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, idPrgrm = rpjmd.IDPRGRM, kinpgrpjmkey = rpjmd.KINPGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgramEdit", rpjmd);
            }
        }


        public ActionResult RealisasiKinerjaProgramEdit(string unitKey, string kdTahap, string idPrgrm, string KINPGRPJMKEY)
        {
            RPJMDKinerjaProgram realisasi = new RPJMDKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit"; 
                realisasi = rpjmdkinerjaSvc.GetKinerjaProgramRealisasi(unitKey, kdTahap, idPrgrm, KINPGRPJMKEY);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaProgramEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaProgram");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaProgram rpjmd = new RPJMDKinerjaProgram();
            try
            {
                rpjmd = BindDataRealisasiKinerjaProgramToObject(param);
                rpjmdkinerjaSvc.UpdateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiKinerjaProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, idPrgrm = rpjmd.IDPRGRM, kinpgrpjmkey = rpjmd.KINPGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgramEdit", rpjmd);
            }
        }
        public ActionResult KinerjaProgram(String msg = "", String unitKey = "", String kdTahap = "", String idPrgrm = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            //unitKey = "108_";
            //kdTahap = "02";
            //idPrgrm = "1_";

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.Info = msg;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                List<Program> prg = genSvc.GetProgramRPJMD(unitKey, kdTahap, idPrgrm);
                if (org.Count > 0)
                {
                    ViewBag.programName = prg[0].KDPRGRM + " - " + prg[0].NMPRGRM;
                }
            }
            else
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
            SetTahuntoViewBag();
            return View();
        }
        public ActionResult KinerjaProgramEdit(string unitKey, string kdTahap, string idPrgrm)
        {
            RPJMDKinerjaProgram kinpgrm = new RPJMDKinerjaProgram();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit";
                kinpgrm = rpjmdkinpgrmSvc.GetListKinerjaProgram(unitKey, kdTahap, idPrgrm);
                SetTahuntoViewBag();
                return View("KinerjaProgramEdit", kinpgrm);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("KinerjaProgram");
            }
        }

        [HttpPost]
        public ActionResult KinerjaProgramEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaProgram rpjmd = new RPJMDKinerjaProgram();
            try
            {
                rpjmd = BindDataKinerjaProgramToObject(param);
                rpjmdkinpgrmSvc.UpdateDataKinpgrm(rpjmd);
                return RedirectToAction("RealisasiKinerjaProgram", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, idPrgrm = rpjmd.IDPRGRM, kinpgrpjmkey = rpjmd.KINPGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("KinerjaProgramEdit", rpjmd);
            }
        }
        private RPJMDKinerjaProgram BindDataRealisasiKinerjaProgramToObject(FormCollection param)
        {
            RPJMDKinerjaProgram rpjmd = new RPJMDKinerjaProgram();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            rpjmd.KINPGRPJMKEY = param["KINPGRPJMKEY"].ToString();
            rpjmd.TARGET1 = param["TARGET1"].ToString();
            rpjmd.TARGET2 = param["TARGET2"].ToString();
            rpjmd.TARGET3 = param["TARGET3"].ToString();
            rpjmd.TARGET4 = param["TARGET4"].ToString();
            rpjmd.TARGET5 = param["TARGET5"].ToString();
            rpjmd.TARGET6 = param["TARGET6"].ToString();
            rpjmd.TARGETAKHIR = param["TARGETAKHIR"].ToString();
            rpjmd.REALTAR1 = param["REALTAR1"].ToString().Replace(".", "");
            rpjmd.REALTAR2 = param["REALTAR2"].ToString().Replace(".", "");
            rpjmd.REALTAR3 = param["REALTAR3"].ToString().Replace(".", "");
            rpjmd.REALTAR4 = param["REALTAR4"].ToString().Replace(".", "");
            rpjmd.REALTAR5 = param["REALTAR5"].ToString().Replace(".", "");
            rpjmd.REALTAR6 = param["REALTAR6"].ToString().Replace(".", "");
            rpjmd.REALTARAKHIR = param["REALTARAKHIR"].ToString().Replace(".", "");
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        private RPJMDKinerjaProgram BindDataKinerjaProgramToObject(FormCollection param)
        {
            RPJMDKinerjaProgram rpjmd = new RPJMDKinerjaProgram();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            rpjmd.KINPGRPJMKEY = param["KINPGRPJMKEY"].ToString();
            rpjmd.NOKIN = param["NOKIN"].ToString();
            rpjmd.URKIN = param["URKIN"].ToString();
            rpjmd.KINLALU = param["KINLALU"].ToString().Replace(".", "");
            rpjmd.TARGET1 = param["TARGET1"].ToString().Replace(".", "");
            rpjmd.TARGET2 = param["TARGET2"].ToString().Replace(".", "");
            rpjmd.TARGET3 = param["TARGET3"].ToString().Replace(".", "");
            rpjmd.TARGET4 = param["TARGET4"].ToString().Replace(".", "");
            rpjmd.TARGET5 = param["TARGET5"].ToString().Replace(".", "");
            rpjmd.TARGET6 = param["TARGET6"].ToString().Replace(".", "");
            rpjmd.TARGETAKHIR = param["TARGETAKHIR"].ToString().Replace(".", "");
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        #endregion

        #region Realisasi Kegiatan
        public ActionResult RealisasiKegiatan(String msg = "", String unitKey = "", String kdTahap = "", String IDKeg = "", String idPrgrm = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.IDKeg = IDKeg;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.Info = msg;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                List<Program> prg = genSvc.GetProgramRPJMD(unitKey, kdTahap, idPrgrm);
                if (org.Count > 0)
                {
                    ViewBag.programName = prg[0].KDPRGRM + " - " + prg[0].NMPRGRM;
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
            SetTahuntoViewBag();
            return View();
        }

        
        public ActionResult RealisasiKegiatanCreate(string unitKey, string kdTahap, string IDKeg)
        {
            RPJMDKegiatan realisasi = new RPJMDKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDKEG = IDKeg;

                SetTahuntoViewBag();
                return View("RealisasiKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKegiatanCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKegiatan rpjmd = new RPJMDKegiatan();
            try
            {
                rpjmd = BindDataKegiatanToObject(param);
                rpjmdkgtSvc.CreateDataRealisasi(rpjmd);
                rpjmd = rpjmdkgtSvc.GetKegiatanRealisasi(rpjmd.UNITKEY, rpjmd.KDTAHAP, rpjmd.IDKEG);
                return RedirectToAction("RealisasiKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKegiatanEdit", rpjmd);
            }
        }


        public ActionResult RealisasiKegiatanEdit(string unitKey, string kdTahap, string IDKeg)
        {
            RPJMDKegiatan realisasi = new RPJMDKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit"; 
                realisasi = rpjmdkgtSvc.GetKegiatanRealisasi(unitKey, kdTahap, IDKeg);
                SetTahuntoViewBag();
                return View("RealisasiKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKegiatanEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKegiatan rpjmd = new RPJMDKegiatan();
            try
            {
                rpjmd = BindDataKegiatanToObject(param);
                rpjmdkgtSvc.UpdateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKegiatanEdit", rpjmd);
            }
        }
      
        //Ian, 20200216
        public ActionResult KegiatanRPJMDEdit(string unitKey, string kdTahap, string idProgram, string IDKeg)
        {
            RPJMDKegiatan kegiatan = new RPJMDKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit";

                kegiatan = rpjmdKegSvc.GetKegiatanPaguById(unitKey, kdTahap, idProgram, IDKeg);

                SetTahuntoViewBag();
                return View("KegiatanRPJMDEdit", kegiatan);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKegiatan");
            }
        }

        [HttpPost]
        public ActionResult KegiatanRPJMDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKegiatan rpjmd = new RPJMDKegiatan();
            try
            {
                rpjmd = BindDataKegiatanPaguToObject(param);
                rpjmdKegSvc.Update(rpjmd);
                return RedirectToAction("RealisasiKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("KegiatanRPJMDEdit", rpjmd);
            }
        }

        private RPJMDKegiatan BindDataKegiatanToObject(FormCollection param)
        {
            RPJMDKegiatan rpjmd = new RPJMDKegiatan();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDKEG = param["IDKEG"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();

            rpjmd.PAGU1 = Convert.ToDecimal(param["PAGU1"].ToString());
            rpjmd.PAGU2 = Convert.ToDecimal(param["PAGU2"].ToString());
            rpjmd.PAGU3 = Convert.ToDecimal(param["PAGU3"].ToString());
            rpjmd.PAGU4 = Convert.ToDecimal(param["PAGU4"].ToString());
            rpjmd.PAGU5 = Convert.ToDecimal(param["PAGU5"].ToString());
            rpjmd.PAGUAKHIR = Convert.ToDecimal(param["PAGUAKHIR"].ToString());
            rpjmd.REAL1 = Convert.ToDecimal(param["REAL1"].ToString().Replace(".", ""));
            rpjmd.REAL2 = Convert.ToDecimal(param["REAL2"].ToString().Replace(".", ""));
            rpjmd.REAL3 = Convert.ToDecimal(param["REAL3"].ToString().Replace(".", ""));
            rpjmd.REAL4 = Convert.ToDecimal(param["REAL4"].ToString().Replace(".", ""));
            rpjmd.REAL5 = Convert.ToDecimal(param["REAL5"].ToString().Replace(".", ""));
            rpjmd.REAL6 = Convert.ToDecimal(param["REAL6"].ToString().Replace(".", ""));
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        private RPJMDKegiatan BindDataKegiatanPaguToObject(FormCollection param)
        {
            RPJMDKegiatan rpjmd = new RPJMDKegiatan();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDKEG = param["IDKEG"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            rpjmd.NUKEG = param["NUKEG"].ToString();
            rpjmd.NMKEG = param["NMKEG"].ToString();

            rpjmd.PAGULALU = Convert.ToDecimal(param["PAGULALU"].ToString().Replace(".", ""));
            rpjmd.PAGU1 = Convert.ToDecimal(param["PAGU1"].ToString().Replace(".", ""));
            rpjmd.PAGU2 = Convert.ToDecimal(param["PAGU2"].ToString().Replace(".", ""));
            rpjmd.PAGU3 = Convert.ToDecimal(param["PAGU3"].ToString().Replace(".", ""));
            rpjmd.PAGU4 = Convert.ToDecimal(param["PAGU4"].ToString().Replace(".", ""));
            rpjmd.PAGU5 = Convert.ToDecimal(param["PAGU5"].ToString().Replace(".", ""));
            rpjmd.PAGU6 = Convert.ToDecimal(param["PAGU6"].ToString().Replace(".", ""));
            rpjmd.PAGUAKHIR = Convert.ToDecimal(param["PAGUAKHIR"].ToString().Replace(".", ""));
            return rpjmd;
        }

        #endregion

        #region Realisasi Kinerja Kegiatan
        public ActionResult RealisasiKinerjaKegiatan(String msg = "", String unitKey = "", String kdTahap = "", String idPrgrm = "", String IDKeg = "", string kinkegrpjmkey = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.unitKey = unitKey;
            ViewBag.kdTahap = kdTahap;
            ViewBag.IDKeg = IDKeg;
            ViewBag.idPrgrm = idPrgrm;
            ViewBag.kinkegrpjmkey = kinkegrpjmkey;
            ViewBag.Info = msg;

            if (unitKey != "")
            {
                List<DaftUnit> org = genSvc.GetUnitOrganization(unitKey);
                if (org.Count > 0) { ViewBag.unitOrgName = org[0].KDUNIT + " - " + org[0].NMUNIT; }

                List<Program> prg = genSvc.GetProgramRPJMD(unitKey, kdTahap, idPrgrm);
                if (org.Count > 0)
                {
                    ViewBag.programName = prg[0].KDPRGRM + " - " + prg[0].NMPRGRM;
                }

                List<RPJMDKegiatan> keg = rpjmdkgtSvc.GetKegiatanPagu(unitKey, kdTahap, idPrgrm, IDKeg);
                if (keg.Count > 0)
                {
                    ViewBag.kegiatanName = keg[0].NUKEG + " - " + keg[0].NMKEG;
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
            SetTahuntoViewBag();
            return View();
        }

        public ActionResult RealisasiKinerjaKegiatanCreate(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            RPJMDKinerjaKegiatan realisasi = new RPJMDKinerjaKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";

                #region set empty
                realisasi.UNITKEY = unitKey;
                realisasi.KDTAHAP = kdTahap;
                realisasi.IDKEG = IDKeg;
                realisasi.KINKEGRPJMKEY = kinkegrpjmkey;
                realisasi.REALTAR1 = string.Empty;
                realisasi.REALTAR2 = string.Empty;
                realisasi.REALTAR3 = string.Empty;
                realisasi.REALTAR4 = string.Empty;
                realisasi.REALTAR5 = string.Empty;
                realisasi.REALTAR6 = string.Empty;
                realisasi.TARGET1 = "-";
                realisasi.TARGET2 = "-";
                realisasi.TARGET3 = "-";
                realisasi.TARGET4 = "-";
                realisasi.TARGET5 = "-";
                realisasi.TARGET6 = "-";
                realisasi.TARGETAKHIR = "-";
                #endregion

                SetTahuntoViewBag();
                return View("RealisasiKinerjaKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaKegiatanCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaKegiatan rpjmd = new RPJMDKinerjaKegiatan();
            try
            {
                rpjmd = BindDataKinerjaKegiatanToObject(param);
                rpjmdkinkgtSvc.CreateDataRealisasi(rpjmd);
                rpjmd = rpjmdkinkgtSvc.GetKinerjaKegiatanRealisasi(rpjmd.UNITKEY, rpjmd.KDTAHAP, rpjmd.IDKEG, rpjmd.KINKEGRPJMKEY );
                return RedirectToAction("RealisasiKinerjaKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM, kinkegrpjmkey = rpjmd.KINKEGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaKegiatanEdit", rpjmd);
            }
        }


        public ActionResult RealisasiKinerjaKegiatanEdit(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            RPJMDKinerjaKegiatan realisasi = new RPJMDKinerjaKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Tambah";
                realisasi = rpjmdkinkgtSvc.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, IDKeg, kinkegrpjmkey);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaKegiatanEdit", realisasi);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaKegiatan");
            }
        }

        [HttpPost]
        public ActionResult RealisasiKinerjaKegiatanEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaKegiatan rpjmd = new RPJMDKinerjaKegiatan();
            try
            {
                rpjmd = BindDataKinerjaKegiatanToObject(param);
                rpjmdkinkgtSvc.UpdateDataRealisasi(rpjmd);
                return RedirectToAction("RealisasiKinerjaKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM, kinkegrpjmkey = rpjmd.KINKEGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaKegiatanEdit", rpjmd);
            }
        }

        private RPJMDKinerjaKegiatan BindDataKinerjaKegiatanToObject(FormCollection param)
        {
            RPJMDKinerjaKegiatan rpjmd = new RPJMDKinerjaKegiatan();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDKEG = param["IDKEG"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            rpjmd.KINKEGRPJMKEY = param["KINKEGRPJMKEY"].ToString();

            rpjmd.TARGET1 = param["TARGET1"].ToString();
            rpjmd.TARGET2 = param["TARGET2"].ToString();
            rpjmd.TARGET3 = param["TARGET3"].ToString();
            rpjmd.TARGET4 = param["TARGET4"].ToString();
            rpjmd.TARGET5 = param["TARGET5"].ToString();
            rpjmd.TARGET6 = param["TARGET6"].ToString();
            rpjmd.TARGETAKHIR = param["TARGETAKHIR"].ToString();
            rpjmd.REALTAR1 = param["REALTAR1"].ToString();
            rpjmd.REALTAR2 = param["REALTAR2"].ToString();
            rpjmd.REALTAR3 = param["REALTAR3"].ToString();
            rpjmd.REALTAR4 = param["REALTAR4"].ToString();
            rpjmd.REALTAR5 = param["REALTAR5"].ToString();
            rpjmd.REALTAR6 = param["REALTAR6"].ToString();
            rpjmd.KET = param["KET"].ToString();
            rpjmd.CTT = param["CTT"].ToString();
            return rpjmd;
        }

        //Ian, 20200216
        public ActionResult KinerjaKegiatanRPJMDEdit(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            RPJMDKinerjaKegiatan kinkeg = new RPJMDKinerjaKegiatan();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                ViewBag.Method = "Edit";
                kinkeg = rpjmdkinkegSvc.GetKinerjaKegiatanById(unitKey, kdTahap, IDKeg, kinkegrpjmkey);
                SetTahuntoViewBag();
                return View("KinerjaKegiatanRPJMDEdit", kinkeg);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo(kdTahap);
                SetTahuntoViewBag();
                return View("RealisasiKinerjaKegiatan");
            }
        }

        [HttpPost]
        public ActionResult KinerjaKegiatanRPJMDEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDKinerjaKegiatan rpjmd = new RPJMDKinerjaKegiatan();
            try
            {
                rpjmd = BindDataKinerjaKegiatanTargetToObject(param);
                rpjmdkinkegSvc.Update(rpjmd);
                return RedirectToAction("RealisasiKinerjaKegiatan", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), unitKey = rpjmd.UNITKEY, kdTahap = rpjmd.KDTAHAP, IDKeg = rpjmd.IDKEG, idPrgrm = rpjmd.IDPRGRM, kinkegrpjmkey = rpjmd.KINKEGRPJMKEY });
            }
            catch (Exception ex)
            {
                SetTahuntoViewBag();
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("KinerjaKegiatanRPJMDEdit", rpjmd);
            }
        }

        private RPJMDKinerjaKegiatan BindDataKinerjaKegiatanTargetToObject(FormCollection param)
        {
            RPJMDKinerjaKegiatan rpjmd = new RPJMDKinerjaKegiatan();
            rpjmd.UNITKEY = param["UNITKEY"].ToString();
            rpjmd.KDTAHAP = param["KDTAHAP"].ToString();
            rpjmd.IDKEG = param["IDKEG"].ToString();
            rpjmd.IDPRGRM = param["IDPRGRM"].ToString();
            rpjmd.KINKEGRPJMKEY = param["KINKEGRPJMKEY"].ToString();
            rpjmd.NUKEG = param["NUKEG"].ToString();
            rpjmd.NMKEG = param["NMKEG"].ToString();

            rpjmd.URKIN = param["URKIN"].ToString();
            rpjmd.KINLALU = param["KINLALU"].ToString();
            rpjmd.TARGET1 = param["TARGET1"].ToString();
            rpjmd.TARGET2 = param["TARGET2"].ToString();
            rpjmd.TARGET3 = param["TARGET3"].ToString();
            rpjmd.TARGET4 = param["TARGET4"].ToString();
            rpjmd.TARGET5 = param["TARGET5"].ToString();
            rpjmd.TARGET6 = param["TARGET6"].ToString();
            rpjmd.TARGETAKHIR = param["TARGETAKHIR"].ToString();
            rpjmd.KET = param["KET"].ToString();
            return rpjmd;
        }

        #endregion

        #region Catatan Laporan Evaluasi
        public ActionResult CatatanLaporanEvaluasi(String msg = "", string kdtahun = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.Info = msg;
            GenerateTahunAnggaranCombo(kdtahun);
            return View();
        }

        public ActionResult CatatanLaporanEvaluasiCreate()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunAnggaranCombo();
            ViewBag.isCreate = "true";
            return View("CatatanLaporanEvaluasiInput");
        }

        [HttpPost]
        public ActionResult CatatanLaporanEvaluasiCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDCatatanLaporanEvaluasi catatan = new RPJMDCatatanLaporanEvaluasi();
            try
            {
                catatan = BindDataToObjectCatatan(param, true);
                if (ValidateDataCatatan(catatan, true))
                {
                    rpjmdCatSvc.InsertData(catatan);
                    return RedirectToAction("CatatanLaporanEvaluasi", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), kdtahun = catatan.KDTAHUN });
                }
                else
                {
                    GenerateTahunAnggaranCombo(catatan.KDTAHUN);
                    ViewBag.isCreate = "true";
                    return View("CatatanLaporanEvaluasiInput", catatan);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahunAnggaranCombo();
                ViewBag.isCreate = "true";
                return View("CatatanLaporanEvaluasiInput");
            }
        }

        public ActionResult CatatanLaporanEvaluasiEdit(string kdtahun, string nocat)
        {
            RPJMDCatatanLaporanEvaluasi catatan = new RPJMDCatatanLaporanEvaluasi();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                GenerateTahunAnggaranCombo(kdtahun);
                ViewBag.isCreate = "false";
                catatan = rpjmdCatSvc.GetCatatanLaporanEvaluasi(kdtahun, nocat);
                return View("CatatanLaporanEvaluasiInput", catatan);
               
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahunAnggaranCombo();
                ViewBag.isCreate = "false";
                return View("CatatanLaporanEvaluasiInput");
            }
        }

        [HttpPost]
        public ActionResult CatatanLaporanEvaluasiEdit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDCatatanLaporanEvaluasi catatan = new RPJMDCatatanLaporanEvaluasi();
            try
            {
                catatan = BindDataToObjectCatatan(param, false);
                if (ValidateDataCatatan(catatan, false))
                {
                    rpjmdCatSvc.UpdateData(catatan);
                    return RedirectToAction("CatatanLaporanEvaluasi", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), kdtahun = catatan.KDTAHUN });
                }
                else
                {
                    GenerateTahunAnggaranCombo(catatan.KDTAHUN);
                    ViewBag.isCreate = "false";
                    return View("CatatanLaporanEvaluasiInput", catatan);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahunAnggaranCombo();
                ViewBag.isCreate = "false";
                return View("CatatanLaporanEvaluasiInput", catatan);
            }
        }

        private RPJMDCatatanLaporanEvaluasi BindDataToObjectCatatan(FormCollection param, bool isCreate)
        {
            RPJMDCatatanLaporanEvaluasi catatan = new RPJMDCatatanLaporanEvaluasi();
            if (isCreate)
            { catatan.KDTAHUN = param["TahunAnggaranCombo"].ToString(); }
            else { catatan.KDTAHUN = param["KDTAHUN"].ToString(); }
            catatan.NOCTT = param["NOCTT"].ToString();
            catatan.NMCTT = param["NMCTT"].ToString();
            catatan.ISICTT = param["ISICTT"].ToString();
            
            return catatan;
        }

        public bool ValidateDataCatatan(RPJMDCatatanLaporanEvaluasi user, bool isCreate)
        {
            Dictionary<string, string> dic = rpjmdCatSvc.ValidateData(user, isCreate);
            foreach (KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        #endregion

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

        private bool GenerateTahapanCombo(string selected_id = "")
        {
            var dict = new Dictionary<string, string>();

            var list = genSvc.GetTahapan("0");
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

        public ActionResult GetUnitOrganization()
        {
            var list = genSvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgram(string unitKey = "", string kdTahapan = "")
        {
           var list = genSvc.GetProgramRPJMD(unitKey, kdTahapan);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKegiatan(string unitKey = "", string kdTahapan = "", string idprgrm = "")
        {
           var list = rpjmdkgtSvc.GetKegiatanPagu(unitKey, kdTahapan, idprgrm);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region JSON Result

        [HttpPost]
        public JsonResult DeleteRealisasiProgram(string unitkey = "", string kdtahapan = "", string idprgrm = "")
        {
            try
            {
                rpjmdprgSvc.DeleteDataRealisasi(unitkey, kdtahapan, idprgrm);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteRealisasiKinerjaProgram(string unitkey = "", string kdtahapan = "", string idprgrm = "", string kinpgrpjmkey = "")
        {
            try
            {
                rpjmdkinerjaSvc.DeleteDataRealisasi(unitkey, kdtahapan, idprgrm, kinpgrpjmkey);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteRealisasiKegiatan(string unitkey = "", string kdtahapan = "", string IDKeg = "")
        {
            try
            {
                rpjmdkgtSvc.DeleteDataRealisasi(unitkey, kdtahapan, IDKeg);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult DeleteRealisasiKinerjaKegiatan(string unitkey = "", string kdtahapan = "", string IDKeg = "", string kinkegrpjmkey="")
        {
            try
            {
                rpjmdkinkgtSvc.DeleteDataRealisasi(unitkey, kdtahapan, IDKeg, kinkegrpjmkey);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteCatatanLaporanEvaluasi(string kdtahun = "", string nocat = "")
        {
            try
            {
                rpjmdCatSvc.DeleteData(kdtahun, nocat);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadDataRealisasiProgram(string unitKey = "", string kdTahapan= "")
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
                List<RPJMDProgram> rpjmdProgram = new List<RPJMDProgram>();
                if (unitKey!= "") //supaya pas first load page diawal load data
                {
                    rpjmdProgram = rpjmdprgSvc.GetProgramPagu(unitKey, kdTahapan);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdProgram = rpjmdProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdProgram = rpjmdProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdProgram.Count();
                //Paging     
                var data = rpjmdProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgram");
            }
        }

        public ActionResult LoadDataRealisasiProgramDetail(string unitKey = "", string kdTahapan = "", string idPrgrm = "")
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
                List<RPJMDProgram> rpjmdProgram = new List<RPJMDProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdProgram = rpjmdprgSvc.GetProgramRealisasiList(unitKey, kdTahapan, idPrgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdProgram = rpjmdProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdProgram = rpjmdProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdProgram.Count();
                //Paging     
                var data = rpjmdProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiProgram");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaProgram(string unitKey = "", string kdTahapan = "", string idprgrm = "")
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
                List<RPJMDKinerjaProgram> rpjmdKinProgram = new List<RPJMDKinerjaProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdKinProgram = rpjmdkinerjaSvc.GetKinerjaProgram(unitKey, kdTahapan, idprgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdKinProgram = rpjmdKinProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdKinProgram = rpjmdKinProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdKinProgram.Count();
                //Paging     
                var data = rpjmdKinProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgram");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaProgramDetail(string unitKey = "", string kdTahapan = "", string idPrgrm = "", string kinpgrpjmkey = "")
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
                List<RPJMDKinerjaProgram> rpjmdKinProgram = new List<RPJMDKinerjaProgram>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdKinProgram = rpjmdkinerjaSvc.GetKinerjaProgramRealisasiList(unitKey, kdTahapan, idPrgrm, kinpgrpjmkey);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdKinProgram = rpjmdKinProgram.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdKinProgram = rpjmdKinProgram.Where(m => m.NUPRGRM.ToLower().Contains(searchValue) ||
                        m.NMPRGRM.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdKinProgram.Count();
                //Paging     
                var data = rpjmdKinProgram.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaProgram");
            }
        }

        public ActionResult LoadDataRealisasiKegiatan(string unitKey = "", string kdTahapan = "", string idprgrm = "")
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
                List<RPJMDKegiatan> rpjmdkegiatan = new List<RPJMDKegiatan>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdkegiatan = rpjmdkgtSvc.GetKegiatanPagu(unitKey, kdTahapan, idprgrm);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdkegiatan = rpjmdkegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdkegiatan = rpjmdkegiatan.Where(m => m.NUKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdkegiatan.Count();
                //Paging     
                var data = rpjmdkegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKegiatan");
            }
        }

        public ActionResult LoadDataRealisasiKegiatanDetail(string unitKey = "", string kdTahapan = "", string IDKeg = "")
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
                List<RPJMDKegiatan> rpjmdkegiatan = new List<RPJMDKegiatan>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdkegiatan = rpjmdkgtSvc.GetKegiatanRealisasiList(unitKey, kdTahapan, IDKeg);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdkegiatan = rpjmdkegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdkegiatan = rpjmdkegiatan.Where(m => m.NUKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdkegiatan.Count();
                //Paging     
                var data = rpjmdkegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKegiatan");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaKegiatan(string unitKey = "", string kdTahapan = "", string IDKeg = "")
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
                List<RPJMDKinerjaKegiatan> rpjmdKinKegiatan = new List<RPJMDKinerjaKegiatan>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdKinKegiatan = rpjmdkinkgtSvc.GetKinerjaKegiatan(unitKey, kdTahapan, IDKeg);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdKinKegiatan = rpjmdKinKegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdKinKegiatan = rpjmdKinKegiatan.Where(m => m.NUKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdKinKegiatan.Count();
                //Paging     
                var data = rpjmdKinKegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        public ActionResult LoadDataRealisasiKinerjaKegiatanDetail(string unitKey = "", string kdTahapan = "", string IDKeg = "", string kinkegrpjmkey = "")
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
                List<RPJMDKinerjaKegiatan> rpjmdKinKegiatan = new List<RPJMDKinerjaKegiatan>();
                if (unitKey != "") //supaya pas first load page diawal load data
                {
                    rpjmdKinKegiatan = rpjmdkinkgtSvc.GetKinerjaKegiatanRealisasiList(unitKey, kdTahapan, IDKeg, kinkegrpjmkey);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdKinKegiatan = rpjmdKinKegiatan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdKinKegiatan = rpjmdKinKegiatan.Where(m => m.NUKEG.ToLower().Contains(searchValue) ||
                        m.NMKEG.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdKinKegiatan.Count();
                //Paging     
                var data = rpjmdKinKegiatan.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RealisasiKinerjaKegiatan");
            }
        }

        public ActionResult LoadDataCatatanLaporanEvaluasi(string kdtahun = "")
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
                List<RPJMDCatatanLaporanEvaluasi> rpjmdCat = new List<RPJMDCatatanLaporanEvaluasi>();
                if (kdtahun != "") //supaya pas first load page diawal load data
                {
                    rpjmdCat = rpjmdCatSvc.GetCatatanLaporanEvaluasiList(kdtahun);
                }

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    rpjmdCat = rpjmdCat.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rpjmdCat = rpjmdCat.Where(m => m.NOCTT.ToLower().Contains(searchValue) ||
                        m.NMCTT.ToLower().Contains(searchValue) || m.ISICTT.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = rpjmdCat.Count();
                //Paging     
                var data = rpjmdCat.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("CatatanLaporanEvaluasi");
            }
        }
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

            RPJMDEvaluasiBijak evaBijak = new RPJMDEvaluasiBijak();
            return CreateInputEvaluasiBijak(evaBijak, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiBijakCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDEvaluasiBijak evaBijak = new RPJMDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, true);
                if (ValidateDataEvaluasiBijak(evaBijak, true))
                {
                    evaBijakSvc.InsertData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), nomor = evaBijak.NOMOR });
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
            RPJMDEvaluasiBijak evaBijak = new RPJMDEvaluasiBijak();
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

            RPJMDEvaluasiBijak evaBijak = new RPJMDEvaluasiBijak();
            try
            {
                evaBijak = BindDataToObjectEvaluasiBijak(param, false);
                if (ValidateDataEvaluasiBijak(evaBijak, false))
                {
                    evaBijakSvc.UpdateData(evaBijak);
                    return RedirectToAction("EvaluasiBijak", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), nomor = evaBijak.NOMOR });
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
                List<RPJMDEvaluasiBijak> lst = new List<RPJMDEvaluasiBijak>();
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

        public bool ValidateDataEvaluasiBijak(RPJMDEvaluasiBijak clr, bool isCreate)
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

        private RPJMDEvaluasiBijak BindDataToObjectEvaluasiBijak(FormCollection param, bool isCreate)
        {
            RPJMDEvaluasiBijak evaBijak = new RPJMDEvaluasiBijak();

            evaBijak.NOMOR = param["NOMOR"].ToString();
            evaBijak.URAIAN = param["URAIAN"].ToString();
            evaBijak.KESESUAIAN = int.Parse(param["KESESUAIAN"].ToString());
            evaBijak.MASALAH = param["MASALAH"].ToString();
            evaBijak.SOLUSI = param["SOLUSI"].ToString();
            evaBijak.KET = param["KET"].ToString();

            return evaBijak;
        }

        private ActionResult CreateInputEvaluasiBijak(RPJMDEvaluasiBijak clr, string isCreate)
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

            RPJMDEvaluasiSimpul evaSimpul = new RPJMDEvaluasiSimpul();
            return CreateInputEvaluasiSimpul(evaSimpul, "true");
        }

        [HttpPost]
        public ActionResult EvaluasiSimpulCreate(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            RPJMDEvaluasiSimpul evaSimpul = new RPJMDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, true);
                if (ValidateDataEvaluasiSimpul(evaSimpul, true))
                {
                    evaSimpulSvc.InsertData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RPJMD", new { msg = GetValueFromResource("SUCCESS_INPUT"), nomor = evaSimpul.NOMOR });
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
            RPJMDEvaluasiSimpul evaSimpul = new RPJMDEvaluasiSimpul();
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

            RPJMDEvaluasiSimpul evaSimpul = new RPJMDEvaluasiSimpul();
            try
            {
                evaSimpul = BindDataToObjectEvaluasiSimpul(param, false);
                if (ValidateDataEvaluasiSimpul(evaSimpul, false))
                {
                    evaSimpulSvc.UpdateData(evaSimpul);
                    return RedirectToAction("EvaluasiSimpul", "RPJMD", new { msg = GetValueFromResource("SUCCESS_EDIT"), nomor = evaSimpul.NOMOR });
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
                List<RPJMDEvaluasiSimpul> lst = new List<RPJMDEvaluasiSimpul>();
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

        public bool ValidateDataEvaluasiSimpul(RPJMDEvaluasiSimpul clr, bool isCreate)
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

        private RPJMDEvaluasiSimpul BindDataToObjectEvaluasiSimpul(FormCollection param, bool isCreate)
        {
            RPJMDEvaluasiSimpul evaSimpul = new RPJMDEvaluasiSimpul();

            evaSimpul.NOMOR = param["NOMOR"].ToString();
            evaSimpul.ASPEK = param["ASPEK"].ToString();
            evaSimpul.PENJELASAN = param["PENJELASAN"].ToString();
            evaSimpul.KET = param["KET"].ToString();

            return evaSimpul;
        }

        private ActionResult CreateInputEvaluasiSimpul(RPJMDEvaluasiSimpul clr, string isCreate)
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
    }
}