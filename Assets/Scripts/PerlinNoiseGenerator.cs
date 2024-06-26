using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public float noiseScale = 0.1f;
    public float heightMultiplier = 5f;

    public float[,] GenerateNoiseMap(int width, int height)
    {
        float[,] noiseMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sampleX = x * noiseScale;
                float sampleY = y * noiseScale;
                float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * heightMultiplier;
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }
}
