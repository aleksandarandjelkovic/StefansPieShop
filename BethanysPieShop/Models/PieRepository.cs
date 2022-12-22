using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        public PieRepository(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _bethanysPieShopDbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _bethanysPieShopDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public async Task<Pie?> GetPieById(int pieId)
        {
            return await _bethanysPieShopDbContext.Pies.FirstOrDefaultAsync(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            return _bethanysPieShopDbContext.Pies.Where(p => p.Name.Contains(searchQuery));
        }

        public async Task SavePieAsync(Pie pie)
        {
            await _bethanysPieShopDbContext.Pies.AddAsync(pie);
            await _bethanysPieShopDbContext.SaveChangesAsync();
        }

        public async Task DeletePie(int pieId)
        {
            var pie = await GetPieById(pieId);
            _bethanysPieShopDbContext.Pies.Remove(pie!);
            await _bethanysPieShopDbContext.SaveChangesAsync();
        }

        public async Task UpdatePieAsync(Pie pie)
        {
            _bethanysPieShopDbContext.Pies.Update(pie);
            await _bethanysPieShopDbContext.SaveChangesAsync();
        }

    }
}