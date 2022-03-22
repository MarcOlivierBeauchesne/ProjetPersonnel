using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Script qui controle le deplacement du perso et met a jour ses ressources
/// </summary>
public class Personnage : MonoBehaviour
{
    [Header("Champs texte des ressources")] // identification de la section Champs texte des ressources
    [SerializeField] private Text _txtNaturePoint; // acces prive pour le champs de texte des points de nature du joueur
    [SerializeField] private Text _txtNaturePower; // acces prive pour le champs de texte de la puissance naturelle du joueur
    [SerializeField] private Text _txtSeed;
    [Header("References Manager et ressources")] // identification de la section References Manager et ressources
    [SerializeField] Timer _timer; // reference au Timer
    public Timer timer{ // acces public au Timer
        get=>_timer; // par timer, on retourne _timer
    }
    [SerializeField] private TaskManager _taskManager; // reference au TaskManager
    public TaskManager taskManager{ // acces public au TaskManager
        get=>_taskManager; // par taskManager, on retourne _taskManager
    }
    [SerializeField] MissionManager _missionManager; // reference au MissionManager
    public MissionManager missionManager{ // acces public au MissionManager
        get=>_missionManager; // par missionManager, on retourne _missionManager
    }
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    public BasicStats basicStats{ // acces public au BasicStats
        get=>_basicStats; // par basicStats, on retourne _basicStats
    }
    [SerializeField] private PlayerRessources _ressourcesPlayer; // reference de PlayerRessources du joueur
    public PlayerRessources ressourcesPlayer{ // acces public au PlayerRessources
        get=> _ressourcesPlayer; // par ressourcesPlayer, on retourne _ressourcesPlayer
    }
    [SerializeField] AudioClip _sonPersoVersPlante; // son quand le joueur se transforme en plante
    [SerializeField] AudioClip _sonPlanteVersPerso; // son quand le joueur se transforme en hero
    [SerializeField] AudioClip[] _tSonMarche; // son quand le joueur marche


    private bool _peutBouger = true; // bool si le personnage peut bouger
    public bool peutBouger{ // acces public a _peutBouger
        get => _peutBouger; // par peutbouger, on retourne _peutBouger
        set{ // on change la valeur de _peutBouger
            _peutBouger = value; // _peutBouger prend la valeur de value
        }
    }
    private float _mouvementSpeed = 5; // acces prive pour _mouvementSpeed
    private float _axeX = 0f; // acces prive pour _axeX du Input Horizontal
    private float _axeY = 0f; // acces prive pour _axeY du Input Vertical
    Vector3 normalOri = new Vector3(0.2f,0.2f,0.2f); // orientation vers la droite du personnage
    Vector3 reversedOri = new Vector3(-0.2f,0.2f,0.2f); // orientation vers la gauche du personnage

    Rigidbody2D _rb; // on stock le Rigidbody2D
    Animator _anim; // on stock l'Animator
    SpriteRenderer _sr; // on stock le SpriteRenderer
    Plantage _pl; // on stock le Plantage
    Light2D _leafLight; // on stock la Light2D de la feuille
    Light2D _areaLight; // on stock la Light2D de zone d'eclairage
    // Start is called before the first frame update
    void Start()
    {
        _ressourcesPlayer.seedAmount = 0; // le seedAmount de _ressourcesPlayer est egal a 0
        _ressourcesPlayer.naturePoint = 0; // le naturePoint de _ressourcesPlayer est egal a 0
        _ressourcesPlayer.naturePower = (int)_basicStats.npMaxPool; // le naturePower de _ressourcesPlayer est egal a npMaxPool du basicStats
        _ressourcesPlayer.naturePowerPool = (int)_basicStats.npMaxPool;// le naturePowerPool de _ressourcesPlayer est egal a npMaxPool du basicStats
        _areaLight = transform.GetChild(1).gameObject.GetComponent<Light2D>(); // _areaLight devient le Light2D du 2em enfant du personnage
        _leafLight = transform.GetChild(2).gameObject.GetComponent<Light2D>(); // _areaLight devient le Light2D du 3em enfant du personnage
        _pl = GetComponent<Plantage>(); // _pl devient la composante Plantage du personnage
        _anim = GetComponent<Animator>(); // anim s'associr au AnimatorController du perso
        _rb = GetComponent<Rigidbody2D>(); // _rb s'associe au RigidBody 2D du perso
        _sr = GetComponent<SpriteRenderer>(); // _sr s'associe au SpriteRenderer du perso
        _mouvementSpeed = _basicStats.mouvementSpeed; // _mouvementSped devient la valeur du mouvementSpeed du BasicStats
        _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString(); // on affiche les points de nature dans le champs approprie
        _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on affiche la puissance naturelle dans le champs approprie
        _txtSeed.text = _ressourcesPlayer.seedAmount.ToString(); // on affiche le seedAmount des _ressourcesPlayer
    }

