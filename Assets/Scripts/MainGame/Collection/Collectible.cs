using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string _nomObjet;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Debug.Log("Objet apparu");
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("on touche a quelque chose");
        if(other.gameObject.CompareTag("Player")){
            Collection.instance.RecevoirObjet(_nomObjet);
            Debug.Log("on envoie l'objet a la collection");
        }
        Destroy(gameObject);
    }

}
