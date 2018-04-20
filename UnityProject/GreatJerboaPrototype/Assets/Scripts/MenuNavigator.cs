using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class MenuNavigator : MonoBehaviour {

	private int playerID = 0;

	public LevelList m_LevelList;
	public Animator stateAnimator;
	private AnimatorStateInfo stateInfo;

	public Text confirmationMsg;

	#region Cursor States

	float prevVertInput = 0;

	#endregion

	#region Animator States
	private static int StartToLvlHash = Animator.StringToHash ("StartXLvlSelect");
	private static int LvlToConfirmHash = Animator.StringToHash ("LvlSelectXConfirm");

	private static int StartState = Animator.StringToHash ("StartingScreen");
	private static int LvlState = Animator.StringToHash ("LevelSelectScreen");
	private static int ConfirmState = Animator.StringToHash ("LevelConfirmationScreen");
	#endregion

	void Awake(){
		DataManager.Load ();
		DataManager.SelectSaveDataSlot (0);
		DataManager.LoadAllLevels (m_LevelList.getMapFilesList());
		DataManager.Save ();
	}


	void FixedUpdate () {
		stateInfo = stateAnimator.GetCurrentAnimatorStateInfo (0);
		InputUpdate ();

		if (stateInfo.shortNameHash == LvlState) {
			CursorUpdate ();
		}

	}

	void InputUpdate(){
		if (PlayerInput.Instance.Jump.Down) {
			//print ("A Button Pressed");
			if (stateInfo.shortNameHash == StartState) {
				m_LevelList.UpdateSelectedItem ();
				stateAnimator.SetTrigger (StartToLvlHash);
			}

			if (stateInfo.shortNameHash == LvlState) {
				m_LevelList.ClickSelectedItem ();
				confirmationMsg.text = "Play " + m_LevelList.GetSelectedItemName() + " ? ";

				stateAnimator.SetTrigger (LvlToConfirmHash);
			}

			if (stateInfo.shortNameHash == ConfirmState) {
				DataManager.BeginLevel (m_LevelList.GetSelectedItemName ());
				print ("Beginning Level");
				SceneManager.LoadScene ("PlayableLevel", LoadSceneMode.Single);
			}
		}

		if (PlayerInput.Instance.BButton.Down) {
			//print ("B Button Pressed");
			if (stateInfo.shortNameHash == LvlState) {
				stateAnimator.SetTrigger (StartToLvlHash);
			}

			if (stateInfo.shortNameHash == ConfirmState) {
				stateAnimator.SetTrigger (LvlToConfirmHash);
			}
		}
	}

	void CursorUpdate(){
		float vertInput = PlayerInput.Instance.Vertical.Value;

		if (vertInput != prevVertInput) {
			if (vertInput > 0)
				m_LevelList.CursorUp ();
			if (vertInput < 0)
				m_LevelList.CursorDown ();
		}
		prevVertInput = vertInput;
	}
}
