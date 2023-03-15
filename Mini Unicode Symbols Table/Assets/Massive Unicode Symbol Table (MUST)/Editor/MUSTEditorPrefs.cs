using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MassiveUnicodeSymbolTable
{
    public class MUSTEditorPrefs
    {
        /// <summary>
        /// Return MD5 hash of the text.
        /// </summary>
        /// <param name="input">Text input string.</param>
        private static string GetMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("x2"));
            return sb.ToString();
        }

        /**Unique id by project path, This id will be pre-pended to each key*/
        private static string mAppKey = null;

        /// <summary>
        /// Static constructor to set unique kay based on project path.
        /// </summary>
        static MUSTEditorPrefs()
        {
            mAppKey = GetMd5Hash(Application.dataPath) + "-";
        }

        /// <summary>
        /// Checks if the given key exists.
        /// </summary>
        /// <param name="key">Key to check if it exists.</param>
        /// <returns>TRUE if the key exists. Otherwise, FALSE.</returns>
        public static bool HasKey(string key)
        {
            return EditorPrefs.HasKey(mAppKey + key);
        }

        /// <summary>
        /// Returns the value corresponding to key.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The value of the given key.</returns>
        public static string GetString(string key)
        {
            return EditorPrefs.GetString(mAppKey + key);
        }

        /// <summary>
        /// Returns the value corresponding to key if it exists, else returns default value
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The value of the given key.</returns>
        public static string GetString(string key, string defaultValue)
        {
            return EditorPrefs.GetString(mAppKey + key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the preference identified by key. Note that EditorPrefs does not support null strings
        /// and will store an empty string instead.
        /// <param name="key">Key</param>
        /// <param name="value">The value to store.</param>
        public static void SetString(string key, string value)
        {
            EditorPrefs.SetString(mAppKey + key, value);
        }

        /// <summary>
        /// Returns the value corresponding to key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The corresponding bool value.</returns>
        public static bool GetBool(string key)
        {
            return EditorPrefs.GetBool(mAppKey + key);
        }

        /// <summary>
        /// Returns the value corresponding to key if it exists, else default value.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The corresponding bool value.</returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            if (HasKey(key))
                return EditorPrefs.GetBool(mAppKey + key);
            return defaultValue;
        }

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <param name="value">The bool value to save.</param>
        public static void SetBool(string key, bool value)
        {
            EditorPrefs.SetBool(mAppKey + key, value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The corresponding int value.</returns>
        public static int GetInt(string key)
        {
            return EditorPrefs.GetInt(mAppKey + key);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists else default value.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The corresponding int value.</returns>
        public static int GetInt(string key, int defaultValue)
        {
            if (HasKey(key))
                return EditorPrefs.GetInt(mAppKey + key);
            return defaultValue;
        }

        /// <summary>
        /// Sets the value of the preference identified by key as an integer.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <param name="value">The int value to save.</param>
        public static void SetInt(string key, int value)
        {
            EditorPrefs.SetInt(mAppKey + key, value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The corresponding float value.</returns>
        public static float GetFloat(string key)
        {
            return EditorPrefs.GetFloat(mAppKey + key);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists else default value.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <returns>The corresponding float value.</returns>
        public static float GetFloat(string key, float defaultValue)
        {
            if (HasKey(key))
                return EditorPrefs.GetFloat(mAppKey + key);
            return defaultValue;
        }

        /// <summary>
        /// Sets the value of the preference identified by key as a float.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <param name="value">The float value to save.</param>
        public static void SetFloat(string key, float value)
        {
            EditorPrefs.SetFloat(mAppKey + key, value);
        }

        /// <summary>
        /// Delete a key.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        public static void DeleteKey(string key)
        {
            EditorPrefs.DeleteKey(mAppKey + key);
        }
    }
}