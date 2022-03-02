using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacheDestruction : MonoBehaviour
{
    [SerializeField] GameObject[] _tJoint;
    [SerializeField] GameObject[] _tMorceaux;
    [SerializeField] Sprite[] _tImgMontant;
    [SerializeField] int _taskValue;

    int _joint = 0;
    int _imgJoint = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        DesactiverJoint();
    }

    private void DesactiverJoint(){
        for (int i = 0; i < _tJoint.Length; i++)
        {
            if(i == 0){
                _tJoint[i].SetActive(true);
            }
            else{
                _tJoint[i].SetActive(false);
            }
        }
    }

    public void UserJoint(){
        _imgJoint++;
        if(_imgJoint<_tImgMontant.Length){
            _tJoint[_joint].GetComponent<SpriteRenderer>().sprite = _tImgMontant[_imgJoint];
        }
        if(_imgJoint == 3){
            _tMorceaux[_joint].GetComponent<Rigidbody2D>().gravityScale = 1;
            Destroy(_tJoint[_joint]);
            _joint++;
            _imgJoint = 0;
            if(_joint<_tJoint.Length){
                _tJoint[_joint].SetActive(true);
            }
        }
        if(_joint == 3){
            Debug.Log("La machine est detuite");
            GetComponentInParent<Tache>().FinirTache(_taskValue);
        }
    }
}
