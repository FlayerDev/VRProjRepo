using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSetter : MonoBehaviour
{
    [Range(0f,1f)]public float Alpha = 0;
    public Color colortemp;
    public MeshRenderer meshrend;
    public Material mat;

    void Update()
    {
        colortemp.a = Alpha;
        meshrend.sharedMaterials[0].color = colortemp;
    }
}
