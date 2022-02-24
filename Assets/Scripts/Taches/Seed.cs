using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<Personnage>().AjusterPoint("seed", 1);
            Destroy(gameObject);
        }
    }
}
