using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintController : MonoBehaviour
{
	public int correctnessCalculateRowsPerFrame = 9;
	public bool canColorBlack = false;

	private MeshRenderer meshRenderer;
	private Texture2D paintTexture;

	//private AudioSource audio;

	// Start is called before the first frame update
	void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		SetCanvasTexture(meshRenderer.sharedMaterial.GetTexture("_ColorLayerTexture") as Texture2D, null);
		//audio = GetComponent<AudioSource>();
	}

	public void SetCanvasTexture(Texture2D colorTexture, Texture2D lineArtTexture)
	{
		// Copy texture
		paintTexture = Instantiate(colorTexture) as Texture2D;

		// Set copied texture as texture rendered
		MaterialPropertyBlock block = new MaterialPropertyBlock();
		meshRenderer.GetPropertyBlock(block);
		block.SetTexture("_ColorLayerTexture", paintTexture);
		if (lineArtTexture != null)
			block.SetTexture("_LineArtTexture", lineArtTexture);
		meshRenderer.SetPropertyBlock(block);
	}

	public void CalculateCorrectnessFraction(Texture2D correctTexture, float colorDistanceThreshold, System.Action<float> intermediateCallback, System.Action<float> finalCallback)
	{
		if (correctnessCalculateRowsPerFrame == 0)
			Debug.LogError("Cannot calculate for this canvas");
		else
			StartCoroutine(CalculateCorrectnessFractionRoutine(correctTexture, colorDistanceThreshold, intermediateCallback, finalCallback));
	}

	private IEnumerator CalculateCorrectnessFractionRoutine(Texture2D correctTexture, float colorDistanceThreshold, System.Action<float> intermediateCallback, System.Action<float> finalCallback)
	{
		int w = correctTexture.width;
		int h = correctTexture.height;

		int pixelsToCalculate = w * h;
		int colorIndex = 0;
		int numCorrect = 0;
		int x = 0;
		int y = 0;

		while (pixelsToCalculate > 0)
		{
			Color[] correctColors = correctTexture.GetPixels(x, y, w, correctnessCalculateRowsPerFrame);
			Color[] paintingColors = paintTexture.GetPixels(x, y, w, correctnessCalculateRowsPerFrame);

			for (int i = 0; i < correctColors.Length; i++)
			{
				float d = 0;
				d += Mathf.Abs(correctColors[i].r - paintingColors[i].r);
				d += Mathf.Abs(correctColors[i].g - paintingColors[i].g);
				d += Mathf.Abs(correctColors[i].b - paintingColors[i].b);
				if (d <= colorDistanceThreshold)
					numCorrect++;
			}

			int numPixelsCalculated = correctnessCalculateRowsPerFrame * w;
			colorIndex += numPixelsCalculated;
			pixelsToCalculate -= numPixelsCalculated;
			y += correctnessCalculateRowsPerFrame;

			if (pixelsToCalculate > 0)
			{
				intermediateCallback?.Invoke(numCorrect / (float)(w * h));
				yield return null;
			}
		}

		finalCallback?.Invoke(numCorrect / (float)(w * h));

	}

	private void OnTriggerStay(Collider other)
	{
		SphereCollider sphere = other.gameObject.GetComponent<SphereCollider>();
		PaintBrush2000 paintBrush = other.gameObject.GetComponentInParent<PaintBrush2000>();

		//Debug.Log($"Trigger stay {gameObject.name}, {sphere}, {paintBrush}");

		if (sphere == null || paintBrush == null)
			return;

		paintBrush.OnPaint();

		// Set the brushsize
		int brushSize = paintBrush.brushSize;
		ApplyPaint(other.gameObject.transform.position, brushSize, paintBrush.Color);
	}

	public void ApplyPaint(Vector3 worldHitPosition, int brushSize, Color color)
	{
		// Calculate where the collider hit in this objects local space
		Vector3 hitPoint = transform.InverseTransformPoint(worldHitPosition);
		Vector3 hitPointScaled = new Vector3(hitPoint.x * gameObject.transform.localScale.x, hitPoint.y * gameObject.transform.localScale.y, hitPoint.z * gameObject.transform.localScale.z);
		//Debug.Log("Hit at: " + hitPoint);
		//Debug.Log("Scaled hit at: " + hitPointScaled);

		// Move this objects local space origin to the bottom left corner to match the Texture2D origin
		Vector3 halfSpriteSizeLocalSpace = transform.InverseTransformDirection(meshRenderer.bounds.extents);
		Vector3 hitPointPixelSpace = hitPointScaled + halfSpriteSizeLocalSpace;
		//Debug.Log("Hit in pixel space: " + hitPointPixelSpace);

		Vector3 boundsLocalSize = transform.InverseTransformDirection(meshRenderer.bounds.size);

		// Scale the hit from local size to pixel size
		float scaleX = (boundsLocalSize.x) / paintTexture.width;
		float scaleY = (boundsLocalSize.y) / paintTexture.height;
		hitPointPixelSpace.x /= scaleX;
		hitPointPixelSpace.y /= scaleY;
		//Debug.Log(hitPointPixelSpace);

		// Change the color of the pixels to the paint color
		Color[] paint = Enumerable.Repeat(color, brushSize * brushSize).ToArray();
		int brushX = (int)Mathf.Clamp(hitPointPixelSpace.x - (brushSize / 2), 0, paintTexture.width - brushSize);
		int brushY = (int)Mathf.Clamp(hitPointPixelSpace.y - (brushSize / 2), 0, paintTexture.height - brushSize);
		Color[] colors = paintTexture.GetPixels(brushX, brushY, brushSize, brushSize);
		float brushRadius = brushSize * 0.5f;

		int pixelsAffected = 0;

		// Make paint brush apply color in circular shape
		for (int x = 0; x < brushSize; x++)
		{
			for (int y = 0; y < brushSize; y++)
			{
				int index = x + y * brushSize;
				if (canColorBlack || !IsBlack(colors[index]))
					if (Vector2.Distance(new Vector2(x, y), new Vector2(brushRadius, brushRadius)) < brushRadius)
					{
						colors[index] = paint[index];
						pixelsAffected++;
					}
			}
		}

		//Debug.Log("Pixels affected: " + pixelsAffected);

		paintTexture.SetPixels(brushX, brushY, brushSize, brushSize, colors, 0);
		paintTexture.Apply();
		/*
		if (!audio.isPlaying)
			audio.PlayOneShot(audio.clip);*/
	}

	private bool IsBlack(Color color)
	{
		return color.r < 0.1f && color.b < 0.1f && color.g < 0.1f;
	}
}
