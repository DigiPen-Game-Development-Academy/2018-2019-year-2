﻿/*
Author: Luke Taranowski
Contributors: NA
Last Edited: 1/29/219
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{
    // Public variable we can edit in inspector
    public float deathTimer;
    // Private variable we can edit without effecting the public variable
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        // Set the private variable to the public one
        timer = deathTimer;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the timer has run out
        if (timer <= 0)
        {
            // destroys this object
            Destroy(gameObject);
        }
        else // decrease the timer
            timer -= Time.deltaTime;
    }
}
