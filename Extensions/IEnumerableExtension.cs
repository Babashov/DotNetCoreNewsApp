using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> AnarKerimov<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.AnarKerimovSecirDeyiskenleri("Name"),
                       Value = item.AnarKerimovSecirDeyiskenleri("ID"),
                       Selected = item.AnarKerimovSecirDeyiskenleri("ID").Equals(selectedValue.ToString())
                   };
        }

        public static string AnarKerimovSecirDeyiskenleri<T>(this T item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }

    }
}
