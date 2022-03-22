using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fonction qui controle et affiche le temps disponible par jour dans le jeu
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Managers")] // Identification de la section Managers
    [SerializeField] private BasicStats _basicStats; // acces prive pour le BasicStats _basicStats
    [SerializeField] private DayManager _dayManager; // reference au DayManager
    [SerializeField] private TaskManager _taskManager; // reference au TaskManager
    [SerializeField] private DayLightManager _dayLightManager; // reference au DayLightManager
    [SerializeField] private MissionManager _missionManager; // reference au MissionManager
    [SerializeField] private Tutoriel _tuto; // reference au Tutoriel
    [SerializeField] private Personnage _perso; // reference au Personnage
    [Header("Champs de texte")] // Identification de la section Champs de texte
    [SerializeField] private Text _champTimer; // acces prive pour le champs de texte qui affiche le Timer
    [SerializeField] private Text _champsJour; // champs de texte qui affiche a quelle journee le joueur est rendu
    [Header("Animators")] // Identification de la section Animators 
    [SerializeField] private Animator _dayWindowAnim; // acces prive pour l'animator de la fenetre de journee
    [Header("Objet dans la scene")] // Identification de la section Objet dans la scene 
    [SerializeField] private GameObject _champsEnnemis;
    [SerializeField] private GameObject _champsProjectiles;
    [Header("Informations de journee")] // Identification de la sectionInformations de journee 
    [SerializeField] private float _endDayWaitTime = 2f; // temps d'attente a la fin de la journee
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonFinJournee; // son quand la journee se termine

    private int _nbJour = 1; // journee actuelle
    public int nbJour{ // acces public a la journee actuelle
        get => _nbJour; // par nbJour, on retourne _nbJour
        set{ // on change la valeur de _nbJour
            _nbJour = value; // _nbJour prend la valeur de value
        }
    }
    private float _minute = 1; // acces prive au nombre de minutes disponible par jour
    public float minute // acces public au nombre de minutes disponible par jour
    {
        get => _minute; // par minute, on retourne la valeur _minute
        set // on change la valeur de _minute
        {
            _minute = value; // par minute, on change la valeur de _minute
            _champTimer.text = minute + ":" + seconde; // on met a jour l'affichage du timer (minute:secondes)
        }
    }
    private int _seconde = 0; // acces prive pour les secondes
    public int seconde // acces public pour les secondes 
    {
        get => _seconde; // par seconde, on retourne _seconde
        set // on change la valeur de _seconde
        {
            _seconde = value; // par seconde, on change la valeur de _seconde
            if (_seconde < 10) { _champTimer.text = minute + ":0" + seconde; } // si _seconde est plus petit que 10, on ajout un 0 devant le chiffre des secondes
            else { _champTimer.text = minute + ":" + seconde; } // sinon (_seconde est plus grand que 10) on met a jour l'affichage du timer (minute:secondes)
        }
    }

    /// <summary>
    /// fonction publique qui demarre une journee
    /// </summary>
    public void DemarrerJournee(){
        StartCoroutine(CoroutineDemarrerJournee()); // on demarre la coroutine CoroutineDemarrerJournee
    }

    /// <summary>
    /// Coroutine qui entame le debut d'une journee
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDemarrerJournee(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        StartCoroutine(CoroutineDebut()); // on demarre la coroutine CoroutineDebut
    }

    /// <summary>
    /// Coroutine qui gere l'animation du champs qui affiche la journee
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineChampsJour(){
        _champsJour.text = "Jour " + _nbJour; // _champsJour affiche la journee actuelle
        _champsJour.gameObject.GetComponent<Animator>().SetBool("NewJour", true); // on change le bool NewJour de l'animator de _champsJour pour true
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        _champsJour.gameObject.GetComponent<Animator>().SetBool("NewJour", false); // on change le bool NewJour de l'animator de _champsJour pour false
    }

    /// <summary>
    /// Coroutine qui demarre une journee
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDebut(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        _champTimer.text = minute + ":00"; // le texte du timer affiche les minute disponible + 00
        StartCoroutine(CoroutineTemps()); // on demarre la coroutine CoroutineTemps
        StartCoroutine(CoroutineChampsJour()); // on demarre la coroutine CoroutineChampsJour
        yield return new WaitForSeconds(2f); // on attend 2 secondes
        StartCoroutine(CoroutineTips(1)); // on demarre la coroutine CoroutineTips
        _missionManager.InitierMission(); // on demande au MissionManager d'initier les mission
        _dayWindowAnim.SetTrigger("NewDay"); // on demande a l'animator de la fenetre de journee de declancher le trigger NewDay
    }

    /// <summary>
    /// Coroutine qui controle la reducation du temps
    /// </summary>
    /// <returns> attente de 1 seconde </returns>
    IEnumerator CoroutineTemps()
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
            GameAudio.instance.JouerSon(_sonFinJournee); // on joue un son quand la journee se termine
            GameAudio.instance.AjusterSon(false); // on arrete la musique
            _perso.ResetRot(); // on appel ResetRot du Personnage
            _perso.ChangerRot(false); // on appel ChangerRot du Personnage
            _perso.ChangerEtat(false); // on appel ChangerEtat du Personnage
            StartCoroutine(CoroutineFinJournee()); // on demarre la coroutine CoroutineFinJournee
            _champsJour.text = ""; // on vide le texte de _champsJour
            _dayManager.genSalle.ClearTache(); // on demande a GenerateurSalle de suppriemr toutes les taches
            _champsEnnemis.SetActive(false); // on ferme la boite d'explication de la tache ennemis
            _champsProjectiles.SetActive(false); // on ferme la boite d'explication pour la tacheCentre
            _missionManager.ResetDayMission(); // on demande au MissionManager de reinitialiser les missions de la journee
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
        _minute = _basicStats.dayTime; // minute prend la valeur de daytime du BasicStats
        StartCoroutine(CoroutineTemps()); // on démarre la coroutine CoroutineTemps
        _taskManager.ResetScore(); // on demande au TaskManager de reinitialiser les scores de la journee
        _nbJour++; // on augmente _nbJour de 1
        StartCoroutine(CoroutineChampsJour()); // on demarre la coroutine CoroutineChampsJour
        _dayWindowAnim.SetTrigger("NewDay"); // on declanche le trigger NewDay du _dayWindowAnim
        _perso.ChangerEtat(true); // on appel ChangerEtat du Personnage
        _dayLightManager.AjusterVitesseJour(); // on demande au DayLightManager d'ajuster al vitesse d'aniamtion de la lumiere de la journee
        _missionManager.fondMission.SetActive(true); // on demande au MissionManager d'activer le fon du tableau de mission
        _missionManager.InitierMission(); // on demande au MissionManager d'initier les missions de la journee
    }

    /// <summary>
    /// Coroutine qui affiches les conseils en debut de partie
    /// </summary>
    /// <param name="waitTime">attente avant le premier conseil</param>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineTips(int waitTime){
        yield return new WaitForSeconds(waitTime); // on attend selon waitTime
        _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
        _tuto.OuvrirTips(1); // on demande au Tutoriel d'ouvrir le tips 1
        yield return new WaitForSeconds(5f); // on attend 5 secondes
        _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
        _tuto.OuvrirTips(0); // on demande au Tutoriel d'ouvrir le tips 0
        yield return new WaitForSeconds(5f); // on attend 5 secondes
        _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
        _tuto.OuvrirTips(9); // on demande au Tutoriel d'ouvrir le tips 9
        yield return new WaitForSeconds(5f); // on attend 5 secondes
        _tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
        _tuto.OuvrirTips(3); // on demande au Tutoriel d'ouvrir le tips 3
    }

    /// <summary>
    /// Fonction publique qui permet de charger les informations sauvegardees du temps
    /// </summary>
    /// <param name="newJour">Journee sauvegardees</param>
    /// <param name="newMinute">Minutes sauvegardees</param>
    /// <param name="newSeconde">Secondes sauvegardees</param>
    public void SetupTime(int newJour, float newMinute, int newSeconde){
        _nbJour = newJour; // _nbJour prend la valeur de newJour
        minute = newMinute; // minute prend la valeur de newMinute
        seconde = newSeconde; // seconde prend la valeur de newSeconde
    }

    /// <summary>
    /// Fonction qui demarre le calcul du temps
    /// </summary>
    public void StartTimer(){
        StartCoroutine(CoroutineTemps()); // on demarre la CoroutineCoroutineTemps
    }

}
