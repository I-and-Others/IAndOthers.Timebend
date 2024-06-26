using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PossibleTileSet
{
    public TileSet tileSet;
    public List<HexRotationEnum> possibleRotations;

    public PossibleTileSet(TileSet tileSet, List<HexRotationEnum> rotations)
    {
        this.tileSet = tileSet;
        this.possibleRotations = rotations;
    }
}