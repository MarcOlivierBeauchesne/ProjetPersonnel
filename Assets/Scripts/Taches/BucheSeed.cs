using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucheSeed : MonoBehaviour
{
    [SerializeField] private int _minSeed;
    [SerializeField] private int _maxSeed;
    [SerializeField] private GameObject _btnInterraction;
    [SerializeField] private SpriteRenderer[] _tSeedVisual;
    [SerializeField] private Sprite _imgEmptySeed;
    [SerializeField] private Sprite _imgFullSeed;
    [SerializeField] private Sprite _bucheFull;
    [SerializeField] private int _taskValue;
    private bool _isFull = false;
    private int _seedAmount = 0;
    private int _seedNeeded = 0;
    private bool _playerClose = false;

    SpriteRenderer _sr;
    GenerateurSalle _genSalle;
    Personnage _perso;
    PlayerRessources _resPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _genSalle = GetComponentInParent<GenerateurSalle>();
        _perso = _genSalle.perso.GetComponent<Personnage>();
        _resPlayer = _perso.ressourcesPlayer;
        _sr = GetComponent<SpriteRenderer>();
        _seedNeeded = CalculerSeedNeeded();
        AjusterSeedNeeded();
        _btnInterraction.SetActive(false);
    }

    private int CalculerSeedNeeded(){
        int seed = Random.Range(_minSeed, _maxSeed +1);
        return seed;
    }

    private void AjusterSeedNeeded(){
        for (int i = 0; i < _tSeedVisual.Length; i++)
        {
            if(i<_seedNeeded){
                _tSeedVisual[i].sprite = _imgEmptySeed;
            }
            else{
                _tSeedVisual[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isFull){
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

    private void AjusterSeedVisuel(){
        for (int i = 0; i < _seedAmount; i++)
        {
            _tSeedVisual[i].sprite = _imgFullSeed;
        }
    }

    private void AjouterSeed(){
        if(_resPlayer.seedAmount >= 1){
            _seedAmount++;
            _perso.AjusterPoint("seed", -1);
            AjusterSeedVisuel();
            if(_seedAmount == _seedNeeded){
                _isFull = true;
                _genSalle.taskManager.AjouterPoint(TypeTache.Tache, _taskValue);
                _sr.sprite = _bucheFull; 
                int totalPoint = (_seedAmount * _taskValue + (int)_perso.basicStats.npGain) + _seedAmount * (int)_perso.basicStats.npGain;
                _perso.AjusterPoint("naturePoint", totalPoint);
                _perso.taskManager.AjouterPoint(TypeTache.Tache, totalPoint);
                _genSalle.taskManager.CreatePopUpPoints(transform.position, totalPoint);
            }
        }
        else{
            Debug.Log("Player n'a pas assez de seed");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){
            AjouterSeed();
        }
    }
}
