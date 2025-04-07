using TheKingsApp.UseCases.Dto;

namespace TheKingsApp.UseCases.Interfaces
{
    public interface IMonarchAnswersService
    {
        Task<MonarchsAnswersDto> GetAnswersAboutMonarchsAsync();
    }
}
