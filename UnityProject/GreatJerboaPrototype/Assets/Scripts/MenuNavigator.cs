using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]

public class MenuNavigator : MonoBehaviour {

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
