using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere la tache qui fait apparaitre des ennemis qui font perdre des points au joueur
/// </summary>
public class TacheCentre : MonoBehaviour
{
    [SerializeField] GameObject _goEnnemis; // ennemis a generer
    [SerializeField] int _minSpawn; // minimum d'ennemi a generer
    [SerializeField] int _maxSpawn; // maximum d'ennemis a generer
    int _totalProjectile = 0; // nombre total d'ennemis generes
    public int totalProjectile{ // acces public au nombre total d'ennemis generes
        get=>_totalProjectile; // par totalProjectile, on retourne _totalProjectile
        set{ // on change la valeur de _totalProjectile
            _totalProjectile = value; // _totalProjectile prend la valeur de value
        }
    }
    int _projectilKilled = 0; // nombre de projectile ennemis tues

    int _nbEnnemis = 0; // nombre d'ennemis a generes
    public int nbEnnemis{ // acces publique au nombre d'ennemis a generes
        get=> _nbEnnemis; // par nbEnnemis, on retourne _nbEnnemis
        set{ // on change la valeur de _nbEnnemis
            _nbEnnemis = value; // _nbEnnemis prend la valeur de value
        }
    }

    Transform _originPos; // reference a la position d'origine de la tache
    SpriteRenderer _sr; // reference au SpriteRenderer de la tache
    BoxCollider2D _boxCol; // reference au BoxCollider2D de la tache 
    CircleCollider2D _circCol; // reference au CircleCollider2D de la tache
    Salle _salle; // reference a la salle dans le parent
    Timer _timer; // reference au Timer
    Tache _tache; // reference a Tache de la tache

    private void Start()
    {
        _tache = GetComponent<Tache>(); // _tache represente la composante Tache de l'objet
        _originPos = transform; // _originPos prend la position de la tache
        _sr = GetComponent<SpriteRenderer>(); // _sr prend la valeur de la composante SpriteRenderer de l'objet
        _boxCol = GetComponent<BoxCollider2D>(); // _boxCol prend la valeur de la composante BoxCollider2D de l'objet
        _circCol = GetComponent<CircleCollider2D>(); // _circCol prend la valeur de la composante CircleCollider2D de l'objet
        _salle = GetComponentInParent<Salle>(); // _salle prend la valeur de la composante Salle du parent de l'objet
        _timer = _salle.genSalle.timer; // Timer prend la valeur du timer de genSalle de _salle
    }

    /// <summary>
    /// Fonction publique qui demarre la generation des ennemis
    /// </summary>
    public void DemarrerSpawn(){
        _salle.AfficherTacheProjectile(); // on demande a la salle d'afficher le suivi de la tache des projectiles
        transform.position = _salle.gameObject.transform.position; // on change la position de la tache pour la position de la salle
        _sr.sprite = null; // l'image de la tache devient vide
        _boxCol.enabled = false; // on desactive _boxCol
        _circCol.enabled = false; // on desactive _circCol
        nbEnnemis = Random.Range(_minSpawn, _maxSpawn + 1); // nbEnnemis prend une valeur entre _minSpawn et _maxSpawn +1
        _totalProjectile = nbEnnemis; //_totalProjectile prend la valeur de nbEnnemis
        _salle.AjusterAffichageProjectiles(_totalProjectile - _projectilKilled); // on demande a _salle d'ajuster l'affichage du suivi de la tache des projectiles
        StartCoroutine(CoroutineSpawnEnnemi()); // on demarre la coroutine CoroutineSpawnEnnemi
    }

    /// <summary>
    /// Fonction publique pour retirer un projectile ennemi
    /// </summary>
    public void RetirerProjectile(){
        _projectilKilled++; // on augmente les projectiles tuees de 1
        _salle.AjusterAffichageProjectiles(_totalProjectile - _projectilKilled); // on demande a _salle d'ajuster l'affichage du suivi de la tache des projectiles
        if(_projectilKilled >= _totalProjectile){ // si _projectilKilled est plus grand ou egal a _totalProjectile
            _tache.perso.ChangerPos(_originPos); // on demande au perso de changer sa position pour _originPos
            _tache.perso.ChangerRot(false); // on dit au personnage qu'il ne peut plus tourner
            _tache.perso.ResetRot(); // on dit au personnage de reinitialiser sa rotation
            _tache.perso.ChangerPourTour(false); // on dit au personnage de se re-transformer en hero
            _tache.FinirTache(0); // on dit a Tache de finir Tache avec 0 points
        }
    }

    /// <summary>
    /// Fonction qui arrete la generation des projectiles ennemis
    /// </summary>
    public void ArreterSpawn(){
        StopAllCoroutines(); // on arrete toutes les coroutines
    }

    /// <summary>
    /// coroutine qui fait apparaitre les projectiles ennemis
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineSpawnEnnemi(){
        Vector2 posPerso = GetComponent<Tache>().perso.gameObject.transform.position; // posPerso prend la position du personnage
        float randomWaitTime = Random.Range(0, 1.2f); // randomWaitTime prend une valeur entre 0 et 1,2
        yield return new WaitForSeconds(randomWaitTime); // on attend selon randomWaitTime
        GameObject ennemi = Instantiate(_goEnnemis, transform.position, Quaternion.identity); // on genere un projetile ennemi
        ennemi.transform.SetParent(_salle.gameObject.transform); // ennemi devient l'enfant de la salle
        Vector2 ajustPos = new Vector2(_salle.gameObject.transform.position.x, _salle.gameObject.transform.position.y); // ajustPos ajuste la position d'apparition des projectiles ennemis
        ennemi.transform.position = Random.insideUnitCircle * 10 + ajustPos; // on change la position du projectile ennemis
        ennemi.GetComponent<EnnemiVersCentre>().refTache = this; // on prend la composante EnnemiVersCentre de ennemis et son refTache prend la valeur de ce script
        ennemi.GetComponent<EnnemiVersCentre>().timer = _timer; // le timer du EnnemiVersCentre de ennemi prend la valeur de _timer
        _tache.AjouterProjectile(ennemi); // on demande a Tache d'ajouter un projectile dans la salle
        nbEnnemis--; // on reduit les ennemis a genere de 1
        if(nbEnnemis>0){ // si nbEnnemis est plus grand que 0
            StartCoroutine(CoroutineSpawnEnnemi()); // on demare la coroutine CoroutineSpawnEnnemi
        }
    }

}
