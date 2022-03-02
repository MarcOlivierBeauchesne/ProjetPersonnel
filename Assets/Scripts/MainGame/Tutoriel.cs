using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    [SerializeField] private GameObject[] _tGoTips;
    [SerializeField] private Timer _timer;
    private int _tips = 0;
    private int _limiteTips = 0;

    public void AfficherTips(){
        StartCoroutine(CoroutineTips());
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

    private IEnumerator CoroutineTips(){
        yield return null;
        Time.timeScale = 0;
        int jour = _timer.nbJour;
        Debug.Log("nombre de jour : " + jour);
        switch(jour){
            case 1:
                _limiteTips = 4;
                _tGoTips[_tips].SetActive(true);
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
