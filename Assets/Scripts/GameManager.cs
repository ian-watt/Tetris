using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Video;

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
    private bool pieceHeld;
    public Tetromino heldPiece;
    public Tetromino goBetweenPiece;
    public GameObject nextWindow;
    public Tetromino nextPiece;

    public TextMeshProUGUI score;
    private int scoreValue;
    public TextMeshProUGUI level;
    public int levelValue = 1;
    public VideoPlayer videoPlayer;
    public VideoClip[] backgrounds;

    List<PlayArea> occupiedRow = new List<PlayArea>();

    void Awake()
    {
        instance = this;
        CreateTetrion();
        CreatePlayArea();
        videoPlayer.clip = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    void Update()
    {
        HandleLevelValue();
        if (levelValue == 2)
        {
            gameSpeed = .9f;
        }else if(levelValue == 3)
        {
            gameSpeed = .8f;
        }else if (levelValue == 3)
        {
            gameSpeed = .7f;
        }else if (levelValue == 4)
        {
            gameSpeed = .6f;
        }else if(levelValue == 5)
        {
            gameSpeed = .5f;
        }else if (levelValue == 6)
        {
            gameSpeed = .3f;
        }else if (levelValue == 7)
        {
            gameSpeed = .2f;
        }else if(levelValue == 8)
        {
            gameSpeed = .1f;
        }else if(levelValue == 9)
        {
            gameSpeed = .05f;
            spedSpeed = .03f;
        }
        else if(levelValue == 10)
        {
            gameSpeed = .03f;
            spedSpeed = .02f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldPiece();
        }

        if (currentTetromino.isFalling == false)
        {

            SpawnTetromino(GetPrefabIndex(nextPiece));
            DisplayNextPiece();
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

    private void HandleLevelValue()
    {
        level.text = "Level: " + levelValue;

        if (scoreValue >= 5000 && scoreValue <= 10000)
        {
            levelValue = 2;
        }
        else if (scoreValue > 10000 && scoreValue <= 15000)
        {
            levelValue = 3;
        }
        else if (scoreValue > 15000 && scoreValue <= 20000)
        {
            levelValue = 4;
        }
        else if (scoreValue > 20000 && scoreValue <= 25000)
        {
            levelValue = 5;
        }
        else if (scoreValue > 25000 && scoreValue <= 30000)
        {
            levelValue = 6;
        }
        else if (scoreValue > 30000 && scoreValue <= 35000)
        {
            levelValue = 7;
        }
        else if (scoreValue > 35000 && scoreValue <= 40000)
        {
            levelValue = 8;
        }
        else if (scoreValue > 40000 && scoreValue <= 45000)
        {
            levelValue = 9;
        }
        else if (scoreValue > 45000 && scoreValue <= 50000)
        {
            levelValue = 10;
        }
        else if (scoreValue > 50000)
        {
            levelValue = 11;
        }
    }

    private int GetPrefabIndex(Tetromino nextPiece)
    {
        int index = 0;
        if(nextPiece.myType == Tetromino.Type.L)
        {
            index = 0;
        }else if(nextPiece.myType == Tetromino.Type.Skew)
        {
            index = 1;
        }
        else if (nextPiece.myType == Tetromino.Type.Square)
        {
            index = 2;
        }
        else if (nextPiece.myType == Tetromino.Type.Straight)
        {
            index = 3;
        }
        else if (nextPiece.myType == Tetromino.Type.T)
        {
            index = 4;
        }
        else if (nextPiece.myType == Tetromino.Type.L1)
        {
            index = 5;
        }
        else if (nextPiece.myType == Tetromino.Type.Skew1)
        {
            index = 6;
        }

        return index;
    }

    private void HoldPiece()
    {
        Vector3 heldPiecePos;
        if (pieceHeld != true)
        {
            currentTetromino.isFalling = false;
            if(currentTetromino.myType == Tetromino.Type.L1)
            {
                currentTetromino.transform.localPosition = new Vector3(-6f, 23, 2);
            }
            else if (currentTetromino.myType == Tetromino.Type.T)
            {
                currentTetromino.transform.localPosition = new Vector3(-5.7f, 22.7f, 2);

            }
            else
            {
                currentTetromino.transform.localPosition = new Vector3(-5.7f, 22.5f, 2);

            }
            currentTetromino.transform.localScale = new Vector3(.7f, .7f, .7f);
            currentTetromino.tag = ("Placed");
            heldPiece = currentTetromino;
            pieceHeld = true;
        }
        else
        {
            heldPiecePos = heldPiece.transform.localPosition;
            heldPiece.transform.localPosition = AssignHeldPosition(heldPiece, currentTetromino);
            heldPiece.transform.localScale = new Vector3(1, 1, 1);
            heldPiece.tag = ("Current");
            heldPiece.isFalling = true;
            if (currentTetromino.myType == Tetromino.Type.L1)
            {
                currentTetromino.transform.localPosition = new Vector3(-6f, 23, 2);
            }
            else if (currentTetromino.myType == Tetromino.Type.T)
            {
                currentTetromino.transform.localPosition = new Vector3(-5.7f, 22.7f, 2);

            }
            else
            {
                currentTetromino.transform.localPosition = new Vector3(-5.7f, 22.5f, 2);

            }
            currentTetromino.tag = ("Placed");
            currentTetromino.transform.localScale = new Vector3(.7f, .7f, .7f);
            currentTetromino.isFalling = false;
            goBetweenPiece = currentTetromino;
            currentTetromino = heldPiece;
            heldPiece = goBetweenPiece;

        }

    }

    private void Start()
    {
        SpawnTetromino(ReturnRandomIndex());
        DisplayNextPiece();
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

        for (int x = 0; x <= 11; x++)
        {
            GameObject goXBottom = Instantiate(tetrionPrefab);
            GameObject goXTop = Instantiate(tetrionPrefab);
            goXBottom.transform.parent = nextWindow.transform;
            goXTop.transform.parent = nextWindow.transform;

            goXBottom.transform.localPosition = new Vector3(x + .5f, .5f, -2);
            goXTop.transform.localPosition = new Vector3(x + .5f, 9.5f, -2);
            for (int y = 0; y <= 8; y++)
            {
                GameObject goYLeft = Instantiate(tetrionPrefab);
                GameObject goYRight = Instantiate(tetrionPrefab);
                goYLeft.transform.parent = nextWindow.transform;
                goYRight.transform.parent = nextWindow.transform;
                goYLeft.transform.localPosition = new Vector3(0.5f, y + .5f, -2);
                goYRight.transform.localPosition = new Vector3(11.5f, y + .5f, -2);
            }
        }
        nextWindow.transform.localScale = new Vector3(.5f, .5f, .5f);
        nextWindow.transform.position = new Vector3(-8.7f, 10, 0);
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
                score.text = ("Score: " + (scoreValue += 100));
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
                    score.text = ("Score: " + (scoreValue += 100));
                    current.isFalling = false;
                    current.tag = "Placed";
                    for (int i = current.bodyParts.Count - 1; i >= 0; i--)
                    {
                        partsToRemove.Add(current.bodyParts[i]);

                    }

                }
            }
        }
        RemoveParts(current, partsToRemove);
    }

    private void RemoveParts(Tetromino current, List<BodyPart> partsToRemove)
    {
        for (int i = partsToRemove.Count - 1; i >= 0; i--)
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
            score.text = "Score: " + (scoreValue += 1000 * linesCleared);
            for (int z = 0; z < placedParts.Count; z++)
            {

                if (placedParts[z].transform.localPosition.y >= highestLineCleared + 1)
                {
                    placedParts[z].occupiedArea.currentPart = null;
                    placedParts[z].transform.localPosition += new Vector3(0, -linesCleared, 0);
                    foreach (PlayArea area in playArea)
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

    private Vector3 AssignHeldPosition(Tetromino held, Tetromino current)
    {
        Vector3 newPosition = new Vector3();

        if (held.myType == current.myType)
        {
            newPosition = currentTetromino.transform.localPosition;
        }
        else if (held.myType == Tetromino.Type.L || held.myType == Tetromino.Type.L1)
        {
            if (current.myType == Tetromino.Type.Skew || current.myType == Tetromino.Type.Skew1 || current.myType == Tetromino.Type.Straight)
            {
                newPosition = current.transform.localPosition + new Vector3(.5f, -.5f, 0);
            }
            else if (current.myType == Tetromino.Type.Square)
            {
                newPosition = current.transform.localPosition + new Vector3(0, -.5f, 0);

            }else if(current.myType == Tetromino.Type.L || current.myType == Tetromino.Type.L1)
            {
                newPosition = current.transform.localPosition;
            }
            else
            {
                newPosition = current.transform.localPosition + new Vector3(.5f, 0, 0);
            }
        }
        else if (held.myType == Tetromino.Type.Skew || held.myType == Tetromino.Type.Skew1 || held.myType == Tetromino.Type.Straight)
        {
            if (current.myType == Tetromino.Type.L || current.myType == Tetromino.Type.L1)
            {
                newPosition = current.transform.localPosition + new Vector3(-.5f, .5f, 0);
            }
            else if (current.myType == Tetromino.Type.Square)
            {
                newPosition = current.transform.localPosition + new Vector3(-.5f, 0, 0);
            }
            else if (current.myType == Tetromino.Type.T)
            {
                newPosition = current.transform.localPosition + new Vector3(0, .5f, 0);
            }
            else
            {
                newPosition = current.transform.localPosition;
            }
        }
        else if (held.myType == Tetromino.Type.Square)
        {
            if (current.myType == Tetromino.Type.L1 || current.myType == Tetromino.Type.L)
            {
                newPosition = current.transform.localPosition + new Vector3(0, .5f, 0);
            }
            else if (current.myType == Tetromino.Type.Skew || current.myType == Tetromino.Type.Skew1 || current.myType == Tetromino.Type.Straight)
            {
                newPosition = current.transform.localPosition + new Vector3(.5f, 0, 0);

            }
            else
            {
                newPosition = current.transform.localPosition + new Vector3(.5f, .5f, 0);

            }
        }
        else
        {
            if (current.myType == Tetromino.Type.L || current.myType == Tetromino.Type.L1)
            {
                newPosition = current.transform.localPosition + new Vector3(-.5f, 0, 0);
            }
            else if (current.myType == Tetromino.Type.Skew || current.myType == Tetromino.Type.Skew1 || current.myType == Tetromino.Type.Straight)
            {
                newPosition = current.transform.localPosition + new Vector3(0, -.5f, 0);

            }
            else if (current.myType == Tetromino.Type.Square)
            {
                newPosition = current.transform.localPosition + new Vector3(-.5f, -.5f, 0);

            }
        }
        return newPosition;
    }

    private void DisplayNextPiece()
    {
        if (nextPiece == null)
        {
            GameObject go = Instantiate(tetrominoPrefabs[ReturnRandomIndex()]);
            go.transform.parent = nextWindow.transform;
            if (go.GetComponent<Tetromino>().myType == Tetromino.Type.L1)
            {
                go.transform.localPosition = new Vector3(5, 6, 0);
            }
            else if (go.GetComponent<Tetromino>().myType == Tetromino.Type.T)
            {
                go.transform.localPosition = new Vector3(6, 5.7f, 0);

            }
            else
            {
                go.transform.localPosition = new Vector3(6, 5, 0);


                
            }
            go.transform.localScale = new Vector3(1.5f,1.5f, 1.5f);
            nextPiece = go.GetComponent<Tetromino>();
        }
        else
        {
            Destroy(nextPiece.gameObject);
            GameObject go = Instantiate(tetrominoPrefabs[ReturnRandomIndex()]);
            go.transform.parent = nextWindow.transform;
            if (go.GetComponent<Tetromino>().myType == Tetromino.Type.L1)
            {
                go.transform.localPosition = new Vector3(5, 6, 0);
            }
            else if (go.GetComponent<Tetromino>().myType == Tetromino.Type.T)
            {
                go.transform.localPosition = new Vector3(6, 5.7f, 0);

            }
            else
            {
                go.transform.localPosition = new Vector3(6, 5, 0);



            }
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            nextPiece = go.GetComponent<Tetromino>();
        }


    }

}
