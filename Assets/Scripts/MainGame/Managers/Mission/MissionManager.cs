using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script qui gere l'affichage et la progression des missions et du tableau des mission
/// </summary>
public class MissionManager : MonoBehaviour
{
    [Header("Composantes du tableau de mission")] // identification de la section Composantes du tableau de mission
    [SerializeField] GameObject _fondMission; // fenetre des missions
    public GameObject fondMission{ // acces public a la fenetre des missions
        get=>_fondMission; // par fondMission, on retourne _fondMission
    }
    [SerializeField] Transform _parentMission; // GameObject parent qui accueil les missions
    [Header("Managers")] // identification de la section Managers
    [SerializeField] TaskManager _taskManager; // reference au TaskManager
    [SerializeField] Timer _timer; // reference au Timer
    public Timer timer{ // acces public au Timer
        get=>_timer; // par timer, on retourne _timer
    }
    [SerializeField] Personnage _perso; // reference au Personnage
    public Personnage perso{ // acces public au Personnage
        get=>_perso; // par perso, on retourne _perso
    }
    [Header("Liste des missions")] // identification de la section Liste des missions
    [SerializeField] List<GameObject> _listMission = new List<GameObject>(); // liste des missions disponibles
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonMissionAccompli; // son quand le joueur accompli une mission
    [SerializeField] AudioClip _sonMissionApparait; // son quand une mission apparait

    List<GameObject> _listMissionPossibles = new List<GameObject>(); // liste qui ajoute et enleve les missions possible au joueur (evite les doublons)
    List<GameObject> _listMissionActives = new List<GameObject>(); // liste des missions actuellement actives

    int _nbMission = 0; // nombre de mission qui seront active pour la journee
    int _actualMission = 0; // nombre de missions actives actuelle 

    Animator _animFondMission;// reference a l'animator de la fenetre des missions

    private void Start()
    {
        _animFondMission = _fondMission.GetComponent<Animator>(); // _animFondMission represente l'Animator de la fenetre de mission
        RemplirList(); // on appel RemplirList
    }

    /// <summary>
    /// Fonction qui determine le nombre de mission pour la journee et entame la generation de missions
    /// </summary>
    public void InitierMission()
    {
        _nbMission = Random.Range(1, 4); // _nbMission prend une valeur entre 1 et 3 
        StartCoroutine(CoroutineMission()); // on demarre la coroutine CoroutineMission
    }

    /// <summary>
    /// Coroutine qui fait apparaitre les missions dans le tableau des missions
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineMission(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
            GameAudio.instance.JouerSon(_sonMissionApparait); // on joue un son quand une mission apparait
            int quelleMission = Random.Range(0, _listMissionPossibles.Count); // quelleMission prend une valeur entre 0 et la longeur de la liste _listMissionPossibles
            GameObject mission = Instantiate(_listMissionPossibles[quelleMission], transform.position, Quaternion.identity); // on genere la mission _listMissionPossibles[quelleMission] et on la stock dans mission
            mission.transform.SetParent(_parentMission); // mission devient l'enfant de _parentMission
            mission.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f); // on reinitialise le scale de la mission
            MissionObjet missionObjet = mission.GetComponent<MissionObjet>(); // missionObjet represente le MissionObjet de mission
            missionObjet.missionManager = this; // le MissionManager de missionObjet fait reference a ce script
            missionObjet.missionInfos.missionTotal = 0; // le missionTotal du missionInfos de missionObjet est egal a 0
            missionObjet.SetupChamps(); // on appel SetupChamps du missionObjet
            _listMissionActives.Add(mission); // on ajoute la missions a la liste _listMissionActives
            _listMissionPossibles.Remove(_listMissionPossibles[quelleMission]); // on enleve _listMissionPossibles[quelleMission] de la liste _listMissionPossibles
            _actualMission++; // on augmente _actualMission de 1
            if(_actualMission < _nbMission){ // si _actualMission est plus petit que _nbMission
                StartCoroutine(CoroutineMission()); // on appel a nouveau la coroutine CoroutineMission
            }
    }

    /// <summary>
    /// Fonction qui rempli la liste _listMissionPossibles ave les missions presentes dans _listMission 
    /// </summary>
    private void RemplirList(){
        _listMissionPossibles = new List<GameObject>(); // pour chaque GameObject dans _listMission
        foreach (GameObject mission in _listMission)
        {
            _listMissionPossibles.Add(mission); // on ajoute la missions a la liste _listMissionPossibles
        }
    }

    /// <summary>
    /// Fonction qui reinitialise la liste des missions de la journee
    /// </summary>
    public void ResetDayMission(){
        RemplirList(); // on appel RemplirList
        foreach (GameObject mission in _listMissionActives) // pour chaque GameObject dans _listMissionActives
        {
            Destroy(mission); // on detruit la mission
        }
        _listMissionActives.Clear(); // on vide la liste _listMissionActives
    }

    /// <summary>
    /// fonction publique qui verifie si une action du joueur accompli une missions de la journee
    /// </summary>
    /// <param name="typeMission">type d'action effectue</param>
    public void AccomplirMission(TypeMission typeMission){
        foreach (GameObject mission in _listMissionActives) // pour chaque GameObject dans _listMissionActives
        {
            MissionObjet missionObjet = mission.GetComponent<MissionObjet>(); // missionObjet prend la valeur du MissionObjet de mission
            Mission missioninfos = missionObjet.missionInfos; // missioninfos prend la valeur du missionInfos de missionObjet
            if(typeMission == missioninfos.typeMission){ // si le type recu est le meme que le typeMission de missioninfos
                missioninfos.missionTotal++; // on augmente le missionTotal du missioninfos de 1
                missionObjet.SetupChamps(); // on met a jour le champs de la mission
                if(missioninfos.missionTotal>=missioninfos.missionAmount){ // si missionTotal de missioninfos est plus grand ou egal au missionAmount du missioninfos
                    StartCoroutine(CoroutineDetruireMission(mission)); // on demarre la coroutine CoroutineDetruireMission avec mission
                    GameAudio.instance.JouerSon(_sonMissionAccompli); // on joue un son quand le joueur accompli une mission
                }
            }
        }
    }

    /// <summary>
    /// Coroutine qui cree un delay avant de retirer et detruire la missions accomplie
    /// </summary>
    /// <param name="mission">la missions accomplie</param>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDetruireMission(GameObject mission){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        _listMissionActives.Remove(mission); // on retire la mission de la liste _listMissionActives
        if(_listMissionActives.Count <= 0){ // si la longeur de la liste _listMissionActives est plus petit ou egal a 0
            StartCoroutine(CoroutineVerifierMissionActives()); // on demarre la coroutine CoroutineVerifierMissionActives
        }
    }

    /// <summary>
    /// Coroutine qui effectue l'animation losque le tableau des missions est vide et desactive la fenetre des missions
    /// </summary>
    /// <returns>temps d'attemps</returns>
    IEnumerator CoroutineVerifierMissionActives(){
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        _animFondMission.SetTrigger("MissionVide"); // on dit a l'animator de la fenetre de missions d'enclancher le trigger MissionVide
        yield return new WaitForSeconds(1f); // temps d'attente de 1 seconde
        _fondMission.SetActive(false); // on desactive la fenetre de missions
    }
}

/// <summary>
/// Enum pour les type de missions possible
/// </summary>
public enum TypeMission
{
    Tache,
    Mimo,
    Arbre
}
