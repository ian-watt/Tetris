using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float timePassed;
    public Vector2 position;
    public bool isFalling = false;
    public BodyPart[] bodyParts;
    public enum Type { T, Straight, Square, Skew, L }
    public Type myType;
    public enum Rotation { Zero, One, Two, Three }
    public Rotation currentRotation;
    public SimulatedPiece simulatedPiece;

    private void Awake()
    {
    }
    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > GameManager.instance.gameSpeed && isFalling == true)
        {
            transform.position += Vector3.down * 1;
            timePassed = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (IsWithinBoundsNeg() == true && CompareTag("Current"))
            {
                transform.localPosition += Vector3.left;

            }
        }
        if (Input.GetKeyDown(KeyCode.D) && CompareTag("Current"))
        {
            if (IsWithinBoundsPos() == true)
            {
                transform.localPosition += Vector3.right;
            }
        }
        HandleRotation();

    }

    private void HandleRotation()
    {
        bool possible = false;
        if (Input.GetKeyDown(KeyCode.R) && myType == Type.L && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Clone();
            UpdateCloneParts(new Vector3(1.5f, -1f, 0), new Vector3(1.5f, 0, 0), new Vector3(-.5f,-1, 0), new Vector3(.5f, -1, 0));
            foreach (BodyPart part in simulatedPiece.bodyParts)
            {
                if (part.pos.x < 10.5f && part.pos.x > 1.5f && part.pos.y > 1.5f)
                {
                    possible = true;
                }
            }
            if (possible)
            {
                for(int i = 0; i < bodyParts.Length; i++)
                {
                    bodyParts[i].transform.localPosition = simulatedPiece.bodyParts[i].transform.localPosition;
                }
            }

        }
    }

    private void UpdateCloneParts(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        //calculate distance from center and use that as a basis for the rotation check
        for (int i = 0; i < simulatedPiece.bodyParts.Length; i++)
        {
            if(i == 0)
            {
                simulatedPiece.bodyParts[i].transform.localPosition = p1;
                simulatedPiece.bodyParts[i].pos = (Vector2)p1 + (Vector2)simulatedPiece.position;
            }else if (i == 1)
            {
                simulatedPiece.bodyParts[i].transform.localPosition = p2;
                simulatedPiece.bodyParts[i].pos = (Vector2)p2 + (Vector2)simulatedPiece.position;
            }
            else if(i == 2)
            {
                simulatedPiece.bodyParts[i].transform.localPosition = p3;
                simulatedPiece.bodyParts[i].pos = (Vector2)p3 + (Vector2)simulatedPiece.position;

            }
            else
            {
                simulatedPiece.bodyParts[i].transform.localPosition = p4;
                simulatedPiece.bodyParts[i].pos = (Vector2)p4 + (Vector2)simulatedPiece.position;
            }
        }
    }

    public bool IsWithinBoundsNeg()
    {
        bool x = true;
        foreach (BodyPart part in bodyParts)
        {
            if (x != false)
            {
                if (part.pos.x - 1 < 1.5f)
                {
                    x = false;
                    break;
                }
                else
                {
                    x = true;
                }
            }



            foreach (Tetromino mino in GameManager.instance.tetrominoesInPlay)
            {
                if (x != false)
                {
                    foreach (BodyPart otherPart in mino.bodyParts)
                    {
                        if (part.pos.x - 1 == otherPart.pos.x && part.pos.y == otherPart.pos.y)
                        {
                            x = false;
                            break;
                        }
                        else
                        {
                            x = true;
                        }
                    }
                }


            }

        }
        return x;
    }

    public bool IsWithinBoundsPos()
    {
        bool x = true;
        foreach (BodyPart part in bodyParts)
        {
            if (x != false)
            {
                if (part.pos.x + 1 > 10.5f)
                {
                    x = false;
                    break;

                }
                else
                {
                    x = true;
                }
            }
            foreach (Tetromino mino in GameManager.instance.tetrominoesInPlay)
            {
                if (x != false)
                {
                    foreach (BodyPart otherPart in mino.bodyParts)
                    {
                        if (part.pos.x + 1 == otherPart.pos.x && part.pos.y == otherPart.pos.y)
                        {
                            x = false;
                            break;
                        }
                        else
                        {
                            x = true;
                        }
                    }
                }

            }
        }
        return x;
    }

    [ContextMenu("Clone")]
    SimulatedPiece Clone()
    {
        simulatedPiece = new SimulatedPiece();
        simulatedPiece.myType = (SimulatedPiece.Type)myType;
        simulatedPiece.bodyParts = bodyParts;
        simulatedPiece.position = transform.localPosition;
        simulatedPiece.isFalling = isFalling;
        simulatedPiece.currentRotation = (SimulatedPiece.Rotation)currentRotation;
        Debug.Log("My type: " + simulatedPiece.myType + " I have " + simulatedPiece.bodyParts.Length + " parts, my position in the play area is " + simulatedPiece.position);
        foreach(BodyPart part in simulatedPiece.bodyParts)
        {
            Debug.Log(part.pos);
        }

        return simulatedPiece;

    }


}
