using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    [SerializeField] private GameObject[] _tGoTips;
    [SerializeField] private Timer _timer;
    private Dictionary<string, bool> _dictTips = new Dictionary<string, bool>(){
        
    };
    public Dictionary<string, bool> dictTips{
        get=>_dictTips;
    }
    List<string> keyList = new List<string>();
    private GameObject _activeTips;

    private void Start()
    {
        SetupDict();
        keyList = new List<string>(_dictTips.Keys);
        gameObject.SetActive(false);
    }    

    private void SetupDict(){
        _dictTips.Add("TipsDefo", false);
        _dictTips.Add("TipsDeplacement", false);
        _dictTips.Add("TipsNoix", false);
        _dictTips.Add("TipsTree", false);
        _dictTips.Add("TipsTache", false);
        _dictTips.Add("TipsSouche", false);
        _dictTips.Add("TipsCollection", false);
    }

    public void OuvrirTips(int indexTips){
        Debug.Log(indexTips);
        string key = keyList[indexTips];
        Debug.Log(key);
        Debug.Log(_dictTips[key]);
        if(!_dictTips[key]){
            _dictTips[key] = true;
            Time.timeScale = 0;
            GameObject tips = transform.GetChild(indexTips).gameObject;
            tips.SetActive(true);
            _activeTips = tips;
        }
        else{
            Debug.Log("Le tips a deja été vu");
        }
    }

    public void FermerTips(){
        Time.timeScale = 1;
        _activeTips.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            bool tipTree = transform.GetChild(3).gameObject.activeInHierarchy;
            if(tipTree){
                FermerTips();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space)){
            bool tipTree = transform.GetChild(2).gameObject.activeInHierarchy;
            if(tipTree){
                FermerTips();
            }
        }
        else if(Input.GetKeyDown(KeyCode.C)){
            bool tipTree = transform.GetChild(6).gameObject.activeInHierarchy;
            if(tipTree){
                FermerTips();
            }
        }
    }
}
