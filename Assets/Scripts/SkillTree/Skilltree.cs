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
    [SerializeField] SkillInfos[] _tSkills; // tableau pour stocker les skills de l'Arbre
    [SerializeField] Button _boutonAbsorber; // reference pour le bouton Absorber de l'arbre
    [SerializeField] PlayerRessources _ressourcePlayer; // reference du PlayerRessources
    [SerializeField] Personnage _perso; // reference au personnage
    [SerializeField] GameObject _goSkillTree; // gameObject de l'arbre des talents
    [SerializeField] GameObject _boiteExplication; // Reference pour la boite d'explication du skill
    [TextArea]
    [SerializeField] string _textAbsorbtion;
    [SerializeField] private int absorbCost = 1000; // le cout d'absorbtion de l'arbre
    private float _absorbCount = 1;
    public float absorbCount{
        get => _absorbCount;
    }

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
            _boiteExplication.SetActive(false);
        }
        else{ // si l'arbre des talent n'est pas actif
            _goSkillTree.SetActive(true); // on active l'arbre des talents
            Debug.Log(_ressourcePlayer.naturePoint + ": point du joueur");
            CheckRessources();
        }
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
        int coutReel = CalculerCout(absorbCost, _absorbCount);
        _perso.AjusterPoint("naturePoint", -coutReel);
        _absorbCount += 0.1f;
        _boiteExplication.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CalculerCout(absorbCost, _absorbCount).ToString(); // on affiche le cout du skill
        foreach (SkillInfos skill in _tSkills) // pour chaque skill dans le tSkills
        {
            skill.ResetSkill(); // on reinitialise le skill
            skill.CheckDepend(); // on verifie les dependance du skill
            skill.AjusterCoutSkill();
        }
        CheckRessources();
    }

    public int CalculerCout(int baseCost, float counter){
        float cout = 1000 + (0.1f * Mathf.Pow(baseCost, counter))/2f;
        return Mathf.RoundToInt(cout);
    }

    /// <summary>
    /// Foncition qui active ou desactive la boite explicative au passage de la souris
    /// </summary>
    public void ActiverBoite()
    {
        int coutReel = CalculerCout(absorbCost, _absorbCount);
        if(!_boiteExplication.activeInHierarchy){ // si la boite explicatioin n'est pas active
            _boiteExplication.SetActive(true); // on active la boite explicative
            _boiteExplication.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Absorber"; // on affiche le nom du skill
            _boiteExplication.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _textAbsorbtion; // on affiche l'explication du du skill
            _boiteExplication.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""; // on affiche le niveau actuel sur le niveau maximum du skill
            TextMeshProUGUI textCout = _boiteExplication.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            textCout.text = coutReel.ToString(); // on affiche le cout du skill
            CheckRessources();
        }
        else{ // si la boite est active
            _boiteExplication.SetActive(false); // on desactive la boite
        }
    }

    /// <summary>
    /// Fonction qui sauvegarde les skill dans le tableau _tSkills
    /// </summary>
    public void SaveSkill(){
        foreach (SkillInfos skill in _tSkills) // pour chaque skill dans le tSkills
        {
            skill.SaveSkill(); // on sauvegarde les information du skill
        }
    }

    /// <summary>
    /// Fonction qui charge les informations sauvegardees des skills dans le tableau _tSkills 
    /// </summary>
    public void LoadSkill(){
        foreach (SkillInfos skill in _tSkills) // pour chaque skill dans le tSkills
        {
            skill.LoadSkill(); // on charge les informations sauvegardees du skill
            skill.CheckDepend(); // on verifie les dependances du skill
        }
    }
}
