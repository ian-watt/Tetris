using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    // <---------- TETRION & PLAY AREA FIELDS ---------->
    public GameObject tetrionPrefab;
    public GameObject tetrion;
    public Vector2 tetrionBounds;
    public Vector2 playAreaBounds;
    public List<PlayArea> playArea;
    public List<BodyPart> placedParts;
    List<PlayArea> list1 = new List<PlayArea>();
    List<PlayArea> list2 = new List<PlayArea>();
    List<PlayArea> list3 = new List<PlayArea>();
    List<PlayArea> list4 = new List<PlayArea>();


    // <---------- TETROMINO FIELDS ---------->
    public GameObject[] tetrominoPrefabs;
    public Vector2 minoBounds;
    private Vector3 startPos = new Vector3(5.5f, 26.5f, -1);
    public Tetromino currentTetromino;
    public Tetromino heldPiece;
    public Tetromino goBetweenPiece;
    public Tetromino nextPiece;

    // <---------- GAME MANAGER FIELDS ---------->
    public static GameManager instance;
    public float gameSpeed;
    public float spedSpeed;
    private float timePassed;
    public bool speedUp = false;
    public bool bgmActive = true;
    public bool sfxActive = true;
    private bool pieceHeld;
    private int scoreValue;
    public int levelValue = 1;
    public string attemptName;
    public int attemptScore;
    public bool isGameOver = false;
    public bool paused = false;

    public GameObject holdWindow;
    public TextMeshProUGUI score;
    public TextMeshProUGUI level;
    public VideoPlayer videoPlayer;
    public VideoClip[] backgrounds;
    public GameObject gameOverMenu;
    public AudioClip gameOverClip;
    public AudioSource audioManager;
    public AudioClip blockPlaced;
    public AudioClip tetrisSmall;
    public AudioClip tetrisBig;
    public TextMeshProUGUI inputName;
    public TextMeshProUGUI confirmation;
    public TextMeshProUGUI error;
    public GameObject nextWindow;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject tutorialMenu;
    public AudioClip menuButtonSFX;
    public GameObject musicCheck;
    public GameObject sfxCheck;


    // <---------- UPDATE, AWAKE AND START ---------->

    //assign the instance, call functions to create the tetrion and play area, and start playing the background video
    void Awake()
    {
        instance = this;
        CreateTetrion();
        CreatePlayArea();
        videoPlayer.clip = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    //spawn the first piece, and display the next piece
    private void Start()
    {
        
        SpawnTetromino(ReturnRandomIndex());
        DisplayNextPiece();
    }

    //call functions every frame to make the current piece fall, update the play area, check for filled lines and end the game if needed
    //run functions to open the pause menu, increase level based on score and speed up the game based on level
    //detect input and run the hold piece function
    //when a piece hits something, spawn a new piece and update the next piece window
    void Update()
    {
        
        HandleMenus();
        HandleLevelValue();
        SpeedUpGame();

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldPiece();
        }


        if (currentTetromino.isFalling == false)
        {

            SpawnTetromino(GetPrefabIndex(nextPiece));
            DisplayNextPiece();
        }

        
        DropPiece();
        UpdateOccupiedPlayArea();
        Tetris();
        GameOver();
    }

    // <---------- END OF UPDATE, AWAKE AND START ---------->





    // <---------- HELD PIECE AND NEXT PIECE HANDLING ---------->

    //logic to assign the current piece to the held piece, and switch them back and forth
    private void HoldPiece()
    {
        Vector3 heldPiecePos;
        if (pieceHeld != true)
        {
            currentTetromino.isFalling = false;
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

    //some logic needed to ensure the piece ends up in the right position when switching back and forth
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

            }
            else if (current.myType == Tetromino.Type.L || current.myType == Tetromino.Type.L1)
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

    //display the next piece that is going to drop
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
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
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

    // <---------- END OF HELD PIECE AND NEXT PIECE HANDLING ---------->





    // <---------- GAME SPEED, TETRION INSTANTIATION AND TETROMINO HANDLING ---------->

    //logic to make the piece fall based on game speed or sped speed
    private void DropPiece()
    {
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
    }

    //speed up the game based on level increase over time
    private void SpeedUpGame()
    {
        if (levelValue == 2)
        {
            gameSpeed = .95f;
        }
        else if (levelValue == 3)
        {
            gameSpeed = .9f;
        }
        else if (levelValue == 3)
        {
            gameSpeed = .85f;
        }
        else if (levelValue == 4)
        {
            gameSpeed = .8f;
        }
        else if (levelValue == 5)
        {
            gameSpeed = .75f;
        }
        else if (levelValue == 6)
        {
            gameSpeed = .7f;
        }
        else if (levelValue == 7)
        {
            gameSpeed = .65f;
        }
        else if (levelValue == 8)
        {
            gameSpeed = .6f;
        }
        else if (levelValue == 9)
        {
            gameSpeed = .55f;
        }
        else if (levelValue == 10)
        {
            gameSpeed = .5f;
        }
        else if (levelValue == 11)
        {
            gameSpeed = .45f;
        }
        else if (levelValue == 12)
        {
            gameSpeed = .4f;
        }
        else if (levelValue == 13)
        {
            gameSpeed = .35f;
        }
        else if (levelValue == 14)
        {
            gameSpeed = .3f;
        }
        else if (levelValue == 15)
        {
            gameSpeed = .25f;
        }
        else if (levelValue == 16)
        {
            gameSpeed = .2f;
        }
        else if (levelValue == 17)
        {
            gameSpeed = .15f;
        }
        else if (levelValue == 18)
        {
            gameSpeed = .1f;
            spedSpeed = .05f;
        }
        else if (levelValue == 19)
        {
            gameSpeed = .05f;
            spedSpeed = .03f;
        }
        else if (levelValue == 20)
        {
            gameSpeed = .03f;
            spedSpeed = .015f;
        }
        else if (levelValue > 20)
        {
            gameSpeed = .03f;
            spedSpeed = .015f;
        }
    }

    //increase the level based on the current score
    private void HandleLevelValue()
    {
        if (levelValue <= 20)
        {
            level.text = "Level: " + levelValue;
        }
        else
        {
            level.text = "Level: Tetris Master!!";
        }


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
        else if (scoreValue > 50000 && scoreValue <= 55000)
        {
            levelValue = 11;
        }
        else if (scoreValue > 55000 && scoreValue <= 60000)
        {
            levelValue = 12;
        }
        else if (scoreValue > 60000 && scoreValue <= 65000)
        {
            levelValue = 13;
        }
        else if (scoreValue > 65000 && scoreValue <= 70000)
        {
            levelValue = 14;
        }
        else if (scoreValue > 70000 && scoreValue <= 75000)
        {
            levelValue = 15;
        }
        else if (scoreValue > 75000 && scoreValue <= 80000)
        {
            levelValue = 16;
        }
        else if (scoreValue > 80000 && scoreValue <= 85000)
        {
            levelValue = 17;
        }
        else if (scoreValue > 85000 && scoreValue <= 90000)
        {
            levelValue = 18;
        }
        else if (scoreValue > 90000 && scoreValue <= 95000)
        {
            levelValue = 19;

        }
        else if (scoreValue > 95000 && scoreValue <= 100000)
        {
            levelValue = 20;
        }
        else if (scoreValue > 100000)
        {
            levelValue = 21;
        }
    }

    //helper function to get the prefab array index, used in conjuction with ReturnRandomIndex()
    private int GetPrefabIndex(Tetromino nextPiece)
    {
        int index = 0;
        if (nextPiece.myType == Tetromino.Type.L)
        {
            index = 0;
        }
        else if (nextPiece.myType == Tetromino.Type.Skew)
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

    //instantiate tetrion, hold window and next window
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

    //instantiate the play area
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

    //call both functions that detect if a piece has hit the bottom or if it is touching another piece
    private void HandleTetrominoes()
    {

        IsCurrentTouchingBottom(currentTetromino);
        StackTetrominoes(currentTetromino);

    }

    //instantiate a random tetromino prefab and assign it to the current falling tetromino
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

    //returns a random tetromino prefab for use with SpawnTetromino()
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
                if (sfxActive)
                {
                    audioManager.PlayOneShot(blockPlaced);
                }


                score.text = ("Score: " + (scoreValue += 100));
            }
        }
    }

    //check if the current tetromino is touching another tetromino, if true stop it from moving
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
                    if (sfxActive)
                    {
                        audioManager.PlayOneShot(blockPlaced);
                    }

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

    //helper function for StackTetrominoes()
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

    // <---------- END OFGAME SPEED, TETRION INSTANTIATION AND TETROMINO HANDLING ---------->





    // ---------- TETRIS HANDLING (DESTROY WHEN LINE IS FULL) ----------

    //every frame, update the play area with the pieces that are currently occupying it
    public void UpdateOccupiedPlayArea()
    {
        foreach (PlayArea area in playArea)
        {
            area.currentPart = null;
            for (int x = 0; x < placedParts.Count; x++)
            {
                if (placedParts[x] != null)
                {
                    if (area.pos == placedParts[x].pos)
                    {
                        placedParts[x].occupiedArea = null;
                        area.currentPart = placedParts[x];
                        placedParts[x].occupiedArea = area;
                    }
                }
            }
        }
    }

    //Check if any line is occupied at any given time, destroy it and move other pieces down respectively
    public void Tetris()
    {
        list1.Clear();
        list2.Clear();
        list3.Clear();
        list4.Clear();

        float highestLineCleared = 0;
        int linesCleared = 0;
        for (float x = 1.5f; x <= 24.5f; x++)
        {
            if (CheckIfRowIsFull(x))
            {
                if (list1.Count == 0)
                {
                    foreach (PlayArea area in occupiedRow)
                    {
                        list1.Add(area);
                    }
                }
                else if (list2.Count == 0)
                {
                    foreach (PlayArea area in occupiedRow)
                    {
                        list2.Add(area);
                    }
                }
                else if (list3.Count == 0)
                {
                    foreach (PlayArea area in occupiedRow)
                    {
                        list3.Add(area);
                    }
                }
                else if (list4.Count == 0)
                {
                    foreach (PlayArea area in occupiedRow)
                    {
                        list4.Add(area);
                    }
                }
                if (x > highestLineCleared)
                {
                    highestLineCleared = x;
                }
                linesCleared++;
                for (int y = 0; y < occupiedRow.Count; y++)
                {
                    placedParts.Remove(occupiedRow[y].currentPart);
                    Destroy(occupiedRow[y].currentPart.gameObject);

                }
            }
        }

        if (linesCleared == 2)
        {
            Debug.Log("Lines cleared = 2");
            for (int y = 0; y < list1.Count; y++)
            {
                if (list1[y].pos.y - 1 == list2[y].pos.y || list1[y].pos.y + 1 == list2[y].pos.y || list1[y].pos.y == list2[y].pos.y + 1 || list1[y].pos.y == list2[y].pos.y - 1)
                {
                    Debug.Log("Test, 2 lines connected as 1-2");
                    foreach (BodyPart part in placedParts)
                    {
                        if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -2, 0);
                        }
                    }
                }
                else if (list1[y].pos.y - 2 == list2[y].pos.y || list1[y].pos.y + 2 == list2[y].pos.y || list1[y].pos.y == list2[y].pos.y + 2 || list1[y].pos.y == list2[y].pos.y - 2)
                {
                    Debug.Log("Test, 2 lines connected as 1-3");
                    foreach (BodyPart part in placedParts)
                    {
                        if (list1[y].pos.y < list2[y].pos.y)
                        {
                            if (part.transform.localPosition.y == list1[y].pos.y + 1 && y == 0)
                            {

                                part.transform.localPosition += new Vector3(0, -1, 0);
                            }
                        }
                        if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {

                            part.transform.localPosition += new Vector3(0, -2, 0);
                        }
                    }
                }
                else if (list1[y].pos.y - 3 == list2[y].pos.y || list1[y].pos.y + 3 == list2[y].pos.y || list1[y].pos.y == list2[y].pos.y + 3 || list1[y].pos.y == list2[y].pos.y - 3)
                {
                    Debug.Log("Test. 2 lines connected as 1-4");
                    foreach (BodyPart part in placedParts)
                    {
                        if (list1[y].pos.y < list2[y].pos.y)
                        {
                            if (part.transform.localPosition.y == list1[y].pos.y + 1 && y == 0 || part.transform.localPosition.y == list1[y].pos.y + 2 && y == 0)
                            {
                                part.transform.localPosition += new Vector3(0, -1, 0);

                            }
                        }
                        if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -2, 0);
                        }
                    }
                }
            }
        }
        else if (linesCleared == 3)
        {
            for (int y = 0; y < list1.Count; y++)
            {
                if (list1[y].pos.y + 1 == list2[y].pos.y && list1[y].pos.y + 2 == list3[y].pos.y)
                {
                    Debug.Log("Test. 3 lines connected as 1-2-3");
                    foreach (BodyPart part in placedParts)
                    {
                        if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -3, 0);
                        }
                    }


                }
                else if (list1[y].pos.y + 2 == list2[y].pos.y && list1[y].pos.y + 3 == list3[y].pos.y)
                {
                    Debug.Log("Test. 3 lines connected as 1-3-4");
                    foreach (BodyPart part in placedParts)
                    {
                        if (part.transform.localPosition.y == list1[y].pos.y + 1 && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -1, 0);
                        }
                        else if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -3, 0);
                        }
                    }

                }
                else if (list1[y].pos.y + 1 == list2[y].pos.y && list1[y].pos.y + 3 == list3[y].pos.y)
                {
                    Debug.Log("Test. 3 lines connected as 1-2-4");
                    foreach (BodyPart part in placedParts)
                    {
                        if (part.transform.localPosition.y == list1[y].pos.y + 2 && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -2, 0);
                        }
                        else if (part.transform.localPosition.y > highestLineCleared && y == 0)
                        {
                            part.transform.localPosition += new Vector3(0, -3, 0);
                        }
                    }

                }
            }
        }
        else if (linesCleared == 1)
        {
            Debug.Log("Yes.");
            for (int y = 0; y < list1.Count; y++)
            {
                foreach (BodyPart part in placedParts)
                {
                    if (part.pos.y > highestLineCleared && y == 0)
                    {
                        part.transform.localPosition += new Vector3(0, -1, 0);
                    }
                }
            }

        }
        else if (linesCleared == 4)
        {
            for (int y = 0; y < list1.Count; y++)
            {
                foreach (BodyPart part in placedParts)
                {
                    if (part.pos.y > highestLineCleared && y == 0)
                    {
                        part.transform.localPosition += new Vector3(0, -4, 0);
                    }
                }
            }
        }
        if (linesCleared > 0 && linesCleared < 4)
        {
            if (sfxActive)
            {
                audioManager.PlayOneShot(tetrisSmall);
            }
            score.text = ("Score: " + (scoreValue += 1000));
        }
        else if (linesCleared >= 4)
        {
            if (sfxActive)
            {
                
                audioManager.PlayOneShot(tetrisBig);

            }
            score.text = ("Score: " + (scoreValue += 4000));
        }
    }

    //check if the row is full and pass it to Tetris()
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

    // ---------- END OF TETRIS HANDLING ----------





    // ---------- MENU HANDLING ----------

    //detect key input to open the pause menu
    private void HandleMenus()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy == false && optionsMenu.activeInHierarchy == false && tutorialMenu.activeInHierarchy == false && isGameOver == false)
        {
            pauseMenu.SetActive(true);
            speedUp = false;
            paused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy == true)
        {
            pauseMenu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }
    }

    //open the options menu and disable main menu
    public void OpenOptionsMenu(GameObject desiredMenu)
    {

        desiredMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }
    
    //exit the options menu, re-enable the main menu
    public void ExitOptionsMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        optionsMenu.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }

    //open the tutorial menu and disable the main menu
    public void OpenTutorialMenu(GameObject desiredMenu)
    {

        desiredMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }

    //exit the tutorial menu and re-enable the main menu
    public void ExitTutorialMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }

    //pass the current attempt name to the save load system
    public void AssignAttemptName()
    {
        if (inputName.text.Length == 1)
        {
            error.gameObject.SetActive(true);
        }
        else
        {
            attemptName = inputName.text;
            attemptScore = scoreValue;
            confirmation.gameObject.SetActive(true);
            error.gameObject.SetActive(false);
            SaveLoad.Instance.SaveScore();
        }

    }

    //check when a block is placed above the play area, and end the game
    private void GameOver()
    {
        if (isGameOver == false)
        {
            foreach (BodyPart block in placedParts)
            {
                if (block.pos.y > 24.5f)
                {
                    isGameOver = true;
                    break;
                }
            }
            if (isGameOver)
            {
                audioManager.PlayOneShot(gameOverClip, 2);
                Time.timeScale = 0;
                gameOverMenu.SetActive(true);
                tetrion.SetActive(false);
                nextWindow.SetActive(false);
                holdWindow.SetActive(false);

            }
        }

    }

    // toggle main BGM on and off
    public void ToggleBGM()
    {
        if (bgmActive)
        {
            audioManager.volume = 0;
            bgmActive = false;
            musicCheck.SetActive(false);
        }
        else
        {
            audioManager.volume = .3f;
            bgmActive = true;
            musicCheck.SetActive(true);

        }
    }
    
    // toggle sfx on and off
    public void ToggleSFX()
    {
        if (sfxActive)
        {
            sfxActive = false;
            sfxCheck.SetActive(false);

        }
        else
        {
            sfxActive = true;
            sfxCheck.SetActive(true);

        }
    }

    //resumes the game when pressed
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    //quit to desktop
    public void QuitGame()
    {
        Application.Quit();
    }

    // ---------- END OF MENU HANDLING ----------
}
