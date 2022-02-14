using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skilltree : MonoBehaviour
{
    [SerializeField] SkillInfos[] _tSkills;
    [SerializeField] Button _boutonAbsorber;
    [SerializeField] PlayerRessources _ressourcePlayer;
    [SerializeField] GameObject _goSkillTree;
    private int absorbCost = 1000;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _goSkillTree.SetActive(false);
        CheckRessources();
    }

    public void VerifierFenetre() { 
        if(_goSkillTree.activeInHierarchy){
            _goSkillTree.SetActive(false);
        }
        else{
            _goSkillTree.SetActive(true);
        }
    }


    public void CheckRessources(){
        _boutonAbsorber.interactable = _ressourcePlayer.naturePoint >= absorbCost;
    }

    public void AbsorbSkills(){
        foreach (SkillInfos skill in _tSkills)
        {
            skill.ResetSkill();
            skill.CheckDepend();
        }
    }

    public void SaveSkill(){
        foreach (SkillInfos skill in _tSkills)
        {
            skill.SaveSkill();
        }
    }

    public void LoadSkill(){
        foreach (SkillInfos skill in _tSkills)
        {
            skill.LoadSkill();
            skill.CheckDepend();
        }
    }
}
