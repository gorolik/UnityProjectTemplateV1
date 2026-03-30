using Sources.Data;

namespace Sources.Services.Data
{
    public interface IDataService
    {
        void Save(PlayerData playerData);
        bool TryLoad(out PlayerData playerData);
        void Clear();
    }
}