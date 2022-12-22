namespace BethanysPieShop.Models
{
    public interface IPieRepository
    {
        IEnumerable<Pie> AllPies { get; }
        IEnumerable<Pie> PiesOfTheWeek { get; }
        Task<Pie?> GetPieById(int pieId);
        IEnumerable<Pie> SearchPies(string searchQuery);
        Task SavePieAsync(Pie pie);
        Task DeletePie(int pieId);
        Task UpdatePieAsync(Pie pie);
    }
}
