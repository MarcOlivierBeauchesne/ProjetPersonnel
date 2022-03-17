using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucheSeed : MonoBehaviour
{
    [SerializeField] private int _minSeed;
    [SerializeField] private int _maxSeed;
    [SerializeField] private SpriteRenderer[] _tSeedVisual;
    [SerializeField] private Sprite _imgEmptySeed;
    [SerializeField] private Sprite _imgFullSeed;
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
            if(_genSalle.tuto.dictTips["TipsSouche"] == false){
                _genSalle.tuto.gameObject.SetActive(true);
                _genSalle.tuto.OuvrirTips(5);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isFull){
            _playerClose = false;
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
            _perso.AjusterPoint("seed", -1, TypeTache.Aucun);
            AjusterSeedVisuel();
            if(_seedAmount == _seedNeeded){
                _isFull = true;
                _genSalle.taskManager.AjouterPoint(TypeTache.Tache, _taskValue);
                int totalPoint = _seedAmount * _taskValue;
                GetComponent<Tache>().FinirTache(totalPoint);
            }
        }
        else{
            Debug.Log("Player n'a pas assez de seed");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose && !_isFull){
            AjouterSeed();
        }
    }
}
