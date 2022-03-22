using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui genre le comportement d'une mission dans le tableau des missions
/// </summary>
public class MissionObjet : MonoBehaviour
{
    [Header("Setup de mission")] // identification de la section Setup de mission
    [SerializeField] int _minMissionAmount = 0; // montant minimum necessaire pour accomplir la missions
    [SerializeField] int _maxMissionAmount = 1; // montant maximum necessaire pour accomplir la missions
    [Header("Reference de missions")] // identification de la section Reference de missions
    MissionManager _missionManager; // reference au MissionManager
    public MissionManager missionManager{ // acces public au MissionManager
        get=>_missionManager; // par missionManager, on retourne _missionManager
        set{ // on change al valeur de _missionManager
            _missionManager = value; // _missionManager prend la valeur recu
        }
    }
    [SerializeField] Mission _missionInfos; // reference a Mission
    public Mission missionInfos{ // acces public a Mission
        get=>_missionInfos; // par missionInfos, on retourne _missionInfos
    }

    Animator _anim; // reference a l'animator du gameObject

    private void Start()
    {
        _missionInfos.missionAmount = (Random.Range(_minMissionAmount, _maxMissionAmount+1)*_missionManager.timer.nbJour); // le missionAmount du _missionInfos est aleatoire entre
        // _minMissionAmount et _maxMissionAmount multiplie par le nbJour de Timer
        _anim = GetComponent<Animator>(); // _anim reprensente le Animator du gameObject
        SetupMission(); // on appel SetupMission
    }

    /// <summary>
    /// Fonction qui affiche les informations de la mission qui apparait dans le tableau
    /// </summary>
    private void SetupMission(){
        Text txtTitre = transform.GetChild(0).gameObject.GetComponent<Text>(); // txtTitre represente le titre de la mission
        Text txtExpli = transform.GetChild(1).gameObject.GetComponent<Text>(); // txtExpli represente l'explication de la mission
        Text txtProgression = transform.GetChild(3).gameObject.GetComponent<Text>(); // txtProgression reprensente la progression de la mission
        txtTitre.text = _missionInfos.nomMission; // txtTitre affiche le nom de la mission
        txtExpli.text = _missionInfos.descriptionMission; // txtExpli explique le but a accomplir
        txtProgression.text = $"{_missionInfos.missionTotal}/{_missionInfos.missionAmount}"; // txtProgression affiche la progression du jour sur la mission
    }

    /// <summary>
    /// Fonction qui met a jour le champs de progression de la mission
    /// </summary>
    public void SetupChamps(){
        Text txtProgression = transform.GetChild(3).gameObject.GetComponent<Text>(); // txtProgression reprensente la progression de la mission
        txtProgression.text = $"{_missionInfos.missionTotal}/{_missionInfos.missionAmount}"; // on affiche le total de la mission par rapport au montant demande
        if(missionInfos.missionTotal == missionInfos.missionAmount){ // si missionTotal est plus grand ou egal a missionAmount
            StartCoroutine(CoroutineDestruction()); // on demarre la coroutine CoroutineDestruction
        }
    }

    /// <summary>
    /// Coroutine qui detruit la mission accompli et attribut les points au joueur
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDestruction(){
        Text txtExpli = transform.GetChild(1).gameObject.GetComponent<Text>(); // txtExpli represente l'explication de la mission
        txtExpli.color = Color.green; // on change la couleur du texte de txtExpli pour vert
        txtExpli.text = "Mission terminée!"; // on affiche au joueur que la mission est termine
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        int totalPoint = _missionInfos.rewardValue * _missionManager.timer.nbJour; // le totalPoint est egal a rewardValue multiplie par le nbJour de Timer
        _missionManager.perso.AjusterPoint("naturePoint", totalPoint, TypeTache.Mission); // on attribut des points au joueur avec totalPoint
        _anim.SetTrigger("FinMission"); // on declanche le trigger FinMission de _anim 
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        Destroy(gameObject); // on detruit la mission
    }
}
