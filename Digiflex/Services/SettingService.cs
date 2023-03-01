using Digiflex.DAL;
using Digiflex.Models;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.Services
{
    public class SettingService
    {
        private readonly DigiflexContext _context;

        public SettingService(DigiflexContext context )
        {
            _context = context;
        }
        public async Task<List<Setting>> GetSettingAsync()
        {
            return await _context.Settings.ToListAsync();
        }
    }
}
