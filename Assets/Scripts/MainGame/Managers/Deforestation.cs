using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script qui gere la deforestion et son affichage visuel
/// </summary>
public class Deforestation : MonoBehaviour
{
    [Header("Managers")] // Identification de la section Managers
    [SerializeField] private Timer _timer; // reference au Tiemr
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    [Header("Composante de l'affichage")] // Identification de la section Composante de l'affichage
    [SerializeField] private Slider _defoSlider; // slider de la deforestation
    [SerializeField] private Slider _nextDefoSlider; // slider de la deforestationa  venir
    [SerializeField] private Text _textDefo; // texte qui affiche la deforestation actuel par rapport au maximum
    [Header("PopUp d'augmentation")] // Identification de la section PopUp d'augmentation
    [SerializeField] private GameObject _goTxtAugmentDefo; // prefab de l'augmentation
    [SerializeField] private GameObject _canvas; // GameObject du canva

    private float _actualDefo = 0; // niveau de deforestation actuel
    public float actualDefo{ // acces public au niveau de deforestation actuel
        get=>_actualDefo; // par actualDefo, on retourne _actualDefo
    }
    private float _maxDefo = 0; // niveau de deforestation maximale
    public float maxDefo{ // acces public au niveau de deforestation maximale 
        get=>_maxDefo; // par maxDefo, on retourne _maxDefo
    }
    private float _nextDefo = 0; // niveau de deforestation a venir
    private float actualVisual = 0f; //niveau visuel actuel 
    private float nextVisual = 0f; // niveau visuel a venir
    float t = 0f; // indication pour le lerp d'augmentation de deforestation actuelle
    float b = 0f; // indication pour le lerp d'augmentation de deforestation a venir
    

    // Start is called before the first frame update
    void Start()
    {
        _maxDefo = _basicStats.deforestPool; // _maxDefo prend la valeur de deforestPool du BasicStats
        _actualDefo = _basicStats.deforestLevel; // _actualDefo  prend la valeur du deforestLevel de BasicStats
        _nextDefo = _actualDefo + _basicStats.deforestAugment; // _nextDefo represente _actualDefo + deforestAugment du BasicStats
        AjusterDefoVisuel(); // on appel AjusterDefoVisuel
    }

    /// <summary>
    /// Fonction publique qui ajuste le niveau de deforestation actuel
    /// </summary>
    public void AjusterDefoLevel(){
        _basicStats.deforestLevel += _basicStats.deforestAugment; // on augmente le nivau de deforestation actuelle par l'augmentation de deforestation du  BasicStats
        _actualDefo = _basicStats.deforestLevel; // _actualDefo  prend la valeur du deforestLevel de BasicStats 
        _basicStats.deforestAugment = (_basicStats.deforestAugmentRef * _timer.nbJour) - ((_basicStats.npGain-100)/2); // on ajuste la valeur de l'augmentation de la deforestation pour la journee
        _maxDefo = _basicStats.deforestPool; // _maxDefo prend la valeur de deforestPool du BasicStats
        t = 0f; // on remet t a 0
        b = 0f; // on remet b a 0
        AjusterNextDefoVisuel(); // on appel AjusterNextDefoVisuel
    }

    /// <summary>
    /// Fonction public qui ajuste le niveau visuel de la deforestation
    /// </summary>
    public void AjusterDefoVisuel(){
        _maxDefo = _basicStats.deforestPool; // _maxDefo prend la valeur de deforestPool du BasicStats
        _actualDefo = _basicStats.deforestLevel; // _actualDefo  prend la valeur du deforestLevel de BasicStats 
        actualVisual = (1 * _actualDefo) / _maxDefo; // actualVisual prend une valeur entre 0 et 1
        _textDefo.text = actualDefo.ToString("f1") + "/"+ maxDefo; // le texte de deforestation affiche la deforestion actuel par rapport au maximum
        AjusterNextDefoVisuel(); // on appel AjusterNextDefoVisuel
    }

    /// <summary>
    /// Fonction publique qui permet aux ennemis de la foret d'augmenter la deforestation
    /// </summary>
    /// <param name="amount">montant de l'augmentation</param>
    public void AugmentationEnnemi(float amount){
        StartCoroutine(CoroutineAugmentation(amount)); // on demarre la coroutine CoroutineAugmentation avec amount
        _basicStats.deforestLevel+= amount; // on augmente le niveau actuel de deforestation de amount
        AjusterDefoVisuel(); // on appel AjusterDefoVisuel
    }

    /// <summary>
    /// Fonction qui ajuste le viseul de la deforestation a venir
    /// </summary>
    public void AjusterNextDefoVisuel(){
        _maxDefo = _basicStats.deforestPool;// _maxDefo prend la valeur de deforestPool du BasicStats
        _nextDefo = _basicStats.deforestAugment; // _nextDefo represente _actualDefo + deforestAugment du BasicStats
        nextVisual = (1 * _actualDefo + _basicStats.deforestAugment) / _maxDefo; // nextVisual represente une valeur entre 0 et 1 ,
        // comme la deforestation actuelle mais avec l'augmentation qui est ajoute
    }

    /// <summary>
    /// Coroutine qui augmente la deforestation par les ennemis
    /// </summary>
    /// <param name="amount">montant de l'augmentation</param>
    /// <returns>temps d'attente</returns>
    private IEnumerator CoroutineAugmentation(float amount){
        GameObject augmentation = Instantiate(_goTxtAugmentDefo, transform.position, Quaternion.identity); // augmentation represente un popUp d'augmentation de deforestation
        augmentation.transform.SetParent(_canvas.transform.GetChild(2)); // augmentation devient l'enfent du 3em enfant de canvas
        RectTransform pos = augmentation.GetComponent<RectTransform>(); // pos prend le RectTransform d'augmentation
        pos.anchoredPosition = new Vector3(0f, -500f, 0f); // on ajuste la position d'augmentation
        augmentation.GetComponent<Text>().text = "+ " + amount.ToString(); // on aficche textuellement l'augmentation de deforestation
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        Destroy(augmentation); // on detruit l'augmentation
    }

    private void Update()
    {
        _defoSlider.value = Mathf.Lerp(_defoSlider.value, actualVisual, t); // on ajuste la valeur du slider de deforestation actuel
        _nextDefoSlider.value = Mathf.Lerp(_nextDefoSlider.value, nextVisual, b); // on ajuste la valeur du slider de al deforestationa  venir
        _textDefo.text = actualDefo.ToString("f1") + "/"+ maxDefo; //  // le texte de deforestation affiche la deforestion actuel par rapport au maximum
        t += (0.1f * Time.deltaTime); // on augmente t
        b += (0.1f * Time.deltaTime); // on augmente b
        if(t>0.99f){ // si t est plus grand que 0.99
            t = 0f; // on remet t a 0
            b = 0f; // on remet b a 0
        }
    }
}
