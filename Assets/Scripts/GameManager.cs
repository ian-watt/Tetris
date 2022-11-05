using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject tetrionPrefab;
    public GameObject tetrion;
    public GameObject[] tetrominoPrefabs;
    public static GameManager instance;
    public Vector2 tetrionBounds;
    public Vector2 playAreaBounds;
    public Vector2 minoBounds;
    private Vector3 startPos = new Vector3(5.5f, 26.5f, -1);
    public float gameSpeed;
    public float spedSpeed;
    private float timePassed;
    public bool speedUp = false;
    public Tetromino currentTetromino;
    public List<Tetromino> tetrominoesInPlay;
    public List<PlayArea> playArea;
    public List<BodyPart> placedParts;
    public GameObject holdWindow;
    private int spawnRange;
    private int nextPiece;
    private bool pieceHeld;

    List<PlayArea> occupiedRow = new List<PlayArea>();

    void Awake()
    {
        instance = this;
        CreateTetrion();
        CreatePlayArea();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldPiece();
        }

        if (currentTetromino.isFalling == false)
        {
            
            SpawnTetromino(ReturnRandomIndex());
        }


        timePassed += Time.deltaTime;
        if (speedUp)
        {
            if (timePassed > spedSpeed && currentTetromino.isFalling == true)
            {
                    HandleTetrominoes();

                    if (currentTetromino.isFalling == true)
                    {
                        currentTetromino.transform.localPosition += Vector3.down * 1;
                    }

                    timePassed = 0;
                
            }
        }
        else
        {
            if (timePassed > gameSpeed && currentTetromino.isFalling == true)
            {
                HandleTetrominoes();
                
                if (currentTetromino.isFalling == true)
                {
                    currentTetromino.transform.localPosition += Vector3.down * 1;
                }
                timePassed = 0;
            }
        }
        UpdateOccupiedPlayArea();
        Tetris();

    }

    private void HoldPiece()
    {
        if(pieceHeld != true)
        {
            currentTetromino.isFalling = false;
            currentTetromino.transform.position = new Vector3(-5.7f, 22.5f, 2);
            currentTetromino.tag = ("Placed");
            pieceHeld = true;
        }
        else
        {

        }
        
    }

    private void Start()
    {
        SpawnTetromino(ReturnRandomIndex());
    }
    //instantiate tetrion (game area borders)
    private void CreateTetrion()
    {
        for (int x = 0; x <= tetrionBounds.x; x++)
        {
            GameObject goXBottom = Instantiate(tetrionPrefab);
            GameObject goXTop = Instantiate(tetrionPrefab);
            goXBottom.transform.parent = tetrion.transform;
            goXTop.transform.parent = tetrion.transform;

            goXBottom.transform.localPosition = new Vector3(x + .5f, .5f, -2);
            goXTop.transform.localPosition = new Vector3(x + .5f, 25.5f, -2);
            for (int y = 0; y <= tetrionBounds.y; y++)
            {
                GameObject goYLeft = Instantiate(tetrionPrefab);
                GameObject goYRight = Instantiate(tetrionPrefab);
                goYLeft.transform.parent = tetrion.transform;
                goYRight.transform.parent = tetrion.transform;
                goYLeft.transform.localPosition = new Vector3(0.5f, y + .5f, -2);
                goYRight.transform.localPosition = new Vector3(11.5f, y + .5f, -2);
            }
        }

        for (int x = 0; x <= 11; x++)
        {
            GameObject goXBottom = Instantiate(tetrionPrefab);
            GameObject goXTop = Instantiate(tetrionPrefab);
            goXBottom.transform.parent = holdWindow.transform;
            goXTop.transform.parent = holdWindow.transform;

            goXBottom.transform.localPosition = new Vector3(x + .5f, .5f, -2);
            goXTop.transform.localPosition = new Vector3(x + .5f, 9.5f, -2);
            for (int y = 0; y <= 8; y++)
            {
                GameObject goYLeft = Instantiate(tetrionPrefab);
                GameObject goYRight = Instantiate(tetrionPrefab);
                goYLeft.transform.parent = holdWindow.transform;
                goYRight.transform.parent = holdWindow.transform;
                goYLeft.transform.localPosition = new Vector3(0.5f, y + .5f, -2);
                goYRight.transform.localPosition = new Vector3(11.5f, y + .5f, -2);
            }
        }
        holdWindow.transform.localScale = new Vector3(.5f, .5f, .5f);
        holdWindow.transform.position = new Vector3(-8.7f, 20, 0);
    }

    //instantiate the play area ; when creating line removal logic potentially add them to a list
    private void CreatePlayArea()
    {


        for (int y1 = 1; y1 <= playAreaBounds.y; y1++)
        {
            for (int x1 = 1; x1 <= playAreaBounds.x; x1++)
            {
                GameObject playAreaSquare = new GameObject();
                playAreaSquare.AddComponent<SpriteRenderer>();
                playAreaSquare.AddComponent<PlayArea>();
                var s = playAreaSquare.GetComponent<SpriteRenderer>();
                s.sprite = tetrionPrefab.GetComponent<SpriteRenderer>().sprite;
                s.color = Color.black;
                playAreaSquare.transform.parent = tetrion.transform;
                playAreaSquare.transform.localPosition = new Vector3(x1 + .5f, y1 + .5f, 0);
                playAreaSquare.GetComponent<PlayArea>().pos = new Vector2(playAreaSquare.transform.localPosition.x, playAreaSquare.transform.localPosition.y);
                playArea.Add(playAreaSquare.GetComponent<PlayArea>());
            }
        }
    }

    private void HandleTetrominoes()
    {

        IsCurrentTouchingBottom(currentTetromino);
        StackTetrominoes(currentTetromino);

    }

    //instantiate a random tetromino prefab and assign it to the current falling tetromino, add to the list of tetrominoes currently in the play area
    private void SpawnTetromino(int next)
    {
        GameObject current = Instantiate(tetrominoPrefabs[next]);
        currentTetromino = current.GetComponent<Tetromino>();
        current.transform.parent = tetrion.transform;
        current.transform.localPosition = startPos + current.transform.position;
        current.tag = "Current";
        currentTetromino.position = transform.position;
        currentTetromino.currentRotation = Tetromino.Rotation.Zero;
        currentTetromino.isFalling = true;

    }



    public int ReturnRandomIndex()
    {
        return Random.Range(0, tetrominoPrefabs.Length);
    }

    //check if the current tetromino is touching the bottom ; if true, stop it from moving
    public void IsCurrentTouchingBottom(Tetromino current)
    {
        for (int x = 0; x < current.bodyParts.Count; x++)
        {
            if (current.bodyParts[x].pos.y == 1.5f && current.bodyParts[x].transform.parent.CompareTag("Current"))
            {

                
                for (int i = current.bodyParts.Count - 1; i >= 0; i--)
                {
                    current.bodyParts[i].transform.parent = tetrion.transform;
                    current.bodyParts[i].transform.localPosition = new Vector3(current.bodyParts[i].pos.x, current.bodyParts[i].pos.y, -2f);
                    placedParts.Add(current.bodyParts[i]);
                    foreach (PlayArea area in playArea)
                    {
                        if (area.pos == current.bodyParts[i].pos)
                        {
                            current.bodyParts[i].occupiedArea = area;
                        }
                    }
                    current.bodyParts.Remove(current.bodyParts[i]);
                    

                }
                current.tag = ("Placed");
                current.isFalling = false;
            }
        }
    }

    public void StackTetrominoes(Tetromino current)
    {
        List<BodyPart> partsToRemove = new List<BodyPart>();
        for (int b = 0; b < current.bodyParts.Count; b++)
        {
            for (int y = 0; y < placedParts.Count; y++)
            {

                if (current.bodyParts[b].pos.y == placedParts[y].transform.localPosition.y + 1 && current.bodyParts[b].pos.x == placedParts[y].transform.localPosition.x && current.bodyParts[b].transform.parent.CompareTag("Current"))
                {
                    current.isFalling = false;
                    current.tag = "Placed";
                    for (int i = current.bodyParts.Count - 1; i >= 0; i--)
                    {
                        partsToRemove.Add(current.bodyParts[i]);

                    }
                    
                }
            }
        }
        NewMethod(current, partsToRemove);
    }

    private void NewMethod(Tetromino current, List<BodyPart> partsToRemove)
    {
        for(int i = partsToRemove.Count - 1; i >= 0; i--)
        {
            current.bodyParts[i].transform.parent = tetrion.transform;
            current.bodyParts[i].transform.localPosition = new Vector3(current.bodyParts[i].pos.x, current.bodyParts[i].pos.y, -2f);
            placedParts.Add(partsToRemove[i]);
            foreach (PlayArea area in playArea)
            {
                if (area.pos == current.bodyParts[i].pos)
                {
                    current.bodyParts[i].occupiedArea = area;
                }
            }
            current.bodyParts.Remove(current.bodyParts[i]);
            
        }
       

    }

    public void UpdateOccupiedPlayArea()
    {
        foreach (PlayArea area in playArea)
        {
            for (int x = 0; x < placedParts.Count; x++)
            {
                if (placedParts[x] != null)
                {
                    if (area.pos == placedParts[x].pos)
                    {
                        area.currentPart = placedParts[x];
                    }
                }
            }
        }
    }

    [ContextMenu("Tetris")]
    public void Tetris()
    {
        float highestLineCleared = 0;
        int linesCleared = 0;
        for (float x = 1.5f; x <= 24.5f; x++)
        {
            if (CheckIfRowIsFull(x))
            {
                if (x > highestLineCleared)
                {
                    highestLineCleared = x;
                }
                linesCleared++;
                Debug.Log(occupiedRow.Count);
                for (int y = 0; y < occupiedRow.Count; y++)
                {
                    Debug.Log(occupiedRow[y].pos);
                    placedParts.Remove(occupiedRow[y].currentPart);
                    Destroy(occupiedRow[y].currentPart.gameObject);
                }
            }
        }
        Debug.Log(highestLineCleared);
        Debug.Log(CheckIfRowIsFull(highestLineCleared + 1));
        Debug.Log(linesCleared);
        if (CheckIfRowIsFull(highestLineCleared + 1) != true)
        {
            for (int z = 0; z < placedParts.Count; z++)
            {

                if (placedParts[z].transform.localPosition.y >= highestLineCleared + 1)
                {
                    placedParts[z].occupiedArea.currentPart = null;
                    placedParts[z].transform.localPosition += new Vector3(0, -linesCleared, 0);
                    foreach(PlayArea area in playArea)
                    {
                        if (area.pos.y - -linesCleared == placedParts[z].pos.y && area.pos.x == placedParts[z].pos.x)
                        {
                            placedParts[z].occupiedArea = area;
                        }
                    }
                }
            }

        }
        
    }
    public bool CheckIfRowIsFull(float row)
    {
        occupiedRow.Clear();
        bool rowFullyOccupied = true;
        for (int x = 0; x < playArea.Count; x++)
        {
            if (playArea[x].pos.y == row)
            {
                if (playArea[x].currentPart != null)
                {
                    occupiedRow.Add(playArea[x]);
                }
                else
                {
                    rowFullyOccupied = false;
                    break;
                }
            }
        }
        return rowFullyOccupied;

    }

}