    /// <summary>
    /// Fonction publique qui ajuste les points de nature, de puissance et de graines du joueur et 
    /// met a jour le champs de texte apprporie
    /// </summary>
    /// <param name="ressources">Type de ressources que l'on modifie</param>
    /// <param name="valeur">valeur que l'on doit ajuster a la ressources</param>
    public void AjusterPoint(string ressources , int valeur, TypeTache type){
        switch (ressources) // selon le type de ressource
        {
            case "naturePower": // si la ressource est "naturePower"
                _ressourcesPlayer.naturePower += valeur; // on change le naturePower des _ressourcesPlayer selon la valeur
                _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on met a jour l'affichage des point de nature
                if(_ressourcesPlayer.naturePower > 9999){ // si le naturePower de _ressourcesPlayer est plus grand que 9999
                    _txtNaturePower.fontSize = 20; // la fontSize de _txtNaturePower est egal a 20
                }
                else{ // si le naturePower de _ressourcesPlayer est plus petit que 9999
                    _txtNaturePower.fontSize = 24; // la fontSize de _txtNaturePower est egal a 24
                }
                break; // on sort de la condition
            case "seed": // si la ressources est "seed"
                _ressourcesPlayer.seedAmount += valeur; // on change la quantite de graine du joueur
                _txtSeed.text = _ressourcesPlayer.seedAmount.ToString(); // on met a jour le champs qui affiche la quantite de noix
                break; // on sort de la condition
            case "naturePoint": // si la ressources est "naturePoint"
                _ressourcesPlayer.naturePoint += valeur; // on change les naturePoint du _ressourcesPlayer selon la valeur
                _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString(); // on met a jour le champs _txtNaturePoint qui affiche les naturePoint
                if(_ressourcesPlayer.naturePoint > 9999){ // si le naturePoint de _ressourcesPlayer est plus grand que 9999
                    _txtNaturePoint.fontSize = 20; // la fontSize de _txtNaturePoint est egal a 20
                }
                else{ // si le naturePoint de _ressourcesPlayer est plus petit que 9999
                    _txtNaturePoint.fontSize = 24; // la fontSize de _txtNaturePoint est egal a 24
                }
                _taskManager.AjouterPoint(type, valeur); // on demande au TaskManager d'ajouter des points selon le typ et valeur
                _taskManager.CreatePopUpPoints(transform.position, valeur, "tache"); // on demande au TaskManager de creer un PopUp des points recu 
                if(type == TypeTache.Mimo){ // si le type est mimo
                    AjusterPoint("naturePower", valeur/10, TypeTache.Aucun); // on ajoute au joueur la valeur des points recu divise par 10 en points de puissance
                }
                break; // on sort de la condition
        }
    }

