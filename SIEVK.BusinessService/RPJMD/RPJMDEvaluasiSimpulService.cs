using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RPJMD;
using SIEVK.BusinessData.RPJMD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RPJMD
{
    public class RPJMDEvaluasiSimpulService
    {
        Mapper map = new Mapper();
        RPJMDEvaluasiSimpulDAO dao = new RPJMDEvaluasiSimpulDAO();

        public List<RPJMDEvaluasiSimpul> GetEvaluasiSimpulList()
        {
            DataTable dt = dao.GetEvaluasiSimpul();
            List<RPJMDEvaluasiSimpul> lst = map.BindDataList<RPJMDEvaluasiSimpul>(dt);
            return lst;
        }

        public RPJMDEvaluasiSimpul GetEvaluasiSimpul(string nomor)
        {
            DataTable dt = dao.GetEvaluasiSimpul(nomor);
            RPJMDEvaluasiSimpul obj = map.BindData<RPJMDEvaluasiSimpul>(dt);
            return obj;
        }

        public void DeleteData(string nomor)
        {
            dao.Delete(nomor);
        }

        public void InsertData(RPJMDEvaluasiSimpul obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RPJMDEvaluasiSimpul obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RPJMDEvaluasiSimpul clr, bool isCreate)
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
