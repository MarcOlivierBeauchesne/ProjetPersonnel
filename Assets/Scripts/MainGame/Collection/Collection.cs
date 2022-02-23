using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    [SerializeField] private GameObject _boiteObjet;
    [SerializeField] private InfosCollection[] _tInfosCollection;
    [SerializeField] private GameObject[] _tObjetCol;
    // Start is called before the first frame update
    private static Collection _instance;
    public static Collection instance => _instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (_instance == null){
            _instance = this; //#tim Marc-Olivier - si _instance est null, _instance represente cette classe
        }
        else //#tim Marc-Olivier - sinon (s'il existe un instance)
        {
            Destroy(gameObject); //#tim Marc-Olivier - on detruit ce gameObject
            return;  //#tim Marc-Olivier - on quiite le fonction
        }
    }

    void Start()
    {
        Debug.Log("La collection est présente");
        _boiteObjet.SetActive(false);
        gameObject.SetActive(false);
        ActiverCollection();
    }

    private void ActiverCollection(){
        for (int i = 0; i < _tInfosCollection.Length; i++)
        {
            bool objetTrouve = _tInfosCollection[i].isFound;
            Image imageObj = _tObjetCol[i].transform.GetChild(0).GetComponent<Image>();
            if(objetTrouve){
                imageObj.sprite = _tInfosCollection[i].imageObjet;
                imageObj.color = new Color (1f,1f,1f,1f);
            }
            else{
                imageObj.sprite = _tObjetCol[i].GetComponent<ObjetCollection>().imageInconnu;
                imageObj.color = new Color (1f,1f,1f,0.5f);
            }
        }
    }

    public void ActiverBoite(){
        switch(gameObject.activeInHierarchy){
            case true :
                gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                break;
            case false:
                gameObject.SetActive(true);
                break;
        }
    }

    public void RecevoirObjet(string nomObjet){
        for (int i = 0; i < _tObjetCol.Length; i++)
        {
            if(_tObjetCol[i].name == nomObjet){
                bool estTrouve = _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet.isFound;
                if(estTrouve){
                    Debug.Log("mimo deja trouvé");
                }
                else{
                    _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet.isFound = true;
                    Image imageObj = _tObjetCol[i].transform.GetChild(0).GetComponent<Image>();
                    imageObj.sprite = _tInfosCollection[i].imageObjet;
                    imageObj.color = new Color (1f,1f,1f,1f);
                    Debug.Log("Nouveau mimo trouvé! il s'appel" + nomObjet);
                    return;
                }
            }
        }
    }
}
