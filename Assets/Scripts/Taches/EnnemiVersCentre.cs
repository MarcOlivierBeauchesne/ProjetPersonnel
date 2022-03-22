using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiVersCentre : MonoBehaviour
{
    [Header("Informations de l'ennemi")] // identification de la section Informations de l'ennemi
    [SerializeField] float _vitesse = 2f; // vitesse de deplacement de l'ennemi
    [SerializeField] int _pertePoint = -30; // points retires du joueur au contact
    [Header("Effets de particules")] // identification de la section Effets de particules
    [SerializeField] ParticleSystem _partProject; // particule du projectile
    [SerializeField] ParticleSystem _partScie; // particule de l'ennemi

    TacheCentre _refTache; // reference au TacheCentre
    public TacheCentre refTache{ // acces public au TacheCentre
        get=>_refTache; // par refTache, on retourne _refTache
        set{ // on change la valeur de _refTache
            _refTache = value; // _refTache prend la valeur de value
        }
    }

    Timer _timer; // reference au Timer
    public Timer timer{ // acces public au Timer
        get=>_timer; // par timer, on retourne _timer
        set{ // on change la valeur de _timer
            _timer = value; // _timer prend la valeur de value
        }
    }

    bool _isDead = false; // indicateur si l'ennemi est mort ou non
    bool _danger = false; // indicateur si l'ennemi est assez proche de son objectif

    SpriteRenderer _sr; // reference au SpriteRenderer de l'objet
    Transform _destination; // reference a la destination de l'ennemi

    // Start is called before the first frame update
    void Start()
    {
        _partProject.Stop(); // on arrete _partProject
        _partScie.Stop(); // on arrete _partScie
        _sr = GetComponent<SpriteRenderer>(); // _sr prend la valeur de la composante SpriteRenderer de l'objet
        _destination = GetComponentInParent<Salle>().transform; // la destination prend la valeur de la position de la salle
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Projectile")){ // si le tag du gameObject du collider entrant est Projectile
            _refTache.RetirerProjectile(); // on demande a TacheCentre de retirer un projectile
            other.gameObject.GetComponent<Projectile>().AttribuerPoint((_destination)); // on demande au projectile d'attribuer des points au joueur
            other.gameObject.GetComponent<Projectile>().Detruire(); // on demande au projectile de se detruire
            StartCoroutine(CoroutineMort()); // on demarre la Coroutine CoroutineMort
            _isDead = true; // l'ennemi est mort
            GetComponent<CircleCollider2D>().enabled = false; // on desactive le collider de l'ennemi
        }
    }

    /// <summary>
    /// Coroutine qui gere la mort de l'ennemi
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineMort(){
        _sr.sprite = null; // on enleve l'image de l'ennemi
        _partProject.Play(); // on demarre _partProject
        _partScie.Play(); // on demarre _partScie
        yield return new WaitForSeconds(2f); // on attend 2 seconde
        Destroy(gameObject); // on detruit l'ennemi
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isDead){ // si l'ennemi n'est pas mort
            transform.position = Vector3.MoveTowards(transform.position, _destination.position,_vitesse * Time.deltaTime); // on bouge l'ennemi vers sa destination
            if(Vector2.Distance(transform.position, _destination.position) < 0.1 && !_danger){ // si la distance entre l'ennemi et sa destination est plus petit que 0.1
            //  et que l'ennemis n'est pas proche de son objectif
                _danger = true; // l'ennemi devient proche de son objectif
                GetComponentInParent<Salle>().genSalle.perso.AjusterPoint("naturePoint", (_timer.nbJour * _pertePoint), TypeTache.Tache); // on fait perdre des points au joueur
                _refTache.RetirerProjectile(); // on demande au TacheCentre de retirer un ennemi
                Destroy(gameObject); // on detruit ;'ennemi
            }
            transform.Rotate(new Vector3(0f,0f,100f)); // on fait tourner l'ennemi
        }
    }
}
