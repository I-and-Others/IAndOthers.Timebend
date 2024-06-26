using UnityEngine;

[CreateAssetMenu(fileName = "TileSet", menuName = "Hexagon/Tile Set", order = 1)]
public class TileSet : ScriptableObject
{
    public string tileSetName;
    public GameObject hexPrefab;
    public HexRotationEnum rotationDegree;
    public HexDirectionConnectionTypeEnum east;
    public HexDirectionConnectionTypeEnum southEast;
    public HexDirectionConnectionTypeEnum southWest;
    public HexDirectionConnectionTypeEnum west;
    public HexDirectionConnectionTypeEnum northWest;
    public HexDirectionConnectionTypeEnum northEast;

    public HexDirectionConnectionTypeEnum GetFaceType(HexMainDirectionEnum direction, HexRotationEnum rotation)
    {
        HexDirectionConnectionTypeEnum[] faceTypes = new HexDirectionConnectionTypeEnum[6];
        faceTypes[0] = east;
        faceTypes[1] = southEast;
        faceTypes[2] = southWest;
        faceTypes[3] = west;
        faceTypes[4] = northWest;
        faceTypes[5] = northEast;

        int rotationSteps = ((int)rotation / 60) % 6;
        HexDirectionConnectionTypeEnum[] rotatedFaceTypes = new HexDirectionConnectionTypeEnum[6];
        for (int i = 0; i < 6; i++)
        {
            rotatedFaceTypes[i] = faceTypes[(i + 6 - rotationSteps) % 6];
        }

        return rotatedFaceTypes[(int)direction];
    }
}