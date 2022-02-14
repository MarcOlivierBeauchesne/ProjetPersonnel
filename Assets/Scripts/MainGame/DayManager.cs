using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tChampsEndDay;
    [SerializeField] Animator _animFenetre;
    [SerializeField] TaskManager _taskManager;
    private int _indexTableau = 0;
    // Start is called before the first frame update
    void Start()
    {
        ResetChamps();
    }

    public void AfficherPoint(){
        StartCoroutine(CoroutineAfficherPoint());
    }

    private IEnumerator CoroutineAfficherPoint(){
        Debug.Log("on affiche les points");
        yield return new WaitForSeconds(1f);
        switch(_indexTableau){
            case 0 : {
                _tChampsEndDay[0].SetActive(true);
                break;
            }
            case 1 : {
                _tChampsEndDay[1].SetActive(true);
                _tChampsEndDay[2].SetActive(true);
                break;
            }
            case 2 : {
                _tChampsEndDay[3].SetActive(true);
                _tChampsEndDay[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _taskManager.scoreArbre.ToString();
                _tChampsEndDay[4].SetActive(true);
                _tChampsEndDay[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetComponent<BasicStats>().deforestAugment.ToString();
                break;
            }
            case 3 : {
                _tChampsEndDay[5].SetActive(true);
                _tChampsEndDay[5].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _taskManager.scoreTache.ToString();
                break;
            }
            case 4 : {
                _tChampsEndDay[6].SetActive(true);
                _tChampsEndDay[6].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetComponent<BasicStats>().deforestAugment.ToString();
                _tChampsEndDay[7].SetActive(true);
                _tChampsEndDay[7].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_taskManager.scoreArbre + _taskManager.scoreTache).ToString();
                break;
            }
            case 5 : {
                _tChampsEndDay[8].SetActive(true);
                break;
            }
        }
        _indexTableau++;
        if(_indexTableau < 8){
            StartCoroutine(CoroutineAfficherPoint());
        }
    }

    public void ResetChamps(){
        for (int i = 0; i < _tChampsEndDay.Length; i++)
        {
            _tChampsEndDay[i].SetActive(false);
            _indexTableau = 0;
        }
    }
}
