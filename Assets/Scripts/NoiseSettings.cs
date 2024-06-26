using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 20f;

    public int octaves = 4;
    [Range(0, 1)]
    public float persistence = 0.5f;
    public float lacunarity = 2f;

    public int seed;
    public Vector2 offset;
}