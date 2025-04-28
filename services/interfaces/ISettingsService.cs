using webbuilder.api.models;

namespace webbuilder.api.services.interfaces
{
    public interface ISettingsService
    {
        Task<Setting?> GetSettingByIdAsync(string id);
        Task<List<Setting>> GetSettingsByElementIdAsync(string elementId);
        Task<Setting> UpdateSettingAsync(string id, Dictionary<string, object> settings);
        Task<bool> DeleteSettingAsync(string id);

        // Helper methods for specific element types
        Task<Setting?> GetOrCreateElementSettingAsync(string elementId, string settingType);
        Task<Setting?> UpdateElementSettingAsync(string elementId, string settingType, Dictionary<string, object> settings);
        Task<Dictionary<string, object>?> GetElementSettingAsync(string elementId, string settingType);
    }
}