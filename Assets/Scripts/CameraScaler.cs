using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] int width = 570;
    [SerializeField] int height = 780;
    
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = Camera.main;

        Screen.SetResolution(width, height, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
