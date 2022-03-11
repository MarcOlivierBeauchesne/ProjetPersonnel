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
    [SerializeField] private Text _champsJour;
    [SerializeField] private BasicStats _basicStats; // acces prive pour le BasicStats _basicStats
    [SerializeField] private Animator _dayWindowAnim; // acces prive pour l'animator de la fenetre de journee
    [SerializeField] private DayManager _dayManager; // reference au DayManager
    [SerializeField] private TaskManager _taskManager; // reference au TaskManager
    [SerializeField] private DayLightManager _dayLightManager;
    [SerializeField] private GameObject _champsEnnemis;
    [SerializeField] private GameObject _champsProjectiles;
    [SerializeField] private GameObject _champsExpliEnnemi;
    [SerializeField] private Deforestation _defoManager;
    [SerializeField] private Tutoriel _tuto;
    [SerializeField] private Animator _animDaylight;
    [SerializeField] private Personnage _perso;
    [SerializeField] private float _endDayWaitTime = 2f; // temps d'attente a la fin de la journee
    [SerializeField] private int _nbJour = 1;
    public int nbJour{
        get => _nbJour;
        set{
            _nbJour = value;
        }
    }

    [SerializeField] private float _minute = 1; // acces prive au nombre de minutes disponible par jour
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
        StartCoroutine(CoroutineDemarrerJournee());
    }

    public IEnumerator CoroutineDemarrerJournee(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(CoroutineDebut());
    }

    private IEnumerator CoroutineChampsJour(){
        _champsJour.text = "Jour " + _nbJour;
        _champsJour.gameObject.GetComponent<Animator>().SetBool("NewJour", true);
        yield return new WaitForSeconds(2f);
        _champsJour.gameObject.GetComponent<Animator>().SetBool("NewJour", false);
    }

    private IEnumerator CoroutineDebut(){
        yield return new WaitForSeconds(1f);
        _champTimer.text = minute + ":00"; // le texte du timer affiche les minute disponible + 00
        StartCoroutine(CoroutineTemps()); // on demarre la coroutine CoroutineTemps
        StartCoroutine(CoroutineChampsJour());
        yield return new WaitForSeconds(2f);
        StartCoroutine(CoroutineTips(1));
        _dayWindowAnim.SetTrigger("NewDay");
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
            _perso.ResetRot();
            _perso.ChangerRot(false);
            _perso.ChangerEtat(false);
            _dayWindowAnim.SetTrigger("EndDay");
            StartCoroutine(CoroutineFinJournee()); // on demarre la coroutine CoroutineFinJournee
            _champsJour.text = "";
            _dayManager.genSalle.ClearTache();
            _champsEnnemis.SetActive(false);
            _champsExpliEnnemi.SetActive(false);
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
        _dayManager.ResetChamps(); // on demande au DayManager de fermer tous les champs
        _dayWindowAnim.SetBool("EndDay", false); // on met le bool de _dayWindowAnim a false
        _minute = _basicStats.dayTime;
        StartCoroutine(CoroutineTemps()); // on démarre la coroutine CoroutineTemps
        _taskManager.ResetScore(); // on demande au TaskManager de reinitialiser les scores de la journee
        _nbJour++;
        StartCoroutine(CoroutineChampsJour());
        _dayWindowAnim.SetTrigger("NewDay");
        _perso.ChangerEtat(true);
        _dayLightManager.AjusterVitesseJour();
    }

    private IEnumerator CoroutineTips(int waitTime){
        yield return new WaitForSeconds(waitTime);
        _tuto.gameObject.SetActive(true);
        _tuto.OuvrirTips(1);
        yield return new WaitForSeconds(5f);
        _tuto.gameObject.SetActive(true);
        _tuto.OuvrirTips(0);
        yield return new WaitForSeconds(2f);
        _tuto.gameObject.SetActive(true);
        _tuto.OuvrirTips(3);
    }

    public void SetupTime(int newJour, float newMinute, int newSeconde){
        _nbJour = newJour;
        minute = newMinute;
        seconde = newSeconde;
    }

    /// <summary>
    /// Fonction qui demarre le calcul du temps
    /// </summary>
    public void StartTimer(){
        StartCoroutine(CoroutineTemps()); // on demarre la CoroutineCoroutineTemps
    }

}
