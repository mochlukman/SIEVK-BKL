using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using SIEVK.Service.Report;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace SIEVK.Service.Controllers
{
    public class ReportController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private GeneralService genSvc = new GeneralService();
        ReportDocument rprt = new ReportDocument();
        static string conString = ConfigurationManager.ConnectionStrings["SIEVKConnection"].ConnectionString;
        static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
        string user = builder.UserID;
        string pass = builder.Password;
        string server = builder.DataSource;
        string databasename = builder.InitialCatalog;

        //public ReportViewer reportViewer { get; set; }
        #endregion

        #region Evaluasi Renstra OPD
        public ActionResult RenstraOPDFormatPermen54()
        {
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                SetUnitKeyDefault();
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();

                return View();
            }
            catch(Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("RenstraOPDFormatPermen54");
            }
        }

        [HttpPost]
        public ActionResult CetakRenstraOPDFormatPermen54(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";
                
                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahap = param["TahapanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarenstraskpd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "report");
                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);//, "Renstra_OPD_Format_Permen54_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");
                //return File(stream, "application/vnd.ms-excel", "Renstra_OPD_Format_Permen54_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RenstraOPDFormatPermen54");
            }
        }

        //ian
        public void Report_RenstraOPDFormatPermen54()
        {
            ReportDocument rd = new ReportDocument();
            var unitkey = "04"; //param["UnitKodeSelected"].ToString();
            var kdtahap = "1";// param["TahapanCombo"].ToString();
            //DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime tgl = DateTime.ParseExact("25/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            rd.Load(Server.MapPath("~/Report/Evarenstraskpd.rpt"));
            OpenConnectionReport(rd);
            rd.SetParameterValue("@KDTAHAP", kdtahap);
            rd.SetParameterValue("@UNITKEY", unitkey);
            rd.SetParameterValue("@Tanggal", tgl);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "report");
        }

        public ActionResult RenstraOPDFormatPenyesuaian()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahapanCombo();
            GenerateTipeLaporanCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRenstraOPDFormatPenyesuaian(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahap = param["TahapanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarenstraskpd_ver.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@TANGGAL", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);//, "Renstra_OPD_Format_Penyesuaian_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo();
                GenerateTipeLaporanCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RenstraOPDFormatPenyesuaian");
            }
        }
        #endregion

        #region Evaluasi RPJMD
        public ActionResult RPJMDFormatPermen54()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahapanCombo("","0");
            GenerateTipeLaporanCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRPJMDFormatPermen54(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahap = param["TahapanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarpjmd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@TANGGAL", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "RPJMD_Format_Permen54_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RPJMDFormatPermen54");
            }
        }

        #endregion

        #region Evaluasi Renja OPD
        public ActionResult RenjaOPDFormatPermen54()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahapanCombo("","0");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRenjaOPDFormatPermen54(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarkpdsasskpd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@TANGGAL", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Renja_OPD_Format_Permen54_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RenjaOPDFormatPermen54");
            }
        }


        public ActionResult RenjaOPDFormatPenyesuaian()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahapanCombo("","0");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRenjaOPDFormatPenyesuaian(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarkpdsasskpd-apbd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@TANGGAL", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Renja_OPD_Format_Penyesuaian_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RenjaOPDFormatPenyesuaian");
            }
        }


        public ActionResult RenjaOPDEvaluasiRenjaSKPDBerdasarkanAPBDPerTriwulan()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahapanCombo("","0");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRenjaOPDEvaluasiRenjaSKPDBerdasarkanAPBDPerTriwulan(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                var kdtrwn = param["TriwulanCombo"].ToString();
                var unitkey = param["UnitKodeSelected"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarkpdsasskpd-apbd-trw.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@KDTRWN", kdtrwn);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "RenjaOPD_Evaluasi_RenjaSKPD_BerdasarkanAPBD_PerTriwulan_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault(); 
                GenerateTahapanCombo("","0");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RenjaOPDEvaluasiRenjaSKPDBerdasarkanAPBDPerTriwulan");
            }
        }

        #endregion

        #region Evaluasi RKPD
        public ActionResult RKPDEvaluasiHasilRKPD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("","1");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRKPDEvaluasiHasilRKPD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarkpdsas.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "RPKD_Evaluasi_Hasil_RKPD" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "1");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                return View("RKPDEvaluasiHasilRKPD");
            }
        }

        public ActionResult RKPDEvaluasiHasilRKPDPerTriwulan()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("", "1");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();
            return View();
        }

        [HttpPost]
        public ActionResult CetakRKPDEvaluasiHasilRKPDPerTriwulan(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                var kdtrwn = param["TriwulanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evarkpdsas_trw.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@KDTRWN", kdtrwn);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "RPKD_Evaluasi_Hasil_RKPD_PerTriwulan" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "1");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                return View("RKPDEvaluasiHasilRKPDPerTriwulan");
            }
        }

        #endregion

        #region Evaluasi APBD
        public ActionResult APBDEvaluasiPerProgram()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("","2");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();
            return View();
        }

        [HttpPost]
        public ActionResult CetakAPBDEvaluasiPerProgram(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                var kdtrwn = param["TriwulanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evakinkeu.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@KDTRWN", kdtrwn);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "APBD_Evaluasi_Kinerja_Keuangan_PerProgram_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "2");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                return View("APBDEvaluasiPerProgram");
            }
        }

        public ActionResult APBDEvaluasiPerKegiatan()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("", "2");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();

            //semenetara
            //reportViewer = new ReportViewer();
            //reportViewer.ProcessingMode = ProcessingMode.Local;
            //reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(100);
            //reportViewer.Height = Unit.Percentage(100);

            //return View(reportViewer);
            return View();
        }

        [HttpPost]
        public ActionResult CetakAPBDEvaluasiPerKegiatan(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtahun = param["TahunCombo"].ToString();
                var kdtrwn = param["TriwulanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/Evakinkeu.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@KDTRWN", kdtrwn);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/"+ tipeLaporan);//, "APBD_Evaluasi_Kinerja_Keuangan_PerKegiatan_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "2");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                return View("APBDEvaluasiPerKegiatan");
            }
        }

        //ian
        [HttpPost]
        public ActionResult ViewAPBDEvaluasiPerKegiatan(FormCollection param)
        {
            try
            {
                //reportViewer = new ReportViewer();
                //reportViewer.ProcessingMode = ProcessingMode.Local;
                //reportViewer.SizeToReportContent = true;
                //reportViewer.Width = Unit.Percentage(100);
                //reportViewer.Height = Unit.Percentage(100);

                //var reportPath = "~/Report/Evakinkeu.rpt";
                ////var kdtahap = param["TahapanCombo"].ToString();
                ////var kdtahun = param["TahunCombo"].ToString();
                ////var kdtrwn = param["TriwulanCombo"].ToString();
                ////DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //var kdtahap = "21";
                //var kdtahun = "2";
                //var kdtrwn = "2";
                ////DateTime tgl = DateTime.ParseExact(DateTime.Now.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //ViewBag.ReportPath = reportPath;
                //ViewBag.kdtahap = kdtahap;
                //ViewBag.kdtahun = kdtahun;
                //ViewBag.kdtrwn = kdtrwn;
                ////ViewBag.tgl = tgl;
                ////return Redirect(Server.MapPath("ReportViewer.aspx"));
                ////return Redirect(Server.MapPath("ReportViewer.aspx?ReportPath=" + reportPath + "&kdtahap=" + kdtahap + "&kdtahun=" + kdtahun + "&kdtrwn=" + kdtrwn + "&tgl=" + tgl.ToString()));
                ////return Redirect("../ReportViewer2/ReportViewer.aspx");
                ////return RedirectToAction("ReportAPBDEvaluasiPerKegiatan");

                ////DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //DateTime tgl = DateTime.ParseExact("25/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //DataSet ds = new DataSet("DataSet1");

                //using (SqlConnection connection = new SqlConnection())
                //{

                //    connection.ConnectionString = conString;

                //    SqlCommand cmd = new SqlCommand("WSPR_EVAKINKEU", connection);

                //    cmd.Parameters.AddWithValue("@KDTAHAP", kdtahap);
                //    cmd.Parameters.AddWithValue("@kdtahun", kdtahun);
                //    cmd.Parameters.AddWithValue("@kdtrwn", kdtrwn);
                //    //cmd.Parameters.AddWithValue("@Tanggal", tgl);
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    SqlDataAdapter da = new SqlDataAdapter();
                //    da.SelectCommand = cmd;
                //    da.Fill(ds);
                //}

                //ReportDataSource source = new ReportDataSource("Evakinkeu", ds.Tables[0]);
                //reportViewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(@"Evakinkeu.rpt");
                //reportViewer.LocalReport.DataSources.Clear();
                //reportViewer.LocalReport.DataSources.Add(source);

                //return RedirectToAction("Report_RenstraOPDFormatPermen54");
                //return View(reportViewer);
                return Redirect("~/ReportViewer/ViewAPBDEvaluasiPerKegiatan2.aspx");
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "2");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                return View("APBDEvaluasiPerKegiatan");
            }
        }

        //ian
        //public void ReportAPBDEvaluasiPerKegiatan()
        //{
        //    reportViewer = new ReportViewer();
        //    reportViewer.ProcessingMode = ProcessingMode.Local;
        //    reportViewer.SizeToReportContent = true;
        //    reportViewer.Width = Unit.Percentage(100);
        //    reportViewer.Height = Unit.Percentage(100);

        //    var reportPath = "~/Report/Evakinkeu.rpt";
        //    //var kdtahap = param["TahapanCombo"].ToString();
        //    //var kdtahun = param["TahunCombo"].ToString();
        //    //var kdtrwn = param["TriwulanCombo"].ToString();
        //    //DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    var kdtahap = "21";
        //    var kdtahun = "2";
        //    var kdtrwn = "2";
        //    //DateTime tgl = DateTime.ParseExact(DateTime.Now.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    ViewBag.ReportPath = reportPath;
        //    ViewBag.kdtahap = kdtahap;
        //    ViewBag.kdtahun = kdtahun;
        //    ViewBag.kdtrwn = kdtrwn;
        //    //ViewBag.tgl = tgl;
        //    //return Redirect(Server.MapPath("ReportViewer.aspx"));
        //    //return Redirect(Server.MapPath("ReportViewer.aspx?ReportPath=" + reportPath + "&kdtahap=" + kdtahap + "&kdtahun=" + kdtahun + "&kdtrwn=" + kdtrwn + "&tgl=" + tgl.ToString()));
        //    //return Redirect("../ReportViewer2/ReportViewer.aspx");
        //    //return RedirectToAction("ReportAPBDEvaluasiPerKegiatan");

        //    //DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    DateTime tgl = DateTime.ParseExact("25/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    DataSet ds = new DataSet("DataSet1");

        //    using (SqlConnection connection = new SqlConnection())
        //    {

        //        connection.ConnectionString = conString;

        //        SqlCommand cmd = new SqlCommand("WSPR_EVAKINKEU", connection);

        //        cmd.Parameters.AddWithValue("@KDTAHAP", kdtahap);
        //        cmd.Parameters.AddWithValue("@kdtahun", kdtahun);
        //        cmd.Parameters.AddWithValue("@kdtrwn", kdtrwn);
        //        //cmd.Parameters.AddWithValue("@Tanggal", tgl);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //    }

        //    ReportDataSource source = new ReportDataSource("Evakinkeu", ds.Tables[0]);
        //    reportViewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(@"Report\Evakinkeu.rpt");
        //    reportViewer.LocalReport.DataSources.Clear();
        //    reportViewer.LocalReport.DataSources.Add(source);
        //}

        public ActionResult APBDRekapitulasiFisikKeuangan()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("", "2");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();
            return View();
        }

        [HttpPost]
        public ActionResult CetakAPBDRekapitulasiFisikKeuangan(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahap = param["TahapanCombo"].ToString();
                var kdtrwn = param["TriwulanCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/RekapRealMonev.rpt"));
                OpenConnectionReport(rd);

                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@KDTRWN", kdtrwn);
                rd.SetParameterValue("@Tanggal", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "APBD_Rekapitulasi_Fisik_Keuangan_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTahapanCombo("", "2");
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                GenerateTriwulanCombo();
                return View("APBDRekapitulasiFisikKeuangan");
            }
        }

        public ActionResult PemetaanProgramKegiatan()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("", "2");
            GenerateTipeLaporanCombo();
            GenerateTahunCombo();
            GenerateTriwulanCombo();
            return View();
        }

        [HttpPost]
        public ActionResult CetakPemetaanProgramKegiatan(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahun = param["TahunCombo"].ToString();
                DateTime tgl = DateTime.ParseExact(param["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rd.Load(Server.MapPath("~/Report/MapProkeg050.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@tgl", tgl);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "APBD_Rekapitulasi_Fisik_Keuangan_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                GenerateTipeLaporanCombo();
                GenerateTahunCombo();
                return View("PemetaanProgramKegiatan");
            }
        }

        #endregion

        #region Grafik OPD
        public ActionResult GrafikOPDFormatPermen54()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahapanCombo("", "0");
            GenerateTipeLaporanCombo();
            GenerateTriwulanCombo();
            return View();
        }

        #endregion

        #region Evaluasi Pengendalian dan Kebijakan
        public ActionResult EvaBijakRPJPD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaBijakRPJPD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";


                rd.Load(Server.MapPath("~/Report/Evabijakrpjpd.rpt"));
                OpenConnectionReport(rd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_RPJPD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RPJPDEvaBijak");
            }
        }

        public ActionResult EvaSimpulRPJPD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaSimpulRPJPD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                rd.Load(Server.MapPath("~/Report/Evasimpulrpjpd.rpt"));
                OpenConnectionReport(rd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Kesimpulan_Pengendalian_Evaluasi_RPJPD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RPJPDEvaSimpul");
            }
        }

        public ActionResult EvaBijakRPJMD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaBijakRPJMD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                rd.Load(Server.MapPath("~/Report/Evabijakrpjmd.rpt"));
                OpenConnectionReport(rd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_RPJMD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RPJMDEvaBijak");
            }
        }

        public ActionResult EvaSimpulRPJMD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaSimpulRPJMD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                rd.Load(Server.MapPath("~/Report/Evasimpulrpjmd.rpt"));
                OpenConnectionReport(rd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Kesimpulan_Pengendalian_Evaluasi_RPJMD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RPJMDEvaSimpul");
            }
        }

        public ActionResult EvaBijakRENSTRA()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaBijakRENSTRA(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();

                rd.Load(Server.MapPath("~/Report/Evabijakrenstra.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@UNITKEY", unitkey);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_Renstra_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RENSTRAEvaBijak");
            }
        }

        public ActionResult EvaSimpulRENSTRA()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaSimpulRENSTRA(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();

                rd.Load(Server.MapPath("~/Report/Evasimpulrenstra.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@UNITKEY", unitkey);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Kesimpulan_Pengendalian_Evaluasi_Renstra_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RENSTRAEvaSimpul");
            }
        }

        public ActionResult EvaBijakRKPD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaBijakRKPD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahun = param["TahunCombo"].ToString();

                rd.Load(Server.MapPath("~/Report/Evabijakrkpd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_RKPD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RKPDEvaBijak");
            }
        }

        public ActionResult EvaSimpulRKPD()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaSimpulRKPD(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var kdtahun = param["TahunCombo"].ToString();

                rd.Load(Server.MapPath("~/Report/Evasimpulrkpd.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Kesimpulan_Pengendalian_Evaluasi_RKPD_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RKPDEvaSimpul");
            }
        }

        public ActionResult EvaBijakRENJA()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaBijakRENJA(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahun = param["TahunCombo"].ToString();

                rd.Load(Server.MapPath("~/Report/Evabijakrenja.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@UNITKEY", unitkey);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_Renja_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RENJAEvaBijak");
            }
        }

        public ActionResult EvaSimpulRENJA()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaSimpulRENJA(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahun = param["TahunCombo"].ToString();

                rd.Load(Server.MapPath("~/Report/Evasimpulrenja.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@UNITKEY", unitkey);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Kesimpulan_Pengendalian_Evaluasi_Renja_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RENJAEvaSimpul");
            }
        }

        public ActionResult EvaKendaliRENJA()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            SetUnitKeyDefault();
            GenerateTahunCombo();
            ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
            return View();
        }

        [HttpPost]
        public ActionResult CetakEvaKendaliRENJA(FormCollection param)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var tipeLaporan = param["TipeLaporanCombo"].ToString();
                var _currTipeLaporan = "pdf";

                var unitkey = param["UnitKodeSelected"].ToString();
                var kdtahun = param["TahunCombo"].ToString();

                rd.Load(Server.MapPath("~/Report/Evakendalirenja.rpt"));
                OpenConnectionReport(rd);
                rd.SetParameterValue("@KDTAHUN", kdtahun);
                rd.SetParameterValue("@UNITKEY", unitkey);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = null;
                if (tipeLaporan.ToLower() == "pdf")
                {
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (tipeLaporan.ToLower() == "excel")
                {
                    _currTipeLaporan = "vnd.ms-excel";
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                }
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + _currTipeLaporan);
                //return File(stream, "application/pdf");//, "Pengendalian_Evaluasi_Pelaksanaan_Renja_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                SetUnitKeyDefault();
                GenerateTahunCombo();
                ViewBag.HasUnitKey = new SecurityService().HasUnitKey();
                return View("RENJAEvaKendali");
            }
        }

        #endregion

        #region Private function
        private void OpenConnectionReport(ReportDocument rd)
        {
            rd.SetDatabaseLogon(user, pass, server, databasename);
            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo.UserID = user;
            connectionInfo.Password = pass;
            connectionInfo.ServerName = server;
            connectionInfo.DatabaseName = databasename;
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in rd.Database.Tables)
            {
                table.LogOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(table.LogOnInfo);
            }
        }

        private bool GenerateTahapanCombo(string selected_id = "", string key = "0")
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

        private bool GenerateTipeLaporanCombo(string selected_id = "")
        {
            if (selected_id == "0" || selected_id == "")
            {
                selected_id = "PDF";
            }
            else
            {
                selected_id = "EXCEL";
            }
            var dict = new Dictionary<string, string>();
            dict.Add("PDF", "PDF");
            dict.Add("EXCEL", "EXCEL");
            ViewBag.TipeLaporanCombo = new SelectList(dict, "Key", "Value", selected_id);
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
            ViewBag.TahunCombo = new SelectList(dict, "Key", "Value", selected_id.Trim());
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

        public ActionResult GetUnitOrganization()
        {
            GeneralService gsvc = new GeneralService();
            var list = gsvc.GetUnitOrganization();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public void SetUnitKeyDefault()
        {
            String kdUnit = System.Web.HttpContext.Current.Session["KdUnit"].ToString();
            String nmUnit = System.Web.HttpContext.Current.Session["NmUnit"].ToString();
             String unitKey = System.Web.HttpContext.Current.Session["UnitKey"].ToString();

             if (kdUnit != "" && nmUnit != "")
                {
                    ViewBag.unitOrgName = kdUnit + " - " + nmUnit;
                    ViewBag.unitKey = unitKey;
                }
        }


        #endregion

    }
}