using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStatus : MonoBehaviour {


    public GameObject text;
    public GameObject infoText;
    public GameObject camera;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    // Use this for initialization
    void Start () {
        text.GetComponent<Text>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        GameObject[] chunks = GameObject.FindGameObjectsWithTag("Chunk");
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }

        if (text.GetComponent<Text>().enabled == true)
        {
            if (Input.GetKeyDown("f3"))
            {
                Debug.Log("You press F3");
                infoText.GetComponent<Text>().enabled = true;
                text.GetComponent<Text>().enabled = false;
            }
        }
        else {
            if (Input.GetKeyDown("f3"))
            {
                infoText.GetComponent<Text>().enabled = false;
                text.GetComponent<Text>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        text.GetComponent<UnityEngine.UI.Text>().text = "Minecraft Clone v0.0.1\nFPS: " + ((int)m_lastFramerate) + "\nX = " + transform.position.x.ToString() + "\nY = " + transform.position.y.ToString() + "\nZ = " + transform.position.z.ToString() +
            "\nCamera Pos[X/Y]: " + camera.GetComponent<Camera>().transform.rotation.x + "/" + camera.GetComponent<Transform>().transform.rotation.y + "\nLoaded Chunks: "+ chunks.Length;

    }

}
