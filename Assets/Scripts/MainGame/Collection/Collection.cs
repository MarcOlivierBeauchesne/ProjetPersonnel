using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    [SerializeField] private GameObject _boiteObjet;
    [SerializeField] private InfosCollection[] _tInfosCollection;
    [SerializeField] private GameObject[] _tObjetCol;
    [SerializeField] private GameObject _goNewMimo;
    [SerializeField] private GameObject _goCanvas;
    [SerializeField] Personnage _perso;
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
        ActiverCollection();
        _boiteObjet.SetActive(false);
        NommerMimo();
        gameObject.SetActive(false);
    }

    private void NommerMimo(){
        foreach (GameObject mimo in _tObjetCol)
        {
            mimo.GetComponent<ObjetCollection>().NommerObjet();
        }
    }


    public void ResetCollection(){
        foreach (InfosCollection mimo in _tInfosCollection)
        {
            mimo.isFound = false;
        }
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
                transform.GetChild(2).gameObject.SetActive(false);
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
                int valeurMimo = _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet.mimoValue;
                if(estTrouve){
                    _perso.AjusterPoint("naturePoint",valeurMimo, TypeTache.Mimo);
                    _perso.missionManager.AccomplirMission(TypeMission.Mimo);
                }
                else{
                    InfosCollection infoMimo = _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet;
                    _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet.isFound = true;
                    Image imageObj = _tObjetCol[i].transform.GetChild(0).GetComponent<Image>();
                    imageObj.sprite = _tInfosCollection[i].imageObjet;
                    imageObj.color = new Color (1f,1f,1f,1f);
                    gameObject.SetActive(true);
                    CreateNewMimo(infoMimo);
                    _perso.AjusterPoint("naturePoint",valeurMimo, TypeTache.Mimo);
                    gameObject.SetActive(false);
                    _perso.missionManager.AccomplirMission(TypeMission.Mimo);
                    return;
                }
            }
        }
    }

    private void CreateNewMimo(InfosCollection infoMimo){
        GameObject newMimo = Instantiate(_goNewMimo, Vector2.zero, Quaternion.identity);
        newMimo.transform.SetParent(_goCanvas.transform);
        newMimo.transform.position = transform.position;
        newMimo.GetComponent<Image>().sprite = infoMimo.imageObjet;
        newMimo.transform.GetChild(1).GetComponent<Text>().text = infoMimo.nomMimo;
    }
}
