using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnnemiForet : MonoBehaviour
{
    [SerializeField] private int _minWaitTime = 2;
    [SerializeField] private int _maxWaitTime = 5;
    [SerializeField] private Sprite _imgMort;
    [SerializeField] private float _cutValue;
    [SerializeField] ParticleSystem _partExplosion;
    [SerializeField] ParticleSystem _partFumee;
    Salle _salle;
    SpriteRenderer _sr;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _salle = GetComponentInParent<Salle>();
        StartCoroutine(CoroutineCoupe());
        _partExplosion.Stop();
        _partFumee.Stop();
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
            GetComponent<CircleCollider2D>().enabled = false;
            StopAllCoroutines();
            StartCoroutine(CoroutineEnnemiMort());
        }
    }

    private void FermerLumiere(){
        Light2D[] _tLumieres = GetComponentsInChildren<Light2D>();
        foreach (Light2D lumiere in _tLumieres)
        {
            lumiere.intensity = 0;
        }
    }

    private IEnumerator CoroutineEnnemiMort(){
        _partExplosion.Play();
        _partFumee.Play();
        _sr.sprite = _imgMort;
        FermerLumiere();
        _anim.SetTrigger("Dead");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
