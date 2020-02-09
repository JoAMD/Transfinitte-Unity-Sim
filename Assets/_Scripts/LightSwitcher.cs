using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightSwitcher : MonoBehaviour
{

    public GameObject half1;
    public GameObject half2;
    private float startTime;

    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("http://192.168.43.105:1880/rest/room"));
        startTime = Time.time;
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    private void Update()
    {
        if(Time.time - startTime > 1)
        {
            StartCoroutine(GetRequest("http://192.168.43.105:1880/rest/room"));
            startTime = Time.time;
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string received_str = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + received_str);

                string[] room_res = received_str.Split(';');

                Debug.Log(room_res[0] + " ; " + room_res[1]);

                if(room_res[0][3] == 'L')
                {
                    //Disable_or_Enable_Lights(half1, false);
                    half1.SetActive(false);
                }
                else
                {
                    //Disable_or_Enable_Lights(half1, true);
                    half1.SetActive(true);
                }
                if (room_res[1][4] == 'L')
                {
                    //Disable_or_Enable_Lights(half2, false);
                    half2.SetActive(false);
                }
                else
                {
                    //Disable_or_Enable_Lights(half2, true);
                    half2.SetActive(true);
                }
            }
        }
    }

    private void Disable_or_Enable_Lights(GameObject half, bool x)
    {
        float col = 84f / 255f;
        Light[] gb = half.GetComponentsInChildren<Light>();
        for (int i = 0; i < gb.Length; i++)
        {
            gb[i].enabled = x;
            //gb[i].transform.parent.GetComponent<MeshRenderer>().sharedMaterial.em = new Color(col, col, col, 1);
        }
    }

}
