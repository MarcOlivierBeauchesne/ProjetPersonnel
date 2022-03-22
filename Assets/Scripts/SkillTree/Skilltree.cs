using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script qui controle le comportement de l'arbre des talent
/// </summary>
public class Skilltree : MonoBehaviour
{
    [Header("Composante du SkillTree")] // identification de la section Composante du SkillTree
    [SerializeField] GameObject _goSkillTree; // gameObject de l'arbre des talents
    [SerializeField] GameObject _boiteExplication; // Reference pour la boite d'explication du skill
    [SerializeField] Button _boutonAbsorber; // reference pour le bouton Absorber de l'arbre
    [TextArea] // zone de texte
    [SerializeField] string _textAbsorbtion; // texte d'explication de l'abosrbtion de l'arbre
    [SerializeField] private int absorbCost = 1000; // le cout d'absorbtion de l'arbre
    private float _absorbCount = 1; // nombre d'absorbtion effectuees
    public float absorbCount{ // acces public au nombre d'absorbtion effectuees
        get => _absorbCount; // par absorbCount, on retourne _absorbCount
        set{ // on change la valeur de _absorbCount
            _absorbCount = value; // _absorbCount prend la valeur de value
        }
    }
    [SerializeField] SkillInfos[] _tSkills; // tableau pour stocker les skills de l'Arbre
    [Header("Informations du personnage")] // identification de la section Informations du personnage
    [SerializeField] PlayerRessources _ressourcePlayer; // reference du PlayerRessources
    [SerializeField] Personnage _perso; // reference au personnage
    [Header("Position du canva")] // identification de la section Position du Canva
    [SerializeField] Transform _canvasPos; // position du Canva

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _boiteExplication.SetActive(false); // on desactive la _boitedExplication
        _goSkillTree.SetActive(false); // on desactive l'arbre des talents
        CheckRessources(); // on appel CheckRessources
    }

    /// <summary>
    /// Fonction public qui verifie l'etat de l'arbre des talent
    /// </summary>
    public void VerifierFenetre() { 
        if(_goSkillTree.activeInHierarchy){ // si l'arbre des talent est actif
            _goSkillTree.SetActive(false); // on desactive l'arbre des talents
            _boiteExplication.SetActive(false); // on desactive la _boiteExplication
        }
        else{ // si l'arbre des talent n'est pas actif
            _goSkillTree.SetActive(true); // on active l'arbre des talents
            CheckRessources(); // on appel CheckRessources
        }
    }

    /// <summary>
    /// fonction publique qui entame la generation d'un popUp
    /// </summary>
    /// <param name="popUp">PopUp du skill a faire apparaitre</param>
    public void CreateSkillPopUp(GameObject popUp){ 
        StartCoroutine(CoroutinePopUp(popUp));// on demarre la coroutine CoroutinePopUp avec popUp
    }

    /// <summary>
    /// Coroutine qui genere un popUp du skill achete
    /// </summary>
    /// <param name="popUp">PopUp du skill a faire apparaitre</param>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutinePopUp(GameObject popUp){
        GameObject newPopUp = Instantiate(popUp, transform.position, Quaternion.identity); // on genere un PopUp 
        newPopUp.transform.SetParent(_canvasPos); // newPopUp devient l'enfant du Canvas
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        Destroy(newPopUp); // on detruit le popUp
    }


    /// <summary>
    /// Fonction qui verifie si le joueur possede assez de resources pour abosrber l'arbre
    /// </summary>
    public void CheckRessources(){
        _boutonAbsorber.interactable = _ressourcePlayer.naturePoint >= absorbCost; // le bouton absorber est cliquable selon si le joueur possede assez de point de nature ou non
    }

    /// <summary>
    /// Fonction qui abosrbe l'arbre, remet tous les talent a 0 et verifie les dependance des skills
    /// </summary>
    public void AbsorbSkills(){
        int coutReel = CalculerCout(absorbCost, _absorbCount); // coutReel prend la valeur de retour de CalculerCout
        _perso.AjusterPoint("naturePoint", -coutReel, TypeTache.Aucun); // on eneleve les points de nature du joueur
        _absorbCount += 0.1f; // on augmente le _absorbCount de 0.1
        _boiteExplication.transform.GetChild(3).GetComponent<Text>().text = CalculerCout(absorbCost, _absorbCount).ToString(); // on affiche le cout du skill
        foreach (SkillInfos skill in _tSkills) // pour chaque skill dans le tSkills
        {
            skill.ResetSkill(); // on reinitialise le skill
            skill.CheckDepend(); // on verifie les dependance du skill
            skill.AjusterCoutSkill(); // on ajuste le cout de chaque skill
        }
        CheckRessources(); // on appel CheckRessources
    }

    /// <summary>
    /// Fonction publique qui calcul le cout d'absorbtion du skillTree
    /// </summary>
    /// <param name="baseCost">Le cout de base</param>
    /// <param name="counter">Le nombre d'absorbtion actuel de l'arbre</param>
    /// <returns></returns>
    public int CalculerCout(int baseCost, float counter){
        float cout = 1000 + (0.1f * Mathf.Pow(baseCost, counter))/2f; // on calcul le cout
        return Mathf.RoundToInt(cout); // on retourne la valeur arrondie de cout
    }

    /// <summary>
    /// Foncition qui active ou desactive la boite explicative au passage de la souris
    /// </summary>
    public void ActiverBoite()
    {
        int coutReel = CalculerCout(absorbCost, _absorbCount);
        if(!_boiteExplication.activeInHierarchy){ // si la boite explicatioin n'est pas active
            _boiteExplication.SetActive(true); // on active la boite explicative
            _boiteExplication.transform.GetChild(0).GetComponent<Text>().text = "Absorber"; // on affiche le nom du skill
            _boiteExplication.transform.GetChild(1).GetComponent<Text>().text = _textAbsorbtion; // on affiche l'explication du du skill
            _boiteExplication.transform.GetChild(2).GetComponent<Text>().text = ""; // on affiche le niveau actuel sur le niveau maximum du skill
            Text textCout = _boiteExplication.transform.GetChild(3).GetComponent<Text>();
            textCout.text = coutReel.ToString(); // on affiche le cout du skill
            if(coutReel <= _ressourcePlayer.naturePoint){ // si le coutReel est plus petit que le nombre de point de nature du joueur
                textCout.color = Color.green; // le texte du cout devient vert
            }
            else{ // si le coutReel est plus grand que le nombre de point de nature du joueur
                textCout.color = Color.red; // le texte du cout devient rouge
            }
            CheckRessources();// on appel CheckRessources
        }
        else{ // si la boite est active
            _boiteExplication.SetActive(false); // on desactive la boite
        }
    }
}
