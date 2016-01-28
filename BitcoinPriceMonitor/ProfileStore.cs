using BitcoinPriceMonitor.Config;
using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;
    using System.Linq;

    public class ProfileStore : IProfileStore
    {
        private ISettings _settings;
        private ITradePriceMonitorFactory _monitorFactory;
        private const string ProfileHeading = "_profile_";

        public ProfileStore(ISettings settings, ITradePriceMonitorFactory monitorFactory)
        {
            _settings = settings;
            _monitorFactory = monitorFactory;
        }

        public IEnumerable<string> Profiles
        {
            get
            {
                return
                    Directory.GetFiles(_settings.ProfileStoreDirectory)
                        .Where(p =>
                        {
                            var fileName = Path.GetFileName(p);
                            return fileName != null && fileName.StartsWith(ProfileHeading);
                        })
                        .Select(p => Path.GetFileName(p)?.Replace(ProfileHeading, string.Empty));
            }
        }

        public ITradePriceMonitor LoadProfile(string profileName)
        {
            string serializedProfile = File.ReadAllText(getProfileFileName(profileName));
            var profile = new JavaScriptSerializer().Deserialize<Profile>(serializedProfile);
            return _monitorFactory.Get(profile);
        }

        public void SaveProfile(ITradePriceMonitor profile, string profileName)
        {
            var serializer = new JavaScriptSerializer();
            var serializedProfile = serializer.Serialize(new Profile(profile));
            File.WriteAllText(getProfileFileName(profileName), serializedProfile);
        }

        private string getProfileFileName(string profileName)
        {
            return Path.Combine(_settings.ProfileStoreDirectory, $"{ProfileHeading}{profileName}");
        }
    }
}
