using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LightBeam : MonoBehaviour
{
    public LayerMask planetLayer;
    public TextMeshProUGUI distText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI millionText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI timeToText;
    public TextMeshProUGUI distToText;
    public TextMeshProUGUI planetInfoText;
    public TextMeshProUGUI endText;
    public Planet[] planets;
    public Planet currentTarget;
    public Planet checkPoint;
    public Planet displayInfo;
    public GameObject explosion;
    public GameObject gameOverMenu;
    public GameObject gameWonMenu;
    public GameObject incomingText;
    public GameObject pauseMenu;
    public TextMeshProUGUI checkpointText;
    public Canvas mainCanvas;
    public int planetsDestroyed;

    public float speed;
    public float testMultiplyier;

    private int planetIndex;
    public bool planetHit;
    public bool gameOver;
    public bool gameWon;
    public bool menu;
    public bool paused;

    public TextAsset info;

    private RaycastHit2D hit;

    public SpriteRenderer spr;
    public SpriteRenderer lightSpr;
    public SpriteRenderer beamSpr;
    public ParticleSystem ps;
    private GameObject explosionPrefab;
    private Rigidbody2D rb2d;

    private float startPos;

    private void Start()
    {
        planetIndex = 1;
        checkPoint = planets[0];
        displayInfo = checkPoint;
        displayInfo.LoadInfo();
        checkpointText.text = checkPoint.planetName;
        currentTarget = planets[planetIndex];
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position.x;
        checkpointText.text = checkPoint.planetName;
        TargetUpdate();
    }

    void Update()
    {
        if (!gameOver)
        {
            if (currentTarget != null)
            {
                distToText.text = ((currentTarget.transform.position.x - transform.position.x) / 100).ToString("00.0") + " million mi.";
            }

            if (!planetHit)
            {
                hit = Physics2D.Raycast(transform.position, Vector2.right, 1500, planetLayer);
                Debug.DrawRay(transform.position, Vector2.right * 1500, Color.blue);
                if (hit)
                {
                    planetHit = true;
                    StartCoroutine(IncomingPlanet());
                }
            }


            float distance = Mathf.Abs(startPos - transform.position.x);
            if (distance > 99999)
            {
                millionText.text = "Billion Miles";
                distText.text = (distance / 100000).ToString("000.00");
            }
            else
            {
                millionText.text = "Million Miles";
                distText.text = (distance / 100).ToString("000.00");
            }

            timeText.text = TimeStructure(Time.timeSinceLevelLoad).ToString();

            if(planetsDestroyed >= 8 && !gameWon)
            {
                StartCoroutine(GameWon(info));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }

            if (!paused)
            {
                ScaleSwap();
            }

        }
        else if (gameOver && menu)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene(0);
            }
        }   
    }

    void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void FixedUpdate()
    {
        float step = (speed * testMultiplyier) * Time.deltaTime;
        rb2d.MovePosition(transform.position + transform.right * step);
    }

    void ScaleSwap()
    {
        float direction = Input.GetAxisRaw("Vertical");
        if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (direction < 0)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }

    string TimeStructure(float t)
    {
        float timeInSeconds = t * testMultiplyier;
        float hours = Mathf.Floor(timeInSeconds / 3600);
        float minutes = Mathf.Floor((timeInSeconds / 60) % 60);
        float seconds = Mathf.Floor(timeInSeconds % 60);
        string timeElapsed = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

        return timeElapsed;
    }

    void UpdateCheckPoint(Planet p)
    {
        planetIndex++;
        Debug.Log(planetIndex);
        currentTarget = planets[planetIndex];
        checkPoint = p;
        checkpointText.text = checkPoint.planetName;
        TargetUpdate();
    }

    void TargetUpdate()
    {
        targetText.text = currentTarget.planetName;
        distToText.text = ((currentTarget.transform.position.x - transform.position.x) / 100).ToString() + " millionText mi.";
        timeToText.text = TimeStructure(currentTarget.transform.position.x / speed);
    }

    IEnumerator IncomingPlanet()
    {
        incomingText.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        displayInfo.UnloadInfo();
        displayInfo = null;
        incomingText.SetActive(false);
        displayInfo = currentTarget;
        displayInfo.LoadInfo();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Planet")
        {
            UpdateCheckPoint(collision.GetComponent<Planet>());
            explosionPrefab = collision.GetComponent<Planet>().explosion;
            GameObject explosionObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collision.gameObject.SetActive(false);
            planetHit = false;
            planetInfoText.text = "";
            StartCoroutine(Slowtime());
        }

        if(collision.tag == "Missiles")
        {
            Destroy(collision.gameObject);
            speed = 0;
            testMultiplyier = 0;
            ps.Stop();
            spr.enabled = false;
            lightSpr.enabled = false;
            beamSpr.enabled = false;
            Instantiate(explosion, transform.position, Quaternion.identity);
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameWon(TextAsset textInfo)
    {
        gameWon = true;
        gameOver = true;
        mainCanvas.enabled = false;
        gameWonMenu.SetActive(true);
        menu = true;

        string endInfo = textInfo.ToString();
        int length = endInfo.Length;
        string information = null;

        foreach (char c in endInfo)
        {
            information += c;
            endText.text = information;
            yield return new WaitForSecondsRealtime(.05f);
        }


        yield return null;

    }

    IEnumerator GameOver()
    {
        gameOver = true;
        mainCanvas.enabled = false;
        yield return new WaitForSecondsRealtime(2f);
        gameOverMenu.SetActive(true);
        menu = true;
    }

    IEnumerator Slowtime()
    {
        Time.timeScale = .2f;

        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = 1;
        planetsDestroyed++;
    }
}
