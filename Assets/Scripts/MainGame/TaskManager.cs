using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere l'attribution des points lorsque le joueur accompli une tache
/// </summary>
public class TaskManager : MonoBehaviour
{
    [SerializeField] GameObject _goNpPoints;
    [SerializeField] GameObject _goNoixGain;
    [SerializeField] private int _scoretaches; // acces prive pour le score obtenu pour les taches
    public int scoreTache{ // acces public pour le score obtenu pour les taches
        get => _scoretaches; // par scoreTache, on retourne _scoreTache
    }
    [SerializeField] private int _scoreArbre; // acces prive pour le score obtenu pour avoir plante des arbres
    public int scoreArbre{ // acces public pour le score obtenu pour avoir plante des arbres
        get => _scoreArbre; // par scoreArbre, on retourne _scoreArbre
    }
    [SerializeField] private BasicStats _basicStats; // reference pour le BasicStats
    
    /// <summary>
    /// Fonction public que les taches appelent pour attribuer des points au joueur
    /// </summary>
    public void AjouterPoint(TypeTache type, int valeur){
        switch(type){
            case TypeTache.Arbre :
                _scoreArbre += valeur; 
            break;
            case TypeTache.Tache : 
                _scoretaches += valeur;
            break;
        }
    }

    /// <summary>
    /// Fonction qui sert a reinitialiser les scores de la journee
    /// </summary>
    public void ResetScore(){
        _scoreArbre = 0; // le _scoreArbre devient 0
        _scoretaches = 0; // le _scoreTaches devient 0
    }

    public void CreatePopUpPoints(Vector2 pos, int amount, string type){
        if(type == "tache"){
            GameObject pointsPopUp = Instantiate(_goNpPoints, pos, Quaternion.identity);
            pointsPopUp.GetComponent<PopUpPoints>().Setup(amount);
        }
        else if(type == "noix"){
            GameObject noixPopUp = Instantiate(_goNoixGain, pos, Quaternion.identity);
            noixPopUp.GetComponent<PopUpPoints>().Setup((int)_basicStats.seedDrop);
        }
    }
}

public enum TypeTache
{
    Arbre,
    Tache,
    Centre,
    Ennemis,
    Aucun
}
