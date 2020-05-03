using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
