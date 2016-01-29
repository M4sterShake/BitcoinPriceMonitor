namespace BitcoinPriceMonitor.Profile
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Config;
    using PriceMonitor;

    public class ProfileStore : IProfileStore
    {
        private readonly ISettings _settings;
        private readonly ITradePriceMonitorFactory _monitorFactory;
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
                            return fileName != null && fileName.StartsWith(ProfileHeading) &&
                                   !fileName.StartsWith($"{ProfileHeading}{_settings.PersistanceProfileName}");
                        })
                        .Select(p => Path.GetFileName(p)?.Replace(ProfileHeading, string.Empty));
            }
        }

        public ITradePriceMonitor LoadProfile(string profileName)
        {
            string serializedProfile = File.ReadAllText(GetProfileFileName(profileName));
            var profile = new JavaScriptSerializer().Deserialize<Profile>(serializedProfile);
            return _monitorFactory.Get(profile);
        }

        public void SaveProfile(ITradePriceMonitor profile, string profileName)
        {
            var serializer = new JavaScriptSerializer();
            var serializedProfile = serializer.Serialize(new Profile(profile));
            File.WriteAllText(GetProfileFileName(profileName), serializedProfile);
        }

        public void SavePersistenceProfile(ITradePriceMonitor profile)
        {
            SaveProfile(profile, _settings.PersistanceProfileName);
        }

        private string GetProfileFileName(string profileName)
        {
            return Path.Combine(_settings.ProfileStoreDirectory, $"{ProfileHeading}{profileName}");
        }
    }
}
