using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacheCentre : MonoBehaviour
{
    [SerializeField] GameObject _goEnnemis;
    [SerializeField] int _minSpawn;
    [SerializeField] int _maxSpawn;
    int _nbProjectile = 0;
    public int nbProjectile{
        get=>_nbProjectile;
    }
    int _spawnEnnemis = 0;
    int _totalProjectile = 0;
    public int totalProjectile{
        get=>_totalProjectile;
        set{
            _totalProjectile = value;
        }
    }
    int _projectilKilled = 0;

    Transform _originPos;
    SpriteRenderer _sr;
    BoxCollider2D _boxCol;
    CircleCollider2D _circCol;
    Salle _salle;
    Timer _timer;
    Tache _tache;

    int _nbEnnemis = 0;
    public int nbEnnemis{
        get=> _nbEnnemis;
        set{
            _nbEnnemis = value;
        }
    }

    private void Start()
    {
        _tache = GetComponent<Tache>();
        _originPos = transform;
        _sr = GetComponent<SpriteRenderer>();
        _boxCol = GetComponent<BoxCollider2D>();
        _circCol = GetComponent<CircleCollider2D>();
        _salle = GetComponentInParent<Salle>();
        _timer = _salle.genSalle.timer;
    }

    public void DemarrerSpawn(){
        GetComponentInParent<Salle>().AfficherTacheProjectile();
        transform.position = _salle.gameObject.transform.position;
        _sr.sprite = null;
        _boxCol.enabled = false;
        _circCol.enabled = false;
        nbEnnemis = Random.Range(_minSpawn, _maxSpawn + 1);
        _totalProjectile = nbEnnemis;
        _salle.AjusterAffichageProjectiles(_totalProjectile - _projectilKilled);
        _spawnEnnemis = nbEnnemis;
        StartCoroutine(CoroutineSpawnEnnemi());
    }

    public void RetirerProjectile(){
        _projectilKilled++;
        _salle.AjusterAffichageProjectiles(_totalProjectile - _projectilKilled);
        if(_projectilKilled >= _totalProjectile){
            _tache.perso.ChangerPos(_originPos);
            _tache.perso.ChangerRot(false);
            _tache.perso.ResetRot();
            _tache.perso.ChangerPourTour(false);
            _tache.FinirTache(0);
            Debug.Log("joueur peut bouger");
        }
    }

    public void ArreterSpawn(){
        StopAllCoroutines();
    }

    private IEnumerator CoroutineSpawnEnnemi(){
        Vector2 posPerso = GetComponent<Tache>().perso.gameObject.transform.position;
        float randomWaitTime = Random.Range(0, 1.2f);
        yield return new WaitForSeconds(randomWaitTime);
        GameObject ennemi = Instantiate(_goEnnemis, transform.position, Quaternion.identity);
        ennemi.transform.SetParent(_salle.gameObject.transform);
        Vector2 ajustPos = new Vector2(_salle.gameObject.transform.position.x, _salle.gameObject.transform.position.y);
        ennemi.transform.position = Random.insideUnitCircle * 10 + ajustPos;
        ennemi.GetComponent<EnnemiVersCentre>().refTache = this;
        ennemi.GetComponent<EnnemiVersCentre>().timer = _timer;
        _tache.AjouterProjectile(ennemi);
        nbEnnemis--;
        if(nbEnnemis>0){
            StartCoroutine(CoroutineSpawnEnnemi());
        }
    }

}
