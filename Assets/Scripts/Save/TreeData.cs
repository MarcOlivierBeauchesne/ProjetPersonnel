using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TreeData
{
    public float absorbCount;

    public TreeData(Skilltree tree){
        absorbCount = tree.absorbCount;
    }
}
