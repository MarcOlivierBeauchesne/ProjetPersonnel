using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjet : MonoBehaviour
{
    MissionManager _missionManager;
    public MissionManager missionManager{
        get=>_missionManager;
        set{
            _missionManager = value;
        }
    }
    [SerializeField] Mission _missionInfos;
    public Mission missionInfos{
        get=>_missionInfos;
    }

    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        SetupMission();
    }

    private void SetupMission(){
        Text txtTitre = transform.GetChild(0).gameObject.GetComponent<Text>();
        Text txtExpli = transform.GetChild(1).gameObject.GetComponent<Text>();
        Text txtProgression = transform.GetChild(3).gameObject.GetComponent<Text>();
        txtTitre.text = _missionInfos.nomMission;
        txtExpli.text = _missionInfos.descriptionMission;
        txtProgression.text = $"{_missionInfos.missionTotal}/{_missionInfos.missionAmount}";
    }

    public void SetupChamps(){
        Text txtProgression = transform.GetChild(3).gameObject.GetComponent<Text>();
        txtProgression.text = $"{_missionInfos.missionTotal}/{_missionInfos.missionAmount}";
        if(missionInfos.missionTotal >= missionInfos.missionAmount){
            StartCoroutine(CoroutineDestruction());
        }
    }

    IEnumerator CoroutineDestruction(){
        Text txtExpli = transform.GetChild(1).gameObject.GetComponent<Text>();
        txtExpli.color = Color.green;
        txtExpli.text = "Mission terminée!";
        yield return new WaitForSeconds(1f);
        _missionManager.perso.AjusterPoint("naturePoint", _missionInfos.rewardValue, TypeTache.Aucun);
        _anim.SetTrigger("FinMission");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}