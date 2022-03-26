using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.APBD;
using SIEVK.BusinessData.APBD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.APBD
{
    public class APBDRealisasiCapaianKinerjaProgramAPBDService
    {
        Mapper map = new Mapper();
        APBDRealisasiCapaianKinerjaProgramAPBDDAO dao = new APBDRealisasiCapaianKinerjaProgramAPBDDAO();

        public List<APBDCapaianKinerjaProgram> GetKinerjaProgramProgramList(string unitKey, string kdTahap, string kdTahun)
        {
            DataTable dt = dao.GetKinerjaProgramProgramList(unitKey, kdTahap, kdTahun);
            List<APBDCapaianKinerjaProgram> lst = map.BindDataList<APBDCapaianKinerjaProgram>(dt);
            return lst;
        }

        public List<APBDCapaianKinerjaProgram> GetKinerjaProgramRealisasiList(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string Kinpgrapbdkey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm, Kinpgrapbdkey);
            List<APBDCapaianKinerjaProgram> lst = map.BindDataList<APBDCapaianKinerjaProgram>(dt);
            return lst;
        }

        public APBDCapaianKinerjaProgram GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string Kinpgrapbdkey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm, Kinpgrapbdkey);
            APBDCapaianKinerjaProgram obj = map.BindData<APBDCapaianKinerjaProgram>(dt);
            return obj;
        }

        public List<APBDCapaianKinerjaProgram> GetKinerjaProgramCapaianKinerjaList(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = dao.GetKinerjaProgramCapaianKinerjaList(unitKey, kdTahap, kdTahun, IDPrgrm);
            List<APBDCapaianKinerjaProgram> lst = map.BindDataList<APBDCapaianKinerjaProgram>(dt);
            return lst;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgrapbdKey)
        {
            dao.Delete(unitKey, kdTahap, kdTahun, IDPrgrm, kinPgrapbdKey);
        }

        public void UpdateDataRealisasi(APBDCapaianKinerjaProgram obj)
        {
            dao.Update(obj);
        }

        public void CreateDataRealisasi(APBDCapaianKinerjaProgram obj)
        {
            dao.Create(obj);
        }

    }
}
