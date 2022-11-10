using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float timePassed;
    public Vector2 position;
    public bool isFalling = false;
    public List<BodyPart> bodyParts;
    private float oldSpeed;
    private float newSpeed = .1f;
    public LayerMask mask;
    public enum Type { T, Straight, Square, Skew, L, L1, Skew1}
    public Type myType;
    public enum Rotation { Zero, One, Two, Three }
    public Rotation currentRotation;
    public SimulatedPiece simulatedPiece;

    private void Update()
    {

        if(bodyParts.Count == 0)
        {
            Destroy(gameObject);
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
        if (Input.GetMouseButtonDown(0))
        {

                GameManager.instance.speedUp = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.instance.speedUp = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRotation();
        }

    }

    private void HandleRotation()
    {
        // L Rotation
        if (myType == Type.L && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(-.5f, 1, 0), new Vector3(.5f, 1, 0), new Vector3(1.5f, 1, 0), new Vector3(-.5f, 0, 0), Rotation.One);
        }
        else if (myType == Type.L && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(1.5f, 0, 0), new Vector3(.5f, 1, 0), new Vector3(1.5f,1, 0), new Vector3(1.5f, -1, 0), Rotation.Two);
        }
        else if (myType == Type.L && CompareTag("Current") && currentRotation == Rotation.Two)
        {
            Rotate(new Vector3(1.5f, 0, 0), new Vector3(.5f,-1, 0), new Vector3(-.5f, -1, 0), new Vector3(1.5f, -1, 0), Rotation.Three);
        }
        else if (myType == Type.L && CompareTag("Current") && currentRotation == Rotation.Three)
        {
            Rotate(new Vector3(-.5f, 0,0), new Vector3(-.5f, 1, 0), new Vector3(-.5f, -1, 0), new Vector3(.5f, -1, 0), Rotation.Zero);
        }

        //Straight Rotation
        if(myType == Type.Straight && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(-2, .5f, 0), new Vector3(-1, .5f, 0), new Vector3(1, .5f, 0), new Vector3(0, .5f, 0), Rotation.One);
        }
        else if(myType == Type.Straight && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(0,.5f,0), new Vector3(0, -.5f, 0), new Vector3(0, -1.5f, 0), new Vector3(0, 1.5f, 0), Rotation.Zero);
        }

        // T Rotation
        if (myType == Type.T && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(0, 1, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, -1, 0), Rotation.One);
        }
        else if (myType == Type.T && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), Rotation.Two);
            //new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, -1, 0), Rotation.Two
        }
        else if (myType == Type.T && CompareTag("Current") && currentRotation == Rotation.Two)
        {
            Rotate(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, -1, 0), Rotation.Three);
        }
        else if (myType == Type.T && CompareTag("Current") && currentRotation == Rotation.Three)
        {
            Rotate(new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, -1, 0), Rotation.Zero);
        }

        //Skew Rotation
        if(myType == Type.Skew && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(-1, .5f, 0), new Vector3(0, -1.5f, 0), new Vector3(-1, -.5f, 0), new Vector3(0, -.5f, 0), Rotation.One);
        }
        else if (myType == Type.Skew && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(1, .5f, 0), new Vector3(0, .5f, 0), new Vector3(-1, -.5f, 0), new Vector3(0, -.5f, 0), Rotation.Zero);
        }

        //L1 Rotation
        if (myType == Type.L1 && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(-.5f, 0, 0), new Vector3(-.5f, 1, 0), new Vector3(-.5f, -1, 0), new Vector3(.5f, 1, 0), Rotation.One);
        }
        else if (myType == Type.L1 && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(-.5f, 0, 0), new Vector3(.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(1.5f, -1, 0), Rotation.Two);
        }
        else if (myType == Type.L1 && CompareTag("Current") && currentRotation == Rotation.Two)
        {
            Rotate(new Vector3(1.5f, -2, 0), new Vector3(.5f, -2, 0), new Vector3(1.5f, 0, 0), new Vector3(1.5f, -1, 0), Rotation.Three);
        }
        else if (myType == Type.L1 && CompareTag("Current") && currentRotation == Rotation.Three)
        {
            Rotate(new Vector3(-.5f, 0, 0), new Vector3(1.5f, -1, 0), new Vector3(-.5f, -1, 0), new Vector3(.5f, -1, 0), Rotation.Zero);
        }

        //Skew1 Rotation
        if (myType == Type.Skew1 && CompareTag("Current") && currentRotation == Rotation.Zero)
        {
            Rotate(new Vector3(1, .5f, 0), new Vector3(0, .5f, 0), new Vector3(1, 1.5f, 0), new Vector3(0, -.5f, 0), Rotation.One);
        }
        else if (myType == Type.Skew1 && CompareTag("Current") && currentRotation == Rotation.One)
        {
            Rotate(new Vector3(-1, .5f, 0), new Vector3(0, .5f, 0), new Vector3(1, -.5f, 0), new Vector3(0, -.5f, 0), Rotation.Zero);
        }
    }

    private void Rotate(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Rotation newRotation)
    {
        bool possible = true;
        Clone();
        UpdateCloneParts(p1, p2, p3, p4);

        for (int x = 0; x < simulatedPiece.bodyParts.Count; x++)
        {
            if (simulatedPiece.bodyParts[x].pos.x > 10.5f || simulatedPiece.bodyParts[x].pos.x < 1.5f || simulatedPiece.bodyParts[x].pos.y < 1.5f)
            {
                possible = false;
                break;
            }
            foreach(BodyPart part in GameManager.instance.placedParts)
            {
                if(simulatedPiece.bodyParts[x].pos.x == part.transform.localPosition.x && simulatedPiece.bodyParts[x].pos.y == part.transform.position.y)
                {
                    possible = false;
                    break;
                }
            }
        }
        if (possible == true)
        {
            UpdateParts(p1, p2, p3, p4);
            currentRotation = newRotation;
            
        }
    }

    private void UpdateParts(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {

            if (i == 0)
            {
                bodyParts[i].transform.localPosition = p1;
            }
            else if (i == 1)
            {
                bodyParts[i].transform.localPosition = p2;
            }
            else if (i == 2)
            {
                bodyParts[i].transform.localPosition = p3;

            }
            else
            {
                bodyParts[i].transform.localPosition = p4;
            }
        }
    }
    private void UpdateCloneParts(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {

        for (int i = 0; i < simulatedPiece.bodyParts.Count; i++)
        {
            if (i == 0)
            {
                simulatedPiece.bodyParts[i].pos = (Vector2)p1 + (Vector2)simulatedPiece.position;
            }
            else if (i == 1)
            {
                simulatedPiece.bodyParts[i].pos = (Vector2)p2 + (Vector2)simulatedPiece.position;
            }
            else if (i == 2)
            {
                simulatedPiece.bodyParts[i].pos = (Vector2)p3 + (Vector2)simulatedPiece.position;

            }
            else
            {
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



            foreach (BodyPart body in GameManager.instance.placedParts)
            {
                if (x != false)
                {
                        if (part.pos.x - 1 == body.pos.x && part.pos.y == body.pos.y)
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
            foreach (BodyPart body in GameManager.instance.placedParts)
            {
                if (x != false)
                {
                    if (part.pos.x + 1 == body.pos.x && part.pos.y == body.pos.y)
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
        return x;
    }

    SimulatedPiece Clone()
    {
        simulatedPiece = new SimulatedPiece();
        simulatedPiece.myType = (SimulatedPiece.Type)myType;
        simulatedPiece.bodyParts = bodyParts;
        simulatedPiece.position = transform.localPosition;
        simulatedPiece.isFalling = isFalling;
        simulatedPiece.currentRotation = (SimulatedPiece.Rotation)currentRotation;

        return simulatedPiece;

    }


}
