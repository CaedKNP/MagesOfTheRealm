using UnityEngine;
using UnityEngine.Tilemaps;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour {

	[Tooltip("The Tilemap to draw onto")]
	public Tilemap tilemap;

	[Tooltip("The Tile to draw (use a RuleTile for best results)")]
	public TileBase tile;

	[Tooltip("The Tile to be used as a spawnerPoint")]
	public TileBase tileSpawner;

	[Tooltip("Width of our map")]
	public int width;

	[Tooltip("Height of our map")]
	public int height;
	
	[Tooltip("The settings of our map")]
	public MapSettings mapSetting;

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			ClearMap();
			GenerateMap();
		}
	}

	[ExecuteInEditMode]
	public int[,] GenerateMap()
	{
		ClearMap();

        int[,] map = new int[width, height];
		float seed;

		if (mapSetting.randomSeed)
		{
			seed = Time.time;
		}
		else
		{
			seed = mapSetting.seed;
		}

		//Generate the map depending omapSen the algorithm selected
				//First generate our array
				map = MapFunctions.GenerateArray(width, height, false);
				//Next generate the random walk cave
				map = MapFunctions.RandomWalkCave(map, seed, mapSetting.clearAmount);		
		//Render the result
		MapFunctions.RenderMap(map, tilemap, tile,tileSpawner);

		GameManager.map = map;

		return map;
    }

	public void ClearMap()
	{
		tilemap.ClearAllTiles();
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		//Reference to our script
		LevelGenerator levelGen = (LevelGenerator)target;
		
		//Only show the mapsettings UI if we have a reference set up in the editor
		if (levelGen.mapSetting != null)
		{
			Editor mapSettingEditor = CreateEditor(levelGen.mapSetting);
			mapSettingEditor.OnInspectorGUI();

			if (GUILayout.Button("Generate"))
			{
				//levelGen.GenerateMap();
			}

			if (GUILayout.Button("Clear"))
			{
				levelGen.ClearMap();
			}
		}
	}
}
#endif