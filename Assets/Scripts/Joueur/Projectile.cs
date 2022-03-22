using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le comportement d'un projectile du joueur 
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _vitesse = 8f; // vitesse de deplacement du projectile
    [SerializeField] int taskValue; // valeur lorsqu'un projectile touche un assaillant

    Personnage _perso; // reference au Personnage
    public Personnage perso{ // acces publique au Personnage
        get=>_perso; // par perso, on retourne _perso
        set{ // on change la valeur de _perso
            _perso = value; // _perso prend la valeur de value
        }
    }

    /// <summary>
    /// Fonction publique qui supprime le projectile
    /// </summary>
    public void Detruire(){
        Destroy(gameObject); // on supprime le projectile
    }

    void Update()
    {
        transform.Translate(Vector3.up * _vitesse * Time.deltaTime, Space.Self); // le projecile avance vers le haut selon sa son propre axe 
    }

    /// <summary>
    /// Fonction publique qui donne des points au joueur quand un projectile touche un asaillant
    /// </summary>
    /// <param name="positionEnnemi">position de l'assaillant</param>
    public void AttribuerPoint(Transform positionEnnemi){
        float bonusDistance = Vector2.Distance(transform.position, positionEnnemi.position); // on donne des points bonus selon la distance entre le personnage et l'assaillant
        Timer timer = perso.timer; // timer prend la valeur du Timer de Personnage
        int totalScore = Mathf.RoundToInt((taskValue * bonusDistance) * timer.nbJour); // le score total est multiplie par le score bonus puis par le nbJour de timer
        perso.AjusterPoint("naturePoint", totalScore, TypeTache.Tache); // on demande au perso de s'ajouter des points de type Tache
    }

    /// <summary>
    /// Fonction qui supprime le projectile s'il quitte la vue de camera
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject); // on supprime le projectile
    }
}
