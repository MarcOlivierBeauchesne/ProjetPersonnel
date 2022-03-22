using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere l'attribution des points lorsque le joueur accompli une tache
/// </summary>
public class TaskManager : MonoBehaviour
{
    [Header("PopUp des gains de points")] // Identification de la section PopUp des gains de points
    [SerializeField] GameObject _goNpPoints; // popUp de point positif
    [SerializeField] GameObject _goNpPointsNegatif; // popUp de point negatifs
    [SerializeField] GameObject _goNoixGain; // popup pour un gain de noix
    [Header("Scores")] // Identification de la section Scores
    [SerializeField] private int _scoretaches; // acces prive pour le score obtenu pour les taches
    public int scoreTache{ // acces public pour le score obtenu pour les taches
        get => _scoretaches; // par scoreTache, on retourne _scoreTache
    }
    [SerializeField] private int _scoreArbre; // acces prive pour le score obtenu pour avoir plante des arbres
    public int scoreArbre{ // acces public pour le score obtenu pour avoir plante des arbres
        get => _scoreArbre; // par scoreArbre, on retourne _scoreArbre
    }
    [SerializeField] private int _scoreMission; // acces prive pour le score obtenu pour avoir plante des arbres
    public int scoreMission{ // acces public pour le score obtenu pour avoir plante des arbres
        get => _scoreMission; // par scoreArbre, on retourne _scoreArbre
    }
    [SerializeField] private int _scoreMimo; // acces prive pour le score obtenu pour avoir plante des arbres
    public int scoreMimo{ // acces public pour le score obtenu pour avoir plante des arbres
        get => _scoreMimo; // par scoreArbre, on retourne _scoreArbre
    }
    [Header("Managers")] // Identification de la section Managers
    [SerializeField] private BasicStats _basicStats; // reference pour le BasicStats
    
    /// <summary>
    /// Fonction publique que les taches appelent pour ajuster les differents scores
    /// </summary>
    public void AjouterPoint(TypeTache type, int valeur){
        switch(type){ // switch selon type
            case TypeTache.Arbre : // si le type est Arbre
                _scoreArbre += valeur; // on ajouter valeur a _scoreAbre
            break; // on sort de la condition
            case TypeTache.Tache :  // si le type est Tache
                _scoretaches += valeur; // on joute valeur a _scoreTaches
            break; // on sort de la condition
            case TypeTache.Mission : // si le type est Mission
                _scoreMission += valeur; // on ajoute valeur a _scoreMission
            break; // on sort de la condition
            case TypeTache.Mimo : // si le type est Mimo
                _scoreMimo += valeur; // on ajoute valeur a _scoreMimo
            break; // on sort de la condition
        }
    }

    /// <summary>
    /// Fonction qui sert a reinitialiser les scores de la journee
    /// </summary>
    public void ResetScore(){
        _scoreArbre = 0; // le _scoreArbre devient 0
        _scoretaches = 0; // le _scoreTaches devient 0
        _scoreMimo = 0; // le _scoreMimo devient 0
        _scoreMission = 0; // le _scoreMission devient 0
    }

    /// <summary>
    /// Fonction publique qui cree un popUp de point
    /// </summary>
    /// <param name="pos"> position du popUp</param>
    /// <param name="amount">Montant a afficher</param>
    /// <param name="type">type de popUp a afficher</param>
    public void CreatePopUpPoints(Vector2 pos, int amount, string type){
        if(type == "tache"){ // si el type est tache
            GameObject pointsPopUp = null;
            if(amount>0){ // si amount est plus grand que 0
                pointsPopUp = Instantiate(_goNpPoints, pos, Quaternion.identity); // on cree un popUp _goNpPoints a la position pos
            }
            else{ // si amount est plus petit que 0
                pointsPopUp = Instantiate(_goNpPointsNegatif, pos, Quaternion.identity); // on cree un popUp _goNpPointsNegatif a la position pos
            }
            pointsPopUp.GetComponent<PopUpPoints>().Setup(amount); // on demande au PopUpPoints du popUp de faire son Setup
        }
        else if(type == "noix"){ // si le type est noix
            GameObject noixPopUp = Instantiate(_goNoixGain, pos, Quaternion.identity); // on cree un popUp _goNoixGain a la position pos
            noixPopUp.GetComponent<PopUpPoints>().Setup((int)_basicStats.seedDrop); // on demande au PopUpPoints du popUp de faire son Setup
        }
    }
}

/// <summary>
/// enum public pour le type de tache
/// </summary>
public enum TypeTache
{
    Arbre,
    Tache,
    Centre,
    Ennemis,
    Aucun,
    Mimo,
    Mission
}
