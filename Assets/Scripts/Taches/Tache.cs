using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Tache : MonoBehaviour
{
    [SerializeField] GameObject _goTache;
    [SerializeField] GameObject _btnInterraction;
    [SerializeField] Sprite _imageDone;
    [SerializeField] int _tacheValue;
    public int tacheValue{
        get=>_tacheValue;
    }
    [SerializeField] private TypeTache _typeTache;
    private Personnage _perso;
    public Personnage perso{
        get => _perso;
        set{
            _perso = value;
        }
    }
    private bool _playerClose = false;
    private bool _isDone = false;
    private bool _peutGenererEnnemi = true;

    Tutoriel _tuto;
    public Tutoriel tuto{
        get=>_tuto;
        set{
            _tuto = value;
        }
    }

    SpriteRenderer _sr;
    Salle _salle;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _salle = GetComponentInParent<Salle>();
        _sr = GetComponent<SpriteRenderer>();
        if(_goTache != null){
            _goTache.SetActive(false);
        }
        _btnInterraction.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isDone){
            GenerateurSalle _gensalle = GetComponentInParent<Salle>().genSalle;
            if(_gensalle.tuto.dictTips["TipsTache"] == false){
                _gensalle.tuto.gameObject.SetActive(true);
                _gensalle.tuto.OuvrirTips(4);
            }
            _playerClose = true;
            _btnInterraction.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            _playerClose = false;
            _btnInterraction.SetActive(false);
            if(_goTache != null){
                _goTache.SetActive(false);
            }
        }   
    }

    private void OuvrirTache(){
        if(_goTache != null){
            if(_goTache.activeInHierarchy){
                _goTache.SetActive(false);
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else{
                _goTache.SetActive(true);
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        else{
            switch(_typeTache){
                case TypeTache.Ennemis :
                    if(_peutGenererEnnemi){
                        if(_tuto.dictTips["TipsDestruction"] == false){
                            _tuto.gameObject.SetActive(true);
                            _tuto.OuvrirTips(7);
                        }
                        GetComponentInParent<Salle>().GenererEnnemi(_tacheValue);
                        _peutGenererEnnemi = false;
                        GetComponentInChildren<Light2D>().intensity = 0;
                        _isDone = true;
                        Animator anim = GetComponent<Animator>();
                        anim.SetTrigger("FinTache");
                        _btnInterraction.SetActive(false);
                        GetComponent<BoxCollider2D>().isTrigger = true;
                    }
                    break;
                case TypeTache.Centre :
                        if(_tuto.dictTips["TipsCentre"] == false){
                            _tuto.gameObject.SetActive(true);
                            _tuto.OuvrirTips(8);
                        }
                    Transform sallePos = transform.GetComponentInParent<Salle>().gameObject.transform;
                    perso.ChangerPos(sallePos);
                    perso.ChangerRot(true);
                    perso.ChangerPourTour(true);
                    GetComponent<TacheCentre>().DemarrerSpawn();
                    break;
            }
        }   
    }

    public void AjouterProjectile(GameObject projectile){
        _salle.AjouterProjectile(projectile);
    }

    public void FinirTache(int points){
        float totalPoint = 0;
        BasicStats basicStats = GetComponentInParent<Salle>().genSalle.basicStats;
        if(points > 0){
            totalPoint = points + basicStats.npGain ;
            StartCoroutine(CoroutineFinTache((int)totalPoint));
        }
        perso.missionManager.AccomplirMission(TypeMission.Tache);
    }

    private IEnumerator CoroutineFinTache(int points){
        yield return new WaitForSeconds(0.5f);
        _isDone = true;
        _sr.sprite = _imageDone;
        _perso.AjusterPoint("naturePoint", points, TypeTache.Tache);
        _perso.taskManager.AjouterPoint(TypeTache.Tache, points);
        if(_goTache!=null){
            _goTache.SetActive(false);
        }
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){
            OuvrirTache();
        }
    }
    
}
