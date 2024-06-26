using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : Singleton<HexMapGenerator>
{
    public HexMapSettings Settings;
    public Hex[,] hexGrid;

    public void GenerateHexMap()
    {
        CalculateHexDimensions();
        ClearMap();

        hexGrid = new Hex[Settings.mapWidth, Settings.mapHeight];

        // Create hex tiles and store them in the array
        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                Vector3 position = CalculateHexPosition(x, y);
                Quaternion rotation = Quaternion.identity;
                GameObject hexGO = Instantiate(Settings.hexPrefab, position, rotation, transform);
                hexGO.name = $"Hex_{x}_{y}";
                Hex hex = hexGO.GetComponent<Hex>();
                hex.Initialize(new Vector2Int(x, y));
                hexGrid[x, y] = hex;
            }
        }

        // Update neighbors
        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                SetNeighbors(hexGrid, x, y);
            }
        }
    }
    
    private void ClearMap()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void CalculateHexDimensions()
    {
        Settings.hexWidth = Mathf.Sqrt(3) * Settings.hexSize;
        Settings.hexHeight = 2 * Settings.hexSize;
    }

    private Vector3 CalculateHexPosition(int x, int y)
    {
        float xPos = 0f;
        float zPos = 0f;

        xPos = x * Settings.hexWidth;
        zPos = y * (Settings.hexHeight * 0.75f);

        if (y % 2 == 1)
        {
            xPos += Settings.hexWidth / 2f;
        }

        return new Vector3(xPos, 0, zPos);
    }

    private void SetNeighbors(Hex[,] hexGrid, int x, int y)
    {
        Hex hex = hexGrid[x, y];

        Vector2Int[] directions = {
            new Vector2Int(+1, 0), new Vector2Int(+1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, +1), new Vector2Int(0, +1)
        };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborAxial = hex.axialCoordinates + directions[i];
            Vector2Int neighborOffset = AxialToOffset(neighborAxial);

            if (IsWithinBounds(neighborOffset))
            {
                hex.neighbors[i] = hexGrid[neighborOffset.x, neighborOffset.y];
            }
        }
    }

    private bool IsWithinBounds(Vector2Int coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < Settings.mapWidth &&
               coordinates.y >= 0 && coordinates.y < Settings.mapHeight;
    }

    private Vector2Int AxialToOffset(Vector2Int axial)
    {
        int col = axial.x + (axial.y - (axial.y & 1)) / 2;
        int row = axial.y;
        return new Vector2Int(col, row);
    }
}
