using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class StatsData
{
    public float speed;
    public float npGain;
    public float npMaxPool;
    public float seedDrop;
    public float daytime;
    public float defoAugment;
    public float defoLevel;
    public float defoPool;

    public StatsData(BasicStats stats){
        speed = stats.mouvementSpeed;
        npGain = stats.npGain;
        npMaxPool = stats.npMaxPool;
        seedDrop = stats.seedDrop;
        daytime = stats.dayTime;
        defoAugment = stats.deforestAugment;
        defoLevel = stats.deforestLevel;
        defoPool = stats.deforestPool;
    }
}
