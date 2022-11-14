using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    //basic properties for each block that makes up a tetromino piece

    public Vector2 pos;
    public PlayArea occupiedArea;
    private void Update()
    {
        pos = transform.parent.localPosition + transform.localPosition;
    }

}
