using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class IPlayerPrefs
{

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void Set<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
    }

    public static T Get<T>(string key)
    {

        if (!PlayerPrefs.HasKey(key))
        {
            return default(T);
        }

        string data = PlayerPrefs.GetString(key);

        if (data.Equals(string.Empty))
        {
            return default(T);
        }

        return JsonUtility.FromJson<T>(data);
    }

    public class FileData
    {

        public static void ConfigSettings()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }

        /// <summary>
        /// Return path data in local
        /// </summary>
        public static string DeviceDataPath
        {
            get
            {
                return Application.persistentDataPath + "/";
            }
        }

        // Trung: To remove all save files.
        public static void ClearSaveFile()
        {
            var path = DeviceDataPath;
            var files = Directory.GetFiles(path);
            Debug.Log(DeviceDataPath);
            foreach (var file in files)
            {
                File.Delete(file);
                Debug.Log("deleted " + file);
            }
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerInfo"></param>
        /// <param name="filename"></param>
        public static void SaveFile<T>(T playerInfo, string filename)
        {
            var path = DeviceDataPath + filename;
            ConfigSettings();
            FileStream file = File.Create(path);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, playerInfo);
            file.Close();
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T LoadFile<T>(string filename)
        {
            var path = DeviceDataPath + filename;
            if (File.Exists(path))
            {
                ConfigSettings();
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                T data = (T)bf.Deserialize(file);
                file.Close();
                return data;
            }
            return default(T);
        }

        public static void DeleteFile(string filename)
        {
            var path = DeviceDataPath + filename;
            File.Delete(path);
        }

        public static bool Exists(string fileName)
        {
            var path = DeviceDataPath + fileName;
            return File.Exists(path);
        }

        public static List<string> GetNamesFile()
        {
            var path = DeviceDataPath;
            var files = Directory.GetFiles(path);

            List<string> names = new List<string>();

            foreach (var file in files)
            {
                int start = path.Length;
                int end = file.Length;
                string name = file.Substring(start, end - start);
                Debug.Log("File Name: " + name);
                names.Add(name);
            }

            return names;
        }
    }
}

