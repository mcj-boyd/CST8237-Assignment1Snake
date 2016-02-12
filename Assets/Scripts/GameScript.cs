using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameScript : MonoBehaviour {

    //Holds the vector for move translations
    Vector3 direction;

    public GameObject snakeHead;

    public GameObject topBorder;
    public GameObject bottomBorder;
    public GameObject leftBorder;
    public GameObject rightBorder;

    public GameObject fruitPrefab;
    GameObject fruit;

    public GameObject bodyPrefab;
    //List of Instantiated body prefabs
    List<GameObject> bodyList = new List<GameObject>();

    public Canvas gameOverCanvas;

    public Text scoreText;
    public Text livesText;
    int scoreValue = 0;
    int livesValue = 3;

    // Use this for initialization
    void Start()
    {
        //Reset the score and lives
        scoreText.text = "" + scoreValue;
        livesText.text = "" + livesValue;

        //spawn a fruit at a random location within the bounds
        fruit = (GameObject)Instantiate(fruitPrefab, new Vector2(
            (int)Random.Range(leftBorder.transform.position.x, rightBorder.transform.position.x),
            (int)Random.Range(bottomBorder.transform.position.y, topBorder.transform.position.y)),
            Quaternion.identity);

        //Set the Move() function to repeating
        InvokeRepeating("Move", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer snakeHeadRenderer = snakeHead.GetComponent<SpriteRenderer>();

        SpriteRenderer topBorderRenderer = topBorder.GetComponent<SpriteRenderer>();
        SpriteRenderer bottomBorderRenderer = bottomBorder.GetComponent<SpriteRenderer>();
        SpriteRenderer leftBorderRenderer = leftBorder.GetComponent<SpriteRenderer>();
        SpriteRenderer rightBorderRenderer = rightBorder.GetComponent<SpriteRenderer>();

        SpriteRenderer fruitRenderer = fruit.GetComponent<SpriteRenderer>();

        //Check if the player has changed the direction
        GetDirection();

        SpriteRenderer bodyRender;
        //Loop through the body segments and see if they are intersecting with the head
        for (int i = 0; i < bodyList.Count; i++)
        {
            bodyRender = bodyList.ElementAt(i).GetComponent<SpriteRenderer>();
            if (snakeHeadRenderer.bounds.Intersects(bodyRender.bounds))
            {
                Collision();
            }
        }

        //Check if the head has intersected with the borders of the game
        if (snakeHeadRenderer.bounds.Intersects(topBorderRenderer.bounds))
        {
            Collision();
        }
        else if (snakeHeadRenderer.bounds.Intersects(bottomBorderRenderer.bounds))
        {
            Collision();
        }
        else if (snakeHeadRenderer.bounds.Intersects(leftBorderRenderer.bounds))
        {
            Collision();
        }
        else if (snakeHeadRenderer.bounds.Intersects(rightBorderRenderer.bounds))
        {
            Collision();
        } //Check if the head has intersected with a fruit
        else if (snakeHeadRenderer.bounds.Intersects(fruitRenderer.bounds))
        {
            GrowSnake();
            SpawnFood();
        }
    }

    //Moves the fruit to another random location on the board
    void SpawnFood()
    {
        int x = (int)Random.Range(leftBorder.transform.position.x, rightBorder.transform.position.x);

        int y = (int)Random.Range(bottomBorder.transform.position.y, topBorder.transform.position.y);

        fruit.transform.position = new Vector2(x, y);

    }

    //Creates a new body segement and adds it to the snake
    void GrowSnake()
    {
        //Update the score
        scoreValue++;
        scoreText.text = "" + scoreValue;

        //Spawn new body segment off screen and add it to the List
        GameObject newBodySegment = (GameObject)Instantiate(bodyPrefab, new Vector2(-10, -10), Quaternion.identity);
        bodyList.Add(newBodySegment);

    }

    //Detects key presses from the player and updated the move translation Vector3
    void GetDirection()
    {

        SpriteRenderer snakeHeadRenderer = snakeHead.GetComponent<SpriteRenderer>();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = new Vector3(snakeHeadRenderer.bounds.size.x * -1, 0);
        }

        // Move up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = new Vector3(0, snakeHeadRenderer.bounds.size.y);
        }

        // Move right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = new Vector3(snakeHeadRenderer.bounds.size.x, 0);
        }

        // Move down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = new Vector3(0, snakeHeadRenderer.bounds.size.y * -1);
        }
    }

    //Moves the head and body segments
    void Move()
    {
        Vector3 v = transform.position;

        //Move the head
        transform.Translate(direction);

        //Move the body segments to the previous position of the segment next to it.
        if (bodyList.Count > 0)
        {
            bodyList.Last().transform.position = v;

            bodyList.Insert(0, bodyList.Last());
            bodyList.RemoveAt(bodyList.Count - 1);
        }
    }

    //Deals with what happens when the head collides with border or body segment
    void Collision()
    {
        //If the player has lives left
        if (livesValue > 1)
        {
            //Decrement the lives value and text
            livesValue--;
            livesText.text = "" + livesValue;

            //Reset the head to the center position
            snakeHead.transform.position = new Vector2(0, 0);
            direction = new Vector3(0, 0);

            //Destroy all body segments and clear the List
            for (int i = 0; i < bodyList.Count; i++)
            {
                Destroy(bodyList.ElementAt(i));
            }
            bodyList.Clear();
        }
        else //if the game is over
        {
            //Cancel the repetition of the Move() function. Essential stop the game
            CancelInvoke("Move");
            //Show the Game Over canvas
            gameOverCanvas.enabled = true;
        }
    }
}
