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
    public class RPJMDEvaluasiBijakService
    {
        Mapper map = new Mapper();
        RPJMDEvaluasiBijakDAO dao = new RPJMDEvaluasiBijakDAO();

        public List<RPJMDEvaluasiBijak> GetEvaluasiBijakList()
        {
            DataTable dt = dao.GetEvaluasiBijak();
            List<RPJMDEvaluasiBijak> lst = map.BindDataList<RPJMDEvaluasiBijak>(dt);
            return lst;
        }

        public RPJMDEvaluasiBijak GetEvaluasiBijak(string nomor)
        {
            DataTable dt = dao.GetEvaluasiBijak(nomor);
            RPJMDEvaluasiBijak obj = map.BindData<RPJMDEvaluasiBijak>(dt);
            return obj;
        }

        public void DeleteData(string nomor)
        {
            dao.Delete(nomor);
        }

        public void InsertData(RPJMDEvaluasiBijak obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RPJMDEvaluasiBijak obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RPJMDEvaluasiBijak clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiBijak(clr.NOMOR);
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
