using SaveSystem.Encryption;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// <see cref="ZPrefs"/> Can Encrypt & Save Data In <see cref="PlayerPrefs"/> Of Unity Core System......
    /// </summary>
    public static class ZPrefs
    {
        /// <summary>
        /// Encrypt & Save Data Against The Given <see cref="key"/>
        /// </summary>
        /// <param name="key">Keys`s <see cref="string"/>.....</param>
        /// <param name="data"><see cref="T"/>'s Type Data....</param>
        /// <param name="useEncryption"></param>
        /// <typeparam name="T">type_of Of Data To Store...</typeparam>
        public static void Set<T>(string key, T @data, bool useEncryption = false)
        {
            var jsonData = JsonUtility.ToJson(new DataWrapper<T>(@data));
            var saveKey = VerifyKeyEncryption(key, useEncryption);
            var saveData = VerifyDataEncryption(jsonData, useEncryption);

            PlayerPrefs.SetString(saveKey, saveData);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Get Data From <see cref="PlayerPrefs"/> Or On Fail Return Default....
        /// </summary>
        /// <param name="key">Keys`s <see cref="string"/>.....</param>
        /// <param name="default">Object To Return If Data Is Not Found!</param>
        /// <param name="usingEncryption"></param>
        /// <typeparam name="T">type_of Of Data To Store...</typeparam>
        /// <returns><see cref="T"/>'s type_of object...</returns>
        public static T Get<T>(string key, T @default = default, bool usingEncryption = false)
        {
            var saveKey = VerifyKeyEncryption(key, usingEncryption);
            var saveData = PlayerPrefs.GetString(saveKey);
            var jsonData = VerifyDataDecryption(saveData, usingEncryption);

            if (string.IsNullOrEmpty(jsonData)) return @default;

            var dataWrapper = JsonUtility.FromJson<DataWrapper<T>>(jsonData);
            return dataWrapper.Data;
        }

        /// <summary>
        /// Remove Given Key & It's Data From <see cref="PlayerPrefs"/> & Return The Operation Success....
        /// </summary>
        /// <param name="key"></param>
        /// <param name="usingEncryption"></param>
        /// <returns></returns>
        public static bool UnSet(string key, bool usingEncryption = false)
        {
            var saveKey = VerifyKeyEncryption(key, usingEncryption);
            if (PlayerPrefs.HasKey(saveKey) == false) return false;
            PlayerPrefs.DeleteKey(saveKey);
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// <see cref="Reset"/> All Of The Data In <see cref="PlayerPrefs"/>
        /// </summary>
        public static void Reset()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        private static string VerifyKeyEncryption(string key, bool useEncryption)
            => useEncryption ? CryptographyProcessor.Process(key, CryptographyType.Encrypt) : key;

        /////////////////////////////////////////////////////////////////////////////////////
        private static string VerifyDataEncryption(string data, bool useEncryption)
            => useEncryption ? CryptographyProcessor.Process(data, CryptographyType.Encrypt) : data;

        /////////////////////////////////////////////////////////////////////////////////////
        private static string VerifyDataDecryption(string data, bool useEncryption)
            => useEncryption ? CryptographyProcessor.Process(data, CryptographyType.Decrypt) : data;
    }
}