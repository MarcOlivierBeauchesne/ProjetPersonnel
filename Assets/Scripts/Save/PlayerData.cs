using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int seed;
    public int naturePoints;
    public int naturePower;

    public PlayerData(Personnage player){
        seed = player.ressourcesPlayer.seedAmount;
        naturePoints = player.ressourcesPlayer.naturePoint;
        naturePower = player.ressourcesPlayer.naturePower;
    }
}
