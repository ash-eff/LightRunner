using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Planet : MonoBehaviour
{
    public string planetName;
    public GameObject explosion;
    public GameObject planetInfoObj;
    public TextMeshProUGUI planetInfoText;

    public TextAsset info;
    public float waitTime;

    private void Awake()
    {
        planetInfoObj = GameObject.Find("InformationText");
        planetInfoText = planetInfoObj.GetComponent<TextMeshProUGUI>();
    }

    public void LoadInfo()
    {
        StartCoroutine(LoadInformation(info));
    }

    public void UnloadInfo()
    {
        StopAllCoroutines();
    }

    public IEnumerator LoadInformation(TextAsset textInfo)
    {
        string planetInfo = textInfo.ToString();
        int length = planetInfo.Length;
        string information = null;

        foreach(char c in planetInfo)
        {
            information += c;
            planetInfoText.text = information;
            yield return new WaitForSecondsRealtime(waitTime);
        }


        yield return null;
    }
}
