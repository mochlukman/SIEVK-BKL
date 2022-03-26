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
    public class RPJPDEvaluasiBijakService
    {
        Mapper map = new Mapper();
        RPJPDEvaluasiBijakDAO dao = new RPJPDEvaluasiBijakDAO();

        public List<RPJPDEvaluasiBijak> GetEvaluasiBijakList()
        {
            DataTable dt = dao.GetEvaluasiBijak();
            List<RPJPDEvaluasiBijak> lst = map.BindDataList<RPJPDEvaluasiBijak>(dt);
            return lst;
        }

        public RPJPDEvaluasiBijak GetEvaluasiBijak(string nomor)
        {
            DataTable dt = dao.GetEvaluasiBijak(nomor);
            RPJPDEvaluasiBijak obj = map.BindData<RPJPDEvaluasiBijak>(dt);
            return obj;
        }

        public void DeleteData(string nomor)
        {
            dao.Delete(nomor);
        }

        public void InsertData(RPJPDEvaluasiBijak obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RPJPDEvaluasiBijak obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RPJPDEvaluasiBijak clr, bool isCreate)
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
