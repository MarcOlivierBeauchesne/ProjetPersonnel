using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] Personnage _perso;
    public Personnage perso{
        get=>_perso;
    }
    [SerializeField] List<GameObject> _listMission = new List<GameObject>();
    [SerializeField] Transform _parentMission;
    List<GameObject> _listMissionPossibles = new List<GameObject>();
    List<GameObject> _listMissionActives = new List<GameObject>();

    private int _nbMission = 0;
    private int _actualMission = 0;

    private void Start()
    {
        RemplirList();
    }

    public void InitierMission()
    {
        _nbMission = Random.Range(1, 4);
        StartCoroutine(CoroutineMission());
    }

    IEnumerator CoroutineMission(){
        yield return new WaitForSeconds(1f);
            int quelleMission = Random.Range(0, _listMissionPossibles.Count);
            GameObject mission = Instantiate(_listMissionPossibles[quelleMission], transform.position, Quaternion.identity);
            mission.transform.SetParent(_parentMission);
            mission.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
            MissionObjet missionObjet = mission.GetComponent<MissionObjet>();
            missionObjet.missionManager = this;
            missionObjet.missionInfos.missionTotal = 0;
            missionObjet.SetupChamps();
            _listMissionActives.Add(mission);
            _listMissionPossibles.Remove(_listMissionPossibles[quelleMission]);
            _actualMission++;
            if(_actualMission < _nbMission){
                StartCoroutine(CoroutineMission());
            }
    }

    private void RemplirList(){
        _listMissionPossibles = new List<GameObject>();
        foreach (GameObject mission in _listMission)
        {
            _listMissionPossibles.Add(mission);
        }
    }

    public void ResetDayMission(){
        RemplirList();
        foreach (GameObject mission in _listMissionActives)
        {
            Destroy(mission);
        }
        _listMissionActives.Clear();
    }

    public void AccomplirMission(TypeMission typeMission){
        foreach (GameObject mission in _listMissionActives)
        {
            MissionObjet missionObjet = mission.GetComponent<MissionObjet>();
            Mission missioninfos = missionObjet.missionInfos;
            if(typeMission == missioninfos.typeMission){
                missioninfos.missionTotal++;
                missionObjet.SetupChamps();
            }
        }
    }
}

public enum TypeMission
{
    Tache,
    Mimo,
    Arbre
}
