using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Services
{
    public class LayoutService
    {
        private readonly ProniaDbContext _context;

        public LayoutService(ProniaDbContext context)
        {
            _context = context;
        }
        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }
    }
}
