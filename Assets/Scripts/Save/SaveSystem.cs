using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Personnage player){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.playerData";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(){
        string path = Application.persistentDataPath + "/player.playerData";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveStats(BasicStats stats){
        string path = Application.persistentDataPath + "/stats.statsData";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        StatsData data = new StatsData(stats);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StatsData LoadStats(){
        string path = Application.persistentDataPath + "/stats.statsData";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StatsData data = formatter.Deserialize(stream) as StatsData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveSkills(SkillInfos skill){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/skills.skillsData" + skill.nom;
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillData data = new SkillData(skill);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SkillData LoadSkills(string nom){
        string path = Application.persistentDataPath + "/skills.skillsData" + nom;
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillData data = formatter.Deserialize(stream) as SkillData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveTree(Skilltree tree){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/tree.treeData";
        FileStream stream = new FileStream(path, FileMode.Create);

        TreeData data = new TreeData(tree);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static TreeData LoadTree(){
        string path = Application.persistentDataPath + "/tree.treeData";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TreeData data = formatter.Deserialize(stream) as TreeData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveTime(Timer timer){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/time.timeData";
        FileStream stream = new FileStream(path, FileMode.Create);

        TimeData data = new TimeData(timer);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static TimeData LoadTime(){
        string path = Application.persistentDataPath + "/time.timeData";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TimeData data = formatter.Deserialize(stream) as TimeData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSave(){
        string pathPlayer = Application.persistentDataPath + "/player.playerData";
        File.Delete(pathPlayer);
        if(!File.Exists(pathPlayer)){
            Debug.Log("Il n'y a aucun document vers ce chemin");
        }
        string pathStats = Application.persistentDataPath + "/stats.statsData";
        File.Delete(pathStats);
        if(!File.Exists(pathPlayer)){
            Debug.Log("Il n'y a aucun document vers ce chemin");
        }
    }
}
