using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script qui le comportement d'un skill
/// </summary>
public class SkillInfos : MonoBehaviour
{
    [SerializeField] string _nom = "Nom du skill"; // acces prive pour le nom du skill
    [SerializeField] float _modifier = 0; // acces prive pour le modificateur du skill
    [SerializeField] TypeStats _typeStats; // Reference au enum du type de stats
    [SerializeField] int _skillCostRef = 200; // cout de reference du skill
    [SerializeField] int _skillCost = 200; // cout actuel du skill
    [SerializeField] int _savedTotalStack = 0; // total de niveau enregistres dans le skill
    [SerializeField] int _actualStack = 0; // acces prive pour le nombre de niveau actuel pour le skill
    public int actualStack{ // acces public pour le nombre de niveau actuel pour le skill
        get => _actualStack; // par actualStack, on retourne la valeur de _actualStack
        set{ 
            _actualStack = value; // par actualStack, on change la valeur de _actualStack
        }
    }
    [SerializeField] int _maxStack = 1; // acces prive pour le niveau maximum que peut avoir un skill 
    public int maxStack{ // acces public pour le niveau maximum que peut avoir un skill 
        get => _maxStack; // par maxStack, on retourne la valeur _maxStack
    }
    [SerializeField] Sprite _iconFullStack; // image quand le skill est plein
    [SerializeField] Sprite _iconBase; // image pour le skill
    [SerializeField] BasicStats _basicStats; // reference pour le BasicStats
    [SerializeField] SkillInfos _dependance; // skill qui doit etre complet avant que le skill actuel puisse etre augmente
    [SerializeField] SkillInfos[] _tHeritiers; // tableau des heritier du skill actuel
    [SerializeField] PlayerRessources _playerRessources; // Reference pour le PlayerRessources
    [SerializeField] Personnage _perso; // Rerefence pour le personnage
    [SerializeField] Skilltree _arbre; // Reference pour le SkillTree
    [SerializeField] SkillExplication _skillExplication; // ScriptableObject qui detient les informations a afficher du skill
    [SerializeField] GameObject _boiteExplication; // Reference pour la boite d'explication du skill
    private float _skillReset = 1;
    private int realCost;

