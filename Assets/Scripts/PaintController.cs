using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintController : MonoBehaviour
{
    public int brushSize = 20;
    public Color paintColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Copy textire
        texture = Instantiate(spriteRenderer.sprite.texture) as Texture2D;

        // Set copied texture as texture rendered
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", texture);
        spriteRenderer.SetPropertyBlock(block);
    }

    private void OnTriggerStay(Collider other)
    {
        // Calculate where the collider hit in this objects local space
        Vector3 hitPoint = transform.InverseTransformPoint(other.gameObject.transform.position);
        //Debug.Log("Hit at: " + hitPoint);

        // Move this objects local space origin to the bottom left corner to match the Texture2D origin
        Vector3 halfSpriteSizeLocalSpace = spriteRenderer.bounds.size / 2;
        Vector3 hitPointPixelSpace = hitPoint + halfSpriteSizeLocalSpace;
        //Debug.Log("Hit in pixel space: " + hitPointPixelSpace);

        // Scale the hit from local size to pixel size
        float scaleX = (spriteRenderer.bounds.size.x) / texture.width;
        float scaleY = (spriteRenderer.bounds.size.y) / texture.height;
        hitPointPixelSpace.x /= scaleX;
        hitPointPixelSpace.y /= scaleY;
        //Debug.Log(hitPointPixelSpace);

        // Change the color of the pixels to the paint color
        Color[] paint = Enumerable.Repeat(paintColor, brushSize * brushSize).ToArray();
        texture.SetPixels((int)Mathf.Clamp(hitPointPixelSpace.x - (brushSize / 2), 0, texture.width - brushSize), (int)Mathf.Clamp(hitPointPixelSpace.y - (brushSize / 2), 0, texture.height - brushSize), brushSize, brushSize, paint, 0);
        texture.Apply();
    }
}
