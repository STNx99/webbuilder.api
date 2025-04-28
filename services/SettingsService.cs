using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;
using webbuilder.api.services.interfaces;

namespace webbuilder.api.services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Setting?> GetSettingByIdAsync(string id)
        {
            return await _settingsRepository.GetSettingByIdAsync(id);
        }

        public async Task<List<Setting>> GetSettingsByElementIdAsync(string elementId)
        {
            return await _settingsRepository.GetSettingsByElementIdAsync(elementId);
        }

        public async Task<Setting> UpdateSettingAsync(string id, Dictionary<string, object> settings)
        {
            var existingSetting = await _settingsRepository.GetSettingByIdAsync(id);
            if (existingSetting == null)
            {
                throw new KeyNotFoundException($"Setting with id {id} not found");
            }

            existingSetting.Settings = settings;

            return await _settingsRepository.UpdateSettingAsync(existingSetting);
        }

        public async Task<bool> DeleteSettingAsync(string id)
        {
            return await _settingsRepository.DeleteSettingAsync(id);
        }

        public async Task<Setting?> GetOrCreateElementSettingAsync(string elementId, string settingType)
        {
            var elementSettings = await _settingsRepository.GetSettingsByElementIdAsync(elementId);
            var setting = elementSettings.FirstOrDefault(s => s.SettingType == settingType);

            if (setting == null)
            {
                setting = new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"{settingType} Settings",
                    SettingType = settingType,
                    ElementId = elementId,
                    Settings = new Dictionary<string, object>()
                };

                return await _settingsRepository.CreateSettingAsync(setting);
            }

            return setting;
        }

        public async Task<Setting?> UpdateElementSettingAsync(string elementId, string settingType, Dictionary<string, object> settings)
        {
            var setting = await GetOrCreateElementSettingAsync(elementId, settingType);
            if (setting != null)
            {
                setting.Settings = settings;
                return await _settingsRepository.UpdateSettingAsync(setting);
            }

            return null;
        }

        public async Task<Dictionary<string, object>?> GetElementSettingAsync(string elementId, string settingType)
        {
            var elementSettings = await _settingsRepository.GetSettingsByElementIdAsync(elementId);
            var setting = elementSettings.FirstOrDefault(s => s.SettingType == settingType);

            return setting?.Settings;
        }
    }
}