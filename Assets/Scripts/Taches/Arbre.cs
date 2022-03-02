using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour
{
    [SerializeField]private Sprite[] _tImgArbre;
    private int _age = 0;

    private SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        StartCoroutine(CoroutineArbre());
    }

    private IEnumerator CoroutineArbre(){
        yield return new WaitForSeconds(1f);
        _age++;
        switch(_age){
            case 2 :
                _sr.sprite = _tImgArbre[1];
                break;
            case 4 :
                _sr.sprite = _tImgArbre[2];
                break;
            case 6 :
                _sr.sprite = _tImgArbre[3];
                break;
        }
        if(_age<6){
            StartCoroutine(CoroutineArbre());
        }
    }

}
