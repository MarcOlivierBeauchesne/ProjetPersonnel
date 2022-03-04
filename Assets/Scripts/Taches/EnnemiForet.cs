using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiForet : MonoBehaviour
{
    [SerializeField] private int _minWaitTime = 2;
    [SerializeField] private int _maxWaitTime = 5;
    [SerializeField] private Sprite _imgMort;
    [SerializeField] private float _cutValue;
    Salle _salle;
    SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _salle = GetComponentInParent<Salle>();
        StartCoroutine(CoroutineCoupe());
    }

    private IEnumerator CoroutineCoupe(){
        int waitTime = Random.Range(_minWaitTime, _maxWaitTime+1);
        yield return new WaitForSeconds(waitTime);
        _salle.AugmenterDeforestation(_cutValue);
        StartCoroutine(CoroutineCoupe());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            _salle.RetirerEnnemi();
            StopAllCoroutines();
            StartCoroutine(CoroutineEnnemiMort());
        }
    }

    private IEnumerator CoroutineEnnemiMort(){
        _sr.sprite = _imgMort;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
