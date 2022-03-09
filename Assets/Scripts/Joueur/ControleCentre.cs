using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCentre : MonoBehaviour
{
    [SerializeField] float _vitesseRot = 1f;
    [SerializeField] GameObject _goProjectile;
    [SerializeField] Transform _SpawnPos;
    bool _peutTourner = false;
    public bool peutTourner{
        get=>_peutTourner;
        set{
            _peutTourner = value;
            Debug.Log("peut Tourner : " + _peutTourner);
        }
    }

    bool _peutTirer = true;

    public void ChangerEtat(bool etat){
        if(etat){
            _peutTourner = etat;
        }
        else{
            _peutTourner = etat;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    private IEnumerator CoroutineTir(){
        _peutTirer = false;
        yield return new WaitForSeconds(0.2f);
        _peutTirer = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(_peutTourner){
            float rotation = Input.GetAxis("Horizontal");
            if(Input.GetButton("Horizontal")){
                transform.Rotate(Vector3.back * Time.deltaTime * _vitesseRot * rotation, Space.Self);
            }
            if(Input.GetKeyDown(KeyCode.Space) && _peutTirer){
                StartCoroutine(CoroutineTir());
                GameObject projectile = Instantiate(_goProjectile, _SpawnPos.position, transform.rotation);
                projectile.GetComponent<Projectile>().perso = GetComponent<Personnage>();
            }
        }
    }
}
