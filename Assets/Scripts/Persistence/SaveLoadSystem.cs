using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem
{
    //public static void SaveLevelData(LevelResults levels)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Application.persistentDataPath + "/leveldata.dat";
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    LevelData data = new LevelData(levels);
    //    formatter.Serialize(stream, data);
    //    stream.Close();
    //}

    //public static void LoadLevelData(LevelResults levels)
    //{
    //    string path = Application.persistentDataPath + "/leveldata.dat";
    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        LevelData data = formatter.Deserialize(stream) as LevelData;
    //        levels.LoadFromData(data);
    //        stream.Close();
    //    }
    //    else
    //    {
    //        Debug.Log("Save file not found in " + path);
    //        throw new FileNotFoundException("Save file not found in " + path);
    //    }
    //}

    public static void SavePlayerData(BundleObject bundleObject)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        // string path = Path.Combine(Application.persistentDataPath, "playerdata.dat");
        string path = Application.persistentDataPath + "/playerdata.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        Weapons[] weapons = { bundleObject.twinBladesWeapon, bundleObject.plasmaGunWeapon, bundleObject.shurikenWeapon, bundleObject.miniBombWeapon };
        PlayerData data = new PlayerData(bundleObject.attributes, bundleObject.levelResults, weapons);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadPlayerData(BundleObject bundleObject)
    {
        // string path = Path.Combine(Application.persistentDataPath, "playerdata.dat");
        string path = Application.persistentDataPath + "/playerdata.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            bundleObject.LoadFromData(data);
            stream.Close();
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            throw new FileNotFoundException("Save file not found in " + path);
        }
    }
}
