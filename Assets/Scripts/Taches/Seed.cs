using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] GameObject _particleNoix;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            Personnage perso = other.gameObject.GetComponent<Personnage>();
            perso.AjusterPoint("seed", 1, TypeTache.Aucun);
            perso.taskManager.CreatePopUpPoints(transform.position, 1, "noix");
            StartCoroutine(CoroutineNoix());
        }
    }

    IEnumerator CoroutineNoix(){
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = null;
        GameObject particle = Instantiate(_particleNoix, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(particle);
        Destroy(gameObject);
    }
}
