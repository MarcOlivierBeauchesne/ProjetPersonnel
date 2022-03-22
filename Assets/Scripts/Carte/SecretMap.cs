using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// script qui controle l'utilisation des passages secrets
/// </summary>
public class SecretMap : MonoBehaviour
{
    [Header("Objet du secret")] // identification de la section Objet du secret
    [SerializeField] GameObject _btnInterraction; // GameObject du bouton d'interraction
    [SerializeField] GameObject _secondDoor; // GameObject de la 2em porte du secret
    [SerializeField] GameObject _secretTilemap; // GameObject du tilemap du secret
    [Header("Cout d'utilisation")] // identification de la section Cout d'utilisation
    [SerializeField] GameObject _objetCout; // GameOjbect du cout d'utilisation
    [SerializeField] int _secretCost = 0; // cout d'utilisation
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonSecret; // son quand le joueur active un secret

    bool _playerClose = false; // bool si le joueur est proche
    Personnage _perso; // reference au Personnage

    private void Start()
    {
        _btnInterraction.SetActive(false); // on desactive _btnInterraction
        _objetCout.SetActive(false); // on desactive _objetCout
        _objetCout.transform.GetChild(1).GetComponent<TextMeshPro>().text = _secretCost.ToString(); // on affiche le cout d'utilisation du secret
        _perso = GetComponentInParent<Salle>().genSalle.perso; // _perso devient le perso du _genSalle dans le parent Salle
    }

    /// <summary>
    /// Fonction qui detecte si un collider est entre dans le collider de la porte
    /// </summary>
    /// <param name="other">le collider2D entrant</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le tag du gameObjet de other est Player
            _playerClose = true; // le personnage est proche
            _btnInterraction.SetActive(true); // on active _btnInterraction
            _objetCout.SetActive(true); // on active _objetCout
        }
    }

    /// <summary>
    /// Fonction qui detecte si un collider est sortie du collider de la porte
    /// </summary>
    /// <param name="other">le collider sortant</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le tag du gameObjet de other est Player
            _playerClose = false; // le personnage n'est plus proche
            _btnInterraction.SetActive(false); // on desactive _btnInterraction
            _objetCout.SetActive(false); // on desactive _objetCout
        }   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose){ // si le joueur appuie sur la touche E et que _playerClose est true
            if(_perso.ressourcesPlayer.naturePower >= _secretCost){ // si le naturePower du perso est plus grand ou egal au _secretCost
                _perso.AjusterPoint("naturePower", -_secretCost, TypeTache.Aucun); // on retire _secretCost des points de naturePower du perso
                GameAudio.instance.JouerSon(_sonSecret); // on joue un son quand le joueur active un secret
                if(_secondDoor != null){ // si _secondDoor n'est pas null
                    Destroy(_secondDoor); // on detruit _secondDoor
                }
                Destroy(_secretTilemap); // on detruit _secretTilemap
                Destroy(gameObject); // on detruit la porte
            }
        }
    }
}
