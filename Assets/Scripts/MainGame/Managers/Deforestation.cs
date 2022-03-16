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

    private bool _nextPoint = true;
    bool _actualPoint = true;
    private float actualVisual = 0f;
    private float nextVisual = 0f;
    float t = 0f;
    float b = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        _nextDefo = _actualDefo + _basicStats.deforestAugment;
        AjusterDefoVisuel();
    }

    public void AjusterDefoLevel(){
        _basicStats.deforestLevel += _basicStats.deforestAugment;
        _actualDefo = _basicStats.deforestLevel;
        _basicStats.deforestAugment = (_basicStats.deforestAugmentRef * _timer.nbJour) - (_basicStats.npGain-100);
        _maxDefo = _basicStats.deforestPool;
        t = 0f;
        b = 0f;
        AjusterNextDefoVisuel();
    }

    public void AjusterDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _actualDefo = _basicStats.deforestLevel;
        actualVisual = (1 * _actualDefo) / _maxDefo;
        _textDefo.text = actualDefo.ToString("f1") + "/"+ maxDefo;
        AjusterNextDefoVisuel();
    }

    public void AugmentationEnnemi(float amount){
        StartCoroutine(CoroutineAugmentation(amount));
        _basicStats.deforestLevel+= amount;
        AjusterDefoVisuel();
    }

    public void AjusterNextDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;
        _nextDefo = _basicStats.deforestAugment;
        nextVisual = (1 * _actualDefo + _basicStats.deforestAugment) / _maxDefo;
    }

    private IEnumerator CoroutineAugmentation(float amount){
        GameObject augmentation = Instantiate(_goTxtAugmentDefo, transform.position, Quaternion.identity);
        augmentation.transform.SetParent(_canvas.transform.GetChild(2));
        RectTransform pos = augmentation.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector3(0f, -500f, 0f);
        augmentation.GetComponent<Text>().text = "+ " + amount.ToString();
        yield return new WaitForSeconds(2f);
        Destroy(augmentation);
    }

    private void Update()
    {
        _defoSlider.value = Mathf.Lerp(_defoSlider.value, actualVisual, t);
        _nextDefoSlider.value = Mathf.Lerp(_nextDefoSlider.value, nextVisual, b);
        _textDefo.text = actualDefo.ToString("f1") + "/"+ maxDefo;
        t += (0.1f * Time.deltaTime);
        b += (0.1f * Time.deltaTime);
        if(t>0.99f){
            t = 0f;
            b = 0f;
        }
    }
}
