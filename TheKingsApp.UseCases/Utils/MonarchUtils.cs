namespace TheKingsApp.UseCases.Utils
{
    public static class MonarchUtils
    {
        public static int CalculateYearsOfRule(string years)
        {
            if (string.IsNullOrEmpty(years))
                return 0;

            var yearsTable = years.Split('-');
            if (!int.TryParse(yearsTable[0], out var yearFrom))
                throw new FormatException("Invalid start year format");

            if (yearsTable.Length == 1)
                return 1;

            int yearTo;
            if (yearsTable.Length > 1 && int.TryParse(yearsTable[1], out var parsedYearTo))
            {
                yearTo = parsedYearTo;
            }
            else
            {
                yearTo = DateTime.Now.Year;
            }

            return yearTo - yearFrom;
        }
    }
}
