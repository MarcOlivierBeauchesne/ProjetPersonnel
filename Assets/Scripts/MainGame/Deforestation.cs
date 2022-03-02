using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deforestation : MonoBehaviour
{
    [SerializeField] private Slider _defoSlider;
    [SerializeField] private Slider _nextDefoSlider;
    [SerializeField] private BasicStats _basicStats;
    [SerializeField] private Timer _timer;
    [SerializeField] private Text _textDefo;
    private float _actualDefo = 0;
    public float actualDefo{
        get=>_actualDefo;
    }
    private float _maxDefo = 0;
    public float maxDefo{
        get=>_maxDefo;
    }
    private float _nextDefo = 0;
    // Start is called before the first frame update
    void Start()
    {
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        _nextDefo = _actualDefo + _basicStats.deforestAugment;
        AjusterDefoVisuel();
        AjusterNextDefoVisuel();
    }

    public void AjusterDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        float actualVisual = (1 * _actualDefo) / _maxDefo;
        _defoSlider.value = actualVisual;
        _textDefo.text = $"{_actualDefo}/{_maxDefo}";
        AjusterNextDefoVisuel();
    }

    public void AjusterDefoLevel(){
        _basicStats.deforestLevel += _basicStats.deforestAugment;
        _actualDefo = _basicStats.deforestLevel;
        Debug.Log("ref : " + _basicStats.deforestAugmentRef + " jour : " + _timer.nbJour);
        Debug.Log("total augment: " + _basicStats.deforestAugmentRef*_timer.nbJour);
        _basicStats.deforestAugment = (_basicStats.deforestAugmentRef * _timer.nbJour) - (_basicStats.npGain-100);
        _maxDefo = _basicStats.deforestPool;
        AjusterDefoVisuel();
        AjusterNextDefoVisuel();
    }

    public void AjusterNextDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _nextDefo = _actualDefo + _basicStats.deforestAugment;
        float actualVisual = (1 * _actualDefo + _basicStats.deforestAugment) / _maxDefo;
        _nextDefoSlider.value = actualVisual;
    }
}
