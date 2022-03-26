using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RPJPD;
using SIEVK.BusinessData.RPJPD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RPJPD
{
    public class RPJPDEvaluasiSimpulService
    {
        Mapper map = new Mapper();
        RPJPDEvaluasiSimpulDAO dao = new RPJPDEvaluasiSimpulDAO();

        public List<RPJPDEvaluasiSimpul> GetEvaluasiSimpulList()
        {
            DataTable dt = dao.GetEvaluasiSimpul();
            List<RPJPDEvaluasiSimpul> lst = map.BindDataList<RPJPDEvaluasiSimpul>(dt);
            return lst;
        }

        public RPJPDEvaluasiSimpul GetEvaluasiSimpul(string nomor)
        {
            DataTable dt = dao.GetEvaluasiSimpul(nomor);
            RPJPDEvaluasiSimpul obj = map.BindData<RPJPDEvaluasiSimpul>(dt);
            return obj;
        }

        public void DeleteData(string nomor)
        {
            dao.Delete(nomor);
        }

        public void InsertData(RPJPDEvaluasiSimpul obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RPJPDEvaluasiSimpul obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RPJPDEvaluasiSimpul clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiSimpul(clr.NOMOR);
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
                    if (dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
