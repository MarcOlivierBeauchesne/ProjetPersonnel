using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "infosCollection", menuName = "infosCollection")]
public class InfosCollection : ScriptableObject
{
    [SerializeField] private string _nomMimo;
    public string nomMimo{
        get => _nomMimo;
    }

    [SerializeField] private Sprite _imageObjet;
    public Sprite imageObjet{
        get => _imageObjet;
    }
    [TextArea]
    [SerializeField] private string _textObjet;
    public string textObjet{
        get => _textObjet;
    }

    [SerializeField] private bool _isFound = false;
    public bool isFound{
        get => _isFound;
        set{
            _isFound = value;
        }
    }
    [SerializeField] int _mimoValue = 0;
    public int mimoValue{
        get=> _mimoValue;
        set{
            _mimoValue = value;
        }
    }
}
