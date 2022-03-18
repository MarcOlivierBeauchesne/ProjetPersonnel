using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script qui gere le comportement du personnage dans sa forme agressive
/// </summary>
public class ControleCentre : MonoBehaviour
{
    [Header("Stats de rotation")] // identification de la section Stats de rotation
    [SerializeField] float _vitesseRot = 1f; // vitesse de rotation
    [Header("Projectile")] // identification de la section Projectile
    [SerializeField] GameObject _goProjectile; // gameOjbect du projectile
    [SerializeField] Transform _ProjectileSpawnPos; // point d'apparition du projectile

    bool _peutTourner = false; // bool si le personnage peut tourner
    public bool peutTourner{ // acces public a _peutTourner
        get=>_peutTourner; // par peutTourner, on retourne _peutTourner
        set{ // on change la valeur de _peutTourner
            _peutTourner = value; // on change la valeur de _peutTourner par value
        }
    }

    bool _peutTirer = true; // bool si le personnage peut tirer

    /// <summary>
    /// Coroutine qui permet le tir du joueur
    /// </summary>
    /// <returns>temps d'atttente</returns>
    IEnumerator CoroutineTir(){
        _peutTirer = false; // le joueur ne peut pas tirer
        yield return new WaitForSeconds(0.2f); // on attend 0.2 seconde
        _peutTirer = true; // le joueur peut tirer
        
    }
    // Update is called once per frame
    void Update()
    {
        if(_peutTourner){ // si _peutTourner est true
            float rotation = Input.GetAxis("Horizontal"); // rotation prend la valeur du Input de GetAxis("Horizontal")
            if(Input.GetButton("Horizontal")){ // si le joueur appuie sur une touche qui affecte GetAxis("Horizontal")
                transform.Rotate(Vector3.back * Time.deltaTime * _vitesseRot * rotation, Space.Self); // on tourne le joueur vers l'avant ou l'arriere selon la valeur de rotation
            }
            if(Input.GetKeyDown(KeyCode.Space) && _peutTirer){ // si le joueur appuie sur Space et que _peutTirer est true
                StartCoroutine(CoroutineTir()); // on demarre la coroutine CoroutineTir
                GameObject projectile = Instantiate(_goProjectile, _ProjectileSpawnPos.position, transform.rotation); // on genere un projectile a l'emplacement de _ProjectileSpawnPos
                projectile.GetComponent<Projectile>().perso = GetComponent<Personnage>(); // on attribue le perso du projectile selon la Composante Personnage du gameObject actuel
            }
        }
    }
}
