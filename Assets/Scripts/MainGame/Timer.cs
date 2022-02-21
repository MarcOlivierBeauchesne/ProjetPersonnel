using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fonction qui controle et affiche le temps disponible par jour dans le jeu
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] private Text _champTimer; // acces prive pour le champs de texte qui affiche le Timer
    [SerializeField] private BasicStats _basicStats; // acces prive pour le BasicStats _basicStats
    [SerializeField] private Animator _dayWindowAnim; // acces prive pour l'animator de la fenetre de journee
    [SerializeField] private DayManager _dayManager; // reference au DayManager
    [SerializeField] private TaskManager _taskManager; // reference au TaskManager
    [SerializeField] private Deforestation _defoManager;
    [SerializeField] private float _endDayWaitTime = 2f; // temps d'attente a la fin de la journee
    [SerializeField] private int _nbJour = 0;
    public int nbJour{
        get => _nbJour;
    }

    [SerializeField] private float _minute = 5; // acces prive au nombre de minutes disponible par jour
    public float minute // acces public au nombre de minutes disponible par jour
    {
        get => _minute; // par minute, on retourne la valeur _minute
        set
        {
            _minute = value; // par minute, on change la valeur de _minute
            _champTimer.text = minute + ":" + seconde; // on met a jour l'affichage du timer (minute:secondes)
        }
    }
    [SerializeField] private int _seconde = 0; // acces prive pour les secondes
    public int seconde // acces public pour les secondes 
    {
        get => _seconde; // par seconde, on retourne _seconde
        set
        {
            _seconde = value; // par seconde, on change la valeur de _seconde
            if (_seconde < 10) { _champTimer.text = minute + ":0" + seconde; } // si _seconde est plus petit que 10, on ajout un 0 devant le chiffre des secondes
            else { _champTimer.text = minute + ":" + seconde; } // sinon (_seconde est plus grand que 10) on met a jour l'affichage du timer (minute:secondes)
        }
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        ProchaineJournee(); // on appel ProchaineJournee
    }

    /// <summary>
    /// Coroutine qui controle la reducation du temps
    /// </summary>
    /// <returns> attente de 1 seconde </returns>
    private IEnumerator CoroutineTemps()
    {
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        seconde--; // on reduit seconde de 1
        if (seconde < 0) // si seconde est plus petit que 0
        {
            seconde = 59; // seconde est egal a 59
            minute--; // on reduit minute de 1
        }
        if (minute == 0 && seconde == 0) // si minute et seconde sont egals a 0
        {
            _dayWindowAnim.SetBool("EndDay", true); // on met le bool de _dayWindowAnim a true
            StartCoroutine(CoroutineFinJournee()); // on demarre la coroutine CoroutineFinJournee
            Debug.Log("Fin de la journee");
            // on termine la journee
            // verification si le joueur est revenu a sa zone de depart
        }
        else if (minute > 0 || seconde >= 0) // si minute est plus grand que 0 ou seconde est plus grand ou egal a 0
        {
            StartCoroutine(CoroutineTemps()); // on demarre la coroutine CoroutineTemps
        }
    }

    /// <summary>
    /// Coroutine qui cree un delai avant de demander au DayManager d'afficher les scores
    /// </summary>
    /// <returns>temps d'attente avant d'afficher les scores</returns>
    private IEnumerator CoroutineFinJournee(){
        yield return new WaitForSeconds(_endDayWaitTime); // delai d'attente(_endDayWaitTime) avant d'afficher les points
        _dayManager.AfficherPoint(); // on demande au DayManager d'afficher les points
    }

    /// <summary>
    /// Fonction qui reinitialise les champs afin de demarrer une nouvelle journee
    /// </summary>
    public void ProchaineJournee(){
        if(_nbJour > 0){
            _defoManager.AjusterDefoLevel();
        }
        _dayManager.ResetChamps(); // on demande au DayManager de fermer tous les champs
        _dayWindowAnim.SetBool("EndDay", false); // on met le bool de _dayWindowAnim a false
        ResetTimer(); // on Apple ResetTimer
        _champTimer.text = minute + ":00"; // le texte du timer affiche les minute disponible + 00
        StartCoroutine(CoroutineTemps()); // on démarre la coroutine CoroutineTemps
        _taskManager.ResetScore(); // on demande au TaskManager de reinitialiser les scores de la journee
        _nbJour++;
    }

    /// <summary>
    /// Fonction qui ajuste les minute de la journee selon le dayTime de _basicStats
    /// </summary>
    public void ResetTimer()
    {
        _minute = _basicStats.dayTime; // _minute prend la valeur du dayTime de _basicStats
    }

    /// <summary>
    /// Fonction qui demarre le calcul du temps
    /// </summary>
    public void StartTimer(){
        StartCoroutine(CoroutineTemps()); // on demarre la CoroutineCoroutineTemps
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            _basicStats.dayTime++;
            ResetTimer();
        }
        else if(Input.GetKeyDown(KeyCode.E)){
            StartTimer();
            _champTimer.text = minute + ":00";
            _dayWindowAnim.SetBool("EndDay", false);
        }
    }
}
