using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightBeam : MonoBehaviour
{
    public TextMeshProUGUI distText;
    public TextMeshProUGUI timeText;
    public float speed;

    private GameObject explosionPrefab;
    private Rigidbody2D rb2d;

    float startPos;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position.x;
    }

    void Update()
    {
        //transform.Translate(Vector3.right * speed * Time.deltaTime);
        float distance = Mathf.Abs(startPos - transform.position.x);
        distText.text = (distance / 100).ToString("000.00") + " million miles";
        timeText.text = TimeStructure(Time.timeSinceLevelLoad).ToString();
        ScaleSwap();
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
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
        float hours = Mathf.Floor(t / 3600);
        float minutes = Mathf.Floor(t / 60);
        float seconds = t % 60;
        string timeElapsed = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

        return timeElapsed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Planet")
        {
            Destroy(collision.gameObject);
            Debug.Log(collision.gameObject.name);
            explosionPrefab = collision.GetComponent<Planet>().explosion;
            GameObject explosionObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            StartCoroutine(Slowtime());
        }
    }

    IEnumerator Slowtime()
    {
        Time.timeScale = .2f;

        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = 1;
    }
}
