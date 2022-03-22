using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere les scores des joueur
/// </summary>
public class ScoreSaved : ScriptableObject
{
    private Dictionary<string, int> _dictScore = new Dictionary<string, int>(){ // dictionnaire des scores avec nom => score
        
    };
    public Dictionary<string, int> dictScore{  // acces public au dictionnaire des scores
        get=>_dictScore; // par dictScore, on retourne _dictScore
        set{ // on change la valeur de _dictScore
            _dictScore = value; // _dictScore prend la valeur recu
        }
    }

    List<string> _keyScoreList = new List<string>(); // liste des key de _dictScore
    public List<string> keyScoreList{ // acces public a la liste des keys de _dictScore
        get=> _keyScoreList; // par keyScoreList, on retourne _keyScoreList
        set{ // on change la valeur de _keyScoreList
            _keyScoreList = value; // _keyScoreList prend la valeur recu
        }
    }
}
