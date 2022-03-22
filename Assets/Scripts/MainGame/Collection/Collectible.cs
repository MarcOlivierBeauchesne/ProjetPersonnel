using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le choix d'un mimo qui apparait et fait apparaitre le tips de la collection
/// </summary>
public class Collectible : MonoBehaviour
{
    [Header("Informations du mimo")] // identification de la section Informations du mimo
    [SerializeField] int[] _tMimoValue; // valeur des diffentes rarete de mimo
    [Header("Mimos possibles")] // identification de la section Mimos possibles
    [SerializeField] private InfosCollection[] _tInfosCollect; // tableau de toutes les informations de tous les mimo possibles

    private string _nomObjet; // nom du mimo

    Tutoriel _tuto; // reference a Tutoriel
    public Tutoriel tuto{ // acces public au Tutoriel
        get=>_tuto; // par tuto, on retourne _tuto
        set{ // on change al valeur de _tuto
            _tuto = value; // _tuto prend la valeur recu
        }
    }

    SpriteRenderer _sr; // reference au SpriteRenderer
    InfosCollection _mimo; // reference au InfoCollection

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>(); // _sr devient le Spriterenderer du gameObject
        ChoisirMimo(); // on apppel ChoisirMimo
    }

    /// <summary>
    /// Fonction qui determine quel mimo sera present lors de l'apparition du GameObject
    /// </summary>
    private void ChoisirMimo(){
        float hasardRarete = Random.Range(0,101); // hasardRarete prend une valeur entre 0 et 100
        if(hasardRarete >= 40){ // si hasardRarete est plus grand ou egal a 40 (60% chance)
            int quelMimo = Random.Range(0,15); // quelMimo prend une valeur entre 0 et 15
            _mimo = _tInfosCollect[quelMimo]; // _mimo devient le InfoCollection du _tInfosCollect a la position quelMimo
            _mimo.mimoValue = _tMimoValue[0]; // mimoValue prend la valeur a la position 0 du tableau _tMimoValue
        }
        else if(hasardRarete >= 10){// si hasardRarete est plus grand ou egal a 10 (30% chance)
            int quelMimo = Random.Range(15, 23); // quelMimo prend une valeur entre 16 et 23
            _mimo = _tInfosCollect[quelMimo]; // _mimo devient le InfoCollection du _tInfosCollect a la position quelMimo
            _mimo.mimoValue = _tMimoValue[1]; // mimoValue prend la valeur a la position 1 du tableau _tMimoValue
        }
        else if(hasardRarete >= 2){// si hasardRarete est plus grand ou egal a 2 (8% chance)
            int quelMimo = Random.Range(23,26); // quelMimo prend une valeur entre 24 et 26
            _mimo = _tInfosCollect[quelMimo]; // _mimo devient le InfoCollection du _tInfosCollect a la position quelMimo
            _mimo.mimoValue = _tMimoValue[2]; // mimoValue prend la valeur a la position 2 du tableau _tMimoValue
        }
        else if (hasardRarete < 2) { // si hasardRarete est plus petit que 2 (2% chance)
            int quelMimo = Random.Range(26,28); // quelMimo prend une valeur entre 27 et 28
            _mimo = _tInfosCollect[quelMimo]; // _mimo devient le InfoCollection du _tInfosCollect a la position quelMimo
            _mimo.mimoValue = _tMimoValue[3]; // mimoValue prend la valeur a la position 3 du tableau _tMimoValue
        }
        _nomObjet = _mimo.nomMimo; // _nomObject prend la valeur du nomMimo de _mimo
        _sr.sprite = _mimo.imageMimo; // le sprite de _sr prend la valeur de imageObjet di _mimo
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le collider entrant est le player
            if(_tuto.dictTips["TipsCollection"] == false){ // si le TipsCollection de dictTips a comme valeur false
                _tuto.gameObject.SetActive(true); // on active la fenetre de tips
                _tuto.OuvrirTips(6); // on demande a _tuto d'ouvrir le tips a l'index 6
            }
            Collection.instance.RecevoirObjet(_nomObjet); // on demande a la collection de recevoir le _nomObjet
            Destroy(gameObject); // on detruit le gameObject
        }
    }
}
