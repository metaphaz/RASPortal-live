using System.Text.RegularExpressions;

namespace RASPortal.Models
{
    public static class EventSocietyTypeExtensions
    {
        public static string ToFriendlyString(this EventSocietyType societyType)
        {
            return Regex.Replace(societyType.ToString(), "([A-Z])", " $1").Trim();
        }
    }
}