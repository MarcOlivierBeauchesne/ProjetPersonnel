using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deforestation : MonoBehaviour
{
    [SerializeField] private Slider _defoSlider;
    [SerializeField] private BasicStats _basicStats;
    private float _actualDefo = 0;
    private float _maxDefo = 0;
    // Start is called before the first frame update
    void Start()
    {
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        AjusterDefoVisuel();
    }

    private void AjusterDefoVisuel(){
        float actualVisual = (1 * _actualDefo) / _maxDefo;
        _defoSlider.value = actualVisual;
        Debug.Log(actualVisual + "niveau");
    }

    public void AjusterDefoLevel(){

    }
}
