using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script qui controle la sauvegarde du jeu
/// </summary>
public class Saver : MonoBehaviour
{
    [Header("Managers")] // Identification de la section Managers
    [SerializeField] Navigation _nav;
    [SerializeField] BasicStats _basicStats;
    [SerializeField] Deforestation _deforestation;
    [SerializeField] Skilltree _skillTree;
    [SerializeField] Timer _timer;
    [SerializeField] Personnage _player;
    [Header("Tableau des skills")] // Identification de la section Tableau des skills
    [SerializeField] SkillInfos[] _tSkillInfos;

    private void Start()
    {
        if(_player != null){ // si _player n'est pas null
            string path = Application.persistentDataPath + "/player.playerData"; // path prend le persistentDataPath et ajoute /player.playerData
            if(File.Exists(path)){ // s'il existe un fichier dans le path
                LoadGame(); // on appel LoadGame
            }
            else{ // s'il n'existe pas un fichier dans le path
                Debug.Log("Aucune sauvegarde"); // aucune sauegarde
            }
        }
    }

    /// <summary>
    /// Fonction qui sauvegarde les différents elements d'une partie
    /// </summary>
    /// <param name="quit">bool si on quitte une parti ou non</param>
    public void SaveGame(bool quit){
        SaveSystem.SavePlayer(_player); // on demande au SaveSystem de sauvegarder le joueur en lui envoyant le Personnage
        SaveSystem.SaveStats(_basicStats); // on demande au SaveSystem de sauvegarder les stats du jeu en lui envoyant le BasicStats
        foreach (SkillInfos skill in _tSkillInfos) // pour chaque SkillInfos dans _tSkillInfos
        {
            SaveSystem.SaveSkills(skill); // on demande au SaveSystem de sauvegarder le skill en lui envoyant le skill
        }
        SaveSystem.SaveTree(_skillTree); // on demande au SaveSystem de sauvegarder l'arbre des talent en lui envoyant l'arbre
        SaveSystem.SaveTime(_timer); // on demande au SaveSysteme de sauvegartder le Timer en lui envoyant le Timer

        if(quit){ // si on quitte la partie
            Time.timeScale = 1; // le temps prend sa vitesse normale
            _nav.ChangerScene(0); // on demande au Navigateur de nous ramener au menu principale
        }
    }

    /// <summary>
    /// Fonction qui charge les differents elements de sauvegarde d'une partie
    /// </summary>
    public void LoadGame(){
        LoadPlayer(); // on appel LoadPlayer
        LoadStats(); // on appel LoadStats
        LoadTree(); // on appel LoadTree
        LoadSkill(); // on appel LoadSkill
        LoadTime(); // on appel LoadTime
    }

    /// <summary>
    /// Fonction qui charge les informations sauvegardees du temps
    /// </summary>
    private void LoadTime(){
        TimeData data = SaveSystem.LoadTime(); // on stock les donnees du temps dans data
        _timer.SetupTime(data.nbJour, data.minute, data.seconde); // on attribut les differentes valeurs sauvegardees au temps
    }

    /// <summary>
    /// fonction qui charge les informations sauvegardees du joueur
    /// </summary>
    private void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer(); // on stock les donnees du joueur dans data
        _player.AjusterPoint("seed", data.seed, TypeTache.Aucun); // on charge le nombre de seed du joueur
        _player.AjusterPoint("naturePoint", data.naturePoints, TypeTache.Aucun); // on charge le nombre de point de nature du joueur
        _player.AjusterPoint("naturePower", data.naturePower, TypeTache.Aucun); // on charge le nombre de puissance naturelle du joueur
    }

    /// <summary>
    /// fonction qui charge les informations sauvegardees des stats de la partie
    /// </summary>
    private void LoadStats(){
        StatsData data = SaveSystem.LoadStats(); // on stock les donnees des stats de la partie dans data
        _basicStats.mouvementSpeed = data.speed; // on charge le mouvementSpeed des stats
        _basicStats.npGain = data.npGain; // on charge le npGain des stats
        _basicStats.npMaxPool = data.npMaxPool; // on charge le npMaxPool des stats
        _basicStats.seedDrop = data.seedDrop; // on charge le seedDrop des stats
        _basicStats.dayTime = data.daytime; // on chage le dayTime des stats
        _basicStats.deforestAugment = data.defoAugment; // on charge le defoAugment des stats
        _basicStats.deforestLevel = data.defoLevel; // on charge le defoLevel des stats
        _basicStats.deforestPool = data.defoPool; // on charge le defoPool des stats
        _deforestation.AjusterDefoVisuel(); // on demande a Deforestation d'ajuster son visuel
    }

    /// <summary>
    /// fonction qui charge les informations sauvegardees du skilltree
    /// </summary>
    private void LoadTree(){
        TreeData data = SaveSystem.LoadTree(); // on stock les donnees du skilltree dans data
        _skillTree.absorbCount = data.absorbCount; // on charge le absorbCount du skilltree
    }

    /// <summary>
    /// fonction qui charge les informations sauvegardees de chaque skill
    /// </summary>
    private void LoadSkill(){
        foreach (SkillInfos skill in _tSkillInfos) // pour chaque SkillInfos dans _tSkillInfos
        {
            SkillData data = SaveSystem.LoadSkills(skill.nom); // on charge les donnees du skill dans data
            skill.actualStack = data.actualStack; // on charge le actualStack du skill
            skill.skillCost = data.skillCost; // on charge le skillCost du skill
        }
    }

    /// <summary>
    /// fonction publique qui supprime les sauvegardes
    /// </summary>
    public void DeleteSave(){
        SaveSystem.DeleteSave(); // on demande au SaveSysteme de supprimer les sauvegardes
    }
}
