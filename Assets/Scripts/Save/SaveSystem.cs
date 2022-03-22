using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Fonction qui sauvegarde, charge et encrypte les donnees du joueur dans un fichier de sauvegarde en binaire
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Fonction publique qui sauvegarde les informations du joueur
    /// </summary>
    /// <param name="player">Reference au Personnage</param>
    public static void SavePlayer(Personnage player){
        BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
        string path = Application.persistentDataPath + "/player.playerData"; // path represente le chemin pour acces au ficher des donnes du joueur
        FileStream stream = new FileStream(path, FileMode.Create); // stream cree un nouveau FileStream avec le path 

        PlayerData data = new PlayerData(player); // data prend la valeur de retour du PlayerData

        formatter.Serialize(stream, data); // on stock les informations de data via le stream
        stream.Close(); // on ferme le stream
    }

    /// <summary>
    /// Fonction publique qui charge les informations du joueur
    /// </summary>
    /// <returns>Les donnees du joueur</returns>
    public static PlayerData LoadPlayer(){
        string path = Application.persistentDataPath + "/player.playerData"; // path represente le chemin pour acces au ficher des donnes du joueur
        if(File.Exists(path)){ // si un ficher existe au chemin path
            BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
            FileStream stream = new FileStream(path, FileMode.Open); // stream ouvre un nouveau FileStream avec le path

            PlayerData data = formatter.Deserialize(stream) as PlayerData; // data prend la valeur des donnees que l'on retrouve avec le stream
            stream.Close(); // on ferme le stream

            return data; // on retourne les informations du joueur
        }
        else{ // si aucune fichier n'existe au chemin path
            Debug.LogError("Save file not found in " + path); // message d'erreur, aucune fichier trouve
            return null; // on retourne rien
        }
    }

    /// <summary>
    /// Fonction public qui sauvegarde les informations du BasicStats
    /// </summary>
    /// <param name="stats">Reference au BasicStats</param>
    public static void SaveStats(BasicStats stats){
        string path = Application.persistentDataPath + "/stats.statsData"; // path represente le chemin pour acces au ficher des donnes du BasicStats
        BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
        FileStream stream = new FileStream(path, FileMode.Create); // stream cree un nouveau FileStream avec le path 

        StatsData data = new StatsData(stats); // data prend la valeur de retour du StatsData

        formatter.Serialize(stream, data); // on stock les informations de data via le stream
        stream.Close(); // on ferme le stream
    }

    /// <summary>
    /// Fonction publique qui charge les informations du BasicStats
    /// </summary>
    /// <returns>Les informations du BasicStats</returns>
    public static StatsData LoadStats(){
        string path = Application.persistentDataPath + "/stats.statsData"; // path represente le chemin pour acces au ficher des donnes du BasicStats
        if(File.Exists(path)){ // si un ficher existe au chemin path
            BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
            FileStream stream = new FileStream(path, FileMode.Open); // stream ouvre un nouveau FileStream avec le path

            StatsData data = formatter.Deserialize(stream) as StatsData; // data prend la valeur des donnees que l'on retrouve avec le stream
            stream.Close(); // on ferme le stream

            return data; // on retourne les informations du BasicStats
        }
        else{
            Debug.LogError("Save file not found in " + path); // message d'erreur, aucune fichier trouve
            return null; // on retourne rien
        }
    }

    /// <summary>
    /// Fonction publique qui sauvegarde un skill
    /// </summary>
    /// <param name="skill">reference a un SkillInfos</param>
    public static void SaveSkills(SkillInfos skill){
        BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
        string path = Application.persistentDataPath + "/skills.skillsData" + skill.nom; // path represente le chemin pour acces au ficher des donnes du skill
        FileStream stream = new FileStream(path, FileMode.Create); // stream cree un nouveau FileStream avec le path 

        SkillData data = new SkillData(skill); // data prend la valeur de retour du SkillData

        formatter.Serialize(stream, data); // on stock les informations de data via le stream
        stream.Close(); // on ferme le stream
    }

    /// <summary>
    /// fonction qui charge les information d'un skill
    /// </summary>
    /// <param name="nom">nom du skill</param>
    /// <returns></returns>
    public static SkillData LoadSkills(string nom){
        string path = Application.persistentDataPath + "/skills.skillsData" + nom; // path represente le chemin pour acces au ficher des donnes du skill
        if(File.Exists(path)){ // si un ficher existe au chemin path
            BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
            FileStream stream = new FileStream(path, FileMode.Open); // stream ouvre un nouveau FileStream avec le path

            SkillData data = formatter.Deserialize(stream) as SkillData; // data prend la valeur des donnees que l'on retrouve avec le stream
            stream.Close(); // on ferme le stream

            return data; // on retourne les informations du skill
        }
        else{
            Debug.LogError("Save file not found in " + path); // message d'erreur, aucune fichier trouve
            return null; // on retourne rien
        }
    }

    /// <summary>
    /// fonction publique qui sauvegarde les informations du SkillTree
    /// </summary>
    /// <param name="tree">Reference au SkillTree</param>
    public static void SaveTree(Skilltree tree){
        BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
        string path = Application.persistentDataPath + "/tree.treeData"; // path represente le chemin pour acces au ficher des donnes du skillTree
        FileStream stream = new FileStream(path, FileMode.Create); // stream cree un nouveau FileStream avec le path 

        TreeData data = new TreeData(tree); // data prend la valeur de retour de TreeData
        formatter.Serialize(stream, data); // on stock les informations de data via le stream
        stream.Close(); // on ferme le stream
    }

    /// <summary>
    /// Fonction qui charge les informations du SkillTree
    /// </summary>
    /// <returns>Les informations du SkillTree</returns>
    public static TreeData LoadTree(){
        string path = Application.persistentDataPath + "/tree.treeData"; // path represente le chemin pour acces au ficher des donnes du skillTree
        if(File.Exists(path)){ // si un ficher existe au chemin path
            BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
            FileStream stream = new FileStream(path, FileMode.Open); // stream ouvre un nouveau FileStream avec le path

            TreeData data = formatter.Deserialize(stream) as TreeData; // data prend la valeur des donnees que l'on retrouve avec le stream
            stream.Close(); // on ferme le stream

            return data; // on retourne les informations SkillTree
        }
        else{
            Debug.LogError("Save file not found in " + path); // message d'erreur, aucune fichier trouve
            return null; // on retourne rien
        }
    }

    /// <summary>
    /// Fonction publique qui sauvegarde les informations du Timer
    /// </summary>
    /// <param name="timer"></param>
    public static void SaveTime(Timer timer){
        BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
        string path = Application.persistentDataPath + "/time.timeData"; // path represente le chemin pour acces au ficher des donnes du timer
        FileStream stream = new FileStream(path, FileMode.Create); // stream cree un nouveau FileStream avec le path 

        TimeData data = new TimeData(timer); // data prend la valeur de retour de TimeData
        formatter.Serialize(stream, data); // on stock les informations de data via le stream
        stream.Close(); // on ferme le stream
    }

    /// <summary>
    /// Fonction qui charge les informations du Timer
    /// </summary>
    /// <returns>Les informations du Timer</returns>
    public static TimeData LoadTime(){
        string path = Application.persistentDataPath + "/time.timeData"; // path represente le chemin pour acces au ficher des donnes du timer
        if(File.Exists(path)){ // si un ficher existe au chemin path
            BinaryFormatter formatter = new BinaryFormatter(); // formatter devient un nouveau BinaryFormatter
            FileStream stream = new FileStream(path, FileMode.Open); // stream ouvre un nouveau FileStream avec le path

            TimeData data = formatter.Deserialize(stream) as TimeData; // data prend la valeur des donnees que l'on retrouve avec le stream
            stream.Close(); // on ferme le stream

            return data; // on retourne les informations du Timer
        }
        else{
            Debug.LogError("Save file not found in " + path); // message d'erreur, aucune fichier trouve
            return null; // on retourne rien
        }
    }

    /// <summary>
    /// Fonction qui supprime une sauvegarde
    /// </summary>
    public static void DeleteSave(){
        string pathPlayer = Application.persistentDataPath + "/player.playerData"; // pathPlayer represente le chemin pour acces au ficher des donnes du joueur
        File.Delete(pathPlayer); // on supprime le ficher au chemin pathPlayer
        if(!File.Exists(pathPlayer)){ // s'il n,exite pas de ficher au chemin pathPlayer
            Debug.Log("Il n'y a aucun document vers ce chemin"); // message d'avertissemnt
        }
        string pathStats = Application.persistentDataPath + "/stats.statsData"; // pathStats represente le chemin pour acces au ficher des donnes du BasicStats
        File.Delete(pathStats); // on supprime le ficher au chemin pathStats
        if(!File.Exists(pathPlayer)){ // s'il n,exite pas de ficher au chemin pathStats
            Debug.Log("Il n'y a aucun document vers ce chemin"); // message d'avertissemnt
        }
    }
}
