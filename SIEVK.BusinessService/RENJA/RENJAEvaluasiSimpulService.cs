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
    public class RENJAEvaluasiSimpulService
    {
        Mapper map = new Mapper();
        RENJAEvaluasiSimpulDAO dao = new RENJAEvaluasiSimpulDAO();

        public List<RENJAEvaluasiSimpul> GetEvaluasiSimpulList(string kdTahun, string kdUnit)
        {
            DataTable dt = dao.GetEvaluasiSimpul(kdTahun, kdUnit);
            List<RENJAEvaluasiSimpul> lst = map.BindDataList<RENJAEvaluasiSimpul>(dt);
            return lst;
        }

        public RENJAEvaluasiSimpul GetEvaluasiSimpul(string kdTahun, string kdUnit, string nomor)
        {
            DataTable dt = dao.GetEvaluasiSimpul(kdTahun, kdUnit, nomor);
            RENJAEvaluasiSimpul obj = map.BindData<RENJAEvaluasiSimpul>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string kdUnit, string nomor)
        {
            dao.Delete(kdTahun, kdUnit, nomor);
        }

        public void InsertData(RENJAEvaluasiSimpul obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RENJAEvaluasiSimpul obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RENJAEvaluasiSimpul clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiSimpul(clr.KDTAHUN, clr.UNITKEY, clr.NOMOR);
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
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["UNITKEY"].ToString().Trim() != clr.UNITKEY.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
