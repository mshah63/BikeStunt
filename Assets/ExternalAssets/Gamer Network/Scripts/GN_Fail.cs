﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GN_Fail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameLoose();
        }
    }
}
