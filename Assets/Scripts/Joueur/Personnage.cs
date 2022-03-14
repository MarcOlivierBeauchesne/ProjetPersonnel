using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui controle le deplacement du perso et met a jour ses ressources
/// </summary>
public class Personnage : MonoBehaviour
{
    [SerializeField] private Text _txtNaturePoint; // acces prive pour le champs de texte des points de nature du joueur
    [SerializeField] private Text _txtNaturePower; // acces prive pour le champs de texte de la puissance naturelle du joueur
    [SerializeField] private Text _txtSeed;
    [SerializeField] Timer _timer;
    public Timer timer{
        get=>_timer;
    }
    [SerializeField] private PlayerRessources _ressourcesPlayer; // reference de PlayerRessources du joueur
    public PlayerRessources ressourcesPlayer{
        get=> _ressourcesPlayer;
    }
    [SerializeField] private TaskManager _taskManager;
    public TaskManager taskManager{
        get=>_taskManager;
    }
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    public BasicStats basicStats{
        get=>_basicStats;
    }

    private bool _peutBouger = true;
    public bool peutBouger{
        get => _peutBouger;
        set{
            _peutBouger = value;
        }
    }
    private float _mouvementSpeed = 5; // acces prive pour _mouvementSpeed
    private float _axeX = 0f; // acces prive pour _axeX du Input Horizontal
    private float _axeY = 0f; // acces prive pour _axeY du Input Vertical
    Vector3 normalOri = new Vector3(0.2f,0.2f,0.2f);
    Vector3 reversedOri = new Vector3(-0.2f,0.2f,0.2f);

    Rigidbody2D _rb; // on stoc le rigidBody2D
    Animator _anim;
    SpriteRenderer _sr;
    Plantage _pl;
    Light _leafLight;
    Light _areaLight;
    // Start is called before the first frame update
    void Start()
    {
        _ressourcesPlayer.seedAmount = 0;
        _ressourcesPlayer.naturePoint = 0;
        _ressourcesPlayer.naturePower = 1000;
        _ressourcesPlayer.naturePowerPool = (int)_basicStats.npMaxPool;
        _areaLight = transform.GetChild(1).gameObject.GetComponent<Light>();
        _leafLight = transform.GetChild(2).gameObject.GetComponent<Light>();
        _pl = GetComponent<Plantage>();
        _anim = GetComponent<Animator>(); // anim s'associr au AnimatorController du perso
        _rb = GetComponent<Rigidbody2D>(); // _rb s'associe au RigidBody 2D du perso
        _sr = GetComponent<SpriteRenderer>(); // _sr s'associe au SpriteRenderer du perso
        _mouvementSpeed = _basicStats.mouvementSpeed; // _mouvementSped devient la valeur du mouvementSpeed du BasicStats
        _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString(); // on affiche les points de nature dans le champs approprie
        _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on affiche la puissance naturelle dans le champs approprie
        _txtSeed.text = _ressourcesPlayer.seedAmount.ToString();
    }

    /// <summary>
    /// Fonction qui ajuste les points de nature, de puissance et de graines du joueur et 
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
                if(_ressourcesPlayer.naturePower > 10000){
                    _txtNaturePower.fontSize = 12;
                }
                else{
                    _txtNaturePower.fontSize = 14;
                }
                Debug.Log("on change la puissance naturelle");
                break; // on sort de la condition
            case "seed": // si la ressources est "seed"
                _ressourcesPlayer.seedAmount += valeur; // on change la quantite de graine du joueur
                _txtSeed.text = _ressourcesPlayer.seedAmount.ToString();
                break; // on sort de la condition
            case "naturePoint": // si la ressources est "naturePoint"
                _ressourcesPlayer.naturePoint += valeur; // on change les naturePoint du _ressourcesPlayer selon la valeur
                _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString();
                _taskManager.AjouterPoint(type, valeur);
                _taskManager.CreatePopUpPoints(transform.position, valeur, "tache");
                if(type == TypeTache.Mimo){
                    AjusterPoint("naturePower", valeur/10, TypeTache.Aucun);
                }
                break; // on sort de la condition
        }
    }

    public void AjusterNaturePowerPool(){
        _ressourcesPlayer.naturePowerPool = (int)_basicStats.npMaxPool;
    }

    public void ChangerEtat(bool etat){
        StartCoroutine(CoroutineChangerEtat(etat));
    }

    public void ChangerRot(bool tourne){
        GetComponent<ControleCentre>().peutTourner = tourne;
    }

    public void ChangerPourTour(bool versTour){
        StartCoroutine(CoroutineVersTour(versTour));
    }

    IEnumerator CoroutineVersTour(bool versTour){
        if(versTour){
            _peutBouger = false;
            _anim.SetTrigger("ToTower");
        }
        else{
            _anim.SetTrigger("FromTower");
        }
        yield return new WaitForSeconds(0.5f);
        if(!versTour){
            _peutBouger = true;
        }
    }

    public void ChangerPos(Transform newPos){
        transform.position = newPos.position;
        _rb.velocity = Vector2.zero;
    }

    public void ResetRot(){
        transform.rotation = Quaternion.Euler(0f,0f,0f);
    }

    public IEnumerator CoroutineChangerEtat(bool etat){
        float waitTime = 0f;
        if(etat){
            _anim.SetTrigger("GoMove");
            waitTime = 1.5f;
        }
        else{
            waitTime = 0f;
            _rb.velocity = Vector2.zero;
            _peutBouger = etat;
        }
        yield return new WaitForSeconds(waitTime);
        if(!etat){
            _anim.SetTrigger("StopMove");
        }
        else{
            _peutBouger = etat;
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
        if(_peutBouger){
            Mouvement(); // on appel Mouvement
        }
        if(Input.GetKeyDown(KeyCode.Space) && _peutBouger){
            if(_ressourcesPlayer.seedAmount >=1){
                _pl.PlanterArbre();
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(_peutBouger){
            _rb.velocity = new Vector2(_axeX * _mouvementSpeed, _axeY * _mouvementSpeed); // on bouge le RigidBody2D du perso ave un Vector2 de _axeX et _axeY multiplie par la _mouvementSpeed
        }
    }

}
