using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
    public Color paintbucketColor;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null) _renderer.material.color = paintbucketColor;
    }

    public void ChangePaintOnBrush(PaintBrush brush)
    {
        brush.color = paintbucketColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PaintBrush>() != null)
        {
            PaintBrush brush = other.gameObject.GetComponent<PaintBrush>();
            ChangePaintOnBrush(brush);
        }
    }
}
