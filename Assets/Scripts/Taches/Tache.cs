using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tache : MonoBehaviour
{
    [SerializeField] GameObject _goTache;
    [SerializeField] GameObject _btnInterraction;
    [SerializeField] Sprite _imageDone;
    [SerializeField] int _tacheValue;
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

    SpriteRenderer _sr;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _goTache.SetActive(false);
        _btnInterraction.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isDone){
            _playerClose = true;
            _btnInterraction.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            _playerClose = false;
            _btnInterraction.SetActive(false);
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
            if(_peutGenererEnnemi){
                GetComponentInParent<Salle>().GenererEnnemi(_tacheValue);
                _peutGenererEnnemi = false;
                _isDone = true;
                Animator anim = GetComponent<Animator>();
                anim.SetTrigger("FinTache");
                _btnInterraction.SetActive(false);
            }
        }   
    }

    public void FinirTache(int points){
        BasicStats basicStats = GetComponentInParent<Salle>().genSalle.basicStats;
        float totalPoint = (points + (basicStats.deforestLevel * basicStats.npGain))/2;
        StartCoroutine(CoroutineFinTache((int)totalPoint));
    }

    private IEnumerator CoroutineFinTache(int points){
        yield return new WaitForSeconds(1f);
        _isDone = true;
        _sr.sprite = _imageDone;
        _perso.AjusterPoint("naturePoint", points);
        _perso.taskManager.AjouterPoint(TypeTache.Tache, points);
        _perso.taskManager.CreatePopUpPoints(transform.position, points);
        _goTache.SetActive(false);
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){
            OuvrirTache();
        }
    }
}