    /// <summary>
    /// Fonction qui joue un son quand le joueur marche
    /// </summary>
    /// <param name="indexSon">index du son a jouer</param>
    public void JouerSonMarche(int indexSon){
        GameAudio.instance.JouerSon(_tSonMarche[indexSon]); // on joue un son quand le joueur marche
    }

    /// <summary>
    /// Fonction publique qui ajuste la luminosite de la lumiere de la feuille du perso
    /// </summary>
    /// <param name="intensity">Direction que l'on veut faire prendre a la lumiere (up ou down)</param>
    public void AjusterLeafLight(string intensity){
        switch(intensity){ // switch selon la valeur d'intensity
            case "up" : // si intensity est ip
                while(_leafLight.intensity < 1){ // tant que l'intensite de la lumiere est plus petite que 1
                    _leafLight.intensity += Time.deltaTime/1000; // on augmente l'intensite de la lumiere de la feuille
                }
            break; // on sort du switch
            case "down" : // si intensity est down
                while(_leafLight.intensity > 0){ // tant que l'intensite de l lumuere est plus grande que 0
                    _leafLight.intensity -= Time.deltaTime/1000; // on reduit l'intensite de la lumierer de la feuille
                }
            break; // on sort du switch
        }
    }

    /// <summary>
    /// fonction publique qui permet au joueur d'ajuster le maximum de puissance naturelle du joueur
    /// </summary>
    public void AjusterNaturePowerPool(){
        _ressourcesPlayer.naturePowerPool = (int)_basicStats.npMaxPool; // on met a jour le maximum de puissance naturelle avec la valeur du npMaxPool de _basicStats
    }

    /// <summary>
    /// fonction publique qui permet d'entamer le changement d'etat du joueur
    /// </summary>
    /// <param name="hero">bool si le joueur prend sa forme de hero ou de fleur</param>
    public void ChangerEtat(bool hero){
        StartCoroutine(CoroutineChangerEtat(hero)); // on appel la coroutine CoroutineChangerEtat en envoyant hero
    }

    /// <summary>
    /// Fonction publique qui permet de dire au joueur s'il peut tourner ou non
    /// </summary>
    /// <param name="tourne">bool si le joueur peut tourner</param>
    public void ChangerRot(bool tourne){
        GetComponent<ControleCentre>().peutTourner = tourne; // on accede a ControleCentre et on change sa valeur peutTourner pour tourne
    }

    /// <summary>
    /// fonction publique qui permet d'entamer le changement de forme du joueur vers ou de sa forme agressive
    /// </summary>
    /// <param name="versTour">bool si on change vers la forme tourelle ou non</param>
    public void ChangerPourTour(bool versTour){
        StartCoroutine(CoroutineVersTour(versTour)); // on demarre la coroutine CoroutineVersTour en envoyant versTour
    }

