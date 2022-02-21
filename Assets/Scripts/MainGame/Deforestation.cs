using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deforestation : MonoBehaviour
{
    [SerializeField] private Slider _defoSlider;
    [SerializeField] private BasicStats _basicStats;
    [SerializeField] private Timer _timer;
    [SerializeField] private TextMeshProUGUI _textDefo;
    private float _actualDefo = 0;
    private float _maxDefo = 0;
    // Start is called before the first frame update
    void Start()
    {
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        AjusterDefoVisuel();
    }

    public void AjusterDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        float actualVisual = (1 * _actualDefo) / _maxDefo;
        _defoSlider.value = actualVisual;
        _textDefo.text = $"{_actualDefo}/{_maxDefo}";
    }

    public void AjusterDefoLevel(){
        _basicStats.deforestLevel += _basicStats.deforestAugment;
        _actualDefo = _basicStats.deforestLevel;
        _basicStats.deforestAugment += _basicStats.deforestAugment * (_timer.nbJour + 0.5f);
        _maxDefo = _basicStats.deforestPool;
        AjusterDefoVisuel();
    }

}