    public Image img; // acces public de l'Image du gameObject
    Button _bouton; // on stock le Button du gameObject
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _bouton = GetComponent<Button>(); // on va chercher le Button du gameObject et on le met dans _button
        img = GetComponent<Image>(); // on va chercher l'image du gameObject et on le met dans img
        CheckDepend(); // on appel CheckDepend
    }

    /// <summary>
    /// Fonction qui verifie si les dependances du skill sont completes
    /// </summary>
    public void VerifierDispo(){
        if(_dependance != null){ // si le skill possede une dependance
            if(_dependance.actualStack == _dependance.maxStack){ // si le niveau actuel de la dependance a atteint son maximum
                VerifierRessources(); // on appel VerifierRessources
            }
            else{ // si le niveau actuel de la dependance n'est pas a son maximum
                Debug.Log("Vous devez débloquer le skill précédent"); // Message d'avertissement
            }
        }
        else{ // si le skill ne possede pas de dependance
            VerifierRessources(); // on appel VerifierRessources
        }
    }

    public int AjusterCoutSkill(){
        float cout = _skillCostRef + (_skillCost * _arbre.absorbCount);
        return Mathf.RoundToInt(cout);
    }

    /// <summary>
    /// Fonction qui verifier si le joueur possede les ressources pour acheter le skill
    /// </summary>
    private void VerifierRessources(){
        realCost = _skillCost * (actualStack + 1); // on calcul le cout real du skill (temporaire)
        if(_playerRessources.naturePoint >= realCost){ // si les points de nature du joueur sont egal ou plus eleves que le cout real du skill
            _perso.AjusterPoint("naturePoint", -realCost); // on demande au personnage d'ajuter ses points de nature
            _arbre.CheckRessources();
            actualStack++; // on augmente le niveau du skill actuel de 1
            _savedTotalStack++; // on augmente le niveau total du skill
            _boiteExplication.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{actualStack}/{maxStack}"; // on met a jour le niveau affiche dans la boite d'explication
            if(actualStack != maxStack){ // si le niveau du skill actuel n'est pas egal a son maximum
                TextMeshProUGUI textCout = _boiteExplication.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                textCout.text = (_skillCost * (actualStack + 1)).ToString(); // on met a jour l'affichage du cout reel du skill
                if(_playerRessources.naturePoint >= realCost){
                    textCout.color = Color.green;
                }
                else{
                    textCout.color = Color.red;
                }
            }
            else{ // si le niveau actuel est egal a son maximum
                _boiteExplication.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Complet";
            }
            _basicStats.ChangerStats(_typeStats, _modifier, false); // on change la stat approprie dans BasicStats
            _arbre.CheckRessources(); // on demande au SkillTree de mettre a jour la disponibilite du bouton Absorber
            if(actualStack == maxStack){ // si le niveau actuel du skill a atteint son niveau maximal
                _bouton.interactable = false; // le skill actuel ne peut plus etre clique
                img.sprite = _iconFullStack; // on change l'image du skill pour son image complete
                if(_tHeritiers.Length > 0){ // si le skill possede au moins 1 heritier dans le _tHeritiers
                    foreach (SkillInfos heritier in _tHeritiers) // pour chaque heritiers dans le tableau
                    {
                        heritier._bouton.interactable = true; // on rend l'heritier cliquable
                        heritier.img.color = new Color(1f, 1f, 1f, 1f); // on remet sa couleur pleine opacite
                    }
                }
            }
        }
    }

    /// <summary>
    /// Foncition qui active ou desactive la boite explicative au passage de la souris
    /// </summary>
    public void ActiverBoite()
    {
        if(!_boiteExplication.activeInHierarchy){ // si la boite explicatioin n'est pas active
            realCost = _skillCost * (actualStack + 1);
            _boiteExplication.SetActive(true); // on active la boite explicative
            _boiteExplication.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _skillExplication.nomSkill; // on affiche le nom du skill
            _boiteExplication.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _skillExplication.explication; // on affiche l'explication du du skill
            _boiteExplication.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{actualStack}/{maxStack}"; // on affiche le niveau actuel sur le niveau maximum du skill
            TextMeshProUGUI textCout = _boiteExplication.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            if(_actualStack != _maxStack){
                textCout.text = (_skillCost * (actualStack + 1)).ToString(); // on affiche le cout du skill
                if(_playerRessources.naturePoint >= realCost){
                    textCout.color = Color.green;
                }
                else{
                    textCout.color = Color.red;
                }
            }
            else{ // si le niveau actuel est egal a son maximum
                textCout.text = "Complet";
            }
        }
        else{ // si la boite est active
            _boiteExplication.SetActive(false); // on desactive la boite
        }
    }

    /// <summary>
    /// Fonction qui remet le niveau actuel du skill a 0
    /// </summary>
    public void ResetSkill(){
        actualStack = 0; // le niveau actuel du skill est egal a 0
        _skillCost = AjusterCoutSkill();
    }

    /// <summary>
    /// Fonction qui verifie les dependance du skill actuel
    /// </summary>
    public void CheckDepend(){
        if(_dependance != null){ // si le skill actuel possede une dependance
            _bouton.interactable = (_dependance.actualStack == _dependance.maxStack && actualStack < maxStack); // le bouton est cliquable selon si le niveau actuel de la dependance 
            // est egal a son maximum ou non
            if (!_bouton.interactable){ // si le bouton du skill actuel n'est pas cliquable
                img.color = new Color(1f, 1f, 1f, 0.5f); // l'image du skill est a demi opacite
            }
            else{ // si le bouton est cliquable
                img.color = new Color(1f, 1f, 1f, 1f); // l'image est a plein opacite
            }
        }
        else{ // si le skill ne possede pas de dependance
            if(actualStack < maxStack){ // si le niveau actuel du skill est plus petit que son maximum
                _bouton.interactable = true; // le skill est cliquable
            }
            else{ // si le niveau actuel du skill est egal ou plus grand que son maximum
                _bouton.interactable = false; // le skill n'est plus cliquable
            }

        }
        CheckStack(); // on appel CheckStack
    }

    /// <summary>
    /// Fonction qui change l'image du skill selon son niveau
    /// </summary>
    private void CheckStack(){
        if(actualStack<maxStack){ // si le niveau actuel du skill est plus petit que son maximum
            img.sprite = _iconBase; // l'image est celle de base
        }
        else if(actualStack == maxStack){ // si le niveau actuel du skill est egal a son maximum
            img.sprite = _iconFullStack; // l'image est celle du skill maximum
        }
    }

    /// <summary>
    /// Fonction qui sauvegarde l'etat actuel du skill
    /// </summary>
    public void SaveSkill(){
        PlayerPrefs.SetInt(_nom + "total", _savedTotalStack); // on cree un PlayerPrefs avec le nom du skill + total et on sauvegarde son niveau total acquis
        PlayerPrefs.SetInt(_nom + "actual", _actualStack); // on cree un PlayerPrefs avec le nom du skill + actual et on sauvegarde le niveau actuel du skill
        Debug.Log("total sauvegardé: "+ _savedTotalStack + "/ actuel sauvegardé: " + _actualStack); // temporaire
    }

    /// <summary>
    /// Fonction qui charge les information sauvegardees du skill
    /// </summary>
    public void LoadSkill(){
        _savedTotalStack = PlayerPrefs.GetInt(_nom + "total"); // _saveTotalStack prend la valeur sauvegardee
        _actualStack = PlayerPrefs.GetInt(_nom + "actual"); // _actualStack prend la valeur sauvegardee
        _basicStats.ChangerStats(_typeStats, (_modifier*_savedTotalStack), true); // on met les informations du BasicStats a jour selon les stats sauvegardees
    }
}
