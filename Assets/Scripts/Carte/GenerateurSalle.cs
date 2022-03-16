using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurSalle : MonoBehaviour
{
    [SerializeField] private GameObject _firstSalle; // gameObject de la premiere salle
    [SerializeField] GameObject _boiteProjectiles;
    public GameObject boiteProjectiles{
        get=>_boiteProjectiles;
    }
    [SerializeField] GameObject _boiteEnnemis;
    public GameObject boiteEnnemis{
        get=>_boiteEnnemis;
    }
    [SerializeField] private Tutoriel _tuto;
    public Tutoriel tuto{
        get=> _tuto;
    }
    [SerializeField] private GameObject _canvas;
    public GameObject canvas{
        get=>_canvas;
    }
    [SerializeField] private Timer _timer;
    public Timer timer{
        get=>_timer;
    }
    [SerializeField] DayLightManager _dayLightManager;
    [SerializeField] private TaskManager _taskManager;
    public TaskManager taskManager{
        get=>_taskManager;
    }
    [SerializeField] private Deforestation _deforestation;
    public Deforestation deforestation{
        get=>_deforestation;
    }
    [SerializeField] private GameObject[] _tSalleForet; // tableau des salle de foret
    [SerializeField] private GameObject[] _tSalleCoupe; // tableau des salles de deforestation
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    [SerializeField] private Animator _animLoading;
    public BasicStats basicStats{
        get=>_basicStats;
    }
    [SerializeField] private GameObject _perso; // reference du personnage
    public GameObject perso {
        get=> _perso;
    }
    private float _pourcentage; // pourcentage de la carte couvert par al deforestation
    [SerializeField] private int _nbSalle = 10; // nombre de salle a generer
    private int _nbSalleRef = 10; // nombre de salle a generer
    private int _qteSalleForet = 10; // quantite de salle de foret a generer
    private int _qteSalleCoupe = 10; // quantite de salle de deforestation a generer
    private bool _salleOuverte = false; // bool pour savoir si les salles sont ouvertes
    private bool secondeVague = false;

    private List<GameObject> _listSalle = new List<GameObject> { }; // liste des salles generees
    private List<GameObject> _listSalleTotal = new List<GameObject> { }; // liste des salles generees
    private List<Vector2> _listPosDispo = new List<Vector2> { }; // liste des positions disponibles pour generer des salles

    int _salleSpawned = 0;
    int _indexSalle = 0;
    bool _firstDay = true;

    private /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _perso.GetComponent<Personnage>().ChangerEtat(false);
        StartCoroutine(CoroutineDemarrerJournee());
    }

    IEnumerator CoroutineDemarrerJournee(){
        yield return new WaitForSeconds(1f);
        DemarrerCarte();
    }

    /// <summary>
    /// Fonction qui detruit toutes les salles de la liste des salles generees
    /// </summary>
    IEnumerator CoroutineClearRooms(){
        for (int i = 0; i < 10; i++)
        {
            Destroy(_listSalleTotal[0]);
        }
        yield return new WaitForSeconds(1f);
        if(_listSalleTotal.Count>0){
            StartCoroutine(CoroutineClearRooms());
        }
        else{
            _listSalleTotal.Clear(); // par securite, on vide la liste
            _listSalle.Clear(); // par securite, on vide la liste
        }
    }

    public void ClearTache(){
        foreach(GameObject salle in _listSalle){ // pour chaque salle dans la liste de _listeSalle
            salle.GetComponent<Salle>().DetuireEnnemis();
        }
    }

    public void DemarrerCarte(){
        _salleSpawned = 0;
        _pourcentage = _basicStats.deforestLevel; // le pourcentage prend la valeur de deforestLevel du BasicStats
        StartCoroutine(CoroutineClearRooms()); // on appel ClearRooms
        _qteSalleCoupe = Mathf.RoundToInt((_nbSalle * _pourcentage)/100); // le nombre de salle de deforestaation prend la valeur en pourcentage selon le total de salle a generer
        _qteSalleForet = _nbSalle - _qteSalleCoupe; // la quantite de salle de forest est la balance du total de salle moins le nombre de salles de deforestation
        GameObject salle = Instantiate(_firstSalle, Vector3.zero, Quaternion.identity); // on genere la premiere salle
        salle.transform.SetParent(transform); // le generateur de salle devient le parent de la premiere salle
        _perso.GetComponent<Personnage>().ChangerPos(salle.transform);
        salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
        salle.GetComponent<Salle>().Scan();
        _listSalle.Add(salle);
        _listSalleTotal.Add(salle);
    }

    public void OuvrirCarte(List<Vector2> listPos){
        
        StartCoroutine(CoroutineSpawnCarte(listPos));
    }

    public IEnumerator CoroutineSpawnCarte(List<Vector2> listPos){
        if(_salleSpawned < _nbSalle){
            GameObject salle = null;
            foreach (Vector2 pos in listPos)
            {
                yield return new WaitForSeconds(0.1f);
                int quelleSalle = Random.Range(0, 2); // on genere un nombre entre 0 et 1 pour le type de salle a generer
                int salleForetRand = Random.Range(0, _tSalleForet.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de foret pour choisir une salle a generer
                int salleCoupeRand = Random.Range(0, _tSalleCoupe.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de coupe pour choisir une salle a generer
                if (quelleSalle == 0) // si quelleSalle donne 0 (salle foret)
                {
                    if (_qteSalleForet >= 1) // si la quantite de salle de foret est superieur ou egal a 1
                    {
                        salle = Instantiate(_tSalleForet[salleForetRand], pos, Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                        _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                    }
                    else // si la quantite de salle de foret est inferieur a 1
                    {
                        salle = Instantiate(_tSalleCoupe[salleCoupeRand], pos, Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                        _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                    }
                }
                else if (quelleSalle == 1) // si quelleSalle donne 1 (salle coupe)
                {
                    if (_qteSalleCoupe >= 1) // si la quantite de salle de coupe est superieur ou egal a 1
                    {
                        salle = Instantiate(_tSalleCoupe[salleCoupeRand], pos, Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                        _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                    }
                    else
                    {
                        salle = Instantiate(_tSalleForet[salleForetRand], pos, Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                        _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                    }
                }
                salle.transform.SetParent(transform); // on dit a la salle que son parent devient le generateur de salles
                salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
                _listSalle.Add(salle);
                _listSalleTotal.Add(salle);
                salle.transform.SetParent(transform); // on dit a la salle que son parent devient le generateur de salles
                salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
            }
            yield return new WaitForSeconds(0.01f);
            if(listPos.Count > 0){
                Debug.Log("la salle a des position");
                int randomSalle = Random.Range(0, _listSalle.Count);
                _listSalle[randomSalle].GetComponent<Salle>().Scan();
                _indexSalle++;
                _salleSpawned++;
                _listSalle.Clear();
            }
            else{
                Debug.Log("on demande a une autre salle de scan");
                int randomSalle = Random.Range(1,_listSalleTotal.Count);
                _listSalleTotal[randomSalle].GetComponent<Salle>().Scan();
            }
        }
        else{
            _animLoading.SetBool("IsLoading", false);
            _dayLightManager.AjusterVitesseJour();
            OuvrirPorte();
            _perso.GetComponent<Personnage>().ChangerEtat(true);
            if(!_firstDay){
                _timer.ProchaineJournee();
            }
            else{
                _timer.DemarrerJournee();
            }
            _firstDay = false;
        }
    }

    private void OuvrirPorte(){
        foreach (GameObject salle in _listSalleTotal)
        {
            salle.GetComponent<Salle>().CreerDetecteurs();
            salle.GetComponent<Salle>().PlacerTransition();
        }
    }
}
