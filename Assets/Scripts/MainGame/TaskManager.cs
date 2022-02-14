using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int _scoretaches;
    public int scoreTache{
        get => _scoretaches;
    }
    [SerializeField] private int _scoreArbre;
    public int scoreArbre{
        get => _scoreArbre;
    }
    [SerializeField] private BasicStats _basicStats;
    
    public void AjouterPoint(){
        int type = Random.Range(0, 2);
        int valeur = Random.Range(0,15);
        if(type == 0){
            _scoretaches += valeur;
        }
        else if(type == 1){
            _scoreArbre += valeur;
        }
    }

    public void ResetScore(){
        _scoreArbre = 0;
        _scoretaches = 0;
    }
}
