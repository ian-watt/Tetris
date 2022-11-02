using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedPiece
{
    public Vector3 position;
    public bool isFalling = false;
    public BodyPart[] bodyParts;
    public enum Type { T, Straight, Square, Skew, L }
    public Type myType;
    public enum Rotation { Zero, One, Two, Three }
    public Rotation currentRotation;
}
