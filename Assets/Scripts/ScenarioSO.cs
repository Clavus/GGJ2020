using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "GGJ2020/Scenario Data", order = 100)]
public class ScenarioSO : ScriptableObject
{
	public Texture2D lineArt;
	public Texture2D colorLayer;
	public Texture2D[] incorrectColorLayers;
	[Range(1, 3)]
	public int difficulty = 1;
}
