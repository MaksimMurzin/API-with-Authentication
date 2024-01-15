using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaStoreRepository
    {
        public Task<IEnumerable<Villa>> GetAllVillas();
        public Task<Villa> GetVilla(int id);
    }
}
