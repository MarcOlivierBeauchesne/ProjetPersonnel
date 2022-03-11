using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui controle l'ouverture du menu et la fermeture des autre fenetres du jeu
/// </summary>
public class MenuJeu : MonoBehaviour
{
    [SerializeField] private GameObject _fenetreMenu; // acces prive a la fenetre du menu du jeu
    [SerializeField] private GameObject _fenetreSkillTree; // acces prive a la fenetre de l'arbre des talents
    [SerializeField] private GameObject _fenetreOptions; // acces prive a la fenetre des options
    [SerializeField] private GameObject _fenetreConfirmations; // acces prive a la fenetre de confirmation de sauvegarde
    [SerializeField] private GameObject _fenetreCollection;

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
            Time.timeScale = 0;
        }
        else{ // sinon (la _fenetreMenu est active dans la hierarchie)
            _fenetreMenu.SetActive(false); // on desactive la _fenetreMenu
            Time.timeScale = 1;
        }
    }

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
            else if(_fenetreCollection.activeInHierarchy){
                _fenetreCollection.SetActive(false);
            }
            else{
                ActiverFenetreMenu(); // on appel ActiverFenetreMenu
            }
        }
        if(Input.GetKeyDown(KeyCode.C)){
            _fenetreCollection.GetComponent<Collection>().ActiverBoite();
        }
        else if(Input.GetKeyDown(KeyCode.F)){
            if(_fenetreSkillTree.activeInHierarchy){
                _fenetreSkillTree.SetActive(false);
                _fenetreSkillTree.transform.GetChild(0).gameObject.SetActive(false);
            }
            else{
                _fenetreSkillTree.SetActive(true);
            }
        }
    }
}
