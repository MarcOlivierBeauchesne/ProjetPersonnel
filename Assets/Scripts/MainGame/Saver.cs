using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui controle la sauvegarde du jeu
/// </summary>
public class Saver : MonoBehaviour
{

    /// <summary>
    /// Fonction qui sauvegarde les elements important de la partie
    /// </summary>
    public void SaveGame(){
        
    }

    /// <summary>
    /// Fonction qui supprime le PlayerPrefs "Game" et reinitialise les elements du jeu
    /// </summary>
    public void DeleteGame(){
        PlayerPrefs.DeleteKey("Game"); // on supprime le PlayerPrefs "Game"
    }

    /// <summary>
    /// Fonction qui cree une nouvelle partie
    /// </summary>
    public void NewGame(){
        PlayerPrefs.SetString("Game", "new"); // creation du PlayerPrefs "Game" avec comme valeur "new"
    }
}
