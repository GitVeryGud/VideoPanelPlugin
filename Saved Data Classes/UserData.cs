using LibVLCSharp.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin.Saved_Data_Classes
{
    [DataContract]
    public class UserData
    {
        private string persistent_path;
        // Update number every time a new member is added, ensuring UserData updates for old users (also remember to update SetData).
        [DataMember(IsRequired = true)] private int _userdata_version = 1;
        [DataMember] public SyncSettingsData sync_settings_data;
        [DataMember] public Plugin.MetaDataType custom_tag;

        public UserData(string path)
        {
            sync_settings_data = new SyncSettingsData();
            custom_tag = Plugin.MetaDataType.Custom18;
            persistent_path = path;
            ReadUserData();
        }

        public void ReadUserData()
        {
            string final_path = Path.Combine(persistent_path, "mb_VideoPanel", "data.json");
            var serializer = new DataContractJsonSerializer(typeof(UserData));
            UserData loadedData;

            try {
                // Read JSON.
                using (var stream = File.OpenRead(final_path))
                {
                    loadedData = (UserData)serializer.ReadObject(stream);
                    
                    if (loadedData._userdata_version != _userdata_version)
                    {
                        throw new VersionMismatchException(loadedData._userdata_version, _userdata_version, loadedData);
                    }

                    SetData(loadedData);
                    Utilities.debugPrint("JSON LOADED");
                }
            }

            // If data version changed.
            catch (VersionMismatchException ex)
            {
                Debug.WriteLine(ex.Message);
                SetDataDifferentVersion(ex.read_data);
                WriteUserData();
            }

            // If data doesn't exist.
            catch (Exception ex){
                Debug.WriteLine(ex.Message);
                WriteUserData();
            }
        }

        public void WriteUserData()
        {
            string final_path = Path.Combine(persistent_path, "mb_VideoPanel", "data.json");
            Directory.CreateDirectory(Path.GetDirectoryName(final_path));

            // Write JSON.
            var serializer = new DataContractJsonSerializer(typeof(UserData));
            using (var stream = File.Create(final_path))
            {
                serializer.WriteObject(stream, this);
                Utilities.debugPrint("JSON CREATED in " + final_path);
            }
        }

        private void SetData(UserData newData)
        {
            sync_settings_data = newData.sync_settings_data;
            custom_tag = newData.custom_tag;
        }

        // Checks if the data is null (or default value for finding nothing) in that specific property, if it is just keeps default value, otherwise changes to read property.
        private void SetDataDifferentVersion(UserData newData)
        {
            sync_settings_data = newData.sync_settings_data != null ? newData.sync_settings_data : sync_settings_data;
            custom_tag = newData.custom_tag != 0 ? newData.custom_tag : custom_tag;
        }
    }

    public class SyncSettingsData
    {
        // Values found through trial and error.
        public int video_delay = -500;
        public int video_click_delay = -200;
        public int constraints = 200;
    }

    public class VersionMismatchException : Exception
    {
        public int found_version;
        public int expected_version;
        public UserData read_data;

        public VersionMismatchException(int found, int expected, UserData data)
            : base(found < expected
              ? $"UserData version {found} is outdated. Updating it to version {expected}."
              : $"UserData version {found} is newer. Downgrading it to version {expected}.")
        {
            found_version = found;
            expected_version = expected;
            read_data = data;
        }
    }
}
