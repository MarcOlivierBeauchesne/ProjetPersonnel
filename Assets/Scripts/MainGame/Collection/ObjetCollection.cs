using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjetCollection : MonoBehaviour
{
    [SerializeField] GameObject _boiteObjet;
    [SerializeField] InfosCollection _infosObjet;
    public InfosCollection infosObjet{
        get => _infosObjet;
    }
    [SerializeField] private Sprite _imageInconnu;
    public Sprite imageInconnu{
        get => _imageInconnu;
    }

    public void ActiverBoite(){
        switch(_boiteObjet.activeInHierarchy){
            case true :
                _boiteObjet.SetActive(false);
                break;
            case false:
                _boiteObjet.SetActive(true);
                if(_infosObjet.isFound){
                    _boiteObjet.transform.GetChild(0).GetComponent<Image>().sprite = _infosObjet.imageObjet;
                    _boiteObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _infosObjet.textObjet;
                }
                else{
                    _boiteObjet.transform.GetChild(0).GetComponent<Image>().sprite = _imageInconnu;
                    _boiteObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Trouvez ce Mimo pour en apprend plus";
                }
                break;
        }
    }

}
