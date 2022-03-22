using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Tache : MonoBehaviour
{
    [Header("Objets de la tache")] // identification de la section Objets de la tache
    [SerializeField] GameObject _goTache; // bojet de la tache qui s'ouvre lors de l'interraction
    [SerializeField] GameObject _btnInterraction; // image du bouton qui indique au joueur la touche a appuyer
    [Header("Informations de la tache")] // identification de la section Informations de la tache
    [SerializeField] private TypeTache _typeTache; // Type de la tache
    [SerializeField] Sprite _imageDone; // image de la tache accompli
    [SerializeField] int _tacheValue; // valeur de la tache une fois accomplie
    public int tacheValue{ // acces public a la valeur de la tache une fois accomplie
        get=>_tacheValue; // par tacheValue, on retourne _tacheValue
    }
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonGenerationEnnemi; // son quand la tache des ennemis de la foret s'active
    [SerializeField] AudioClip _sonGenerationProjectiles; // son quand la tache des projectiles ennemis s'active

    private Personnage _perso; // reference au Personnage
    public Personnage perso{ // acces public au Personnage
        get => _perso; // par perso, on retourne _perso
        set{ // on change la valeur de _perso
            _perso = value; // _perso prend la valeur de value
        }
    }
    private bool _playerClose = false; // indicatif si le joueur est proche
    private bool _isDone = false; // indicatif si la tache est termine
    private bool _peutGenererEnnemi = true; // indicatif si la tache peut generer des ennemis

    Tutoriel _tuto; // reference au Tutoriel 
    public Tutoriel tuto{ // acces publique au Tutoriel
        get=>_tuto; // par tuto, on retourne _tuto
        set{ // on change al valeur de _tuto
            _tuto = value; // _tuto prend la valeur de value
        }
    }

    SpriteRenderer _sr; // reference au SpriteRenderer du gameObject
    Salle _salle; // reference a la Composante Salle du parent

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _salle = GetComponentInParent<Salle>(); // _salle represente la composante Salle dans le parent
        _sr = GetComponent<SpriteRenderer>(); // _sr prend la valeur de SpriteRenderer de la tache
        if(_goTache != null){ // si _goTache n'est pas null
            _goTache.SetActive(false); // on desactive _goTache
        }
        _btnInterraction.SetActive(false); // on desactive l'indicatif du bouton d'interraction
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isDone){ // si le tage du gameObject du collider entrant est Player
            GenerateurSalle _gensalle = _salle.genSalle; // _genSalle prend la valeur du genSalle de _salle
            if(_gensalle.tuto.dictTips["TipsTache"] == false){ // si le key TipsTache du dictionnaire dictTips est false
                _gensalle.tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
                _gensalle.tuto.OuvrirTips(4); // on demande au Tutoriel d'ouvrir le tips a l'index 4
            }
            _playerClose = true; // le player est proche
            _btnInterraction.SetActive(true); // on active l'indicatif du boton d'interraction
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le tage du gameObject du collider entrant est Player
            _playerClose = false; // le player n'est plus proche
            _btnInterraction.SetActive(false); // on desactive l'indicatif du bouton d'interraction
            if(_goTache != null){ // si _goTache n'est pas null
                _goTache.SetActive(false); // on desactive _goTache
            }
        }   
    }
    
    /// <summary>
    /// fonction qui permet d'ouvrir ou d'active la tache
    /// </summary>
    private void OuvrirTache(){
        if(_goTache != null){ // si _goTache n'est pas null
            if(_goTache.activeInHierarchy){ // si _goTache est actif
                _goTache.SetActive(false); // on desactive _goTache
                GetComponent<BoxCollider2D>().isTrigger = false; // on dit au BoxCollider2D de la tache qu'il n'est plus en mode trigger
            }
            else{ // si _goTache n'est pas actif
                _goTache.SetActive(true); // on active _goTache
                GetComponent<BoxCollider2D>().isTrigger = true; // on dit au BoxCollider2D de la tache qu'il est en mode trigger
            }
        }
        else{ // si _goTache est null
            switch(_typeTache){ // switch selon _typeTache
                case TypeTache.Ennemis : // si le type est Ennemis
                    if(_peutGenererEnnemi){ // si la tache peut generer des ennemis
                        if(_tuto.dictTips["TipsDestruction"] == false){ // si le key TipsDestruction du dictionnaire dictTips est false
                            _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
                            _tuto.OuvrirTips(7); // on demande au Tutoriel d'ouvrir le tips a l'index 7
                        }
                        GameAudio.instance.JouerSon(_sonGenerationEnnemi); // on joue un son quand la tache des ennemis de la foret s'active
                        _salle.GenererEnnemi(_tacheValue); // on dit a la salle de generer des ennemis
                        _peutGenererEnnemi = false; // la tache ne peut plus generer d'ennemis
                        GetComponentInChildren<Light2D>().intensity = 0; // on met la Light2D de la tache a intensite 0
                        _isDone = true; // la tache est termine
                        Animator anim = GetComponent<Animator>(); // anim represente l'Animator de la tache
                        anim.SetTrigger("FinTache"); // on declanche le trigger FinTache d'anim
                        _btnInterraction.SetActive(false); // on desactive l'indicatif du bouton d'interraction
                        GetComponent<BoxCollider2D>().isTrigger = true; // on dit au BoxCollider2D de la tache qu'il est en mode trigger
                    }
                    break; // on sort de la condition
                case TypeTache.Centre : // si le type est Centre
                        if(_tuto.dictTips["TipsCentre"] == false){ // si le key TipsCentre du dictionnaire dictTips est false
                            _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
                            _tuto.OuvrirTips(8); // on demande au Tutoriel d'ouvrir le tips a l'index 8
                        }
                    GameAudio.instance.JouerSon(_sonGenerationProjectiles); // on joue un son quand la tache des projectiles ennemis s'active
                    Transform sallePos = _salle.gameObject.transform; // sallePos prend la position de _salle 
                    perso.ChangerPos(sallePos); // on change la position du perso pour la position de la salle
                    perso.ChangerRot(true); // on dit au personnage qu'il peut tourner
                    perso.ChangerPourTour(true); // on dit au personnage de changer sa forme pour sa forme agressive
                    GetComponent<TacheCentre>().DemarrerSpawn(); // on demande a la composante TacheCentre de la tache de demarrer l'apparition des la generation des ennemis volant
                    break; // on sort de la condition
            }
        }   
    }

    /// <summary>
    /// fonction publique pour ajouter les ennemisVolant a la liste de projectiles de la salle
    /// </summary>
    /// <param name="projectile">ennemis a ajouter a la salle</param>
    public void AjouterProjectile(GameObject projectile){
        _salle.AjouterProjectile(projectile); // on demande a la salle d'ajouter l'ennemis a sa liste
    }

    /// <summary>
    /// fonction publique pour terminer la tache
    /// </summary>
    /// <param name="points">point a attribuer au personnage</param>
    public void FinirTache(int points){
        BasicStats basicStats = _salle.genSalle.basicStats; // basicStats prend la valeur du BasicStats du genSalle de _salle
        if(points > 0){ // si points est plus grand que 0
            float totalPoint = points + basicStats.npGain ; // totalPoint est egal a points + le npGain du BasicStats
            StartCoroutine(CoroutineFinTache((int)totalPoint));
        }
        perso.missionManager.AccomplirMission(TypeMission.Tache); // on demande au MissionManager d'accomplir une missions de type Tache
    }

    /// <summary>
    /// Coroutine pour terminer une tache
    /// </summary>
    /// <param name="points">point a attribuer au personnage</param>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineFinTache(int points){
        yield return new WaitForSeconds(0.5f); // on attend 0.5 seconde
        _isDone = true; // la tache est termine
        _sr.sprite = _imageDone; // on dit a l'image de la tache de devenir vide
        _perso.AjusterPoint("naturePoint", points, TypeTache.Tache); // demander au joueur de s'ajouter des points
        if(_goTache!=null){ // si _goTache n'est pas null
            _goTache.SetActive(false); // on desactive _goTache
        }
        GetComponent<BoxCollider2D>().isTrigger = false; // on dit au BoxCollider2D de la tache qu'il n'est plus en mode trigger
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){ // si le joueur appuie sur E et que le joueur est proche
            OuvrirTache(); // on appel OuvrirTache
        }
    }
    
}
