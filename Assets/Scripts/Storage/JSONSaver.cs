using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Storage
{
    public class JSONSaver
    {
        private static string _path = Application.persistentDataPath;

        public static void SaveFarmSourceData(string fileName, int value)
        {
            FarmSourceData data = new FarmSourceData();
            data.Value = value;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path + "/" + fileName + ".json", json);
        }
        public static FarmSourceData LoadFarmSourceData(string fileName)
        {
            if (!File.Exists(_path + "/" + fileName + ".json"))
                return null;

            string json = File.ReadAllText(_path + "/" + fileName + ".json");
            FarmSourceData data = JsonUtility.FromJson<FarmSourceData>(json);
            return data;
        }



        public static void SaveLockData(string fileName, bool value)
        {
            LockData data = new LockData();
            data.Value = value;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path + "/" + fileName + ".json", json);
        }
        public static LockData LoadLockData(string fileName)
        {
            if (!File.Exists(_path + "/" + fileName + ".json"))
                return null;

            string json = File.ReadAllText(_path + "/" + fileName + ".json");
            LockData data = JsonUtility.FromJson<LockData>(json);
            return data;
        }


#if UNITY_EDITOR
        [MenuItem("Edit/JSON/Delete Files")]
        private static void DeleteJsonFiles()
        {
            FileUtil.DeleteFileOrDirectory(_path);
        }
        [MenuItem("Edit/JSON/Show Explorer")]
        private static void ShowExplorerData()
        {
            Application.OpenURL(_path);
        }
#endif
    }
}
