﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
	public static int lives = 3;
    Text score;
	// Use this for initialization
	void Start ()
    {
        score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //scoreValue = scoreValue + 1;
        score.text = "SCORE: " + scoreValue;
	}
}
