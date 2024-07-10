using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TileSet", menuName = "Hexagon/Tile Set", order = 1)]
public class TileSet : ScriptableObject
{
    public string tileSetName;
    public GameObject hexPrefab;
    public HexRotationEnum rotationDegree;
    public List<HexDirectionConnectionTypeEnum> east;
    public List<HexDirectionConnectionTypeEnum> southEast;
    public List<HexDirectionConnectionTypeEnum> southWest;
    public List<HexDirectionConnectionTypeEnum> west;
    public List<HexDirectionConnectionTypeEnum> northWest;
    public List<HexDirectionConnectionTypeEnum> northEast;

    public List<HexDirectionConnectionTypeEnum> GetFaceTypes(HexMainDirectionEnum direction, HexRotationEnum rotation)
    {
        List<HexDirectionConnectionTypeEnum>[] faceTypes = new List<HexDirectionConnectionTypeEnum>[6]
        {
            east, southEast, southWest, west, northWest, northEast
        };

        int rotationSteps = ((int)rotation / 60) % 6;
        List<HexDirectionConnectionTypeEnum>[] rotatedFaceTypes = new List<HexDirectionConnectionTypeEnum>[6];
        
        for (int i = 0; i < 6; i++)
        {
            rotatedFaceTypes[i] = faceTypes[(i + 6 - rotationSteps) % 6];
        }

        return rotatedFaceTypes[(int)direction];
    }

    public bool CanConnect(TileSet other, HexMainDirectionEnum direction, HexRotationEnum rotation, HexMainDirectionEnum otherDirection, HexRotationEnum otherRotation)
    {
        List<HexDirectionConnectionTypeEnum> thisFaceTypes = GetFaceTypes(direction, rotation);
        List<HexDirectionConnectionTypeEnum> otherFaceTypes = other.GetFaceTypes(otherDirection, otherRotation);

        foreach (var thisType in thisFaceTypes)
        {
            foreach (var otherType in otherFaceTypes)
            {
                if (AreCompatibleTypes(thisType, otherType))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool AreCompatibleTypes(HexDirectionConnectionTypeEnum type1, HexDirectionConnectionTypeEnum type2)
    {
        if (type1 == type2) return true;
        if ((type1 == HexDirectionConnectionTypeEnum.CoastalWater && type2 == HexDirectionConnectionTypeEnum.Water) ||
            (type1 == HexDirectionConnectionTypeEnum.Water && type2 == HexDirectionConnectionTypeEnum.CoastalWater))
            return true;
        if ((type1 == HexDirectionConnectionTypeEnum.CoastalLand && type2 == HexDirectionConnectionTypeEnum.Land) ||
            (type1 == HexDirectionConnectionTypeEnum.Land && type2 == HexDirectionConnectionTypeEnum.CoastalLand))
            return true;
        // Add more rules as needed
        return false;
    }
}