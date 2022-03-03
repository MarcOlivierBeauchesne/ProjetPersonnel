using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    [SerializeField] private GameObject[] _tGoTips;
    [SerializeField] private Timer _timer;
    private int _tips = 0;
    private int _limiteTips = 0;

    public void AfficherTips(bool debut){
        StartCoroutine(CoroutineTips(debut));
    }

    public void NextTips(){
        _tGoTips[_tips].SetActive(false);
        _tips++;
        _limiteTips--;
        if(_limiteTips >0){
            _tGoTips[_tips].SetActive(true);
        }
        else{
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator CoroutineTips(bool debut){
        int waitTime = 0;
        if(debut){
            waitTime = 0;
        }
        else{
            waitTime = 3;
        }
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 0;
        int jour = _timer.nbJour;
        Debug.Log("nombre de jour : " + jour);
        switch(jour){
            case 1:
                _limiteTips = 4;
                _tGoTips[0].SetActive(true);
                Debug.Log("on active :" + _tGoTips[_tips].name);
            break;
            case 2:
                _limiteTips = 1;
                _tGoTips[_tips].SetActive(true);
            break;
            case 3:
                _limiteTips = 1;
                _tGoTips[_tips].SetActive(true);
            break;
        }
    }
}
