using TheKingsApp.Core.Entities;
namespace TheKingsApp.Core.Interfaces
{
    public interface IMonarchService
    {
        Task<List<Monarchs>> GetMonarchsAsync();
    }
}
