using CanhGac.Models;

namespace CanhGac.Respository
{
    public class DaiDoiRespository : IDaiDoiRespository
    {
        private readonly CanhGacContext _context;
        public DaiDoiRespository(CanhGacContext context)
        {
            _context = context;
        }
        public DonVi Add(DonVi donvi)
        {
            _context.DonVis.Add(donvi);
            _context.SaveChanges();
            return donvi;
        }

        public DonVi Delete(string MaDonVi)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DonVi> GetAllDonVi()
        {
            return _context.DonVis;
        }

        public DonVi GetDonVi(string MaDonVi)
        {
            return _context.DonVis.Find(MaDonVi);
        }

        public DonVi Update(DonVi donvi)
        {
            _context.Update(donvi);
            _context.SaveChanges();
            return donvi;
        }
    }
}
