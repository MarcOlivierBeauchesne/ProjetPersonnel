using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui controle l'affichage des differents score a la fin d'une journee
/// </summary>
public class DayManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tChampsEndDay; // tableau des differents champs de la page de fin de journee
    [SerializeField] Animator _animFenetre; // animator de la fenetre de fin de journee
    [SerializeField] Timer _timer;
    [SerializeField] TaskManager _taskManager; // reference au TaskManager qui gere les taches
    [SerializeField] private GenerateurSalle _genSalle;
    public GenerateurSalle genSalle{
        get=>_genSalle;
    }
    private int _indexTableau = 0;

    Deforestation _deforestation;
    BasicStats _baseStats;
    Deforestation _defoManager;

    // Start is called before the first frame update
    void Start()
    {
        _baseStats = GetComponent<BasicStats>();
        _deforestation = GetComponent<Deforestation>();
        _defoManager = GetComponent<Deforestation>();
        ResetChamps(); // On appel ResetChamps
    }

    /// <summary>
    /// Fonction qui demarre la coroutine d'affichage des points
    /// </summary>
    public void AfficherPoint(){
        if(_baseStats.deforestLevel + _baseStats.deforestAugment >= _baseStats.deforestPool){
            Debug.Log("la couleur est brune");
        }
        else{
            Debug.Log("la couleur est bleu");
        }
        StartCoroutine(CoroutineAfficherPoint()); // on demarre la coroutine CoroutineAfficherPoint
    }

    /// <summary>
    /// Coroutine qui affiche les points de la journee
    /// </summary>
    /// <returns>attente entre l'affichage des differents champs</returns>
    private IEnumerator CoroutineAfficherPoint(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        switch(_indexTableau){ // selon _indexTableau
            case 0 : { // si _indexTableau est de 0
                _tChampsEndDay[0].SetActive(true); // on affiche le titre de la page
                break; // on sort de la condition
            }
            case 1 : { // si _indexTableau est de 1
                _tChampsEndDay[1].SetActive(true); // on affiche le titre Deforestation
                _tChampsEndDay[2].SetActive(true); // on affiche le titre Nature
                break; // on sort de la condition
            }
            case 2 : { // si _indexTableau est de 2
                _tChampsEndDay[3].SetActive(true); // on affiche la categorie Arbres plantes
                _tChampsEndDay[3].transform.GetChild(0).GetComponent<Text>().text = _taskManager.scoreArbre.ToString(); // la valeur affichee de la categorie prend la valeur du scoreArbre du _taskManager
                _tChampsEndDay[4].SetActive(true); // on affiche la categorie Progression (deforestation)
                _tChampsEndDay[4].transform.GetChild(0).GetComponent<Text>().text = _baseStats.deforestAugment.ToString(); // la valeur affichee de la categorie prend la valeur du deforestAugment du BasicStats
                break; // on sort de la condition
            }
            case 3 : { // si _indexTableau est de 3
                _tChampsEndDay[5].SetActive(true); // on affiche la categorie taches effectuees
                _tChampsEndDay[5].transform.GetChild(0).GetComponent<Text>().text = _taskManager.scoreTache.ToString(); // la valeur affichee de la categorie prend la valeur du scoreTache du _taskManager 
                if(_timer.nbJour > 0){
                    _defoManager.AjusterDefoLevel();
                }
                break; // on sort de la condition
            }
            case 4 : { // si _indexTableau est de 4
                _tChampsEndDay[6].SetActive(true); // on affiche la categorie Total des scores
                _tChampsEndDay[6].transform.GetChild(0).GetComponent<Text>().text = _baseStats.deforestAugment.ToString(); // le valeur du total prend la valeur du deforestAugment du BasicStats
                _tChampsEndDay[7].SetActive(true); // on affiche la categorie Total des scores
                _tChampsEndDay[7].transform.GetChild(0).GetComponent<Text>().text = (_taskManager.scoreArbre + _taskManager.scoreTache).ToString(); // le valeur du total prend la valeur cumulee des arbres plantes et des taches accomplies
                _genSalle.GenererFirstSalle();
                Debug.Log("On recommence la carte");
                break; // on sort de la condition
            }
            case 5 : { // si _indexTableau est de 5¸
                if(_deforestation.actualDefo > _deforestation.maxDefo){
                    _tChampsEndDay[8].SetActive(true);
                    _tChampsEndDay[11].SetActive(true);
                }
                else{
                    _tChampsEndDay[9].SetActive(true); // on affiche le bouton pour passer a la prochaine journee
                    _tChampsEndDay[10].SetActive(true);
                }
                break; // on sort de la condition
            }
        }
        _indexTableau++; // on augmente l'index de 1
        if(_indexTableau < _tChampsEndDay.Length){ // si l'index est plus petit que la longueur du _tChampsEndDay
            StartCoroutine(CoroutineAfficherPoint()); // on redemarre la coroutine CoroutineAfficherPoint
        }
    }

    /// <summary>
    /// Fonction qui ferme tous les champs de texte et remet l'index a 0
    /// </summary>
    public void ResetChamps(){
        for (int i = 0; i < _tChampsEndDay.Length; i++) // selon la longueur du tableau
        {
            _tChampsEndDay[i].SetActive(false); // on prend chaque element du tableau et on le ferme
            StopAllCoroutines();
        }
        _indexTableau = 0; // on remet _indexTableau a 0
    }
}