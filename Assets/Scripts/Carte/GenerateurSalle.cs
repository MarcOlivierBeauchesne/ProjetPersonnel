using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui s'occupe de la generation de la carte
/// </summary>
public class GenerateurSalle : MonoBehaviour
{
    [Header("Information de la carte")] // identification de la section Information de la carte
    [SerializeField] private int _nbSalle = 10; // nombre de salle a generer
    [SerializeField] int largeurMap = 10; // largeur de la carte
    [SerializeField] int hauteurMap = 10; // hauteur de la carte
    [SerializeField] int largeursalle = 26; // largeur d'une salle
    [SerializeField] int hauteursalle = 22; // hauteur d'une salle
    [SerializeField] private GameObject _firstSalle; // gameObject de la premiere salle
    [SerializeField] private GameObject[] _tSalleForet; // tableau des salle de foret
    [SerializeField] private GameObject[] _tSalleCoupe; // tableau des salles de deforestation
    [Header("References Managers")] // identification de la section References Managers
    [SerializeField] private Tutoriel _tuto; // reference au Tutoriel
    public Tutoriel tuto{ // acces public au Tutoriel
        get=> _tuto; // par tuto, on retourne _tuto
    }
    [SerializeField] private Timer _timer; // refenrence au Timer
    public Timer timer{ // acces public au Timer
        get=>_timer; // par timer, on retourne _timer
    }
    [SerializeField] DayLightManager _dayLightManager; // reference au DayLightManager
    [SerializeField] private TaskManager _taskManager; // reference au TaskManager
    public TaskManager taskManager{ // acces public au TaskManager
        get=>_taskManager; // par taskManager, on retourne _taskManager
    }
    [SerializeField] private Deforestation _deforestation; // reference a Deforestation
    public Deforestation deforestation{ // acces public a Deforestation
        get=>_deforestation; // par deforestation, on retourne _deforestation
    }
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    public BasicStats basicStats{ // acces public au BasicStats
        get=>_basicStats; // par basicStats, on retourne _basicStats
    }
    [Header("Objets dans la scene")]
    [SerializeField] private Animator _animLoading; // reference a l'animator de la fenetre de chargement
    [SerializeField] private Personnage _perso; // reference du personnage
    public Personnage perso { // acces public au personnage
        get=> _perso; // par perso, on retourne _perso
    }
    [SerializeField] GameObject _boiteProjectiles; // boite qui englobe les informations a afficher de la tacheCentre
    public GameObject boiteProjectiles{ // acces public a la boite qui englobe les informations a afficher de la tacheCentre
        get=>_boiteProjectiles; // par boiteProjectiles, on retourne _boiteProjectiles
    }
    [SerializeField] GameObject _boiteEnnemis; // boite qui englobe les informations a afficher pour les ennemis dans la foret
    public GameObject boiteEnnemis{ // acces public a la boite qui englobe les informations a afficher pour les ennemis dans la foret
        get=>_boiteEnnemis; // par boiteEnnemis, on retourne _boiteEnnemis
    }

    private float _pourcentage; // pourcentage de la carte couvert par al deforestation
    private int _qteSalleForet = 10; // quantite de salle de foret a generer
    private int _qteSalleCoupe = 10; // quantite de salle de deforestation a generer
    private List<GameObject> _listSalleTotal = new List<GameObject> { }; // liste des salles generees
    bool _firstDay = true; // bool pour indiquer que c'est le jour 1

