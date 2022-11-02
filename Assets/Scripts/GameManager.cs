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
    private Vector3 startPos = new Vector3(5.5f, 26.5f, -1);
    public float gameSpeed;
    public Tetromino currentTetromino;
    public List<Tetromino> tetrominoesInPlay;
    public List<PlayArea> playArea;

    void Awake()
    {
        instance = this;
        CreateTetrion();
        CreatePlayArea();
    }

    void Update()
    {
        if (currentTetromino.isFalling == false)
        {
            SpawnTetromino();
        }
        HandleTetrominoes();
    }

    private void Start()
    {
        SpawnTetromino();
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
    private void SpawnTetromino()
    {
        GameObject current = Instantiate(tetrominoPrefabs[0]);
        currentTetromino = current.GetComponent<Tetromino>();
        current.transform.parent = tetrion.transform;
        current.transform.localPosition = startPos + current.transform.position;
        current.tag = "Current";
        currentTetromino.position = transform.position;
        currentTetromino.currentRotation = Tetromino.Rotation.Zero;
        currentTetromino.isFalling = true;

    }

    //check if the current tetromino is touching the bottom ; if true, stop it from moving
    public void IsCurrentTouchingBottom(Tetromino current)
    {
        foreach (BodyPart part in current.bodyParts)
        {
            if (part.pos.y == 1.5f && part.transform.parent.CompareTag("Current"))
            {
                current.isFalling = false;
                tetrominoesInPlay.Add(current);
                current.tag = ("Placed");
            }
        }
    }

    public void StackTetrominoes(Tetromino current)
    {
        foreach (BodyPart part in current.bodyParts)
        for (int y = 0; y < tetrominoesInPlay.Count; y++)
        {
            for (int x = 0; x < tetrominoesInPlay[y].bodyParts.Length; x++)
            {
                if (part.pos.y == tetrominoesInPlay[y].bodyParts[x].pos.y + 1 && part.pos.x == tetrominoesInPlay[y].bodyParts[x].pos.x && part.transform.parent.CompareTag("Current"))
                {
                    current.isFalling = false;
                    current.tag = "Placed";
                    tetrominoesInPlay.Add(current);
                }
            }
        }
    }

}
