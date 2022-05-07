using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace CountryClubMVC.Extensions;

public static class TempDataExtensions
{
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        string json = JsonSerializer.Serialize<T>(value);
        tempData[key] = json;
    }

    public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        if (tempData != null && tempData.TryGetValue(key, out object json))
        {
            return JsonSerializer.Deserialize<T>(json.ToString());
        }
        else
        {
            return null;
        }
    }
}