    private /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _perso.ChangerEtat(false); // on dit au perso qu'il ne peut pas bouger
        StartCoroutine(CoroutineDemarrerJournee()); // on demarre la coroutine CoroutineDemarrerJournee
    }

    /// <summary>
    /// Coroutine qui cree un delai avant de generer la carte
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDemarrerJournee(){
        yield return new WaitForSeconds(1f); // attente de 1 seconde
        DemarrerCarte(); // on appel DemarrerCarte
    }
    
    /// <summary>
    /// Fonction qui calcule les informations necessaires pour la generation de la carte et supprime les salles presentes
    /// </summary>
    public void DemarrerCarte(){
        _pourcentage = _basicStats.deforestLevel; // le pourcentage prend la valeur de deforestLevel du BasicStats
        StartCoroutine(CoroutineClearRooms()); // on appel ClearRooms
        _qteSalleCoupe = Mathf.RoundToInt((_nbSalle * _pourcentage)/100); // le nombre de salle de deforestation prend la valeur en pourcentage selon le total de salle a generer
        _qteSalleForet = _nbSalle - _qteSalleCoupe; // la quantite de salle de forest est la balance du total de salle moins le nombre de salles de deforestation
    }

    /// <summary>
    /// Fonction qui detruit toutes les salles de la liste des salles generees
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineClearRooms(){
        foreach (GameObject salle in _listSalleTotal) // boucle pour chaque salle dans _listSalleTotal
        {
            Destroy(salle); // on detruit la salle
            yield return new WaitForSeconds(0.1f); // attente de 0.1 seconde
        }
        _listSalleTotal.Clear(); // par securite, on vide la liste
        StartCoroutine(CoroutineSpawnCarte()); // on demarre la coroutine CoroutineSpawnCarte
        _perso.gameObject.GetComponent<Plantage>().NettoyerArbre(); // on demande au joueur de supprimer tous les arbres qu'il a plantes
    }

    /// <summary>
    /// Fonction qui demande aux salles de supprimer tous les ennemis et projectiles encore presents 
    /// </summary>
    public void ClearTache(){
        foreach(GameObject salle in _listSalleTotal){ // pour chaque salle dans la liste de _listSalleTotal
            salle.GetComponent<Salle>().DetuireEnnemis(); // on demande a salle d'appeler DetruiresEnnemis
        }
    }

    /// <summary>
    /// Coroutine qui genere la carte et demarre la journee
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineSpawnCarte(){
        for (int x = 0; x < largeurMap; x++) // boucle tant que x est plus petit que largeurMap
        {
            for (int y = 0; y < hauteurMap; y++) // boucle tant que y est plus petit que hauteurMap
            {
                GameObject salle = null; // stockage du GameObject salle pour utilisation a venir
                int quelleSalle = Random.Range(0, 2); // on genere un nombre entre 0 et 1 pour le type de salle a generer
                int salleForetRand = Random.Range(0, _tSalleForet.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de foret pour choisir une salle a generer
                int salleCoupeRand = Random.Range(0, _tSalleCoupe.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de coupe pour choisir une salle a generer
                    if(x == largeurMap/2 && y == hauteurMap/2){ // si x et y sont respectivement a la moitie de largeurMap et de hauteurMap
                        salle = Instantiate(_firstSalle, new Vector2(x * largeursalle,y * hauteursalle), Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                        _perso.ChangerPos(salle.transform); // on change la position du perso pour la position de la salle
                    }
                    else{ // autre valeurs  qui ne sont pas la moitie de largeurMap ni de hauteurMap
                        if (quelleSalle == 0) // si quelleSalle donne 0 (salle foret)
                        {
                            if (_qteSalleForet >= 1) // si la quantite de salle de foret est superieur ou egal a 1
                            {
                                salle = Instantiate(_tSalleForet[salleForetRand], new Vector2(x * largeursalle,y * hauteursalle), Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                                _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                            }
                            else // si la quantite de salle de foret est inferieur a 1
                            {
                                salle = Instantiate(_tSalleCoupe[salleCoupeRand], new Vector2(x * largeursalle,y * hauteursalle), Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                                _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                            }
                        }
                        else if (quelleSalle == 1) // si quelleSalle donne 1 (salle coupe)
                        {
                            if (_qteSalleCoupe >= 1) // si la quantite de salle de coupe est superieur ou egal a 1
                            {
                                salle = Instantiate(_tSalleCoupe[salleCoupeRand], new Vector2(x * largeursalle,y * hauteursalle), Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                                _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                            }
                            else
                            {
                                salle = Instantiate(_tSalleForet[salleForetRand], new Vector2(x * largeursalle,y * hauteursalle), Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                                _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                            }
                        }
                    }
                salle.transform.SetParent(transform); // on dit a la salle que son parent devient le generateur de salles
                salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
                _listSalleTotal.Add(salle); // on ajoute salle a _listSalleTotal
                salle.transform.SetParent(transform); // on dit a la salle que son parent devient le generateur de salles
                yield return new WaitForSeconds(0.1f); // attente de 0.1 seconde
            }
        }
        
        _animLoading.SetBool("IsLoading", false); // on dit a _animLoading que son bool IsLoading est false
        _dayLightManager.AjusterVitesseJour(); // on demande a _dayLightManager d'ajuster la vitesse de la journee (cycle jour/nuit)
        OuvrirPorte(); // on appel OuvrirPore
        _perso.ChangerEtat(true); // on dit au perso qu'il peut bouger
        if(!_firstDay){ // si ce n'est pas la premiere journee
            _timer.ProchaineJournee(); // on appel ProchaineJournee du Timer
        }
        else{ // si c'est la premiere journee
            _timer.DemarrerJournee(); // on appel DemarrerJournee du Timer
            _firstDay = false; // _firstDay devient false (ce n'est plus la premiere journee)
        }
    }

    /// <summary>
    /// Fonction qui demande aux salle d'ouvrir leur porte et de placer les transition entre les salles
    /// </summary>
    private void OuvrirPorte(){
        foreach (GameObject salle in _listSalleTotal) // pour chaque salle dans _listSalleTotal
        {
            salle.GetComponent<Salle>().CreerDetecteurs(); // on appel CreerDetecteurs de Salle
            salle.GetComponent<Salle>().PlacerTransition(); // on appel PlacerTransition de Salle
        }
    }
}
