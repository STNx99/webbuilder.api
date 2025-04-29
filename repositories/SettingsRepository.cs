using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;

namespace webbuilder.api.repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ElementStoreContext _context;

        public SettingsRepository(ElementStoreContext context)
        {
            _context = context;
        }

        public async Task<Setting?> GetSettingByIdAsync(string id)
        {
            return await _context.Settings.FindAsync(id);
        }

        public async Task<List<Setting>> GetSettingsByElementIdAsync(string elementId)
        {
            return await _context.Settings
                .Where(s => s.ElementId == elementId)
                .ToListAsync();
        }


        public async Task<Setting> CreateSettingAsync(Setting setting)
        {
            setting.Id = setting.Id == string.Empty ? Guid.NewGuid().ToString() : setting.Id;

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<Setting> UpdateSettingAsync(Setting setting)
        {
            _context.Entry(setting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<bool> DeleteSettingAsync(string id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return false;
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}