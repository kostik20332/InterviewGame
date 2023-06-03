using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawingLogic : MonoBehaviour
{
    private Animator animator;

    public static bool play = false;
    public static bool finish = false;
    private bool isMobile;
    private bool touchDidMove = false;

    public static int score = 0;
    public static int fallGuysAmount = 20;
    public int i = 0;
    public int fallGuysAmountView = 0;

    private Vector2[] tapPositions = new Vector2[100];
    public Vector2 tapPosition;
    private Vector2 previousSpawnPosition;

    private float delta = 100f;
    private float tapTime;
    private float timeTouchBegan;
    private float tapTimeThreshold = 0.3f;

    public GameObject linePrefab;
    public GameObject canvas;
    private GameObject line;
    public static GameObject[] fallGuys = new GameObject[100];
    public GameObject[] fallGuys2 = new GameObject[20];
    public GameObject[] lines;

    public TMP_Text scoreText;

    public void Play()
    {
        play = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        isMobile = Application.isMobilePlatform;
        for (int i = 0; i < 20; i++)
        {
            fallGuys[i] = fallGuys2[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;
        if (finish)
        {
            play = false;
        }
        if (play)
        {
            fallGuysAmountView = fallGuysAmount;
            if (!isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    fallGuysArrayCheck();
                    lines = GameObject.FindGameObjectsWithTag("line");
                    foreach (GameObject line in lines)
                    {
                        Destroy(line);
                    }
                    i = 0;
                    tapPosition = Input.mousePosition;
                    previousSpawnPosition = Input.mousePosition;
                    tapPositions[i++] = tapPosition;

                    CreateLine(tapPosition);
                }
                else if (Input.GetMouseButton(0) && i < fallGuysAmount)
                {
                    tapPosition = Input.mousePosition;
                    if ((Mathf.Abs(previousSpawnPosition.x - tapPosition.x) + Mathf.Abs(previousSpawnPosition.y - tapPosition.y)) > delta)
                    {
                        previousSpawnPosition = tapPosition;
                        tapPositions[i++] = tapPosition;
                    }
                    CreateLine(tapPosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Cunstructioning(tapPositions, fallGuysAmount, i);
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        fallGuysArrayCheck();
                        lines = GameObject.FindGameObjectsWithTag("line");
                        foreach (GameObject line in lines)
                        {
                            Destroy(line);
                        }
                        i = 0;
                        tapPosition = Input.GetTouch(0).position;
                        previousSpawnPosition = tapPosition;
                        tapPositions[i++] = tapPosition;

                        CreateLine(tapPosition);
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved && i < fallGuysAmount)
                    {
                        tapPosition = Input.GetTouch(0).position;
                        if ((Mathf.Abs(previousSpawnPosition.x - tapPosition.x) + Mathf.Abs(previousSpawnPosition.y - tapPosition.y)) > delta)
                        {
                            previousSpawnPosition = tapPosition;
                            tapPositions[i++] = tapPosition;
                        }
                        CreateLine(tapPosition);
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        Cunstructioning(tapPositions, fallGuysAmount, i);
                    }
                }
            }
        }
    }

    void fallGuysArrayCheck()
    {
        for (int i = 0; i < 99; i++)
        {
            if (fallGuys[i] == null && fallGuys[i+1] != null)
            {
                for (int j = i; j < 99; j++)
                {
                    fallGuys[j] = fallGuys[j + 1];
                }
                i = 0;
            }
        }
    }

    void CreateLine(Vector2 pos)
    {
        line = Instantiate(linePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        line.transform.SetParent(canvas.transform, true);
    }

    void Cunstructioning(Vector2[] tapPositions, int positionsAmount, int newPositionsAmount)
    {
        if (newPositionsAmount == 1)
        {
            for (int i = 1; i < positionsAmount; i++)
            {
                tapPositions[i] = tapPositions[0];
            }
        }
        else
        {
            //увеличения количесва позиций до количества челиков
            while (newPositionsAmount < positionsAmount)
            {
                tapPositions[newPositionsAmount].x = tapPositions[newPositionsAmount - 1].x;
                tapPositions[newPositionsAmount].y = tapPositions[newPositionsAmount - 1].y;
                tapPositions[newPositionsAmount - 1].x = (tapPositions[newPositionsAmount - 2].x + tapPositions[newPositionsAmount].x) / 2;
                tapPositions[newPositionsAmount - 1].y = (tapPositions[newPositionsAmount - 2].y + tapPositions[newPositionsAmount].y) / 2;
                for (int i = newPositionsAmount - 3; i > 0; i--)
                {
                    tapPositions[i].x -= ((tapPositions[i].x - tapPositions[i - 1].x) / newPositionsAmount) * i;
                    tapPositions[i].y -= ((tapPositions[i].y - tapPositions[i - 1].y) / newPositionsAmount) * i;
                }
                newPositionsAmount++;
            }
        }
        for (int i = 0; i < positionsAmount; i++)
        {
            SetPosition(fallGuys[i], tapPositions[i]);

        }
    }

    public static void AddGuy(GameObject guy)
    {
        fallGuys[fallGuysAmount++] = guy;
    }

    void SetPosition(GameObject fallGuy, Vector2 position)
    {
        Vector2 convertedPosition;
        convertedPosition.x = position.x / 150;
        convertedPosition.y = position.y / 150;
        Vector3 newPos = new Vector3(convertedPosition.x - 3.5f, fallGuy.transform.position.y, convertedPosition.y - 45f);
        StartCoroutine(SmoothMoving(fallGuy, newPos));
    }

    public void Disactivation(GameObject obj)
    {
        obj.SetActive(false);
    }

    IEnumerator SmoothMoving(GameObject fallGuy, Vector3 newPos)
    {
        while (Mathf.Abs((fallGuy.transform.position.x - newPos.x) + (fallGuy.transform.position.z - newPos.z)) > 0.01f)
        {
            fallGuy.transform.position = Vector3.MoveTowards(fallGuy.transform.position, newPos, 0.5f);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
