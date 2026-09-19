using System.IO;
using System.Runtime.Serialization.Json;

namespace MusicBeePlugin.Saved_Data_Classes
{
    public class UserData
    {
        public SyncSettingsData syncSettingsData;
    }

    public class SyncSettingsData
    {
        // Values found through trial and error.
        public int video_delay = -500;
        public int video_click_delay = -200;
        public int constraints = 200;

        public static SyncSettingsData ReadSyncSettings(string path)
        {
            string final_path = Path.Combine(path, "mb_VideoPanel", "data.json");
            var serializer = new DataContractJsonSerializer(typeof(SyncSettingsData));
            SyncSettingsData loadedData;

            // If data doesn't exist it writes it and returns
            if (!File.Exists(final_path))
            {
                return WriteSyncSettings(new SyncSettingsData(), path);
            }

            // Read JSON.
            using (var stream = File.OpenRead(final_path))
            {
                loadedData = (SyncSettingsData)serializer.ReadObject(stream);
                Utilities.debugPrint("JSON LOADED");
            }

            return loadedData;
        }

        public static SyncSettingsData WriteSyncSettings(SyncSettingsData data, string path)
        {
            string finalPath = Path.Combine(path, "mb_VideoPanel", "data.json");
            Directory.CreateDirectory(Path.GetDirectoryName(finalPath));

            // Write JSON.
            var serializer = new DataContractJsonSerializer(typeof(SyncSettingsData));
            using (var stream = File.Create(finalPath))
            {
                serializer.WriteObject(stream, data);
                Utilities.debugPrint("JSON CREATED");
            }

            return data;
        }
    }
}
