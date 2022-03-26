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
    public class RKPDRealisasiKinerjaProgramService
    {
        Mapper map = new Mapper();
        RKPDRealisasiKinerjaProgramDAO dao = new RKPDRealisasiKinerjaProgramDAO();

        public List<RKPDKinerjaProgram> GetKinerjaProgram(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = dao.GetKinerjaProgram(unitKey, kdTahap, kdTahun, idPrgrm);
            List<RKPDKinerjaProgram> lst = map.BindDataList<RKPDKinerjaProgram>(dt);
            return lst;
        }

        public List<RKPDKinerjaProgram> GetKinerjaProgramRealisasiList(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgRKPDKey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm, kinPgRKPDKey);
            List<RKPDKinerjaProgram> lst = map.BindDataList<RKPDKinerjaProgram>(dt);
            return lst;
        }

        public RKPDKinerjaProgram GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgRKPDKey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm, kinPgRKPDKey);
            RKPDKinerjaProgram obj = map.BindData<RKPDKinerjaProgram>(dt);
            return obj;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgRKPDKey)
        {
            dao.Delete(unitKey, kdTahap, kdTahun, IDPrgrm, kinPgRKPDKey);
        }

        public void UpdateDataRealisasi(RKPDKinerjaProgram obj)
        {
            dao.Update(obj);
        }

        public void CreateDataRealisasi(RKPDKinerjaProgram obj)
        {
            dao.Create(obj);
        }

    }
}
