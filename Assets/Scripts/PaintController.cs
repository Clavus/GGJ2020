﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintController : MonoBehaviour
{
	public Color paintColor = Color.red;

	private MeshRenderer meshRenderer;
	private Texture2D texture;
	private int _brushSize;

	// Start is called before the first frame update
	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		UpdateTexture();
	}

	public void UpdateTexture()
	{
		// Copy texture
		texture = Instantiate(meshRenderer.material.mainTexture) as Texture2D;

		// Set copied texture as texture rendered
		MaterialPropertyBlock block = new MaterialPropertyBlock();
		block.SetTexture("_BaseMap", texture);
		meshRenderer.SetPropertyBlock(block);
	}

	private void OnTriggerStay(Collider other)
	{
		SphereCollider sphere = other.gameObject.GetComponent<SphereCollider>();
		PaintBrush2000 paintBrush = other.gameObject.GetComponentInParent<PaintBrush2000>();

		if (sphere == null || paintBrush == null)
			return;

		// Set the brushsize
		_brushSize = paintBrush.brushSize;


		// Calculate where the collider hit in this objects local space
		Vector3 hitPoint = transform.InverseTransformPoint(other.gameObject.transform.position);
		Vector3 hitPointScaled = new Vector3(hitPoint.x * gameObject.transform.localScale.x, hitPoint.y * gameObject.transform.localScale.y, hitPoint.z * gameObject.transform.localScale.z);
		Debug.Log("Hit at: " + hitPoint);
		Debug.Log("Scaled hit at: " + hitPointScaled);

		// Move this objects local space origin to the bottom left corner to match the Texture2D origin
		Vector3 halfSpriteSizeLocalSpace = meshRenderer.bounds.size / 2;
		Vector3 hitPointPixelSpace = hitPointScaled + halfSpriteSizeLocalSpace;
		Debug.Log("Hit in pixel space: " + hitPointPixelSpace);

		// Scale the hit from local size to pixel size
		float scaleX = (meshRenderer.bounds.size.x) / texture.width;
		float scaleY = (meshRenderer.bounds.size.y) / texture.height;
		hitPointPixelSpace.x /= scaleX;
		hitPointPixelSpace.y /= scaleY;
		Debug.Log(hitPointPixelSpace);

		// Change the color of the pixels to the paint color
		Color[] paint = Enumerable.Repeat(paintBrush.color, _brushSize * _brushSize).ToArray();
		texture.SetPixels((int)Mathf.Clamp(hitPointPixelSpace.x - (_brushSize / 2), 0, texture.width - _brushSize), (int)Mathf.Clamp(hitPointPixelSpace.y - (_brushSize / 2), 0, texture.height - _brushSize), _brushSize, _brushSize, paint, 0);
		texture.Apply();
	}
}
