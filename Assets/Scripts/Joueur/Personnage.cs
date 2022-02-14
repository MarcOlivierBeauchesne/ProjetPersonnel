using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui controle le deplacement du perso et met a jour ses ressources
/// </summary>
public class Personnage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtNaturePoint; // acces prive pour le champs de texte des points de nature du joueur
    [SerializeField] private TextMeshProUGUI _txtNaturePower; // acces prive pour le champs de texte de la puissance naturelle du joueur
    [SerializeField] private PlayerRessources _ressourcesPlayer; // reference de PlayerRessources du joueur
    // Start is called before the first frame update
    void Start()
    {
        _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString(); // on affiche les points de nature dans le champs approprie
        _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on affiche la puissance naturelle dans le champs approprie
    }

    /// <summary>
    /// Fonction qui ajuste les points de nature, de puissance et de graines du joueur et 
    /// met a jour le champs de texte apprporie
    /// </summary>
    /// <param name="ressources">Type de ressources que l'on modifie</param>
    /// <param name="valeur">valeur que l'on doit ajuster a la ressources</param>
    public void AjusterPoint(string ressources , int valeur){
        switch (ressources) // selon le type de ressource
        {
            case "naturePower": // si la ressource est "naturePower"
                _ressourcesPlayer.naturePower += valeur; // on change le naturePower des _ressourcesPlayer selon la valeur
                _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on met a jour l'affichage des point de nature
                break; // on sort de la condition
            case "seed": // si la ressources est "seed"
                _ressourcesPlayer.seedAmount += valeur; // on change la quantite de graine du joueur
                break; // on sort de la condition
            case "naturePoint": // si la ressources est "naturePoint"
                _ressourcesPlayer.naturePoint += valeur; // on change les naturePoint du _ressourcesPlayer selon la valeur
                _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString();
                break; // on sort de la condition
        }
    }

}
