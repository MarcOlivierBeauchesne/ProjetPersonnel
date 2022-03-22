using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui controle l'ouverture du menu et la fermeture des autre fenetres du jeu
/// </summary>
public class MenuJeu : MonoBehaviour
{
    [Header("Fenetres du jeu")] // Identification de la section Fenetres du jeu
    [SerializeField] private GameObject _fenetreMenu; // acces prive a la fenetre du menu du jeu
    [SerializeField] private GameObject _fenetreSkillTree; // acces prive a la fenetre de l'arbre des talents
    [SerializeField] private GameObject _fenetreOptions; // acces prive a la fenetre des options
    [SerializeField] private GameObject _fenetreConfirmations; // acces prive a la fenetre de confirmation de sauvegarde
    [SerializeField] private GameObject _fenetreCollection; // fenetre de la collection

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _fenetreMenu.SetActive(false); // la _fenetreMenu devient inactive
    }

    /// <summary>
    /// Fonction qui active ou desactive la _fenetreMenu
    /// </summary>
    public void ActiverFenetreMenu(){
        if(!_fenetreMenu.activeInHierarchy){ // si la _fenetreMenu est inactive dans la hierarchie
            _fenetreMenu.SetActive(true); // on active _fenetreMenu
            Time.timeScale = 0; // on arrete le temps
        }
        else{ // sinon (la _fenetreMenu est active dans la hierarchie)
            _fenetreMenu.SetActive(false); // on desactive la _fenetreMenu
            Time.timeScale = 1; // le temps va a vitesse normale
        }
    }

    /// <summary>
    /// fonction qui active ou desactive la fenetre de confirmation pour quitter/sauvegarder
    /// </summary>
    public void ActiverFeneterConf(){
        if(!_fenetreConfirmations.activeInHierarchy){ // si la _fenetreMenu est inactive dans la hierarchie
            _fenetreConfirmations.SetActive(true); // on active _fenetreMenu
        }
        else{ // sinon (la _fenetreMenu est active dans la hierarchie)
            _fenetreConfirmations.SetActive(false); // on desactive la _fenetreMenu
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){ // si le joueur appuie sur la touche Escape
            if(_fenetreSkillTree.activeInHierarchy){ // si la _fenetreSkillTree est active dans la hierarchie
                _fenetreSkillTree.SetActive(false); // on desactive la _fenetreSkillTree
            }
            else if(_fenetreOptions.activeInHierarchy){ // si la _fenetreOptions est active dans la hierarchie
                _fenetreOptions.SetActive(false); // on desactive la _fenetreOptions
            }
            else if(_fenetreConfirmations.activeInHierarchy){ // si la _fenetreConfirmations est active dans la hierarchie
                _fenetreConfirmations.SetActive(false); // on desactive la _fenetreConfirmations
            }
            else if(_fenetreCollection.activeInHierarchy){ // si la _fenetreCollection est active dans la hierarchie
                _fenetreCollection.SetActive(false); // on desactive _fenetreCollection
            }
            else{ // si la _fenetreCollection n'est pas active dans la hierarchie
                ActiverFenetreMenu(); // on appel ActiverFenetreMenu
            }
        }
        if(Input.GetKeyDown(KeyCode.C)){ // si le joueur appuie sur C
            _fenetreCollection.GetComponent<Collection>().ActiverBoite(); // on appel ActiverBoite de la Collection
        }
        else if(Input.GetKeyDown(KeyCode.F)){ // si le joueur appuie sur F
            if(_fenetreSkillTree.activeInHierarchy){ // si la _fenetreSkillTree est active dans la hierarchie
                _fenetreSkillTree.SetActive(false); // on desactive la _fenetreSkillTree
                _fenetreSkillTree.transform.GetChild(0).gameObject.SetActive(false); // on desactive la fenetre de description de skill
            }
            else{ // si la _fenetreSkillTree n'est pas active dans la hierarchie
                _fenetreSkillTree.SetActive(true); // on active la _fenetreSkillTree
            }
        }
    }
}
