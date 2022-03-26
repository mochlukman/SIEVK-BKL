using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RKPD;
using SIEVK.BusinessData.RKPD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RKPD
{
    public class RKPDEvaluasiSimpulService
    {
        Mapper map = new Mapper();
        RKPDEvaluasiSimpulDAO dao = new RKPDEvaluasiSimpulDAO();

        public List<RKPDEvaluasiSimpul> GetEvaluasiSimpulList(string kdTahun)
        {
            DataTable dt = dao.GetEvaluasiSimpul(kdTahun);
            List<RKPDEvaluasiSimpul> lst = map.BindDataList<RKPDEvaluasiSimpul>(dt);
            return lst;
        }

        public RKPDEvaluasiSimpul GetEvaluasiSimpul(string kdTahun, string nomor)
        {
            DataTable dt = dao.GetEvaluasiSimpul(kdTahun, nomor);
            RKPDEvaluasiSimpul obj = map.BindData<RKPDEvaluasiSimpul>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string nomor)
        {
            dao.Delete(kdTahun, nomor);
        }

        public void InsertData(RKPDEvaluasiSimpul obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RKPDEvaluasiSimpul obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RKPDEvaluasiSimpul clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiSimpul(clr.KDTAHUN, clr.NOMOR);
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
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
