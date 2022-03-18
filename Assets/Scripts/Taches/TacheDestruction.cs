using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TacheDestruction : MonoBehaviour
{
    [SerializeField] GameObject[] _tJoint;
    [SerializeField] GameObject[] _tMorceaux;
    [SerializeField] Sprite[] _tImgMontant;
    [SerializeField] TextMeshPro[] _tChampsAmount;
    [SerializeField] int _taskValue;

    int _clicAmount = 0;
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
            _tJoint[i].GetComponent<JointTache>().timer = GetComponentInParent<Tache>().perso.timer;
            if(i == 0){
                GameObject jointText = _tJoint[i].transform.GetChild(0).gameObject;
                _tJoint[i].SetActive(true);
                jointText.GetComponent<TextMeshPro>().text = _tJoint[i].GetComponent<JointTache>().actualClic.ToString();
            }
            else{
                _tJoint[i].SetActive(false);
            }
        }
    }

    public void UserJoint(int actualClic, int goalClic){
        _clicAmount++;
        SpriteRenderer spriteJoint = _tJoint[_joint].GetComponent<SpriteRenderer>();
        int tier = goalClic - (goalClic/3);
        Debug.Log(tier + " : tier1");
        int tier2 = goalClic - ((goalClic/3) * 2);
        Debug.Log(tier2 + " : tier2");
        if(actualClic > tier){
            spriteJoint.sprite = _tImgMontant[0];
            Debug.Log("chiffre plus grand que tier1");
        }
        else if(actualClic > tier2){
            spriteJoint.sprite = _tImgMontant[1];
            Debug.Log("chiffre plus grand que tier2");
        }
        else if(actualClic < tier2){
            Debug.Log("chiffre plus grand que tier3");
            spriteJoint.sprite = _tImgMontant[2];
            if(actualClic <= 0){
                _tMorceaux[_joint].GetComponent<Rigidbody2D>().gravityScale = 1;
                Destroy(_tJoint[_joint]);
                _joint++;
                if(_joint<_tJoint.Length){
                    _tJoint[_joint].SetActive(true);
                }
                if(_joint >= _tJoint.Length){
                    float npGain = GetComponentInParent<Tache>().perso.basicStats.npGain;
                    GetComponentInParent<Tache>().FinirTache(_taskValue + (_clicAmount * (int)npGain));
                }
            }
        }
    }
}
