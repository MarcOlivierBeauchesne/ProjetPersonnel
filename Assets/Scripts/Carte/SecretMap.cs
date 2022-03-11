using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretMap : MonoBehaviour
{
    [SerializeField] GameObject _btnInterraction;
    [SerializeField] GameObject _secondDoor;
    [SerializeField] GameObject _secretTilemap;
    [SerializeField] int _secretCost = 0;

    Personnage _perso;

    private void Start()
    {
        _btnInterraction.SetActive(false);
        _perso = GetComponentInParent<Salle>().genSalle.perso.GetComponent<Personnage>();
    }

    bool _playerClose = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){
            if(_perso.ressourcesPlayer.naturePower >= _secretCost){
                _perso.AjusterPoint("naturePower", -_secretCost, TypeTache.Aucun);
                if(_secondDoor != null){
                    Destroy(_secondDoor);
                }
                Destroy(_secretTilemap);
                Destroy(gameObject);
            }
        }
    }
}
