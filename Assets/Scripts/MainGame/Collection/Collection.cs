using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui gere la collection de mimos
/// </summary>
public class Collection : MonoBehaviour
{
    [Header("Nouveau Mimo")] // identification de la section Nouveau Mimo
    [SerializeField] private GameObject _goNewMimo; // GameObject qui apparait lorsque le joueur trouve un mimo qu'il n'avait pas encore decouvert
    [SerializeField] private GameObject _goCanvas; // GameObject du canvas
    [Header("Liste des mimos")] // identification de la section Liste des mimos
    [SerializeField] private InfosCollection[] _tInfosCollection; // tableau de toutes les informations de tous les mimo possibles
    [SerializeField] private GameObject[] _tObjetCol; // tableau de tous les GameObject de mimo dans la fenetre de collection
    [SerializeField] Personnage _perso; // reference au Personnage
    [SerializeField] TaskManager _taskManager; // reference au TaskManager

    private static Collection _instance; // reference static pour la collection
    public static Collection instance => _instance; // reference static publique pour la colleciton (Singleton)

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (_instance == null){ // si _instance est null
            _instance = this; // _instance represente cette classe
        }
        else //sinon (s'il existe un instance)
        {
            Destroy(gameObject); //on detruit ce gameObject
            return; // on quiite le fonction
        }
    }

    void Start()
    {
        ActiverCollection(); // on appel ActiverCollection
        NommerMimo(); // on appel NommerMimo
        gameObject.SetActive(false); // on desactive la collection
    }

    /// <summary>
    /// Fonction qui nomme les GameObject de mimo dans la fenetre de collection avec le nom du mimo qu'il represente
    /// </summary>
    private void NommerMimo(){
        foreach (GameObject mimo in _tObjetCol) // boucle pour chaque GameObject dans _tObjetCol
        {
            mimo.GetComponent<ObjetCollection>().NommerObjet(); // on acced au ObjetCollection de mimo et on appel NommerObjet
        }
    }

    /// <summary>
    /// fonction publique qui permet de reinitialiser la collection
    /// </summary>
    public void ResetCollection(){
        foreach (InfosCollection mimo in _tInfosCollection) // pour chaque InfosCollection dans _tInfosCollection
        {
            mimo.isFound = false; // le isFoudn du mimo est false
        }
    }

    /// <summary>
    /// fonction qui permet d'afficher visuellement les mimo trouves ou inconnu
    /// </summary>
    private void ActiverCollection(){
        for (int i = 0; i < _tInfosCollection.Length; i++) // pour selon la longueur du tableau _tInfosCollection
        {
            bool mimoTrouve = _tInfosCollection[i].isFound; // objetTrouve est egal au isFound du InfosCollection a la position i dans _tInfosCollection
            Image imageObj = _tObjetCol[i].transform.GetChild(0).GetComponent<Image>(); // imageObj prend la valeur du Image du GameOjbect du mimo dans la fenetre de collection
            if(mimoTrouve){ // si mimoTrouve est true
                imageObj.sprite = _tInfosCollection[i].imageMimo; // on change le sprite du gameObject pour le imageObjet du _tInfosCollection[i]
                imageObj.color = new Color (1f,1f,1f,1f); // on met l'image a sa pleine couleur et transparence
            }
            else{ // si mimoTrouve est false
                imageObj.sprite = _tObjetCol[i].GetComponent<ObjetCollection>().imageInconnu; // on change le sprite du gameObject pour le imageInconnu du ObjetCollection 
                imageObj.color = new Color (1f,1f,1f,0.5f); // on met l'image a sa pleine couleur et a demi transparente
            }
        }
    }

    /// <summary>
    /// fonction publique qui permet d'activer et desactiver la fenetre de collection
    /// </summary>
    public void ActiverBoite(){
        switch(gameObject.activeInHierarchy){ // switch selon si la collection est active ou non
            case true : // si la collection est active
                gameObject.SetActive(false); // on desactive la collection
                transform.GetChild(2).gameObject.SetActive(false); // on prend l'enfant a l'index 2 de la collection et on le desactive
                break; // on sort de la condition
            case false: // si la collection n'est pas active
                gameObject.SetActive(true); // on active la fenetre de collection
                break; // on sort de la condition
        }
    }

    /// <summary>
    /// fonction publique qui permet de recevoir un mimo et de mettre a jour la collection
    /// </summary>
    /// <param name="nomObjet">nom du mimo recu</param>
    public void RecevoirObjet(string nomObjet){
        for (int i = 0; i < _tObjetCol.Length; i++) // boucle pour chaque GameObject de mimo dans la fenetre de collection
        {
            if(_tObjetCol[i].name == nomObjet){ // si le nom du GameObject est le meme que le nom du mimo recu
                ObjetCollection objectCol = _tObjetCol[i].GetComponent<ObjetCollection>(); // on stock le ObjetCollection du GameObject dans objectCol
                bool estTrouve = objectCol.infosObjet.isFound; // estTrouve prend la valeur du isFound du ObjetCollection du gameObject
                int valeurMimo = objectCol.infosObjet.mimoValue; // valeurMimo prend la valeur du mimoValue du ObjetCollection du gameObject
                if(estTrouve){ // si le mimo est deja decouvert
                    _perso.AjusterPoint("naturePoint",valeurMimo, TypeTache.Mimo); // on demande au joueur de s'ajouter des points avec valeurMimo
                    _perso.missionManager.AccomplirMission(TypeMission.Mimo); // on demande au MissionManager d'accomplir la mission de type Mimo
                }
                else{ // si le mimo n'est pas encore decouvert
                    InfosCollection infoMimo = _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet; // on stock le InfosCollection du GameObject dans infoMimo
                    _tObjetCol[i].GetComponent<ObjetCollection>().infosObjet.isFound = true; // on dit au InfosCollection que le mimo est trouve
                    Image imageObj = _tObjetCol[i].transform.GetChild(0).GetComponent<Image>(); // on stock l'Image de l'enfant a l'index 0 du _tObjetCol[i] dans imageObj
                    imageObj.sprite = _tInfosCollection[i].imageMimo; // on change le sprite du imageObj pour l'imageObjet du _tInfosCollection[i]
                    imageObj.color = new Color (1f,1f,1f,1f); // on met l'image a sa pleine couleur et transparence
                    gameObject.SetActive(true); // on active la fenetre de collection
                    CreateNewMimo(infoMimo); // on appel CreateNewMimo en lui envoyant infoMimo
                    _perso.AjusterPoint("naturePoint",valeurMimo, TypeTache.Mimo); // on demande au joueur de s'ajouter des points avec valeurMimo
                    gameObject.SetActive(false); // on desactive la fenetre de collection
                    _perso.missionManager.AccomplirMission(TypeMission.Mimo); // on demande au MissionManager d'accomplir la mission de type Mimo
                    return; // on quitte la fonction
                }
            }
        }
    }

    /// <summary>
    /// Fonction qui cree un PopUp pour indiquer au joueur qu'il a trouve un nouveau mimo
    /// </summary>
    /// <param name="infoMimo">information du mimo decouvert</param>
    private void CreateNewMimo(InfosCollection infoMimo){
        GameObject newMimo = Instantiate(_goNewMimo, Vector2.zero, Quaternion.identity); // on cree un nouveau _goNewMimo qu'on stock dans newMimo
        newMimo.transform.SetParent(_goCanvas.transform); // newMimo devient un enfant du Canvas
        newMimo.transform.position = transform.position; // on change la position du mimo pour la position de la collection
        newMimo.GetComponent<Image>().sprite = infoMimo.imageMimo; // on change l'image du popUp pour l'image du mimo decouvert
        newMimo.transform.GetChild(1).GetComponent<Text>().text = infoMimo.nomMimo; // on change le nom affiche du nouveau mimo decouvert par le nomMimo du infoMimo
    }
}
