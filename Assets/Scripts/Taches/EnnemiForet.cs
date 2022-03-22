using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Script qui gere le comportement d'un ennemis dans la foret
/// </summary>
public class EnnemiForet : MonoBehaviour
{
    [Header("Informations de la coupe")] // identification de la section Informations de la coupe
    [SerializeField] private int _minWaitTime = 2; // temps d'attente minimal avant une coupe
    [SerializeField] private int _maxWaitTime = 5; // temps d'attente maximal avant une coupe
    [SerializeField] private float _cutValue; // valeur de l'augmentation de la deforestation
    [Header("Visuel de l'ennemi")] // identification de la section Visuel de l'ennemi
    [SerializeField] private Sprite _imgMort; // image de l'ennemi mort
    [SerializeField] ParticleSystem _partExplosion; // particleEffect de l'explosion
    [SerializeField] ParticleSystem _partFumee; // particleEffect de la fumeee
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonDestruction; // son quand l'ennemi meurt

    Salle _salle; // reference a la Salle parent
    SpriteRenderer _sr; // Reference au SpriteRenderer de l'objet
    Animator _anim; // reference a l'Animator de l'objet

    private void Start()
    {
        _anim = GetComponent<Animator>(); // _anim prend la valeur de l'animator de l'objet
        _sr = GetComponent<SpriteRenderer>(); // _sr prend la valeur du Spriterenderer de l'objet
        _salle = GetComponentInParent<Salle>(); // salle prend la valeur du composante Salle du parent
        StartCoroutine(CoroutineCoupe()); // on demarre la coroutine CoroutineCoupe
        _partExplosion.Stop(); // on arrete _partExplosion
        _partFumee.Stop(); // on arrete _partFumee
    }

    /// <summary>
    /// Coroutine qui augmente la deforestation
    /// </summary>
    /// <returns>Temps d'attente</returns>
    IEnumerator CoroutineCoupe(){
        int waitTime = Random.Range(_minWaitTime, _maxWaitTime+1); // waitTime prend une valeur entre _minWaitTime et _maxWaitTime +1
        yield return new WaitForSeconds(waitTime); // on attemps selon waitTime
        _salle.AugmenterDeforestation(_cutValue); // on demande a _salle d'augmenter la deforestation avec _cutValue
        StartCoroutine(CoroutineCoupe()); // on redemarre la coroutine CoroutineCoupe
    }

    /// <summary>
    /// Fonction publique qui fait arreter la coupe
    /// </summary>
    public void ArreterCoupe(){
        StopAllCoroutines(); // on arrete toute les coroutines
        Destroy(gameObject); // on detruit l'ennemi
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){ // si le tage du gameObject du colldier entrant est Player
            _salle.RetirerEnnemi(); // on demande a salle de retirer un ennemi
            GameAudio.instance.JouerSon(_sonDestruction); // on joue un son l'ennemi meurt
            GetComponent<CircleCollider2D>().enabled = false; // on desactive le collider de l'ennemi
            StopAllCoroutines(); // on arrete toutes les coroutines
            StartCoroutine(CoroutineEnnemiMort()); // on demarre la coroutine CoroutineEnnemiMort
        }
    }

    /// <summary>
    /// fonction qui arrete toutes les lumieres de l'ennemi
    /// </summary>
    private void FermerLumiere(){
        Light2D[] _tLumieres = GetComponentsInChildren<Light2D>(); // on stock les Light2D de l'ennemi dans _tLumieres
        foreach (Light2D lumiere in _tLumieres) // pour chaque Light2D dans _tLumieres
        {
            lumiere.intensity = 0; // on met l'intensite de la lumiere a 0
        }
    }

    /// <summary>
    /// Coroutine qui fait mourrir l'ennemmi
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineEnnemiMort(){
        _partExplosion.Play(); // on demarre _partExplosion
        _partFumee.Play(); // on demarre _partFumee
        _sr.sprite = _imgMort; // on affiche l'image de l'ennemi mort
        FermerLumiere(); // on appel FermerLumiere
        _anim.SetTrigger("Dead"); // on declanche le trigger Dead de _anim
        yield return new WaitForSeconds(3f); // on attend 3 secondes
        Destroy(gameObject); //on detruit l'ennemi
    }
}
