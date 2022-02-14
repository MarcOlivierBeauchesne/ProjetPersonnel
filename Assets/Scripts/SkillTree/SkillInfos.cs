using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfos : MonoBehaviour
{
    [SerializeField] string _nom = "Nom du skill";
    [SerializeField] float _modifier = 0;
    [SerializeField] TypeStats _typeStats;
    [SerializeField] int _skillCost = 200;
    [SerializeField] int _costModif = 100;
    [SerializeField] int _savedTotalStack = 0;
    [SerializeField] int _actualStack = 0;
    public int actualStack{
        get => _actualStack;
        set{
            _actualStack = value;
        }
    }
    [SerializeField] int _maxStack = 1;
    public int maxStack{
        get => _maxStack;
    }
    [SerializeField] Sprite _iconFullStack;
    [SerializeField] Sprite _iconBase;
    [SerializeField] BasicStats _basicStats;
    [SerializeField] SkillInfos _dependance;
    [SerializeField] SkillInfos[] _tHeritiers;
    [SerializeField] PlayerRessources _playerRessources;
    [SerializeField] Personnage _perso;
    [SerializeField] Skilltree _arbre;
    [SerializeField] SkillExplication _skillExplication;
    [SerializeField] GameObject _boiteExpliction;

    Image img;
    Button _bouton;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _bouton = GetComponent<Button>();
        img = GetComponent<Image>();
        CheckDepend();
        _boiteExpliction.SetActive(false);
    }

    public void VerifierDispo(){
        if(_dependance != null){
            if(_dependance.actualStack == _dependance.maxStack){
                VerifierRessources();
            }
            else{
                Debug.Log("Vous devez débloquer le skill précédent");
            }
        }
        else{
            VerifierRessources();
        }
    }

    private void VerifierRessources(){
        int realCost = _skillCost * (actualStack + 1);
        if(_playerRessources.naturePoint >= realCost){
            _perso.AjusterPoint("naturePoint", -realCost);
            //_playerRessources.naturePoint -= realCost;
            actualStack++;
            _savedTotalStack++;
            _boiteExpliction.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{actualStack}/{maxStack}";
            _basicStats.ChangerStats(_typeStats, _modifier, false);
            _arbre.CheckRessources();
            if(actualStack == maxStack){
                GetComponent<Button>().interactable = false;
                img.sprite = _iconFullStack;
                if(_tHeritiers.Length > 0){
                    foreach (SkillInfos heritier in _tHeritiers)
                    {
                        heritier._bouton.interactable = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    public void ActiverBoite()
    {
        if(!_boiteExpliction.activeInHierarchy){
            _boiteExpliction.SetActive(true);
            _boiteExpliction.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _skillExplication.nomSkill;
            _boiteExpliction.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _skillExplication.explication;
            _boiteExpliction.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{actualStack}/{maxStack}";
        }
        else{
            _boiteExpliction.SetActive(false);
        }
    }

    public void ResetSkill(){
        actualStack = 0;
    }

    public void CheckDepend(){
        if(_dependance != null){
            _bouton.interactable = (_dependance.actualStack == _dependance.maxStack && actualStack < maxStack);
        }
        else{
            if(actualStack < maxStack){
                _bouton.interactable = true;
            }
            else{
                _bouton.interactable = false;
            }

        }
        CheckStack();
    }

    private void CheckStack(){
        if(actualStack<maxStack){
            img.sprite = _iconBase;
        }
        else if(actualStack == maxStack){
            img.sprite = _iconFullStack;
        }
    }

    public void SaveSkill(){
        // PlayerPrefs.DeleteKey(_nom + "total");
        // PlayerPrefs.DeleteKey(_nom + "actual");
        PlayerPrefs.SetInt(_nom + "total", _savedTotalStack);
        PlayerPrefs.SetInt(_nom + "actual", _actualStack);
        Debug.Log("total sauvegardé: "+ _savedTotalStack + "/ actuel sauvegardé: " + _actualStack);
    }

    public void LoadSkill(){
        _savedTotalStack = PlayerPrefs.GetInt(_nom + "total");
        _actualStack = PlayerPrefs.GetInt(_nom + "actual");
        _basicStats.ChangerStats(_typeStats, (_modifier*_savedTotalStack), true);
    }
}
