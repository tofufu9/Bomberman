using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEaterBehaviour : MonoBehaviour
{
    Vector2 pos;
    public float speed = 1;
    public LayerMask checkOnlyThis;
    private int randomIndex, previousIndex, bombIndex;
    Vector2[] rayDirection = new Vector2[4];
    private Vector2 facingRayInitial, nextPosition, playerPosition;
    private bool orientation, isValid, bombClose;   

    public int life = 3;
    public static float time;
    public bool hittable = true; //Variable to know if the enemy is in the damage animation or not
    public static Rigidbody2D rig;

    void Start()
    {        
        rig = GetComponent<Rigidbody2D>();

        rayDirection[0] = new Vector2(0, 1);
        rayDirection[1] = new Vector2(0, -1);
        rayDirection[2] = new Vector2(-1, 0);
        rayDirection[3] = new Vector2(1, 0);
        bombClose = false;
        pos = transform.position;
        InitialDirection();
    }
    void OnDestroy()
    {
        MenusManager.enemyAmount -= 1;
    }
    void FixedUpdate()
    {       
        GotHit();

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (playerPosition.y > transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }

        if (transform.position.x >= pos.x + 1 || transform.position.y >= pos.y + 1
                || transform.position.x <= pos.x - 1 || transform.position.y <= pos.y - 1)
         {
            bombClose = false;
           if ((transform.position.x / Mathf.Round(transform.position.x) != 1 || transform.position.y / Mathf.Round(transform.position.y) != 1))
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
            }
        }       

        if (hittable && bombClose)
        {            
            Move(randomIndex);  
        }
        else if (hittable && !bombClose)
        {            
            Move(randomIndex);
            facingRayInitial = new Vector2(transform.position.x, transform.position.y - 0.3f);
            RaycastHit2D facing = Physics2D.Raycast(facingRayInitial, rayDirection[randomIndex], 0.2f, checkOnlyThis);          

            if (facing.collider != null && !facing.collider.CompareTag("Bomba"))
            {
                switch (randomIndex)
                {
                    case 0:
                        randomIndex = 1;
                        break;
                    case 1:
                        randomIndex = 0;
                        break;
                    case 2:
                        randomIndex = 3;
                        break;
                    case 3:
                        randomIndex = 2;
                        break;
                }
            }

            if (transform.position.x >= pos.x + 1 || transform.position.y >= pos.y + 1
               || transform.position.x <= pos.x - 1 || transform.position.y <= pos.y - 1)   //Every fixed update, check if the position has changed by 1 and then choose a new direction        
            {
                if ((transform.position.x / Mathf.Round(transform.position.x) != 1 || transform.position.y / Mathf.Round(transform.position.y) != 1))
                {
                    transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
                }
                ChangeDirection();
                pos = transform.position;
                TestBomb();
            }
        }       
    }    

    void ChangeDirection() //Function to change enemy direction
    {       
        previousIndex = randomIndex;       
        if (orientation == true)
        {
            randomIndex = Random.Range(0, 2);
        }
        else
        {
            randomIndex = Random.Range(2, 4);
        }
        if (Raycasting(randomIndex, 1))
        {
            randomIndex = previousIndex;
        }
    } 

    void Move(int direction) //Function to move the enemy depending on the chosen radius
    {        
        switch (direction)
        {
            case 0: //Up              
                nextPosition = new Vector2(transform.position.x, transform.position.y + 1);
                transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
                GetComponent<Animator>().SetInteger("y", 1);
                GetComponent<Animator>().SetInteger("x", 0);
                orientation = false;
                break;
            case 1: //Down
                nextPosition = new Vector2(transform.position.x, transform.position.y - 1);
                transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
                GetComponent<Animator>().SetInteger("y", -1);
                GetComponent<Animator>().SetInteger("x", 0);
                orientation = false;
                break;
            case 2: //Left
                GetComponent<SpriteRenderer>().flipX = false;
                nextPosition = new Vector2(transform.position.x - 1, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
                GetComponent<Animator>().SetInteger("x", -1);
                GetComponent<Animator>().SetInteger("y", 0);
                orientation = true;
                break;
            case 3: //Right
                GetComponent<SpriteRenderer>().flipX = true;
                nextPosition = new Vector2(transform.position.x + 1, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
                GetComponent<Animator>().SetInteger("x", 1);
                GetComponent<Animator>().SetInteger("y", 0);
                orientation = true;
                break;
        }
    }  
    void InitialDirection() //Function to choose the starting direction
    {
        do
        {
            isValid = false;
            randomIndex = Random.Range(0, 4);
            if (Raycasting(randomIndex, 1) == null)
            {
                isValid = true;
            }
        } while (isValid == false);

        if (randomIndex == 0 || randomIndex == 1) 
        {
            orientation = true;
        }
        else
        {
            orientation = false;
        }
    }
    void TestBomb() //Function to test if there is a bomb nearby
    {
        for (int i = 0; i < 4; i++)
        {
            var rayCast = Raycasting(i, 5);
            if (rayCast && rayCast.CompareTag("Bomba"))
            {                
                isValid = true;
                bombClose = true;
                randomIndex = i;
                if (i == 0 || i == 1)
                {
                    orientation = true;                   
                }
                else
                {
                    orientation = false;                    
                }
                isValid = false;
                return;
            }
            else
            {
                bombClose = false;
            }         
        }
    }

    Collider2D Raycasting(int index, float size) //Function to test the random direction of the ray
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection[index], size, checkOnlyThis);        
        return hit.collider;
    }    
    void OnTriggerEnter2D(Collider2D other) //Function to kill the player
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.live = false;
        }
        if (other.gameObject.CompareTag("Bomba")) {
            Destroy(other.gameObject);            
        }
        if (other.gameObject.CompareTag("Explosion"))
        {
            hittable = false;
        }
    }

    private void GotHit() //Enemy damage animation function
    {
        if (hittable == false)
        {
            transform.position = transform.position;
            GetComponent<Animator>().SetBool("isHit", true);
            time += Time.deltaTime;
            if (time >= 0.7)
            {               
                GetComponent<Rigidbody2D>().isKinematic = false;
                life -= 1;
                if (life <= 0)
                {
                    Destroy(gameObject);                   
                   // MenusManager.enemyAmount -= 1;
                }
                hittable = true;
                time = 0;
                GetComponent<Animator>().SetBool("isHit", false);
            }
        }
    }
}
