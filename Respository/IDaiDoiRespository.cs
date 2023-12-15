using CanhGac.Models;

namespace CanhGac.Respository
{
    public interface IDaiDoiRespository
    {
        DonVi Add(DonVi donvi);
        DonVi Update(DonVi donvi);
        DonVi Delete(string MaDonVi);
        DonVi GetDonVi(string MaDonVi);
        IEnumerable<DonVi> GetAllDonVi();
    }
}
