using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] GameObject _cell;
    
    public int queueNumber;
    
    private void Awake() {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(_cell, new Vector3(transform.position.x + (i * 20), transform.position.y,transform.position.z), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
