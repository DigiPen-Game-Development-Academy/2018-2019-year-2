﻿/*
Author: Kevin P
Contributors: ***REMOVED*** ***REMOVED***
Date Last Modified: 2/13/2019
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public GameObject itemDrop;
	new public GameObject camera;
	public List<string> itemDrops = new List<string>();
	public List<int> itemDropCounts = new List<int>();
	public float dropDistance = 0.5f;

	// Max health of the entity
	public float maxHealth = 10;
	// Current health of the entity
	public float currentHealth;
	// Health bar used to display current health (if any)
	public GameObject healthBarObj;
	// The speed to change back to the normal color
	public float colorChangeSpeed = 1.0f;
	public float screenColorChangeSpeed = 2.0f;
	// The color when hurt
	public Color hurtColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	// The screen color when hurt
	public Color hurtScreenColor = new Color(1.0f, 1.0f, 0.0f, 85.0f);
	// The color when healed
	public Color healColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
	// The normal color
	public Color normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	// The normal screen color
	public Color normalScreenColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	// The object to spawn when the entity dies
	public GameObject deathObject;
	// two variables are used for passing to the screen shake script.
	public float shakeDuration = 0;
	public float shakeIntensity = 0;
	// The SpriteRenderer of the entity
	SpriteRenderer spriteRenderer;
	// The Bar of the health bar
	Bar healthBar;

	Image damageScreenIMG = null;

	public AudioClip hurtSound;
	AudioSource audioSource;
	public GameObject damageScreen;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		// Get the SpriteRenderer component
		spriteRenderer = GetComponent<SpriteRenderer>();

		// If the health bar is not null..
		if (healthBarObj != null)
		{
			// Get the Bar component of the health bar
			healthBar = healthBarObj.GetComponent<Bar>();
		}

		// Set the current health to the max health
		currentHealth = maxHealth;

		// Update the health bar
		UpdateHealthBar();

		if (damageScreen != null)
			damageScreenIMG = damageScreen.GetComponent<Image>();
	}

	void Update()
	{
		// Used for testing
		if (Input.GetKeyDown(KeyCode.Alpha0))
			Heal(1.0f);
		else if (Input.GetKeyDown(KeyCode.Alpha9))
			Damage(1.0f);

		// Lerp back to the normal color
		spriteRenderer.color = Color.Lerp(spriteRenderer.color, normalColor, Time.deltaTime * colorChangeSpeed);

		if (damageScreenIMG != null)
			damageScreenIMG.color = Color.Lerp(damageScreenIMG.color, normalScreenColor, Time.deltaTime * screenColorChangeSpeed);
	}

	// Used to damage the entity
	public void Damage(float amount)
	{
		float vol = Random.Range(0.5f, 0.8f);
		// Decrease the current health
		currentHealth -= amount;

		// Set the entity color
		spriteRenderer.color = hurtColor;

		if (damageScreenIMG != null)
			damageScreenIMG.color = hurtScreenColor;

		audioSource.PlayOneShot(hurtSound, vol);

		//if (camera != null)
		//camera.GetComponent<ScreenShake>().Shake(shakeIntensity, shakeDuration);

		// If the current health is less than 0..
		if (currentHealth <= 0)
		{
			// Kill the entity
			Death();
		}

		// Update the health bar
		UpdateHealthBar();
	}

	// Used to heal the entity
	public void Heal(float amount)
	{
		// Increase the currentHealth
		currentHealth += amount;

		// If the current health is greater than the max health
		if (currentHealth > maxHealth)
		{
			// Set the current health to the max health
			currentHealth = maxHealth;
		}

		// Set the entity color
		spriteRenderer.color = healColor;

		// Update the health bar
		UpdateHealthBar();
	}

	// Used to update the health bar
	public void UpdateHealthBar()
	{
		// If the health bar is not null
		if (healthBar != null)
		{
			// Set the health bar max
			healthBar.max = maxHealth;
			// Set the health bar current
			healthBar.current = currentHealth;
		}
	}

	// Used to kill the entity
	public void Death()
	{
		for (int i = 0; i < itemDrops.Count; ++i)
		{
			ItemPickup drop = Instantiate(itemDrop, transform.position + new Vector3(Random.Range(-dropDistance, dropDistance), Random.Range(-dropDistance, dropDistance)), transform.rotation).GetComponent<ItemPickup>();

			drop.itemID = itemDrops[i];
			drop.itemAmount = itemDropCounts[i];
		}

		// Create the death entity
		//if (deathObject != null)
		//Instantiate(deathObject, transform.position, transform.rotation);
		// Destroy the entity
		Destroy(gameObject);
	}
}
