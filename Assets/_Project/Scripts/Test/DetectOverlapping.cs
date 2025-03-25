using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DetectOverlapping : MonoBehaviour
{
    public Transform target;
    public MeshFilter meshFilter;
    public bool isContinue;
    public bool isEnd;
    public float count;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            SceneManager.LoadSceneAsync("Level_1_nin");
    }

    private void OnTriggerStay(Collider other)
    {
        if (isContinue) return;
        
        count += Time.deltaTime;
        Stop();
    }

    public void Stop()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
             isEnd = true;   
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame && isEnd)
        {
            isContinue = true;
            Debug.Log("CC toi");
        }
    }
}
