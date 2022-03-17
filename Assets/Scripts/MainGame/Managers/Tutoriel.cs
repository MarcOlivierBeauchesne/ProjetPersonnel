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

    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
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
        _dictTips.Add("TipsDestruction", false);
        _dictTips.Add("TipsCentre", false);
        _dictTips.Add("TipsMission", false);
    }

    public void OuvrirTips(int indexTips){
        StartCoroutine(CoroutineOuvrirTips(indexTips));
    }

    IEnumerator CoroutineOuvrirTips(int indexTips){
        string key = keyList[indexTips];
        if(!_dictTips[key]){
            _dictTips[key] = true;
            GameObject tips = transform.GetChild(indexTips).gameObject;
            tips.SetActive(true);
            _activeTips = tips;
        }
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 0;
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
