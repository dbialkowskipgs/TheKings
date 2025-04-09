using TheKingsApp.Core.Interfaces;
using TheKingsApp.UseCases.Dto;
using TheKingsApp.UseCases.Interfaces;
using TheKingsApp.UseCases.Utils;

namespace TheKingsApp.UseCases.Services
{
    public class MonarchAnswersService : IMonarchAnswersService
    {
        private readonly IMonarchService _monarchService;
        public MonarchAnswersService(IMonarchService monarchService)
        {
            _monarchService = monarchService;
        }

        public async Task<MonarchsAnswersDto> GetAnswersAboutMonarchsAsync()
        {

            var monarchsList = await _monarchService.GetMonarchsAsync();
            if (monarchsList == null)
                throw new ArgumentNullException(nameof(monarchsList));

            var monarchsDtos = monarchsList.Select(m => new MonarchsDto
            {
                Id = m.Id,
                Country = m.Country,
                House = m.House,
                Name = m.Name,
                Years = m.Years,
                YearsOfRule = MonarchUtils.CalculateYearsOfRule(m.Years)
            }).ToList();

            var monarchsAnswers = new MonarchsAnswersDto();
            monarchsAnswers.Total = monarchsDtos.Count;

            FindLongestRuleHouse(monarchsDtos, monarchsAnswers);
            FindLongestRuleMonarch(monarchsDtos, monarchsAnswers);
            FindCommonMonarchFirstName(monarchsDtos, monarchsAnswers);

            return monarchsAnswers;
        }

        private void FindLongestRuleHouse(List<MonarchsDto> monarchsDtos, MonarchsAnswersDto monarchsAnswers)
        {
            var houseYears = monarchsDtos.GroupBy(m => m.House)
                   .Select(h => new
                   {
                       House = h.Key,
                       TotalYears = h.Sum(m => m.YearsOfRule),
                   })
                   .ToList();


            // Get the maximum number of years of rule
            var maxYearsOfRule = houseYears.Max(h => h.TotalYears);

            // Find houses that have the maximum years of rule
            var rulingHouses = houseYears
                .Where(h => h.TotalYears == maxYearsOfRule)
                .Select(h => h.House)
                .ToList();

            monarchsAnswers.HouseName = string.Join(", ", rulingHouses);
            monarchsAnswers.HouseYears = maxYearsOfRule;
        }

        private void FindLongestRuleMonarch(List<MonarchsDto> monarchsDtos, MonarchsAnswersDto monarchsAnswers)
        {
            var monarchsRuleTotalYears = monarchsDtos
            .MaxBy(x => x.YearsOfRule);

            monarchsAnswers.MonarchName = monarchsRuleTotalYears.Name;
            monarchsAnswers.MonarchYears = monarchsRuleTotalYears.YearsOfRule;
        }

        private void FindCommonMonarchFirstName(List<MonarchsDto> monarchsDtos, MonarchsAnswersDto monarchsAnswers)
        {
            var firstNames = monarchsDtos
                .Where(n => !string.IsNullOrWhiteSpace(n.Name))
                .Select(n => n.Name.Split(' ')[0]).ToList();

            var firstNameCounts = firstNames
                                   .GroupBy(fn => fn)
                                   .Select(group => new
                                   {
                                       FirstName = group.Key,
                                       Count = group.Count()
                                   })
                                   .ToList();

            // Get the maximum number of commmon first name
            var maxFirstNameOccured = firstNameCounts.Max(h => h.Count);

            // Find most common first name
            var commonFirstName = firstNameCounts
                .Where(h => h.Count == maxFirstNameOccured)
                .Select(h => h.FirstName)
                .ToList();

            monarchsAnswers.CommonName = string.Join(", ", commonFirstName);
        }
    }
}
