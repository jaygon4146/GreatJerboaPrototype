﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;
	public PlayerCharacter Character;

    public Transform CameraTransform;

    private bool InPlay = true;

	public GoalTrigger Goal;
	public GameObject CollectableHolder;
	public GameObject CollectableIconGroup;
	public GameObject CollectableIconPrefab;

    public MenuSounds MenuAudioSource;

	private static List<GameObject> collectedList = new List<GameObject>();
	private int numberOfCollectedObjects = 0;
	private int filledIcons = 0;

	private static List<CollectableIcon> collectableIcons = new List<CollectableIcon> ();

	[SerializeField] private bool LevelComplete = false;

	public Text LevelCompleteText;

	private Animator stateAnimator;
	private AnimatorStateInfo stateInfo;

	public Animator environmentAnimator;
	private AnimatorStateInfo environmentInfo;

	public Button ResumeButtonObj;
	public Button RestartButtonObj;
	public Button QuitButtonObj;
	private float prevVertInput;

	private int SelectedButton;

	private float TimePassed = 0f;
	public Text TimeText;

	enum ButtonInts {
		Resume, 
		Restart, 
		Quit,
	};


	#region Animator States
	private static int PlayingState = Animator.StringToHash ("PlayingLevelScreen");
	private static int PauseState = Animator.StringToHash ("PauseScreen");
	private static int CompletedState = Animator.StringToHash ("CompletedLevelScreen");

	private static int PlayXPause = Animator.StringToHash ("PauseButton");
    private static int UnPauseXPlay = Animator.StringToHash("UnPauseButton");
    private static int PlayXCompleted = Animator.StringToHash ("Completed");

    private static int PHeightHash = Animator.StringToHash("PHeight");
    private static int PHorzHash = Animator.StringToHash("PHorz");
    #endregion

    void Awake(){
		MapLoader.Activate ();
		Character.transform.position = MapLoader.getPCSpawnPoint () + Vector2.up *0.05f;
		Goal.transform.position = MapLoader.getPCGoalPoint ();

		collectedList = new List<GameObject>();
		collectedList = MapLoader.getCollectableList ();
		collectableIcons = new List<CollectableIcon> ();

		if (CollectableHolder == null) {
			CollectableHolder = GameObject.Find ("CollectableHolder");
		}

		for (int i = 0; i < collectedList.Count; i++) {
			collectedList [i].transform.SetParent (CollectableHolder.transform);

			GameObject obj = Instantiate (CollectableIconPrefab, CollectableIconGroup.transform);
			collectableIcons.Add (obj.GetComponent<CollectableIcon>());
		}

		stateAnimator = GetComponent<Animator> ();
		DataManager.PrepareLevel (collectedList.Count);
	}

	void FixedUpdate(){
		stateInfo = stateAnimator.GetCurrentAnimatorStateInfo (0);

		if (Goal.HasBeenReached () && !LevelComplete) {

			FinishAction ();

		}
		if (stateInfo.shortNameHash == CompletedState) {
			if (PlayerInput.Instance.Jump.Down) {
				SceneManager.LoadScene ("TitleScreen", LoadSceneMode.Single);
                MenuAudioSource.PlayConfirm();
            }
		}

		if (stateInfo.shortNameHash == PlayingState) {

			TimePassed += Time.deltaTime;
			TimeText.text = "Time: " + System.Math.Round ((double)TimePassed, 2);

			CountCollectedObjects ();

			if (numberOfCollectedObjects > filledIcons) {

				collectableIcons [filledIcons].fill ();
				filledIcons++;
			}

			if (PlayerInput.Instance.MenuButton.Down) {
				SelectedButton = (int)ButtonInts.Resume;
				UpdateSelectedItem ();
				PauseAction ();
                MenuAudioSource.PlayConfirm();
            }

		}

		if (stateInfo.shortNameHash == PauseState) {

			PauseMenuUpdate ();

			if (PlayerInput.Instance.MenuButton.Down) {
				UnPauseAction ();
            }

            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    //print("RightShift + P = SWITCH CONTROLLER");
                    //PlayerInput.Instance.SwitchInputType();
                }
            }

        }

        BackGroundUpdate();

	}

    void BackGroundUpdate()
    {
        Vector3 v = CameraTransform.position;
        //environmentAnimator.transform.position = new Vector3(v.x, v.y-2, environmentAnimator.transform.position.z);
        environmentAnimator.SetFloat(PHeightHash, v.y);
        environmentAnimator.SetFloat(PHorzHash, v.x);
    }


	void PauseMenuUpdate(){
		float vertInput = PlayerInput.Instance.Vertical.Value;

        if (vertInput != prevVertInput)
        {
            if (vertInput > 0)
            { 
                CursorUp();
                MenuAudioSource.PlayCursor();
            }
            if (vertInput < 0)
            { 
                CursorDown();
                MenuAudioSource.PlayCursor();
            }
        }

		if (PlayerInput.Instance.Jump.Down) {
			ClickSelectedItem ();
            MenuAudioSource.PlayConfirm();
            //print ("ClickSelectedItem()");
        }

		prevVertInput = vertInput;
	}

	public void CursorUp(){
		if (SelectedButton > (int)ButtonInts.Resume) {
			SelectedButton --;
			UpdateSelectedItem();
		}
	}

	public void CursorDown(){
		if (SelectedButton < (int)ButtonInts.Quit) {
			SelectedButton ++;
			UpdateSelectedItem();
		}
	}

	private void UpdateSelectedItem (){
		switch (SelectedButton){

		case (int)ButtonInts.Resume:
			ResumeButtonObj.Select ();
			break;

		case (int)ButtonInts.Restart:
			RestartButtonObj.Select ();
			break;

		case (int)ButtonInts.Quit:
			QuitButtonObj.Select ();
			break;

		default:
			break;
		}
	}

	private void ClickSelectedItem(){
		switch (SelectedButton){

		case (int)ButtonInts.Resume:
			ResumeButtonObj.onClick.Invoke();
			break;

		case (int)ButtonInts.Restart:
			RestartButtonObj.onClick.Invoke();
			break;

		case (int)ButtonInts.Quit:
			QuitButtonObj.onClick.Invoke();

			break;

		default:
			break;
		}

	}
		
	private void PauseAction(){

        if (InPlay)
        {
            InPlay = false;
            stateAnimator.SetTrigger(PlayXPause);
            //environmentAnimator.speed = 0;
            Character.PauseCharacter();
        }
	}

	private void UnPauseAction()
    {
        if (!InPlay)
        {
            InPlay = true;
            MenuAudioSource.PlayCancel();
            stateAnimator.SetTrigger(UnPauseXPlay);
            //environmentAnimator.speed = 1;
            Character.UnPauseCharacter();
        }
	}

	private void FinishAction()
    {
        //environmentAnimator.speed = 0;
        Character.PauseCharacter();

        LevelComplete = true;
		CountCollectedObjects ();
		DataManager.FinishLevel (numberOfCollectedObjects);
		LevelCompleteText.text = DataManager.GetCompletionMessage ();

        if (stateInfo.shortNameHash == PlayingState) {
			stateAnimator.SetTrigger (PlayXCompleted);
		}
	}

	public void ResumeButton(){
		//print ("Resume Button");
		UnPauseAction ();
	}

	public void RestartButton(){
		//print ("Restart Button");
		SceneManager.LoadScene ("PlayableLevel", LoadSceneMode.Single);

		//TimePassed = 0f;
		//Awake ();
		//UnPauseAction ();
	}

	public void QuitButton(){
		//print ("Quit Button");
		SceneManager.LoadScene ("TitleScreen", LoadSceneMode.Single);
	}

	void CountCollectedObjects(){
		numberOfCollectedObjects = 0;

		for (int i = 0; i < collectedList.Count; i++) {
			CollectableObject obj = collectedList [i].GetComponent<CollectableObject>();
			if (obj != null) {
				//Confirm that what we are looking at is a collectable
				if (obj.wasCollected ()) {
					numberOfCollectedObjects++;
				}
			}
		}

		//print ("CountCollectedObjects()");
	}
}
