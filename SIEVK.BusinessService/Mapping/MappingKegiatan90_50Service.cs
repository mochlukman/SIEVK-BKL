using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.Mapping;
using SIEVK.BusinessData.Mapping;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Mapping
{
    public class MappingKegiatan90_50Service
    {
        Mapper map = new Mapper();
        MappingKegiatan90_50DAO dao = new MappingKegiatan90_50DAO();

        public List<MappingKegiatan90_50> GetKegiatanMappingList(string idKEG90)
        {
            DataTable dt = dao.GetKegiatanMappingList(idKEG90);
            List<MappingKegiatan90_50> lst = map.BindDataList<MappingKegiatan90_50>(dt);
            return lst;
        }


        public MappingKegiatan90_50 GetKegiatanMappingByID(string idKEG90, string idKEG050)
        {
            DataTable dt = dao.GetKegiatanMappingByID(idKEG90, idKEG050);
            MappingKegiatan90_50 MappingKegiatan90_50 = map.BindData<MappingKegiatan90_50>(dt);
            return MappingKegiatan90_50;
        }

        public void Delete(string idKEG90, string idKEG050)
        {
            dao.Delete(idKEG90, idKEG050);
        }

        public void Update(MappingKegiatan90_50 kegMapping)
        {
            dao.Update(kegMapping);
        }

        public void Create(MappingKegiatan90_50 kegMapping)
        {
            dao.Create(kegMapping);
        }
    }
}
