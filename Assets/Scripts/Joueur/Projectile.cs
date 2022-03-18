using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int taskValue;
    private float _vitesse = 8f;

    Personnage _perso;
    public Personnage perso{
        get=>_perso;
        set{
            _perso = value;
        }
    }

    public void Detruire(){
        Destroy(gameObject);
    }

    void Update()
    {
        // Le projectile avance selon sa vitesse dans la direction
        // de son propre axe horizontal (Space.Self)
        transform.Translate(Vector3.up * _vitesse * Time.deltaTime, Space.Self);
    }

    public void AttribuerPoint(Transform destinationEnnemi){
        float bonusDistance = Vector2.Distance(transform.position, destinationEnnemi.position);
        Timer timer = perso.timer;
        int totalScore = Mathf.RoundToInt((taskValue * bonusDistance) * timer.nbJour);
        perso.AjusterPoint("naturePoint", totalScore, TypeTache.Tache);
        perso.taskManager.AjouterPoint(TypeTache.Tache, totalScore);
    }

    // Détruire le projectile si il quitte la scène
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
