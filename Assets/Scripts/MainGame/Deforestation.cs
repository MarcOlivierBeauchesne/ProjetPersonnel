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
    [SerializeField] private GameObject _goTxtAugmentDefo;
    [SerializeField] private Transform _posAugment;
    [SerializeField] private GameObject _canvas;
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
        _basicStats.deforestAugment = (_basicStats.deforestAugmentRef * _timer.nbJour) - (_basicStats.npGain-100);
        _maxDefo = _basicStats.deforestPool;
        AjusterDefoVisuel();
        AjusterNextDefoVisuel();
    }

    public void AugmentationEnnemi(float amount){
        StartCoroutine(CoroutineAugmentation(amount));
        _basicStats.deforestLevel+= amount;
        AjusterDefoVisuel();
    }

    private IEnumerator CoroutineAugmentation(float amount){
        GameObject augmentation = Instantiate(_goTxtAugmentDefo, transform.position, Quaternion.identity);
        augmentation.transform.SetParent(_canvas.transform.GetChild(1));
        RectTransform pos = augmentation.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector3(0f, -500f, 0f);
        augmentation.GetComponent<Text>().text = "+ " + amount.ToString();
        yield return new WaitForSeconds(2f);
        Destroy(augmentation);
    }

    public void AjusterNextDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _nextDefo = _actualDefo + _basicStats.deforestAugment;
        float actualVisual = (1 * _actualDefo + _basicStats.deforestAugment) / _maxDefo;
        _nextDefoSlider.value = actualVisual;
    }
}
