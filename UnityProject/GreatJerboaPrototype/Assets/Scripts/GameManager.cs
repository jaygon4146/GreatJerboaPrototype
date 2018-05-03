using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;
	public PlayerCharacter Character;

	public GoalTrigger Goal;
	public GameObject CollectableHolder;

	private static List<GameObject> collectedList = new List<GameObject>();
	private int numberOfCollectedObjects = 0;

	[SerializeField] private bool LevelComplete = false;

	public Text LevelCompleteText;

	private Animator stateAnimator;
	private AnimatorStateInfo stateInfo;

	public Animator environmentAnimator;
	private AnimatorStateInfo environmentInfo;


	#region Animator States
	private static int PlayingState = Animator.StringToHash ("PlayingLevelScreen");
	private static int PauseState = Animator.StringToHash ("PauseScreen");
	private static int CompletedState = Animator.StringToHash ("CompletedLevelScreen");

	private static int PlayXPause = Animator.StringToHash ("PauseButton");
	private static int PlayXCompleted = Animator.StringToHash ("Completed");
	#endregion

	void Awake(){
		MapLoader.Activate ();
		Character.transform.position = MapLoader.getPCSpawnPoint () + Vector2.up *0.05f;
		Goal.transform.position = MapLoader.getPCGoalPoint ();

		collectedList = new List<GameObject>();
		collectedList = MapLoader.getCollectableList ();

		if (CollectableHolder == null) {
			CollectableHolder = GameObject.Find ("CollectableHolder");
		}

		for (int i = 0; i < collectedList.Count; i++) {
			collectedList [i].transform.SetParent (CollectableHolder.transform);
		}

		stateAnimator = GetComponent<Animator> ();
		DataManager.PrepareLevel (collectedList.Count);
	}

	void FixedUpdate(){
		stateInfo = stateAnimator.GetCurrentAnimatorStateInfo (0);

		if (Goal.HasBeenReached () && !LevelComplete) {
			LevelComplete = true;
			CountCollectedObjects ();
			DataManager.FinishLevel (numberOfCollectedObjects);
			LevelCompleteText.text = DataManager.GetCompletionMessage ();
			if (stateInfo.shortNameHash == PlayingState) {
				stateAnimator.SetTrigger (PlayXCompleted);
			}
		}
		if (stateInfo.shortNameHash == CompletedState) {
			if (PlayerInput.Instance.Jump.Down) {
				SceneManager.LoadScene ("TitleScreen", LoadSceneMode.Single);
			}
		}

		if (stateInfo.shortNameHash == PlayingState) {
			if (PlayerInput.Instance.MenuButton.Down) {
				stateAnimator.SetTrigger (PlayXPause);
				environmentAnimator.speed = 0;
				Character.PauseCharacter ();
			}
		}

		if (stateInfo.shortNameHash == PauseState) {

			PauseMenuUpdate ();

			if (PlayerInput.Instance.MenuButton.Down) {
				stateAnimator.SetTrigger (PlayXPause);
				environmentAnimator.speed = 1;
				Character.UnPauseCharacter ();


			}
		}
	}

	void PauseMenuUpdate(){

	}


	void CountCollectedObjects(){
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
