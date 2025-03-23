using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectOverlapping : MonoBehaviour
{
    public Transform target;
    public MeshFilter meshFilter;
    public bool isContinue;
    public bool isEnd;
    public float count;

    private void Update()
    {
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
