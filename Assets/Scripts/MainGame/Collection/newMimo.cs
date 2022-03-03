using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newMimo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineLifeTime());
    }

    private IEnumerator CoroutineLifeTime(){
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
