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
    public class MappingKegiatan13_90Service
    {
        Mapper map = new Mapper();
        MappingKegiatan13_90DAO dao = new MappingKegiatan13_90DAO();

        public List<MappingKegiatan13_90> GetKegiatanMappingList(string idKeg)
        {
            DataTable dt = dao.GetKegiatanMappingList(idKeg);
            List<MappingKegiatan13_90> lst = map.BindDataList<MappingKegiatan13_90>(dt);
            return lst;
        }


        public MappingKegiatan13_90 GetKegiatanMappingByID(string idKeg, string idKEG90)
        {
            DataTable dt = dao.GetKegiatanMappingByID(idKeg, idKEG90);
            MappingKegiatan13_90 mappingKegiatan13_90 = map.BindData<MappingKegiatan13_90>(dt);
            return mappingKegiatan13_90;
        }

        public void Delete(string idKeg, string idKEG90)
        {
            dao.Delete(idKeg, idKEG90);
        }

        public void Update(MappingKegiatan13_90 kegMapping)
        {
            dao.Update(kegMapping);
        }

        public void Create(MappingKegiatan13_90 kegMapping)
        {
            dao.Create(kegMapping);
        }
    }
}
