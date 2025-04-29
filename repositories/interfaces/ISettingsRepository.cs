using webbuilder.api.models;

namespace webbuilder.api.repositories.interfaces
{
    public interface ISettingsRepository
    {
        Task<Setting?> GetSettingByIdAsync(string id);
        Task<List<Setting>> GetSettingsByElementIdAsync(string elementId);
        Task<Setting> CreateSettingAsync(Setting setting);
        Task<Setting> UpdateSettingAsync(Setting setting);
        Task<bool> DeleteSettingAsync(string id);
    }
}