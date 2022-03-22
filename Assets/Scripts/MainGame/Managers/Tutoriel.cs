using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere l'affichage des conseils
/// </summary>
public class Tutoriel : MonoBehaviour
{
    [SerializeField] private Timer _timer; // reference au Timer
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonAfficherTips; // son quand un tips apparait
    private Dictionary<string, bool> _dictTips = new Dictionary<string, bool>(){ // dictionnaire pour les conseils (nom du conseil => deja montre)
        
    };
    public Dictionary<string, bool> dictTips{ // acces public au dictionnaire des conseils
        get=>_dictTips; // par dictTips, on retourne _dictTips
    }
    List<string> keyList = new List<string>(); // liste qui va se remplir des keys des elements du dictionnaire dictTips
    string[] _tTips = new string[]{ // tableau des keys qui seront utilises par le dictionnaire dictTips
        "TipsDefo",
        "TipsDeplacement",
        "TipsNoix",
        "TipsTree",
        "TipsTache",
        "TipsSouche",
        "TipsCollection",
        "TipsDestruction",
        "TipsCentre",
        "TipsMission"
    };
    private GameObject _activeTips; // gameObject qui prendra la valeur du conseila ctif

    Animator _anim; // Animator du GameObject

    private void Start()
    {
        _anim = GetComponent<Animator>(); // _anim prend la valeur de l'animator du GameObject
        SetupDict(); // on appel SetupDict
        keyList = new List<string>(_dictTips.Keys); // on rempli la liste keyList avec les Keys du dictionnaire _dictTips
        gameObject.SetActive(false); // on desactive le gameOjbect
    }    

    /// <summary>
    /// Fonction qui rempli le dictionnaire avec les string de _tTips
    /// </summary>
    private void SetupDict(){
        for (int i = 0; i < _tTips.Length; i++) // boucle selon la longueur du tableau _tTips
        {
            _dictTips.Add(_tTips[i], false); // on ajout un element au dictionnaire _dictTips, _tTips[i] => false
        }
    }

    /// <summary>
    /// Fonction publique qui permet d'entamer l'ouverture d'un conseil
    /// </summary>
    /// <param name="indexTips">index dans le tableau keyList que l'on desir ouvrir</param>
    public void OuvrirTips(int indexTips){
        GameAudio.instance.JouerSon(_sonAfficherTips); // on joue un son quand un tips apparait
        StartCoroutine(CoroutineOuvrirTips(indexTips)); // on demarre la coroutine CoroutineOuvrirTips avec indexTips
    }

    /// <summary>
    /// Coroutine qui ouvre un conseil
    /// </summary>
    /// <param name="indexTips">index dans le tableau keyList que l'on desir ouvrir</param>
    /// <returns></returns>
    IEnumerator CoroutineOuvrirTips(int indexTips){
        string key = keyList[indexTips]; // key prend la valeur de keyList[indexTips]
        if(!_dictTips[key]){ // si le conseil a la clef key est false
            _dictTips[key] = true; //le conseil a la clef key devient true
            GameObject tips = transform.GetChild(indexTips).gameObject; // on stop le gameObject enfant du tutoriel selon indexTips dans tips
            tips.SetActive(true); // on active le gameObject tips
            _activeTips = tips; // on dit que _activeTips reprensente tips
        }
        yield return new WaitForSeconds(0.4f); // on attend 0.4 seconde
        Time.timeScale = 0; // on arrete le temps
    }

    /// <summary>
    /// Fonction publique qui permet de ferme un conseil
    /// </summary>
    public void FermerTips(){
        Time.timeScale = 1; // le temps prend sa vitesse normale
        _activeTips.SetActive(false); // on desactive le gameObject du _activeTips
        gameObject.SetActive(false); // on desactive la fenetre de conseil
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){ // si le joueur appuie sur la touche F
            bool tipTree = transform.GetChild(3).gameObject.activeInHierarchy; //tipTree represente si le tip de skillTree est ouvert
            if(tipTree){ // s'il est ouvert
                FermerTips(); // on appel FermerTips
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space)){ // si le joueur appuie sur Space
            bool tipNoix = transform.GetChild(2).gameObject.activeInHierarchy; // tipNoix represente si le tip des noix est ouvert
            if(tipNoix){ // s'il est ouvert 
                FermerTips(); // on appel FermerTips
            }
        }
        else if(Input.GetKeyDown(KeyCode.C)){
            bool tipCollection = transform.GetChild(6).gameObject.activeInHierarchy; // tipCollection represente si le tip de la collection est ovuert
            if(tipCollection){ // s'il est ouvert
                FermerTips(); // on appel FermerTips
            }
        }
    }
}
