﻿/*
Author: Justin V
Contributors: ***REMOVED*** ***REMOVED***
Date Last Edited: 2/25/2019
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	/*	public int directionVar = 1;
        public float runSpeed = 1.5f;
        public float dashSpeed = 25.0f;
        public float staminaCapacity = 30;
        public float staminaCapacitycap = 30;
        bool frameDelayer = false;
        int rollDirection = 0;
        private Rigidbody2D rb;
        public Vector2 direction = Vector2.down;*/

	public float walkSpeed = 1;
	public float dashSpeed = 5;
    public float pauseOnHurt = 1;
    float stamina = 3;
	public float dashCooldown = 1.0f;
	public float dashTime = 0.5f;
	float dashTimeRemaining = 0.0f;
	public float dashLerpTime = 0.1f;
	public float dashLerpSpeed = 2.0f;
	[HideInInspector]
	public Vector2 dashDirection = Vector2.zero;
	float currentSpeed = 0.0f;
	[HideInInspector]
	public Vector2 currentDirection = Vector2.down;

	public string walkLeft = "WalkLeft";
	public string walkRight = "WalkRight";
	public string walkUp = "WalkUp";
	public string walkDown = "WalkDown";

	public string dashLeft = "DashLeft";
	public string dashRight = "DashRight";
	public string dashUp = "DashUp";
	public string dashDown = "DashDown";

	public string idleLeft = "Idle";
	public string idleRight = "Idle";
	public string idleUp = "Idle";
	public string idleDown = "Idle";
	
	public Sprite dashRightSpr;
	public Sprite dashUpSpr;
	public Sprite dashDownSpr;

	public AudioClip walkSound1;
	public AudioClip walkSound2;
	public AudioClip walkSound3;
	public AudioClip dashSound;

	public GameObject onionSkin;

	public float walkSoundDelay = 1.0f;
	float timeTillPlaySound = 0.0f;

	public GameObject hitMarker;
	float eTime = 0.0f;
	float timeTillPlaySound2 = 0.0f;

	[HideInInspector]
	public float timeTillCanMove = 0.0f;

	bool acceptingPassword = false;
	bool acceptingPassword2 = false;
	int passwordLevel = 0;
	int passwordLevel2 = 0;

	SpriteRenderer spriteRenderer;
	Animator animator;
	AudioSource audioSource;
	Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		stamina = dashCooldown;
	}

	void Update()
	{
		stamina += Time.deltaTime;
		dashTimeRemaining -= Time.deltaTime;
		timeTillPlaySound -= Time.deltaTime;
		timeTillPlaySound2 -= Time.deltaTime;

		Vector2 direction = Vector2.zero;
		if (Input.GetKey(Settings.KeyBinds.down))
			direction.y -= 1;
		if (Input.GetKey(Settings.KeyBinds.up))
			direction.y += 1;
		if (Input.GetKey(Settings.KeyBinds.right))
			direction.x += 1;
		if (Input.GetKey(Settings.KeyBinds.left))
			direction.x -= 1;

		currentSpeed = walkSpeed;

		if ((Input.GetKey(Settings.KeyBinds.dash) || Input.GetKeyDown(KeyCode.Space)) && stamina >= dashCooldown)
		{
			audioSource.PlayOneShot(dashSound);
			dashDirection = direction;
			dashTimeRemaining = dashTime;
			stamina = 0;
		}

		if (dashTimeRemaining >= 0)
		{
			direction = dashDirection;
			currentSpeed = dashSpeed;

			//Debug.Log("Dashing");

			if (direction.x > 0.0f)
			{
				animator.SetBool("MovingLeft", false);
				animator.SetBool("MovingRight", false);
				animator.SetBool("MovingUp", false);
				animator.SetBool("MovingDown", false);

				animator.SetBool("DashRight", true);
				animator.SetBool("DashBack", false);
				animator.SetBool("DashFront", false);

				spriteRenderer.flipX = false;
			}
			else if (direction.x < 0.0f)
			{
				animator.SetBool("MovingLeft", false);
				animator.SetBool("MovingRight", false);
				animator.SetBool("MovingUp", false);
				animator.SetBool("MovingDown", false);

				animator.SetBool("DashRight", true);
				animator.SetBool("DashBack", false);
				animator.SetBool("DashFront", false);

				spriteRenderer.flipX = true;
			}
			else if (direction.y > 0.0f)
			{
				animator.SetBool("MovingLeft", false);
				animator.SetBool("MovingRight", false);
				animator.SetBool("MovingUp", false);
				animator.SetBool("MovingDown", false);

				animator.SetBool("DashRight", false);
				animator.SetBool("DashBack", true);
				animator.SetBool("DashFront", false);

				spriteRenderer.flipX = false;
			}
			else if (direction.y < 0.0f)
			{
				animator.SetBool("MovingLeft", false);
				animator.SetBool("MovingRight", false);
				animator.SetBool("MovingUp", false);
				animator.SetBool("MovingDown", false);

				animator.SetBool("DashRight", false);
				animator.SetBool("DashBack", false);
				animator.SetBool("DashFront", true);

				spriteRenderer.flipX = false;
			}

			if (dashTimeRemaining % 0.025f <= 0.1f)
			{
				GameObject newOnionSkin = Instantiate(onionSkin, transform.position, transform.rotation);

				newOnionSkin.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
				newOnionSkin.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
			}
		}
		else
		{
			//Debug.Log("Not dashing");

			animator.SetBool("DashRight", false);
			animator.SetBool("DashBack", false);
			animator.SetBool("DashFront", false);

			//spriteRenderer.flipX = false;
		}

		if (dashTimeRemaining <= dashLerpTime)
			currentSpeed /= dashLerpSpeed;

		if (timeTillCanMove <= 0.0f)
			rb.velocity = direction.normalized * currentSpeed;
		else
			rb.velocity = Vector2.zero;
		timeTillCanMove -= Time.deltaTime;

		if (direction != Vector2.zero)
			currentDirection = direction.normalized;

		if (direction.x == -1 && dashTimeRemaining < 0)
		{
			//Debug.Log("DASH LEFT SPRITE");
			//animator.Play(dashLeft);
			animator.SetBool("MovingLeft", false);
			animator.SetBool("MovingRight", true);
			animator.SetBool("MovingUp", false);
			animator.SetBool("MovingDown", false);
			spriteRenderer.flipX = true;
		}
		else if (direction.x == 1 && dashTimeRemaining < 0)
		{
			//Debug.Log("DASH RIGHT SPRITE");
			//animator.Play(dashRight);
			animator.SetBool("MovingLeft", false);
			animator.SetBool("MovingRight", true);
			animator.SetBool("MovingUp", false);
			animator.SetBool("MovingDown", false);
			spriteRenderer.flipX = false;
		}
		else if (direction.y == -1 && dashTimeRemaining < 0)
		{
			//Debug.Log("DASH DOWN SPRITE");
			//animator.Play(dashDown);
			animator.SetBool("MovingLeft", false);
			animator.SetBool("MovingRight", false);
			animator.SetBool("MovingUp", false);
			animator.SetBool("MovingDown", true);
			spriteRenderer.flipX = false;
		}
		else if (direction.y == 1 && dashTimeRemaining < 0)
		{
			//Debug.Log("DASH UP SPRITE");
			//animator.Play(dashUp);
			animator.SetBool("MovingLeft", false);
			animator.SetBool("MovingRight", false);
			animator.SetBool("MovingUp", true);
			animator.SetBool("MovingDown", false);
			spriteRenderer.flipX = false;
		}

		//if (direction.x == -1 && dashTimeRemaining <= 0)
		//{
		//	//Debug.Log("WALK LEFT SPRITE");
		//	//animator.Play(walkLeft);
		//}
		//if (direction.x == 1 && dashTimeRemaining <= 0)
		//{
		//	//Debug.Log("WALK RIGHT SPRITE");
		//	//animator.Play(walkRight);
		//}
		//if (direction.y == -1 && dashTimeRemaining <= 0)
		//{
		//	//Debug.Log("WALK DOWN SPRITE");
		//	//animator.Play(walkDown);
		//}
		//if (direction.y == 1 && dashTimeRemaining <= 0)
		//{
		//	//Debug.Log("WALK UP SPRITE");
		//	//animator.Play(walkUp);
		//}

		if (direction.x == 0 && direction.y == 0)
		{
			//Debug.Log("IDLE SPRITE");
			animator.SetBool("MovingLeft", false);
			animator.SetBool("MovingRight", false);
			animator.SetBool("MovingUp", false);
			animator.SetBool("MovingDown", false);
		}
		else
		{
			if (timeTillPlaySound <= 0.0f)
			{
				timeTillPlaySound = walkSoundDelay;
				int snd = Mathf.RoundToInt(Random.Range(1.0f, 3.0f));
				float vol = Random.Range(0.5f, 0.8f);
				if (snd == 1)
					audioSource.PlayOneShot(walkSound1, vol);
				else if (snd == 2)
					audioSource.PlayOneShot(walkSound2, vol);
				else if (snd == 3)
					audioSource.PlayOneShot(walkSound3, vol);
			}
		}

		if (Input.GetKey(KeyCode.E))
			eTime += Time.deltaTime;
		else
			eTime = 0.0f;

		if (eTime >= 10.0f)
		{
			acceptingPassword = true;
			acceptingPassword2 = true;
		}

		if (acceptingPassword)
		{
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(key))
				{
					if (key == KeyCode.S && passwordLevel == 0)
						++passwordLevel;
					else if (key == KeyCode.U && passwordLevel == 1)
						++passwordLevel;
					else if (key == KeyCode.B && passwordLevel == 2)
						++passwordLevel;
					else if (key == KeyCode.Alpha2 && passwordLevel == 3)
						++passwordLevel;
					else if (key == KeyCode.P && (passwordLevel == 4 || passwordLevel == 10))
						++passwordLevel;
					else if (key == KeyCode.E && (passwordLevel == 5 || passwordLevel == 9 || passwordLevel == 12))
						++passwordLevel;
					else if (key == KeyCode.W && passwordLevel == 6)
						++passwordLevel;
					else if (key == KeyCode.D && passwordLevel == 7)
						++passwordLevel;
					else if (key == KeyCode.I && (passwordLevel == 8 || passwordLevel == 11))
						++passwordLevel;
					else
					{
						passwordLevel = 0;
						acceptingPassword = false;
					}
				}
			}
		}
		if (acceptingPassword2)
		{
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(key))
				{
					if (key == KeyCode.S && passwordLevel2 == 0 || passwordLevel2 == 5 || passwordLevel2 == 10)
						++passwordLevel2;
					else if (key == KeyCode.U && passwordLevel2 == 1)
						++passwordLevel2;
					else if (key == KeyCode.B && passwordLevel2 == 2)
						++passwordLevel2;
					else if (key == KeyCode.Alpha2 && passwordLevel2 == 3)
						++passwordLevel2;
					else if (key == KeyCode.T && passwordLevel2 == 4)
						++passwordLevel2;
					else if (key == KeyCode.E && (passwordLevel2 == 6 || passwordLevel2 == 9))
						++passwordLevel2;
					else if (key == KeyCode.R && passwordLevel2 == 7)
						++passwordLevel2;
					else if (key == KeyCode.I && passwordLevel2 == 8)
						++passwordLevel2;
					else
					{
						passwordLevel2 = 0;
						acceptingPassword2 = false;
					}
				}
			}
		}

		if (passwordLevel >= 13 || (System.DateTime.Today.Day == 1 && System.DateTime.Today.Month == 4))
		{
			Settings.memeMode = true;
			acceptingPassword = false;
		}
		if (passwordLevel2 >= 11)
		{
			Inventory.clear = true;
			Health.alwaysLow = true;
			UnityEngine.SceneManagement.SceneManager.LoadScene("SplashScreen");
		}

		if (Settings.memeMode)
		{
			Camera.main.GetComponent<InvertColors>().active = true;

			if (timeTillPlaySound2 <= 0.0f)
			{
				audioSource.PlayOneShot(GetComponent<Health>().hurtSound);
				timeTillPlaySound2 = 0.05f;
			}

			for (int i = 0; i < 40; ++i)
				Instantiate(hitMarker, transform.position + new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
		}

		/*Vector2 velocity = Vector2.zero;
        if (frameDelayer)
        {
            if (rollDirection == 1)
            {
                velocity.x = dashSpeed;
                direction.x = 1;
                staminaCapacity -= 1.0f;
            }
            if (rollDirection == 2)
            {
                velocity.x = -dashSpeed;
                direction.x = -1;
                staminaCapacity -= 1.0f;
            }
            if (rollDirection == 3)
            {
                velocity.y = dashSpeed;
                direction.y = 1;
                staminaCapacity -= 1.0f;
            }
            if (rollDirection == 4)
            {
                velocity.y = -dashSpeed;
                direction.y = -1;
                staminaCapacity -= 1.0f;
            }
        }
        //frameDelayer == false &&
        //this if stament halts the dash loop when stamina runs out
        if (staminaCapacity <= 0) frameDelayer = false;
        if (staminaCapacity <= 0) rollDirection = 0;
        //this if stament adds to the current stamina when it isnt in the delayer loop and has space to fill
        if (staminaCapacity < staminaCapacitycap) staminaCapacity += Time.deltaTime;
        //this if stament checks if the dash delay loop is active if so it doesn't do it 
        if (frameDelayer == false)
        {
            Debug.Log("E");
            if (Input.GetKey(Settings.KeyBinds.up))
            {
                velocity.y += runSpeed;
                direction.y = 1;
            }
            if (Input.GetKey(Settings.KeyBinds.down))
            {
                velocity.y += -runSpeed;
                direction.y = -1;
            }
            if (Input.GetKey(Settings.KeyBinds.left))
            {
                velocity.x += -runSpeed;
                direction.x = -1;
            }
            if (Input.GetKey(Settings.KeyBinds.right))
            {
                velocity.x += runSpeed;
                direction.x = 1;
            }
        }

        ///these 4 if statements check if the player can and wants to dash and roll direction specifies where
        if (Input.GetKey(Settings.KeyBinds.right) && Input.GetKey(Settings.KeyBinds.dash) && staminaCapacity >= staminaCapacitycap)
        {
            frameDelayer = true;
            rollDirection = 1;
        }
        if (Input.GetKey(Settings.KeyBinds.left) && Input.GetKey(Settings.KeyBinds.dash) && staminaCapacity >= staminaCapacitycap)
        {
            frameDelayer = true;
            rollDirection = 2;
        }
        if (c && Input.GetKey(Settings.KeyBinds.dash) && staminaCapacity >= staminaCapacitycap)
        {
            frameDelayer = true;
            rollDirection = 3;
        }
        if (Input.GetKey(Settings.KeyBinds.down) && Input.GetKey(Settings.KeyBinds.dash) && staminaCapacity >= staminaCapacitycap)
        {
            frameDelayer = true;
            rollDirection = 4;
        }
        rb.velocity = velocity;

    }*/
	}
}