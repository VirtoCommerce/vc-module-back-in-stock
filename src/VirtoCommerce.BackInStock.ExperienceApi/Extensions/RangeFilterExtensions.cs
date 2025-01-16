using System;
using System.Linq;
using VirtoCommerce.SearchModule.Core.Model;

namespace VirtoCommerce.BackInStock.ExperienceApi.Extensions;

public static class RangeFilterExtensions
{
    private delegate bool TryParse<T>(string str, out T result);

    public static void MapTo(this RangeFilter rangeFilter, Action<DateTime> setStart, Action<DateTime> setEnd)
    {
        rangeFilter.MapTo<DateTime>(DateTime.TryParse,
            (lower, includeLower) => setStart(lower + (includeLower ? TimeSpan.Zero : TimeSpan.MinValue)),
            (upper, includeUpper) => setEnd(upper - (includeUpper ? TimeSpan.Zero : TimeSpan.MinValue)));
    }

    private static void MapTo<T>(this RangeFilter rangeFilter, TryParse<T> tryParse, Action<T, bool> setStart, Action<T, bool> setEnd)
    {
        var range = rangeFilter?.Values?.FirstOrDefault();
        if (range == null)
        {
            return;
        }

        if (tryParse(range.Lower, out var lower))
        {
            setStart(lower, range.IncludeLower);
        }

        if (tryParse(range.Upper, out var upper))
        {
            setEnd(upper, range.IncludeUpper);
        }
    }
}
