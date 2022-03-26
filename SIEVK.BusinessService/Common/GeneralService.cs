using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using SIEVK.BusinessData.Common;
using System.Web;
using System.Resources;
using System.Data;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Common
{
    public class GeneralService
    {
        GeneralDAO dao = new GeneralDAO();
        Mapper map = new Mapper();

        public void InsertErrorLog(Exception ex, string userName)
        {
            GeneralDAO l = new GeneralDAO();
            ErrorLog error= new ErrorLog();
            error.ErrorMessage = ex.Message;
            error.ExceptionStackTrace = ex.StackTrace;
            error.CreatedBy = userName;
            if (ex.InnerException != null)
            {
                error.InnerException = ex.InnerException.ToString();
            }
            l.InsertError(error);
        }

        public List<DaftUnit> GetUnitOrganization(string unitKey = null, string kdLevel = null)
        {
            DataTable dt = dao.GetUnitOrganization(unitKey, kdLevel);
            List<DaftUnit> lst = map.BindDataList<DaftUnit>(dt);
            return lst;
        }

        public List<Tahapan> GetTahapan(string key)
        {
            DataTable dt = dao.GetTahapan(key);
            List<Tahapan> lst = map.BindDataList<Tahapan>(dt);
            return lst;
        }

        public List<TahunAnggaran> GetTahunAnggaran()
        {
            DataTable dt = dao.GetTahunAnggaran();
            List<TahunAnggaran> lst = map.BindDataList<TahunAnggaran>(dt);
            return lst;
        }

        public List<Program> GetProgramRPJMD(string unitkey, string kdtahapan, string idprgrm = null)
        {
            DataTable dt = dao.GetProgramRPJMD(unitkey, kdtahapan, idprgrm);
            List<Program> lst = map.BindDataList<Program>(dt);
            return lst;
        }

        public List<Triwulan> GetTriwulan()
        {
            DataTable dt = dao.GetTriwulan();
            List<Triwulan> lst = map.BindDataList<Triwulan>(dt);
            return lst;
        }
        
        public List<Program> GetProgramRKPD(string unitKey, string IDPrgrm = null)
        {
            DataTable dt = dao.GetProgramRKPD(unitKey, IDPrgrm);
            List<Program> lst = map.BindDataList<Program>(dt);
            return lst;
        }

        public List<Program> GetProgramAPBD(string unitKey, string kdTahun, string idPrgrm = null, string kdTahap=null)
        {
            DataTable dt = dao.GetProgramAPBD(unitKey, kdTahun, idPrgrm, kdTahap);
            List<Program> lst = map.BindDataList<Program>(dt);
            return lst;
        }

        public List<JFAKTOR> GetJFaktor()
        {
            DataTable dt = dao.GetJFaktor();
            List<JFAKTOR> lst = map.BindDataList<JFAKTOR>(dt);
            return lst;
        }
    }
}
