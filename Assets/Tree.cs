using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject healthy;
    public GameObject sick;

    public bool isSick;

    public bool IsSick
    {
        get { return isSick; }
        set
        {
            if (isSick != value)
            {
                isSick = value;
                healthy.SetActive(!isSick);
                sick.SetActive(isSick);
            }
        }
    }

    void UpdateView()
    {
        healthy.SetActive(!isSick);
        sick.SetActive(isSick);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
