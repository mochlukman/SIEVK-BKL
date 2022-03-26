using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIEVK.BusinessService.Common;
using SIEVK.Domain.Common;
using System.Resources;

namespace SIEVK.Service.Controllers
{
    public class PegawaiController : Controller
    {
        #region private Variable Declaration
        private SecurityService secSvc = new SecurityService();
        private PegawaiService uSvc = new PegawaiService();
        private GeneralService genSvc = new GeneralService();
        #endregion

        public ActionResult LoadDataPegawai()
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
                List<Pegawai> pegawaiData = uSvc.GetAllPegawai();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "ID")
                {
                    pegawaiData = pegawaiData.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    pegawaiData = pegawaiData.Where(m => m.NAMA.ToLower().Contains(searchValue) ||
                        m.NAMA.ToLower().Contains(searchValue)
                        ).ToList();
                }

                //total number of rows count     
                recordsTotal = pegawaiData.Count();
                //Paging     
                var data = pegawaiData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return View("Index");
            }
        }

        public ActionResult Index(String msg = "")
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            ViewBag.Info = msg;
            return View();   
        }

        public ActionResult Edit(String nip)
        {
            Pegawai pegawai = new Pegawai();
            try
            {
                if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
                if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

                pegawai = uSvc.GetPegawai(nip);
                return CreateInputView(pegawai);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(pegawai);
            }
        }

        public ActionResult Detail(string nip)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            //Pegawai pegawai = new Pegawai();
            try
            {
                Pegawai pegawai = uSvc.GetPegawai(nip);
                return View(pegawai);
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return RedirectToAction("Index");
            }
        }

        private string GetValueFromResource(String key)
        {
            ResourceManager rm = new ResourceManager("SIEVK.Service.Properties.Resources", this.GetType().Assembly);
            return rm.GetString(key);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Pegawai user = new Pegawai();
            try
            {
                user = BindDataToObject(param, false);
                if (ValidateData(user, false))
                {
                    uSvc.UpdateDataPegawai(user);
                    return RedirectToAction("Index", "Pegawai", new { msg = GetValueFromResource("SUCCESS_EDIT") });
                }
                else
                {
                    return CreateInputView(user);
                }
            }
            catch(Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(user);
            }
        }

        
        public ActionResult Create()
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Pegawai user = new Pegawai();
            return CreateInputView(user);
        }

        [HttpPost]
        public ActionResult Create(FormCollection param)
        {
            if (!secSvc.IsLogin()) { return RedirectToAction("SessionEnd", "Login"); }
            if (!secSvc.CekPrivilege(this)) { return RedirectToAction("AccessDenied", "General"); }

            Pegawai pegawai = new Pegawai();
            try
            {
                pegawai = BindDataToObject(param, true);
                if (ValidateData(pegawai, true))
                {
                    uSvc.InsertDataPegawai(pegawai);
                    return RedirectToAction("Index", "Pegawai", new { msg = GetValueFromResource("SUCCESS_INPUT") });
                }
                else
                {
                    return CreateInputView(pegawai);
                }
            }
            catch (Exception ex)
            {
                new GeneralController().Error(ex);
                ModelState.AddModelError("ErrException", ex.Message);
                return CreateInputView(pegawai);
            }
        }

        public bool ValidateData(Pegawai user, bool isCreate)
        {
            Dictionary<string, string> dic = uSvc.ValidateData(user, isCreate);
            foreach(KeyValuePair<string, string> entry in dic)
            {
                ModelState.AddModelError(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            if (dic.Count > 0) { return false; }
            else { return true; }
        }

        private ActionResult CreateInputView(Pegawai pegawai)
        {
            GenerateGolonganCombo(pegawai.KDGOL);
            GenerateUnitCombo(pegawai.UnitKey);
            
            if (String.IsNullOrEmpty(pegawai.NIP))
            {
                ViewBag.isEdit = false;
            }
            else
            {
                ViewBag.isEdit = true;
            }
            return View("Input", pegawai);
        }

        private bool GenerateGolonganCombo(string selected_id = "")
        {
            if (selected_id == "0")
            {
                selected_id = "";
            }
            GolonganService svc = new GolonganService();
            List<Golongan> list = svc.GetAllGolongan();
            list = list.OrderBy(x => x.PANGKAT).ToList();
            ViewBag.GolonganCombo = new SelectList(list, "KDGOL", "PANGKAT", selected_id);
            return list.Count > 0;
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
            return list.Count > 0;
        }

         
        private Pegawai BindDataToObject(FormCollection param, bool isCreate)
        {
            String pegawainame = secSvc.GetUserLogin();
            Pegawai pegawai = new Pegawai();
            pegawai.UnitKey = param["UnitCombo"].ToString();
            pegawai.KDGOL = param["GolonganCombo"].ToString();
            //pegawai.PANGKAT = param["PANGKAT"].ToString();
            pegawai.NAMA = param["NAMA"].ToString();
            pegawai.ALAMAT = param["ALAMAT"].ToString();
            pegawai.JABATAN = param["JABATAN"].ToString();
            pegawai.PDDK = param["PDDK"].ToString();

            //if (isCreate)
            //{
            pegawai.NIP = param["NIP"].ToString();
            //}
            return pegawai;
        }

        [HttpPost]
        public JsonResult Delete(string nip)
        {
            try
            {
                Pegawai pegawai = new Pegawai();
                pegawai.NIP = nip;

                uSvc.DeleteDataPegawai(pegawai);

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(data: ex.Message, behavior: JsonRequestBehavior.AllowGet);
            }
        }

    }
}