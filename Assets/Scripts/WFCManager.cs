using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    public HexMapGenerator hexMapGenerator;
    public TileSet[] tileSets;

    private bool initialized = false;

    private void Start()
    {
        if (hexMapGenerator.hexGrid == null || hexMapGenerator.hexGrid.Length == 0)
        {
            Debug.LogError("Hex map is not generated. Please generate the hex map first.");
            return;
        }

        InitializePossibleTileSets();
        initialized = true;
    }

    public void InitializePossibleTileSets()
    {
        if (hexMapGenerator.hexGrid == null)
        {
            Debug.LogError("Hex grid is null. Ensure the hex map is generated first.");
            return;
        }

        for (int x = 0; x < hexMapGenerator.hexGrid.GetLength(0); x++)
        {
            for (int y = 0; y < hexMapGenerator.hexGrid.GetLength(1); y++)
            {
                Hex hex = hexMapGenerator.hexGrid[x, y];
                if (hex != null)
                {
                    List<PossibleTileSet> possibleTileSets = new List<PossibleTileSet>();
                    foreach (var tileSet in tileSets)
                    {
                        List<HexRotationEnum> rotations = new List<HexRotationEnum>
                        {
                            HexRotationEnum.Deg0,
                            HexRotationEnum.Deg60,
                            HexRotationEnum.Deg120,
                            HexRotationEnum.Deg180,
                            HexRotationEnum.Deg240,
                            HexRotationEnum.Deg300
                        };
                        possibleTileSets.Add(new PossibleTileSet(tileSet, rotations));
                    }
                    hex.possibleTileSets = possibleTileSets;
                }
            }
        }
    }

    public void CompleteWaveFunctionCollapse()
    {
        while (true)
        {
            CollapseNext();
            if (IsWaveFunctionCollapsed())
            {
                break;
            }
        }
    }

    private bool IsWaveFunctionCollapsed()
    {
        foreach (var hex in hexMapGenerator.hexGrid)
        {
            if (hex != null && hex.possibleTileSets.Count > 1)
            {
                return false; // Found a hex that hasn't collapsed to a single state
            }
        }
        return true; // All hexes have collapsed to a single state
    }

    public void StartWaveFunctionCollapse()
    {
        InitializePossibleTileSets();
        initialized = true;
    }

    public void CollapseNext()
    {
        if (!initialized)
        {
            Debug.LogError("Wave Function Collapse has not been initialized. Call StartWaveFunctionCollapse first.");
            return;
        }

        Hex hexToCollapse = Observe();
        if (hexToCollapse == null)
        {
            Debug.Log("No more hexagons to collapse.");
            return;
        }
        Collapse(hexToCollapse);
        Propagate(hexToCollapse);
    }

    private Hex Observe()
    {
        Hex minEntropyHex = null;
        float minEntropy = float.MaxValue;

        foreach (var hex in hexMapGenerator.hexGrid)
        {
            if (hex != null && hex.possibleTileSets.Count > 1)
            {
                float entropy = CalculateEntropy(hex);
                if (entropy < minEntropy)
                {
                    minEntropy = entropy;
                    minEntropyHex = hex;
                }
            }
        }

        return minEntropyHex;
    }

    private float CalculateEntropy(Hex hex)
    {
        return hex.possibleTileSets.Count;
    }

    private void Collapse(Hex hex)
    {
        List<PossibleTileSet> possibleTileSets = hex.possibleTileSets;
        PossibleTileSet selectedTileSet = possibleTileSets[Random.Range(0, possibleTileSets.Count)];
        TileSet selectedTile = selectedTileSet.tileSet;
        HexRotationEnum selectedRotation = selectedTileSet.possibleRotations[Random.Range(0, selectedTileSet.possibleRotations.Count)];

        hex.currentTileSet = selectedTile;
        hex.currentRotation = selectedRotation;

        // Set the mesh filter of the hex to the selected tile set's mesh
        MeshFilter meshFilter = hex.GetComponent<MeshFilter>();
        if (meshFilter != null && selectedTile.hexPrefab != null)
        {
            MeshFilter prefabMeshFilter = selectedTile.hexPrefab.GetComponent<MeshFilter>();
            if (prefabMeshFilter != null)
            {
                meshFilter.mesh = prefabMeshFilter.sharedMesh;
            }
        }
        // Set the rotation of the hex to the selected rotation
        hex.transform.rotation = Quaternion.Euler(0, (int)selectedRotation, 0);

        hex.possibleTileSets.Clear();
    }

    private void Propagate(Hex collapsedHex)
    {
        Queue<Hex> propagationQueue = new Queue<Hex>();
        propagationQueue.Enqueue(collapsedHex);

        while (propagationQueue.Count > 0)
        {
            Hex currentHex = propagationQueue.Dequeue();

            foreach (HexMainDirectionEnum direction in System.Enum.GetValues(typeof(HexMainDirectionEnum)))
            {
                Hex neighbor = currentHex.neighbors[(int)direction];
                if (neighbor == null || neighbor.possibleTileSets == null || neighbor.possibleTileSets.Count == 0)
                {
                    continue;
                }

                int oppositeDirectionInt = ((int)direction + 3) % 6;
                HexMainDirectionEnum oppositeDirection = (HexMainDirectionEnum)oppositeDirectionInt;

                if (currentHex.currentTileSet == null)
                {
                    continue;
                }

                HexDirectionConnectionTypeEnum requiredFaceType = currentHex.currentTileSet.GetFaceType(direction, currentHex.currentRotation);

                List<PossibleTileSet> validTileSets = new List<PossibleTileSet>();
                foreach (var possibleTileSet in neighbor.possibleTileSets)
                {
                    List<HexRotationEnum> validRotations = new List<HexRotationEnum>();
                    foreach (var rotation in possibleTileSet.possibleRotations)
                    {
                        var faceType = possibleTileSet.tileSet.GetFaceType(oppositeDirection, rotation);

                        if (requiredFaceType == faceType)
                        {
                            validRotations.Add(rotation);
                        }
                    }

                    if (validRotations.Count > 0)
                    {
                        validTileSets.Add(new PossibleTileSet(possibleTileSet.tileSet, validRotations));
                    }
                }

                if (!AreTileSetsEqual(neighbor.possibleTileSets, validTileSets))
                {
                    neighbor.possibleTileSets = validTileSets;
                    propagationQueue.Enqueue(neighbor);
                }

                if (validTileSets.Count == 1)
                {
                    Collapse(neighbor);
                    propagationQueue.Enqueue(neighbor);
                }
            }
        }
    }

    private bool AreTileSetsEqual(List<PossibleTileSet> current, List<PossibleTileSet> updated)
    {
        if (current.Count != updated.Count)
        {
            return false;
        }

        foreach (var currentTileSet in current)
        {
            var updatedTileSet = updated.Find(t => t.tileSet == currentTileSet.tileSet);
            if (updatedTileSet == null || !currentTileSet.possibleRotations.SequenceEqual(updatedTileSet.possibleRotations))
            {
                return false;
            }
        }

        return true;
    }

    public void CollapseOuterHexagonsAsWater()
    {
        if (!initialized)
        {
            Debug.LogError("Wave Function Collapse has not been initialized. Call StartWaveFunctionCollapse first.");
            return;
        }

        TileSet waterTileSet = null;

        // Find the water tile set
        foreach (var tileSet in tileSets)
        {
            if (tileSet.tileSetName == "Coast_Water") // Adjust the name as necessary
            {
                waterTileSet = tileSet;
                break;
            }
        }

        if (waterTileSet == null)
        {
            Debug.LogError("Water tile set not found.");
            return;
        }

        int width = hexMapGenerator.hexGrid.GetLength(0);
        int height = hexMapGenerator.hexGrid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (IsEdgeHexagon(x, y, width, height))
                {
                    Hex hex = hexMapGenerator.hexGrid[x, y];
                    if (hex.currentTileSet == null)
                    { 
                        hex.SetTileSet(waterTileSet);
                    }
                }
            }
        }
    }

    private bool IsEdgeHexagon(int x, int y, int width, int height)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }
}
