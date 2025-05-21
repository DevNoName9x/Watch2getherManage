using Microsoft.EntityFrameworkCore;
using Watch2getherManage.Data;
using Watch2getherManage.Models;

namespace Watch2getherManage.Services
{
    public class StreamKeyService(AppDbContext context)
    {
        public void Insert(string streamKey)
        {
            var newKey = new SteamKey { StreamKey = streamKey };
            context.SteamKeys.Add(newKey);
            context.SaveChanges();
        }

        public async Task InsertAsync(string streamKey)
        {
            var newKey = new SteamKey { StreamKey = streamKey };
            await context.SteamKeys.AddAsync(newKey);
            await context.SaveChangesAsync();
        }

        public List<SteamKey> GetAllKeys()
        {
            return [.. context.SteamKeys];
        }
        public async Task< List<SteamKey>> GetAllKeysAsync()
        {
            return await context.SteamKeys.ToListAsync();
        }
        public SteamKey? Get(string streamKey)
        {
            return context.SteamKeys.FirstOrDefault(x => x.StreamKey == streamKey);
        }
        public async Task< SteamKey?> GetAsync(string streamKey)
        {
            return await context.SteamKeys.FirstOrDefaultAsync(x => x.StreamKey == streamKey);
        }
    }
}
