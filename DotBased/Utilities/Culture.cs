using System.Globalization;
using DotBased.Logging;

namespace DotBased.Utilities;

public static class Culture
{
    private static List<CultureInfo> _sysCultures = new List<CultureInfo>();
    private static Dictionary<string, RegionInfo> _regions = new Dictionary<string, RegionInfo>();
    private static readonly ILogger _logger = LogService.RegisterLogger(typeof(Culture));

    /// <summary>
    /// Get all system known cultures.
    /// </summary>
    /// <remarks>Will be cached after first call, to clear the internal list call <see cref="ClearCached"/> function</remarks>
    /// <returns>The list with <see cref="CultureInfo"/>'s the system knows</returns>
    public static IEnumerable<CultureInfo> GetSystemCultures()
    {
        _logger.Debug("Getting system cultures...");
        if (_sysCultures.Count == 0)
            _sysCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        return _sysCultures;
    }

    /// <summary>
    /// Get the regions the system knows.
    /// </summary>
    /// <remarks>The list will internally be cached, clear the cache with the <see cref="ClearCached"/> function</remarks>
    /// <returns>A list with regions from the system</returns>
    public static Dictionary<string, RegionInfo> GetRegions()
    {
        if (_regions.Count == 0)
        {
            var cultureInfos = GetSystemCultures().Where(cul => !cul.IsNeutralCulture).Where(cul => cul.LCID != 0x7F);
            foreach (var culture in cultureInfos)
            {
                var region = new RegionInfo(culture.Name);
                _regions.Add(culture.Name, region);
            }
        }
        return _regions;
    }

    /// <summary>
    /// Clears the specified cache.
    /// </summary>
    public static void ClearCached(CacheType type)
    {
        switch (type)
        {
            case CacheType.Culture:
                _sysCultures.Clear();
                break;
            case CacheType.Region:
                _regions.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public enum CacheType
    {
        Culture,
        Region
    }

    public static class Currency
    {
        /// <summary>
        /// Formats the currency amount to the given culture currency.
        /// </summary>
        /// <param name="amount">The amount to format</param>
        /// <param name="culture">The culture to be formatted in</param>
        /// <returns>Formatted amount in the given culture format</returns>
        public static string FormatAmountToCultureCurrency(double amount, CultureInfo culture) => string.Format(culture, "{0:C}", amount);

        /// <summary>
        /// Formats the amount to the culture which is found by the ISO currency symbol.
        /// </summary>
        /// <remarks>WIP & slow!</remarks>
        /// <param name="amount">The amount to fomrat</param>
        /// <param name="isoCurencySymbol">Three-character ISO 4217 currency symbol</param>
        /// <returns>Formatted amount in the given ISO currency symbol</returns>
        public static string FormatAmountFromIsoCurrency(double amount, string isoCurencySymbol)
        {
            var culture = CultureInfo.CurrentCulture;
            var systemRegion = new RegionInfo(culture.Name);
            if (systemRegion.ISOCurrencySymbol != isoCurencySymbol)
            {
                string? result = GetRegions().Where(x => x.Value.ISOCurrencySymbol == isoCurencySymbol).Select(x => x.Key).FirstOrDefault();
                culture = GetSystemCultures().FirstOrDefault(x => x.Name == result);
            }
            return culture == null ? string.Empty : FormatAmountToCultureCurrency(amount, culture);
        }

        /// <summary>
        /// Get a list of ISO 3 letter currency symbols.
        /// </summary>
        /// <returns>List with ISOCurrencySymbols</returns>
        public static IEnumerable<string> GetIsoCurrencySymbols() => GetRegions().Select(x => x.Value.ISOCurrencySymbol).Distinct().ToList();
    }
}