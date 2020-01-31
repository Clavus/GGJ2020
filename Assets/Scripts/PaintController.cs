using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Texture2D texture;
    private Color[] paint;

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

        paint = Enumerable.Repeat(Color.red, 10 * 10).ToArray();
    }

    // Update is called once per frame
    void Update()
    {  
        
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 hitPoint = transform.InverseTransformPoint(other.gameObject.transform.position);
        Debug.Log("Hit at: " + hitPoint);

        Vector3 halfSpriteSizeLocalSpace = spriteRenderer.bounds.size / 2;

        Vector3 hitPointPixelSpace = hitPoint + halfSpriteSizeLocalSpace;
        Debug.Log("Hit in pixel space: " + hitPointPixelSpace);

        float scaleX = (spriteRenderer.bounds.size.x) / texture.width;
        float scaleY = (spriteRenderer.bounds.size.y) / texture.height;

        hitPointPixelSpace.x /= scaleX;
        hitPointPixelSpace.y /= scaleY;

        texture.SetPixels((int)hitPointPixelSpace.x, (int)hitPointPixelSpace.y, 10, 10, paint);
        texture.Apply();
    }
}
