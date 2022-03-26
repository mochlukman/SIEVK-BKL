using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RENSTRA;
using SIEVK.BusinessData.RENSTRA;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RENSTRA
{
    public class RENSTRAEvaluasiBijakService
    {
        Mapper map = new Mapper();
        RENSTRAEvaluasiBijakDAO dao = new RENSTRAEvaluasiBijakDAO();

        public List<RENSTRAEvaluasiBijak> GetEvaluasiBijakList(string kdUnit)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdUnit);
            List<RENSTRAEvaluasiBijak> lst = map.BindDataList<RENSTRAEvaluasiBijak>(dt);
            return lst;
        }

        public RENSTRAEvaluasiBijak GetEvaluasiBijak(string kdUnit, string nomor)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdUnit, nomor);
            RENSTRAEvaluasiBijak obj = map.BindData<RENSTRAEvaluasiBijak>(dt);
            return obj;
        }

        public void DeleteData(string kdUnit, string nomor)
        {
            dao.Delete(kdUnit, nomor);
        }

        public void InsertData(RENSTRAEvaluasiBijak obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RENSTRAEvaluasiBijak obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RENSTRAEvaluasiBijak clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiBijak(clr.UNITKEY, clr.NOMOR);
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
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.UNITKEY.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
