using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using SIEVK.BusinessData.Common;
using SIEVK.BusinessData;
using System.Data;

namespace SIEVK.BusinessService.Common
{
    public class PegawaiService
    {
        Mapper map = new Mapper();
        PegawaiDAO dao = new PegawaiDAO();

        public Pegawai GetPegawai (String nip)
        {
            DataTable dt = dao.GetList(nip);
            Pegawai usr = null;
            if (dt.Rows.Count > 0)
            {
                usr = map.BindData<Pegawai>(dt);
            }
            return usr;
        }

        public List<Pegawai> GetAllPegawai()
        {
            DataTable dt = dao.GetList();
            List<Pegawai> lst = map.BindDataList<Pegawai>(dt);
            return lst;
        }

        public void UpdateDataPegawai(Pegawai user)
        {
            dao.Update(user);
        }

        public void InsertDataPegawai(Pegawai user)
        {
            
            dao.Insert(user);
        }

        public void DeleteDataPegawai(Pegawai user)
        {
            dao.Delete(user);
        }

        public Dictionary<string, string> ValidateData(Pegawai pegawai, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(pegawai.KDGOL.ToString()))
            {
                dic.Add("ErrKDGOL", "Golongan harus diisi");
            }

            if (String.IsNullOrEmpty(pegawai.NAMA))
            {
                dic.Add("ErrName", "Nama harus diisi");
            }

            if (isCreate)
            {
                DataTable dt = dao.GetList(pegawai.NAMA);
                if (dt.Rows.Count> 0)
                {
                    dic.Add("ErrNameExist", "User Name sudah ada");
                }
            }

            return dic;
        }

    }
}
