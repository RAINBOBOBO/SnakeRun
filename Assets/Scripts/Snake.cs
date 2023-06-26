using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;

public class Snake : MonoBehaviour
{
    private int healthPoints;
    private SnakeSegment headSegment = new SnakeSegment();
    private SnakeSegment tailSegment;
    private GameState gameState;
    private DeathScreen deathScreen;
    private float currentTimeStep = 0f;

    public int startHP = 5;
    public int bodySegmentCount = 4;
    public int score = 0;
    public int scoreIncrement = 1;
    public float scoreTimeStep = 1f;

    public GameObject headGameObject;
    public GameObject gameStateObject;
    public GameObject deathScreenObject;

    void Start()
    {
        gameState = gameStateObject.GetComponent<GameState>();
        deathScreen = gameStateObject.GetComponent<DeathScreen>();
    }

    void Update()
    {
        if (gameState.currentState != GameState.State.Endless) return;

        if (healthPoints == 0)
        {
            deathScreen.SubmitFinalScore(score);
            gameState.SetStateDeath();
        }

        if (CheckIfSelfEaten())
        {
            healthPoints = 0;
        }

        if (Time.time >= currentTimeStep)
        {
            currentTimeStep += scoreTimeStep;
            IncreaseScore();
            Debug.Log($"Current Score: {score}");
        }
    }

    class SnakeSegment
    {
        public SnakeSegment nextSegment;
        public SnakeSegment previousSegment;
        public GameObject snakeSegmentGameObject;
    }

    public void CreateSnake()
    {
        currentTimeStep = Time.time;
        healthPoints = startHP;

        headSegment.snakeSegmentGameObject = headGameObject;
        SnakeSegment nextSegment = headSegment;
        for (int currentBodySegmentIndex = 1; currentBodySegmentIndex <= bodySegmentCount; currentBodySegmentIndex++)
        {
            SnakeSegment currentBodySegment = new SnakeSegment();
            currentBodySegment.nextSegment = nextSegment;
            nextSegment.previousSegment = currentBodySegment;

            GameObject currentBodySegmentGameObject = new GameObject();
            currentBodySegmentGameObject.name = $"Body{currentBodySegmentIndex}";
            currentBodySegmentGameObject.AddComponent<SpriteRenderer>();
            currentBodySegmentGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/SnakeBod");
            currentBodySegmentGameObject.transform.localScale = new Vector3(2, 2, 1);
            currentBodySegmentGameObject.AddComponent<BoxCollider2D>();

            currentBodySegmentGameObject.AddComponent<Rigidbody2D>();
            Rigidbody2D currentBodySegmentRigidBody = currentBodySegmentGameObject.GetComponent<Rigidbody2D>();
            currentBodySegmentRigidBody.position = nextSegment.snakeSegmentGameObject.GetComponent<Rigidbody2D>().position + Vector2.down;
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

    bool CheckIfSelfEaten()
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

    void IncreaseScore()
    {
        score += scoreIncrement;
    }
}
