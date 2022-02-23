using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string _nomObjet;
    [SerializeField] private InfosCollection[] _tInfosCollect;

    SpriteRenderer _sr;
    InfosCollection _mimo;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        ChoisirMimo();
    }

    private void ChoisirMimo(){
        float hasardRarete = Random.Range(0,100);
        if(hasardRarete >= 40){
            int quelMimo = Random.Range(0,15);
            _mimo = _tInfosCollect[quelMimo];
        }
        else if(hasardRarete >= 10){
            int quelMimo = Random.Range(15, 23);
            _mimo = _tInfosCollect[quelMimo];
        }
        else if(hasardRarete >= 2){
            int quelMimo = Random.Range(23,26);
            _mimo = _tInfosCollect[quelMimo];
        }
        else if (hasardRarete < 2) { 
            int quelMimo = Random.Range(26,28);
            _mimo = _tInfosCollect[quelMimo];
        }
        _nomObjet = _mimo.nomMimo;
        _sr.sprite = _mimo.imageObjet;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            Collection.instance.RecevoirObjet(_nomObjet);
            Debug.Log("on envoie l'objet a la collection");
        }
        Destroy(gameObject);
    }

}
