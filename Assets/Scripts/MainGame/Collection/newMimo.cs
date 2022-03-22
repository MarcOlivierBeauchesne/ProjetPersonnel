using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le popUp du nouveau mimo decouvert
/// </summary>
public class newMimo : MonoBehaviour
{
    [SerializeField] float _destructionDelay = 3f; // float pour l'attente avant la destruction de l'objet
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineLifeTime()); // on demarre la coroutine CoroutineLifeTime
    }

    /// <summary>
    /// Coroutine qui cree un delay avant la destruction du popUp
    /// </summary>
    /// <returns>temps d'attente</returns>
    IEnumerator CoroutineLifeTime(){
        yield return new WaitForSeconds(_destructionDelay); // on attend selon _destructionDelay
        Destroy(gameObject); // on detruit le popUp
    }
}
