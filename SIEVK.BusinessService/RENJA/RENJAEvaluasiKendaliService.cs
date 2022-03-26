using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RENJA;
using SIEVK.BusinessData.RENJA;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RENJA
{
    public class RENJAEvaluasiKendaliService
    {
        Mapper map = new Mapper();
        RENJAEvaluasiKendaliDAO dao = new RENJAEvaluasiKendaliDAO();

        public List<RENJAEvaluasiKendali> GetEvaluasiKendaliList(string kdTahun, string kdUnit, string kdTahap)
        {
            DataTable dt = dao.GetEvaluasiKendali(kdTahun, kdUnit, kdTahap);
            List<RENJAEvaluasiKendali> lst = map.BindDataList<RENJAEvaluasiKendali>(dt);
            return lst;
        }

        public RENJAEvaluasiKendali GetEvaluasiKendali(string kdTahun, string kdUnit, string nomor, string kdTahap)
        {
            DataTable dt = dao.GetEvaluasiKendali(kdTahun, kdUnit, nomor, kdTahap);
            RENJAEvaluasiKendali obj = map.BindData<RENJAEvaluasiKendali>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string kdUnit, string nomor, string kdTahap)
        {
            dao.Delete(kdTahun, kdUnit, nomor, kdTahap);
        }

        public void InsertData(RENJAEvaluasiKendali obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RENJAEvaluasiKendali obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RENJAEvaluasiKendali clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiKendali(clr.KDTAHUN, clr.UNITKEY, clr.NOMOR);
            if (isCreate)
            {
                if (dt.Rows.Count > 0)
                {
                    dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["UNITKEY"].ToString().Trim() != clr.UNITKEY.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim() || dt.Rows[0]["KDTAHAP"].ToString().Trim() != clr.KDTAHAP.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
