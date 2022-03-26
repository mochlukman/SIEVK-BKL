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
    public class RPJMDCatatanLaporanEvaluasiService
    {
        Mapper map = new Mapper();
        RPJMDCatatanLaporanEvaluasiDAO dao = new RPJMDCatatanLaporanEvaluasiDAO();

        public List<RPJMDCatatanLaporanEvaluasi> GetCatatanLaporanEvaluasiList(String kdtahun, String noctt = null)
        {
            DataTable dt = dao.GetCatatanLaporanEvaluasi(kdtahun, noctt);
            List<RPJMDCatatanLaporanEvaluasi> lst = map.BindDataList<RPJMDCatatanLaporanEvaluasi>(dt);
            return lst;
        }

        public RPJMDCatatanLaporanEvaluasi GetCatatanLaporanEvaluasi(String kdtahun, String noctt = null)
        {
            DataTable dt = dao.GetCatatanLaporanEvaluasi(kdtahun, noctt);
            RPJMDCatatanLaporanEvaluasi lst = map.BindData<RPJMDCatatanLaporanEvaluasi>(dt);
            return lst;
        }

        public void DeleteData(String kdtahun, String noctt)
        {
            dao.Delete(kdtahun, noctt);
        }

        public void UpdateData(RPJMDCatatanLaporanEvaluasi cat)
        {
            dao.Update(cat);
        }

        public void InsertData(RPJMDCatatanLaporanEvaluasi cat)
        {
            dao.Insert(cat);
        }

        public Dictionary<string, string> ValidateData(RPJMDCatatanLaporanEvaluasi catatan, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(catatan.KDTAHUN.ToString()))
            {
                dic.Add("ErrUserGroupID", "Kode Tahun harus diisi");
            }

            if (String.IsNullOrEmpty(catatan.NOCTT))
            {
                dic.Add("ErrUserName", "No Catatan harus diisi");
            }

            if (String.IsNullOrEmpty(catatan.NMCTT))
            {
                dic.Add("ErrFirstName", "Nama Catatan Harus diisi");
            }

            if (String.IsNullOrEmpty(catatan.ISICTT))
            {
                dic.Add("ErrStatusActive", "Isi catatan harus diisi");
            }

            if (isCreate)
            {
                DataTable dt = dao.GetCatatanLaporanEvaluasi(catatan.KDTAHUN, catatan.NOCTT);
                if (dt.Rows.Count > 0)
                {
                    dic.Add("ErrUserNameExist", "No Catatan sudah ada");
                }
            }

            return dic;
        }
    }
}
