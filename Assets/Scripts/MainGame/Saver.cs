using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script qui controle la sauvegarde du jeu
/// </summary>
public class Saver : MonoBehaviour
{
    [SerializeField] Navigation _nav;
    [SerializeField] Personnage _player;
    [SerializeField] Collection _collection;
    [SerializeField] BasicStats _basicStats;
    [SerializeField] Deforestation _deforestation;
    [SerializeField] Skilltree _skillTree;
    [SerializeField] Timer _timer;
    [SerializeField] SkillInfos[] _tSkillInfos;

    private void Start()
    {
        if(_player != null){
            string path = Application.persistentDataPath + "/player.playerData";
            if(File.Exists(path)){
                LoadGame();
            }
            else{
                Debug.Log("Aucune sauvegarde");
            }
        }
    }

    public void SaveGame(bool quit){
        SaveSystem.SavePlayer(_player);
        SaveSystem.SaveStats(_basicStats);
        foreach (SkillInfos skill in _tSkillInfos)
        {
            SaveSystem.SaveSkills(skill);
        }
        SaveSystem.SaveTree(_skillTree);
        SaveSystem.SaveTime(_timer);

        if(quit){
            Time.timeScale = 1;
            _nav.ChangerScene(0);
        }
    }

    public void LoadGame(){
        LoadPlayer();
        LoadStats();
        LoadTree();
        LoadSkill();
        LoadTime();
    }

    private void LoadTime(){
        TimeData data = SaveSystem.LoadTime();
        _timer.SetupTime(data.nbJour, data.minute, data.seconde);
        Debug.Log("jour save : " + data.nbJour);
        Debug.Log("minute save : " + data.minute);
        Debug.Log("seconde : " + data.seconde);
    }

    private void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer();
        _player.AjusterPoint("seed", data.seed);
        _player.AjusterPoint("naturePoint", data.naturePoints);
        _player.AjusterPoint("naturePower", data.naturePower);
    }

    private void LoadStats(){
        StatsData data = SaveSystem.LoadStats();
        _basicStats.mouvementSpeed = data.speed;
        _basicStats.npGain = data.npGain;
        _basicStats.npMaxPool = data.npMaxPool;
        _basicStats.seedDrop = data.seedDrop;
        _basicStats.dayTime = data.daytime;
        _basicStats.deforestAugment = data.defoAugment;
        _basicStats.deforestLevel = data.defoLevel;
        _basicStats.deforestPool = data.defoPool;
        _deforestation.AjusterDefoVisuel();
    }

    private void LoadTree(){
        TreeData data = SaveSystem.LoadTree();
        _skillTree.absorbCount = data.absorbCount;
    }

    private void LoadSkill(){
        foreach (SkillInfos skill in _tSkillInfos)
        {
            SkillData data = SaveSystem.LoadSkills(skill.nom);
            skill.actualStack = data.actualStack;
            skill.skillCost = data.skillCost;
        }
        // foreach (SkillInfos skill in _tSkillInfos)
        // {
        //     skill.CheckDepend();
        // }
        
    }

    public void DeleteSave(){
        SaveSystem.DeleteSave();
        _collection.ResetCollection();
    }
}
