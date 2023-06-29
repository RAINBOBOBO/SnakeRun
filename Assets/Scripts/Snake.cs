using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;

public class Snake : MonoBehaviour
{
    private SnakeSegment headSegment = new SnakeSegment();
    private SnakeSegment tailSegment;
    
    public int bodySegmentCount = 4;
    public int startHP = 5;
    public int healthPoints;
    public int score = 0;
    public int scoreIncrement = 1;
    public float scoreTimeStep = 1f;

    public GameObject headGameObject;

    public GameObject gameStateObject;
    private GameState gameState;

    public GameObject gameScreenObject;
    private GameLoop gameLoop;

    void Start()
    {
        gameLoop = gameScreenObject.GetComponent<GameLoop>();
        gameState = gameStateObject.GetComponent<GameState>();
    }

    void Update()
    {
        if (gameState.currentState != GameState.State.Endless) return;

        if (IsSelfEaten() | IsOutOfBounds())
        {
            healthPoints = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border") healthPoints = 0;
    }

    public string ScoreAsString
    {
        get => score.ToString("#,#");
    }

    class SnakeSegment
    {
        public SnakeSegment nextSegment;
        public SnakeSegment previousSegment;
        public GameObject snakeSegmentGameObject;
    }

    public void CreateSnake()
    {
        healthPoints = startHP;

        headSegment.snakeSegmentGameObject = headGameObject;
        SnakeSegment nextSegment = headSegment;
        for (int currentBodySegmentIndex = 1; currentBodySegmentIndex <= bodySegmentCount; currentBodySegmentIndex++)
        {
            SnakeSegment currentBodySegment = new SnakeSegment();
            currentBodySegment.nextSegment = nextSegment;
            nextSegment.previousSegment = currentBodySegment;

            GameObject currentBodySegmentGameObject = new GameObject($"Body{currentBodySegmentIndex}");
            currentBodySegmentGameObject.transform.SetParent(gameScreenObject.transform);

            currentBodySegmentGameObject.AddComponent<SpriteRenderer>();
            currentBodySegmentGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/SnakeBod");
            currentBodySegmentGameObject.transform.localScale = headGameObject.transform.localScale;
            currentBodySegmentGameObject.AddComponent<BoxCollider2D>();

            currentBodySegmentGameObject.AddComponent<Rigidbody2D>();
            Rigidbody2D currentBodySegmentRigidBody = currentBodySegmentGameObject.GetComponent<Rigidbody2D>();
            currentBodySegmentRigidBody.position = nextSegment.snakeSegmentGameObject.GetComponent<Rigidbody2D>().position + new Vector2(0, - (headGameObject.transform.localScale.y * 0.5f));
            currentBodySegmentRigidBody.bodyType = RigidbodyType2D.Kinematic;

            currentBodySegmentGameObject.AddComponent<SnakeBodyController>();
            SnakeBodyController snakeBodyController = currentBodySegmentGameObject.GetComponent<SnakeBodyController>();
            snakeBodyController.nextRigidBody = nextSegment.snakeSegmentGameObject.GetComponent<Rigidbody2D>();
            snakeBodyController.thisRigidBody = currentBodySegmentRigidBody;

            currentBodySegment.snakeSegmentGameObject = currentBodySegmentGameObject;

            nextSegment = currentBodySegment;
        }

        tailSegment = nextSegment;
    }

    public void DeleteSnake()
    {
        SnakeSegment currentSegment = tailSegment;
        int loopCount = 0;
        int maxLoopCount = 100;
        while (currentSegment != headSegment)
        {
            if (loopCount >= maxLoopCount)
            {
                Debug.LogWarning($"checkIfSelfEaten has looped {maxLoopCount} times. Something is wrong.");
                break;
            }

            currentSegment = currentSegment.nextSegment;

            Destroy(currentSegment.previousSegment.snakeSegmentGameObject);
            currentSegment.previousSegment = null;
        }


    }

    bool IsSelfEaten()
    {
        SnakeSegment currentSegment = headSegment.previousSegment;
        Vector2 headPosition = headGameObject.GetComponent<Rigidbody2D>().position;
        int loopCount = 0;
        int maxLoopCount = 100;
        while (currentSegment!= null)
        {
            if (loopCount >= maxLoopCount)
            {
                Debug.LogWarning($"checkIfSelfEaten has looped {maxLoopCount} times. Something is wrong.");
                break;
            }

            Vector2 currentSegmentPosition = currentSegment.snakeSegmentGameObject.GetComponent<Rigidbody2D>().position;
            if (currentSegmentPosition == headPosition) return true;

            currentSegment = currentSegment.previousSegment;
        }

        return false;
    }

    public bool IsOutOfBounds()
    {
        List<BoxCollider2D> bounds = gameLoop.GetBorderBounds();
        SnakeSegment currentSegment = headSegment;
        int loopCount = 0;
        int maxLoopCount = 100;
        while (currentSegment != null)
        {
            if (loopCount >= maxLoopCount)
            {
                Debug.LogWarning($"IsOutOfBounds has looped {maxLoopCount} times. Something is wrong.");
                break;
            }

            BoxCollider2D currentCollider = currentSegment.snakeSegmentGameObject.GetComponent<BoxCollider2D>();
            if (currentCollider == null)
            {
                Debug.LogWarning($"IsOutOfBounds couldn't find the BoxCollider2D for the current segment.");
                continue;
            }

            foreach (BoxCollider2D boxCollider in bounds)
            {
                if (currentCollider.IsTouching(boxCollider)) return true;
            }

            currentSegment = currentSegment.previousSegment;
        }

        return false;
    }
}
