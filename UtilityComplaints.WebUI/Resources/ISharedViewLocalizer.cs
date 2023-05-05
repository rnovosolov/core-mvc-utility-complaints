using Microsoft.Extensions.Localization;

namespace UtilityComplaints.WebUI.Resources
{
    public interface ISharedViewLocalizer
    {
        public LocalizedString this[string key]
        {
            get;
        }

        LocalizedString GetLocalizedString(string key);
    }
}
