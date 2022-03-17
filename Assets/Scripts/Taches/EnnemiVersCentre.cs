using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiVersCentre : MonoBehaviour
{
    [SerializeField] float _vitesse = 2f;
    [SerializeField] int _pertePoint = -30;
    [SerializeField] ParticleSystem _partProject;
    [SerializeField] ParticleSystem _partScie;
    Transform _destination;
    bool _danger = false;

    TacheCentre _refTache;
    public TacheCentre refTache{
        get=>_refTache;
        set{
            _refTache = value;
        }
    }

    SpriteRenderer _sr;
    Timer _timer;
    public Timer timer{
        get=>_timer;
        set{
            _timer = value;
        }
    }

    bool _isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _partProject.Stop();
        _partScie.Stop();
        _sr = GetComponent<SpriteRenderer>();
        _destination = GetComponentInParent<Salle>().transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Projectile")){
            Debug.Log("on touche a un projectile");
            _refTache.RetirerProjectile();
            other.gameObject.GetComponent<Projectile>().AttribuerPoint((_destination));
            other.gameObject.GetComponent<Projectile>().Detruire();
            StartCoroutine(CoroutineMort());
            _isDead = true;
        }
    }

    IEnumerator CoroutineMort(){
        _sr.sprite = null;
        _partProject.Play();
        _partScie.Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isDead){
            transform.position = Vector3.MoveTowards(transform.position, _destination.position,_vitesse * Time.deltaTime);
            if(Vector2.Distance(transform.position, _destination.position) < 0.1 && !_danger){
                _danger = true;
                GetComponentInParent<Salle>().genSalle.perso.GetComponent<Personnage>().AjusterPoint("naturePoint", (_timer.nbJour * _pertePoint), TypeTache.Tache);
                _refTache.RetirerProjectile();
                Destroy(gameObject);
            }
            transform.Rotate(new Vector3(0f,0f,100f));
        }
    }
}
