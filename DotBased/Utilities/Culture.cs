using System.Globalization;

namespace DotBased.Utilities;

public static class Culture
{
    private static List<CultureInfo> _sysCultures = new List<CultureInfo>();
    private static List<RegionInfo> _regions = new List<RegionInfo>();

    /// <summary>
    /// Get all system known cultures.
    /// </summary>
    /// <remarks>Will be cached after first call, to clear the internal list call <see cref="ClearCached"/> function</remarks>
    /// <returns>The list with <see cref="CultureInfo"/>'s the system knows</returns>
    public static IEnumerable<CultureInfo> GetSystemCultures()
    {
        //TODO: Probally need some internal caching for this
        if (_sysCultures.Count == 0)
            _sysCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        return _sysCultures;
    }

    /// <summary>
    /// Get the regions the system knows.
    /// </summary>
    /// <remarks>The list will internally be cached, clear the cache with the <see cref="ClearCached"/> function</remarks>
    /// <returns>A list with regions from the system</returns>
    public static IEnumerable<RegionInfo> GetRegions()
    {
        if (_regions.Count == 0)
            _regions = GetSystemCultures().Where(cul => !cul.IsNeutralCulture).Where(cul => cul.LCID != 0x7F).Select(x => new RegionInfo(x.Name)).ToList();
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
        /// <returns></returns>
        public static string AmountToCultureCurrency(double amount, CultureInfo culture) => string.Format(culture, "{0:C}", amount);

        /// <summary>
        /// Get a list of ISO 3 letter currency symbols.
        /// </summary>
        /// <returns>List with ISOCurrencySymbols</returns>
        public static IEnumerable<string> GetIsoCurrencySymbols() => GetRegions().Select(x => x.ISOCurrencySymbol).Distinct().ToList();
    }
}