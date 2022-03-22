using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui gere le comportement d'un Pop up de point
/// </summary>
public class PopUpPoints : MonoBehaviour
{
    [SerializeField] private float _showTime = 2f; // temps d'affichage du pop up
    private float _monteStock = 0; // augmentation de la position en y
    private TextMeshPro _textPoints; // champs de texte du popup

    /// <summary>
    /// Fonction qui met a jour le champs de texte du popUp
    /// </summary>
    /// <param name="points">Point a afficher</param>
    public void Setup(int points){
        _textPoints = GetComponent<TextMeshPro>();
        _textPoints.SetText(points.ToString()); // on affiche textuellement les points
        StartCoroutine(CoroutineDestroyPopUp()); // on demarre la coroutine CoroutineDestroyPopUp
    }

    /// <summary>
    /// Coroutine qui cree un delay avant la destruction du popUp
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineDestroyPopUp(){
        yield return new WaitForSeconds(1f); // on attend 1 seconde
        Destroy(gameObject); // on detruit le popUp
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _showTime -= Time.deltaTime; // on reduit _showTime par Time.deltaTime
        _monteStock += Time.deltaTime/200; // on augmente _monteStock par Time.deltaTime divise par 200
        Vector2 pos = new Vector2(transform.position.x, transform.position.y +_monteStock); // on cree une position ajuste avec le y + _monteStock
        transform.position = pos; // on change la positio du popUp pour le faire monter
    }
}

