using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSwitcher : MonoBehaviour
{
    public GameObject PaintCanvasObject;
    public Material[] Paintings;


    private static PaintingSwitcher _instance;
    public static PaintingSwitcher Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Update()
    {
        
    }

    public void SwitchPainting()
    {
        int index = UnityEngine.Random.Range(0, Paintings.Length);
        PaintCanvasObject.GetComponent<MeshRenderer>().material = Paintings[index];
    }
}
