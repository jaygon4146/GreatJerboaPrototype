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

	public Image PreviewImage;

    public MenuSounds MenuAudioSource;


	#region Cursor States

	float prevVertInput = 0;

	#endregion

	#region Animator States
	private static int Start_Select     = Animator.StringToHash ("Start-Select");
	private static int Select_Start     = Animator.StringToHash ("Select-Start");
	private static int Select_Confirm   = Animator.StringToHash ("Select-Confirm");
    private static int Confirm_Select   = Animator.StringToHash("Confirm-Select");
    private static int Select_Controls  = Animator.StringToHash("Select-Controls");
    private static int Confirm_Controls = Animator.StringToHash("Confirm-Controls");
    private static int Controls_Select  = Animator.StringToHash("Controls-Select");


    private static int StartState       = Animator.StringToHash ("StartingScreen");
	private static int LvlState         = Animator.StringToHash ("LevelSelectScreen");
	private static int ControlsState    = Animator.StringToHash ("MenuControls");
	private static int ConfirmState     = Animator.StringToHash ("LevelConfirmationScreen");
	#endregion

	void Awake(){
		//DataManager.clearAllData ();
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

    void InputUpdate() {

        bool inTransition = false;

        if (stateAnimator.IsInTransition(0))
        { 
        inTransition = true;
            return;
        }


		if (PlayerInput.Instance.Jump.Down) {
			//print ("A Button Pressed");
			if (stateInfo.shortNameHash == StartState) {
				m_LevelList.UpdateSelectedItem ();
				stateAnimator.SetTrigger (Start_Select);
                MenuAudioSource.PlayConfirm();
			}

			if (stateInfo.shortNameHash == LvlState) {
				m_LevelList.ClickSelectedItem ();
				confirmationMsg.text = "Play " + m_LevelList.GetSelectedItemName() + " ? ";

				stateAnimator.SetTrigger (Select_Confirm);
                MenuAudioSource.PlayConfirm();
            }

			if (stateInfo.shortNameHash == ConfirmState) {
				DataManager.BeginLevel (m_LevelList.GetSelectedItemName ());
				//print ("Beginning " + m_LevelList.GetSelectedItemName());
				m_LevelList.DestroyList();
				SceneManager.LoadScene ("PlayableLevel", LoadSceneMode.Single);
                MenuAudioSource.PlayConfirm();
                MenuAudioSource.PlayBeginLevel();
            }
		}

		if (PlayerInput.Instance.BButton.Down) {
			//print ("B Button Pressed");
			if (stateInfo.shortNameHash == LvlState) {
				stateAnimator.SetTrigger (Select_Start);
                MenuAudioSource.PlayCancel();
			}

			if (stateInfo.shortNameHash == ConfirmState) {
				stateAnimator.SetTrigger (Confirm_Select);
                MenuAudioSource.PlayCancel();
            }

			if (stateInfo.shortNameHash == ControlsState) {
				stateAnimator.SetTrigger (Controls_Select);
                MenuAudioSource.PlayCancel();
            }
		}

		if (PlayerInput.Instance.MenuButton.Down) {
			if (stateInfo.shortNameHash == LvlState)
            {
				stateAnimator.SetTrigger (Select_Controls);
                MenuAudioSource.PlayConfirm();
            }
            if (stateInfo.shortNameHash == ConfirmState)
            {
                stateAnimator.SetTrigger(Confirm_Controls);
                MenuAudioSource.PlayConfirm();
            }

            if (stateInfo.shortNameHash == StartState)
            {
                print("Application.Quit()");
                Application.Quit();
            }
        }

        if (stateInfo.shortNameHash == ControlsState)
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    print("Delete + P = CLEAR ALL DATA");
                    DataManager.clearAllData ();
                    DataManager.Load();
                    DataManager.SelectSaveDataSlot(0);
                    DataManager.LoadAllLevels(m_LevelList.getMapFilesList());
                    DataManager.Save();
                }
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    print("RightShift + P = SWITCH CONTROLLER");
                    PlayerInput.Instance.SwitchInputType();
                }
            }
        }
    }

	void CursorUpdate(){
		float vertInput = PlayerInput.Instance.Vertical.Value;

        if (vertInput != prevVertInput)
        {
            if (vertInput > 0)
            {
                m_LevelList.CursorUp();
                MenuAudioSource.PlayCursor();
            }
            if (vertInput < 0) { 
                m_LevelList.CursorDown();
                MenuAudioSource.PlayCursor();
            }
		}
		prevVertInput = vertInput;

        /*
		Sprite s = new Sprite();
		string path =  "Art/LevelMaps/" + m_LevelList.GetSelectedItemName ();
		path = path.Substring (0, path.Length - 4);
		s = (Sprite) Resources.Load(path, typeof(Sprite));
		PreviewImage.sprite = s;
        */

        string path = "Art/LevelMaps/" + m_LevelList.GetSelectedItemName();
        path = path.Substring(0, path.Length - 4);
        Sprite s = (Sprite)Resources.Load(path, typeof(Sprite));
        PreviewImage.sprite = s;

	}
}
