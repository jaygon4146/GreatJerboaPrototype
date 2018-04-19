using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : InputComponent {

	public static PlayerInput Instance
	{
		get { return s_Instance; }
	}

	protected static PlayerInput s_Instance;

	public bool HaveControl { get { return m_HaveControl; } }

	public InputButton Jump = new InputButton (KeyCode.Space, XBoxControllerButtons.A);
	public InputButton BButton = new InputButton (KeyCode.B, XBoxControllerButtons.B);
	public InputAxis Horizontal = new InputAxis (KeyCode.D, KeyCode.A, XBoxControllerAxes.LeftStickHorizontal);
	public InputAxis Vertical = new InputAxis (KeyCode.W, KeyCode.S, XBoxControllerAxes.LeftStickVertical);

	public InputAxis LTrigger = new InputAxis (KeyCode.Less, KeyCode.Greater, XBoxControllerAxes.LeftTrigger);
	public InputAxis RTrigger = new InputAxis (KeyCode.Less, KeyCode.Greater, XBoxControllerAxes.RightTrigger);

	protected bool m_HaveControl = true;
	protected bool m_DebugMenuOpen = false;

	void Awake ()
	{
		if (s_Instance == null)
			s_Instance = this;
		else
			throw new UnityException ("There may only be one PlayerInput script. The instances are " + s_Instance.name + " & " + name + ".");
	}

	void OnEnable()
	{
		if (s_Instance == null)
			s_Instance = this;
		else if(s_Instance != this)
			throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");

	}

	void OnDisable()
	{
		s_Instance = null;
	}

	protected override void GetInputs(bool fixedUpdateHappened)
	{
		//print ("PlayerInput.GetInputs ()");
		Jump.Get (fixedUpdateHappened, inputType);
		BButton.Get (fixedUpdateHappened, inputType);
		Horizontal.Get (inputType);
		Vertical.Get (inputType);
		RTrigger.Get (inputType);
		LTrigger.Get (inputType);

		if (Input.GetKeyDown (KeyCode.F12)) {
			m_DebugMenuOpen = !m_DebugMenuOpen;
		}
	}

	public override void GainControl()
	{
		m_HaveControl = true;

		GainControl (Jump);
		GainControl (Horizontal);
		GainControl (Vertical);
	}

	public override void ReleaseControl(bool resetValues = true)
	{
		m_HaveControl = false;

		ReleaseControl (Jump, resetValues);
		ReleaseControl (Horizontal, resetValues);
		ReleaseControl (Vertical, resetValues);
	}

	void OnGUI()
	{
		if (m_DebugMenuOpen) {
			const float height = 100;

			GUILayout.BeginArea(new Rect(30, Screen.height - height, 200, height));

			GUILayout.BeginVertical("box");
			GUILayout.Label("Press F12 to close");

			GUILayout.EndVertical();
			GUILayout.EndArea();

		}

	}

}
