using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightManager : MonoBehaviour
{
    [SerializeField] BasicStats _basicStats;
    [SerializeField] Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim.speed = 1 / _basicStats.dayTime;
    }

    public void AjusterVitesseJour(){
        _anim.speed = 1 / _basicStats.dayTime;
        _anim.SetTrigger("DayTime");
    }

}
