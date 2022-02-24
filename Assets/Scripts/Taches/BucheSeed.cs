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
    [SerializeField] private int _taskValue;
    private bool _isFull = false;
    private int _seedAmount = 0;
    private int _seedNeeded = 0;
    private bool _playerClose = false;
    // Start is called before the first frame update
    void Start()
    {
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
        Personnage player = transform.GetComponentInParent<GenerateurSalle>().perso.GetComponent<Personnage>();
        PlayerRessources resPlayer = player.ressourcesPlayer;
        Debug.Log("nombre de graine du joueur : " + resPlayer.seedAmount);
        if(resPlayer.seedAmount >= 1){
            _seedAmount++;
            player.AjusterPoint("seed", -1);
            AjusterSeedVisuel();
            if(_seedAmount == _seedNeeded){
                Debug.Log("Le tronc est plein");
                _isFull = true;
                GetComponent<SpriteRenderer>().color = Color.green;
                GetComponentInParent<GenerateurSalle>().taskManager.AjouterPoint(TypeTache.Tache, _taskValue);
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
