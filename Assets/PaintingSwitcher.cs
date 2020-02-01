using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSwitcher : MonoBehaviour
{
    public GameObject PaintCanvasObject;
    public Material[] Paintings;
    private Material[] _paintings;

    private void Awake()
    {
        _paintings = (Material[])Paintings.Clone();
    }

    public void SwitchPainting()
    {
        var currentMaterial = PaintCanvasObject.GetComponent<MeshRenderer>().material;
        int index = UnityEngine.Random.Range(0, Paintings.Length);
        bool quitLoop = false;
        int nullCount = 0;
        while (_paintings[index] == null || currentMaterial.mainTexture == _paintings[index].mainTexture)
        {
            index = UnityEngine.Random.Range(0, _paintings.Length);
            for(int i = 0; i < _paintings.Length; i++) if (_paintings[i] == null) nullCount++;
            if (nullCount == _paintings.Length)
            {
                _paintings = (Material[])Paintings.Clone();
                quitLoop = true;
            }
        }

        PaintCanvasObject.GetComponent<MeshRenderer>().material = _paintings[index];
        _paintings[index] = null;
        PaintCanvasObject.GetComponent<PaintController>().UpdateTexture();
    }
}
