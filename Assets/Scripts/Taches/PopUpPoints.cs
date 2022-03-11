using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpPoints : MonoBehaviour
{
    [SerializeField] private float _showTime = 2f;
    private float _monteStock = 0;
    private TextMeshPro _textPoints;

    Color _color;

    void Awake()
    {
        _textPoints = GetComponent<TextMeshPro>();
        _color = _textPoints.color;
    }

    public void Setup(int points){
        _textPoints.SetText(points.ToString());
        if(points<0){
            _color = Color.red;
        }
        StartCoroutine(CoroutineDestroyPopUp());
    }

    private IEnumerator CoroutineDestroyPopUp(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _showTime -= Time.deltaTime;
        _monteStock += Time.deltaTime/200;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y +_monteStock);
        transform.position = pos;
    }
}

