using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le comportement d'une noix
/// </summary>
public class Seed : MonoBehaviour
{
    [SerializeField] GameObject _particleNoix; // effect de particule lorsqu'on ramasse une noix
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonNoix; // son quand le joueur ramasse une noix

    Tutoriel _tuto; // reference a Tutoriel
    public Tutoriel tuto{ // acces public a Tutoriel
        get=>_tuto; // par tuto, on retourne _tuto
        set{ // on change la valeur de _tuto
            _tuto = value; // _tuto prend la valeur de value
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le tag du gameObject du collider entrant est Player
            Personnage perso = other.gameObject.GetComponent<Personnage>(); // on accede au Personnage du collider entrant
            perso.AjusterPoint("seed", 1, TypeTache.Aucun); // on ajoute 1 noix au joueur
            GameAudio.instance.JouerSon(_sonNoix); // on joue un son quand le joueur ramasse une noix
            perso.taskManager.CreatePopUpPoints(transform.position, 1, "noix"); // on demande au TaskManger de creer un popUp de noix
            StartCoroutine(CoroutineNoix()); // on demarre la coroutineCoroutineNoix
            if(_tuto.dictTips["TipsNoix"] == false){ // si la clef TipsNoix du dictionnaire dictTips est false 
                _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
                _tuto.OuvrirTips(2); // on demande a tutoriel d'activer le tutoriel a l'index 2
            }
        }
    }

    /// <summary>
    /// coroutine qui gere la noix lorsque le joueur la ramsse
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineNoix(){
        GetComponent<CircleCollider2D>().enabled = false; // on desactive le collider de la noix
        GetComponent<SpriteRenderer>().sprite = null; // on dit a l'image de la noix de devenir vide
        GameObject particle = Instantiate(_particleNoix, transform.position, Quaternion.identity); // on instantie l'effet de particule _particleNoix a la position de la noix
        particle.transform.SetParent(transform); // on dit que l'effet de particule devient l'enfant de la noix
        yield return new WaitForSeconds(0.7f); // on attend 0.7 seconde
        Destroy(gameObject); // on detruit la noix
    }
}
