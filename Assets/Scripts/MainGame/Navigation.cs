using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script qui controle la navigation au travers du jeu
/// </summary>
public class Navigation : MonoBehaviour
{
    [SerializeField] private GameObject _boutonContinuer; // acces prive au _boutonContinuer
    [SerializeField] private Saver _gameSaver; // acces prive au Saver _gameSaver

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex; // int indexScene prend la valeur de l'index de la scene actuelle
        if(indexScene == 0){ // si indexScene == 0
            if(PlayerPrefs.GetString("Game") == ""){ // Si le PlayerPrefs "Game" est egal a ""
                _boutonContinuer.SetActive(false); // le _boutonContinuer est desactive
            }
            else{ // sinon (si le PlayerPrefs "Game" n'est pas egal a "")
                _boutonContinuer.SetActive(true); // on active le _boutonContinuer
            }
        }
    }

    /// <summary>
    /// Fonction qui change de scene selon la valeur recu
    /// </summary>
    /// <param name="index">index de la scene voulu dans le build</param>
    public void ChangerScene(int index){
        if(index == 1){ // si l'index est de 1
            _gameSaver.DeleteGame(); // on appel DeleteGame du _gameSaver
        }
        SceneManager.LoadScene(index); // on charge la scenedu build dont l'index est index
    }

    /// <summary>
    /// Fonction qui fait quitter le jeu
    /// </summary>
    public void Quitter(){
        Application.Quit(); // on demande au jeu de fermer
    }
}
