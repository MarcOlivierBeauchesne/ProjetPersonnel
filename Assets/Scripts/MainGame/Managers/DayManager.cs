using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui controle l'affichage des differents score a la fin d'une journee
/// </summary>
public class DayManager : MonoBehaviour
{
    [Header("Managers")] // Identification de la section Managers
    [SerializeField] Timer _timer; // reference au Timer
    [SerializeField] TaskManager _taskManager; // reference au TaskManager qui gere les taches
    [SerializeField] private GenerateurSalle _genSalle; // reference au GenerateurSalle
    public GenerateurSalle genSalle{ // acces public au GenerateurSalle
        get=>_genSalle; // par genSalle, on retourne _genSalle
    }
    [Header("Composante de la fenetre")] // Identification de la section Composante de la fenetre
    [SerializeField] GameObject[] _tChampsEndDay; // tableau des differents champs de la page de fin de journee
    [SerializeField] Animator _animFenetre; // animator de la fenetre de fin de journee
    [SerializeField] Animator _animDefoDefaite; // Animator de la fenetre de defaite
    [Header("Composante de la fenetre")] // Identification de la section Composante de la fenetre
    [SerializeField] GameObject _fenetreLoading; // fenetre de changement

    private int _indexTableau = 0; // index qui separe les differents affichages lors de al fin d'une journee

    Deforestation _deforestation; // reference a Deforestation
    BasicStats _baseStats; // reference au BasicStats

    // Start is called before the first frame update
    void Start()
    {
        _baseStats = GetComponent<BasicStats>(); // _baseStats prend la valeur du BasicStats du gameObject
        _deforestation = GetComponent<Deforestation>(); // _deforestation prend la valeur du Deforestation du gameObject
        ResetChamps(); // On appel ResetChamps
    }

    /// <summary>
    /// Fonction qui demarre la coroutine d'affichage des points
    /// </summary>
    public void AfficherPoint(){
        if(_baseStats.deforestLevel + _baseStats.deforestAugment >= _baseStats.deforestPool){ // si le niveau actuel de deforestation est plus grand ou egal au maximum de deforestation possible
            StartCoroutine(CoroutineDefaite()); // on demarre la coroutine CoroutineDefaite
        }
        else{ // si le niveau actuel de deforestation est plus petit que le maximum de deforestation possible
            _animFenetre.SetTrigger("EndDay"); // on declanche le trigger EndDay de _animFenetre
            StartCoroutine(CoroutineAfficherPoint()); // on demarre la coroutine CoroutineAfficherPoint
        }
    }

    /// <summary>
    /// Coroutine qui affiche les differents champs de la fenetre de defaite
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDefaite(){
        _animFenetre.SetTrigger("EndGame"); // on declanche le trigger EndGame de _animFenetre
        _animDefoDefaite.SetTrigger("DefoDefaite"); // on declanche le trigger DefoDefaite de _animDefoDefaite
        yield return new WaitForSeconds(3f); // on attend 3 secondes
        _deforestation.AjusterDefoLevel(); // on demande  a Deforestation d'ajuster son visuel
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        _tChampsEndDay[11].SetActive(true); // on active le message de defaite
        yield return new WaitForSeconds(1f); // on attend 1 secondes
        _tChampsEndDay[8].SetActive(true); // on active le bouton pour retourner au menu
        _tChampsEndDay[12].SetActive(true); // on active le bouton our reessayer
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
                _tChampsEndDay[5].transform.GetChild(0).GetComponent<Text>().text = _taskManager.scoreTache.ToString(); // la valeur affichee de la categorie prend la valeur du scoreTache du _taskManager 
                _tChampsEndDay[13].transform.GetChild(0).GetComponent<Text>().text = _taskManager.scoreMission.ToString(); // la valeur affichee de la categorie prend la valeur du  du _taskManager 
                _tChampsEndDay[14].transform.GetChild(0).GetComponent<Text>().text = _taskManager.scoreMimo.ToString(); // la valeur affichee de la categorie prend la valeur du  du _taskManager 
                break; // on sort de la condition
            }
            case 3 : { // si _indexTableau est de 3
                _tChampsEndDay[5].SetActive(true); // on affiche la categorie taches effectuees
                _tChampsEndDay[13].SetActive(true); // on affiche la categorie mission
                _tChampsEndDay[14].SetActive(true); // on affiche la categorie mimos sauves
                if(_timer.nbJour > 0){ // si le nbJour de Timer est plus grand que 0
                    _deforestation.AjusterDefoLevel(); // on demande  a Deforestation d'ajuster son visuel
                }
                break; // on sort de la condition
            }
            case 4 : { // si _indexTableau est de 4
                _tChampsEndDay[6].SetActive(true); // on affiche la categorie Total des scores
                _tChampsEndDay[6].transform.GetChild(0).GetComponent<Text>().text = _baseStats.deforestAugment.ToString(); // le valeur du total prend la valeur du deforestAugment du BasicStats
                _tChampsEndDay[7].SetActive(true); // on affiche la categorie Total des scores
                int totalPoint = _taskManager.scoreArbre + _taskManager.scoreTache + _taskManager.scoreMission + _taskManager.scoreMimo; // on affiche le total de tous les scores
                _tChampsEndDay[7].transform.GetChild(0).GetComponent<Text>().text = totalPoint.ToString(); // le valeur du total prend la valeur cumulee des arbres plantes et des taches accomplies
                break; // on sort de la condition
            }
            case 5 : { // si _indexTableau est de 5¸
                _tChampsEndDay[9].SetActive(true); // on affiche le bouton pour passer a la prochaine journee
                _tChampsEndDay[10].SetActive(true); // on affiche un message de felicitation
                break; // on sort de la condition
            }
        }
        _indexTableau++; // on augmente l'index de 1
        if(_indexTableau < _tChampsEndDay.Length){ // si l'index est plus petit que la longueur du _tChampsEndDay
            StartCoroutine(CoroutineAfficherPoint()); // on redemarre la coroutine CoroutineAfficherPoint
        }
    }

    /// <summary>
    /// Coroutine qui enclanche une nouvelle journee
    /// </summary>
    public void DemarrerJournee(){
        StartCoroutine(CoroutineNouvelleJournee()); // on demarre la coroutine CoroutineNouvelleJournee
    }

    /// <summary>
    /// Coroutine qui declanche la generation d'une nouvelle carte
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineNouvelleJournee(){
        _fenetreLoading.SetActive(true); // on active la fenetre de chargement
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        _genSalle.DemarrerCarte(); // on demande au GenerateurSalle de generer une nouvelle foret
    }

    /// <summary>
    /// Fonction qui ferme tous les champs de texte et remet l'index a 0
    /// </summary>
    public void ResetChamps(){
        for (int i = 0; i < _tChampsEndDay.Length; i++) // selon la longueur du tableau _tChampsEndDay
        {
            _tChampsEndDay[i].SetActive(false); // on prend chaque element du tableau et on le ferme
            StopAllCoroutines(); // on arrete toutes les coroutines
        }
        _indexTableau = 0; // on remet _indexTableau a 0
    }
}
