using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedPiece
{
    //tetromino clone used to manage rotational checks

    public Vector3 position;
    public bool isFalling = false;
    public List<BodyPart> bodyParts;
    public enum Type { T, Straight, Square, Skew, L, L1, Skew1 }
    public Type myType;
    public enum Rotation { Zero, One, Two, Three }
    public Rotation currentRotation;
}