    /// <summary>
    /// Coroutine qui change la forme du joueur vers sa forme de tourelle ou vers sa forme hero
    /// </summary>
    /// <param name="versTour">bool si on change la forme du joueur vers sa forme tourelle ou hero</param>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineVersTour(bool versTour){
        if(versTour){ // si versTour est vrai
            _peutBouger = false; // le joueur ne peut plus bouger
            _anim.SetTrigger("ToTower"); // on demande au Animator du perso de changer sa forme vers la tourelle
            GameAudio.instance.JouerSon(_sonPersoVersPlante); // on joue un son quand le joueur se transforme en sa forme agressive
        }
        else{ // si versTour est faux
            _anim.SetTrigger("FromTower"); // on demande au Animator du pero de changer sa forme vers sa forme hero
            GameAudio.instance.JouerSon(_sonPlanteVersPerso); // on joue un son quand le joueur se transforme en hero
        }
        yield return new WaitForSeconds(0.5f); // attente de 0.5 seconde
        if(!versTour){ // si versTour est faux
            _peutBouger = true; // le joueur peut bouger
        }
    }

    /// <summary>
    /// Fonction publique pour changer la position du personnage
    /// </summary>
    /// <param name="newPos">La nouvelle position du joueur</param>
    public void ChangerPos(Transform newPos){
        transform.position = newPos.position; // on change la position du joueur pour la position newPos recu
        _rb.velocity = Vector2.zero; // on change la velocite du Rigibody2D du perso pour 0,0
    }

    /// <summary>
    /// Fonction publique pour reinitialiser la rotation du joueur
    /// </summary>
    public void ResetRot(){
        transform.rotation = Quaternion.Euler(0f,0f,0f); // on remet le joueur droit avec aucune rotation
    }

    /// <summary>
    /// Coroutine qui permet de changer la forme du joueur en hero ou en plante
    /// </summary>
    /// <param name="hero">bool si on change vers al forme hero ou non</param>
    /// <returns> temps d'attente</returns>
    IEnumerator CoroutineChangerEtat(bool hero){
        float waitTime = 0f; // float qui represente le temps d'attente dans la coroutine
        if(hero){// si hero est vrai
            _anim.SetTrigger("GoMove"); // on demande a l'animator du joueur d'activer le trigger GoMove (transformation en hero)
            waitTime = 1.5f; // le temps d'attente sera de 1.5 seconde
        }
        else{ // si hero est false
            waitTime = 0f; // le temps d'attente sera de 0 seconde
            _rb.velocity = Vector2.zero; // on change la velocite du Rigibody2D du perso pour 0,0
            _peutBouger = hero; // _peutBouger prend la valeur de hero (false)
        }
        yield return new WaitForSeconds(waitTime); // temps d'attente selon waitTime
        if(!hero){ // si hero est false
            _anim.SetTrigger("StopMove"); // on demande a l'animator du joueur d'activer le trigger StopMove (transformation en fleur)
        }
        else{ // si hero est vrai
            _peutBouger = hero; // _peutBouger prend la valeur de hero (true)
        }
    }

    /// <summary>
    /// Fonction public pour mettre a jour la vitesse de deplacement du joueur
    /// </summary>
    public void ModifierMoveSpeed(){
        _mouvementSpeed = _basicStats.mouvementSpeed; // _mouvementSped devient la valeur du mouvementSpeed du BasicStats
    }

    /// <summary>
    /// Fonction qui met prend les valuers des Input Horizontal et Vertical du joueur
    /// </summary>
    private void Mouvement(){
        _axeX = Input.GetAxisRaw("Horizontal"); // _axeX prend la valeur du Input de l'axe Horizontal
        _axeY = Input.GetAxisRaw("Vertical"); // _axeY prend la valeur du Input de l'axe Vertical
        bool enMouvement = _axeX != 0 || _axeY != 0; // enMouvement est true si le personnage bouge
        _anim.SetBool("Move", enMouvement); // on met le bool "Move" selon enMouvement
        if(_axeX < 0){ // si _axeX est plus petit que 0
            transform.localScale = reversedOri; // on inverse l'orientation du perso en X
        }
        else if(_axeX > 0){ // si _axeX est plus grand que 0
            transform.localScale = normalOri; // on inverse pas l'orientation du perso en x
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(_peutBouger){ // si _peutBouger est vrai
            Mouvement(); // on appel Mouvement
        }
        if(Input.GetKeyDown(KeyCode.Space) && _peutBouger){ // si le joueur appuie sur Space et que _peutBouger est vrai
            if(_ressourcesPlayer.seedAmount >=1){ // si le seedAmount de _ressourcesPlayer est plus grand ou egal a 1
                _pl.PlanterArbre(); // on appel PlanterArbre de Plantage
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(_peutBouger){ // si _peutBouger est vrai
            _rb.velocity = new Vector2(_axeX * _mouvementSpeed, _axeY * _mouvementSpeed); // on bouge le RigidBody2D du perso ave un Vector2 de _axeX et _axeY multiplie par la _mouvementSpeed
        }
    }

}
