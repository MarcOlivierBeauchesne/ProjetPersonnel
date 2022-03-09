using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject pour les ressources du joueur
/// </summary>
[CreateAssetMenu(fileName = "ressourcesPlayer", menuName = "ressourcesPlayer")]
public class PlayerRessources : ScriptableObject
{
    [SerializeField]private int _seedAmount = 4; // acces prive pour le nombre de graines que possede le joueur
    public int seedAmount{ // acces public pour le nombre de graines que possede le joueur
        get => _seedAmount; // par seedAmount, on retourne la valeur prive _seedAmount
        set{
            _seedAmount = value; // par seedAmount, on change la valeur de _seedAmount
        }
    }
    [SerializeField] private int _naturePoint = 100000; // acces prive pour la quantite de point de nature que possede le joueur
    public int naturePoint{ // acces public pour la quantite de point de nature que possede le joueur
        get => _naturePoint; // par naturePoint, on retourne la valeur prive _naturePoint
        set{
            _naturePoint = value; // par naturePoint, on change la valeur de _naturePoint
            if(_naturePoint < 0){
                _naturePoint = 0;
            }
        }
    }
    [SerializeField]private int _naturePower = 100; // acces prive pour la quantite de puissance de nature que possede le joueur
    public int naturePower{ // acces public pour la quantite de puissance de nature que possede le joueur
        get => _naturePower; // par naturePower, on retourne la valeur prive _naturePower
        set{
            _naturePower = value; // par naturePower, on change la valeur de _naturePower
            if(_naturePower < 0){
                _naturePower = 0;
            }
        }
    }
}
