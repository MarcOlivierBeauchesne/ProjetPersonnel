using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le comportement d'un arbre plante par le joueur
/// </summary>
public class Arbre : MonoBehaviour
{
    [SerializeField]private Sprite[] _tImgArbre; // tableau des images de l'arbre
    private int _age = 0; // age de l'arbre

    SpriteRenderer _sr; // on stock le SpriteRenderer de l'arbre
    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>(); // _sr prend la valeur du SpriteRenderer du gameObject
        StartCoroutine(CoroutineArbre()); // on demarre la coroutine CoroutineArbre
    }

    /// <summary>
    /// Coroutine qui fait grandir l'arbre
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineArbre(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        _age++; // on augmente _age de 1
        switch(_age){ // switch selon l'age de l'arbre
            case 2 : // si _age == 2
                _sr.sprite = _tImgArbre[1]; // on affiche la 2em image dans _tImgArbre
                break; // on sort de la condition
            case 4 : // si _age == 4
                _sr.sprite = _tImgArbre[2]; // on affiche la 3em image dans _tImgArbre
                break; // on sort de la condition
            case 6 : // si _age == 6
                _sr.sprite = _tImgArbre[3]; // on affiche la 4em image dans _tImgArbre
                break; // on sort de la condition
        }
        if(_age<6){ // si _age est plus petit que 6
            StartCoroutine(CoroutineArbre()); // on redemarre la coroutine CoroutineArbre
        }
    }

}
