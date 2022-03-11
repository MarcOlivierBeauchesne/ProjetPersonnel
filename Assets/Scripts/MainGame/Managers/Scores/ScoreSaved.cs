using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaved : ScriptableObject
{
    private Dictionary<string, bool> _dictScore = new Dictionary<string, bool>(){
        
    };
    public Dictionary<string, bool> dictScore{
        get=>_dictScore;
        set{
            _dictScore = value;
        }
    }

    List<string> _keyScoreList = new List<string>();
    public List<string> keyScoreList{
        get=> _keyScoreList;
        set{
            _keyScoreList = value;
        }
    }
}
