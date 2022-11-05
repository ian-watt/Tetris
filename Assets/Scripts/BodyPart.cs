using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public Vector2 pos;
    public PlayArea occupiedArea;
    private void Update()
    {
        pos = transform.parent.localPosition + transform.localPosition;
    }

}
