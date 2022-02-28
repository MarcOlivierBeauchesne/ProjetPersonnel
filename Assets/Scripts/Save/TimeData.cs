using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TimeData 
{
    public float minute;
    public int seconde;
    public int nbJour;

    public TimeData(Timer timer){
        minute = timer.minute;
        seconde = timer.seconde;
        nbJour = timer.nbJour;
    }
}
