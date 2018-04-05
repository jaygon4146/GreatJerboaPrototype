using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputComponent : MonoBehaviour {

	public enum InputType
	{
		Keyboard,
		Controller,
	}

	public enum XBoxControllerButtons
	{
		None,
		A,
		B,
		X,
		Y,
		LeftStick,
		RightStick,
		View,
		Menu,
		LeftBumper,
		RightBumper,
	}

	public enum XBoxControllerAxes
	{
		None,
		LeftStickHorizontal,
		LeftStickVertical,
		DPadHorizontal,
		DPadVertical,
		RightStickHorizontal,
		RightStickVertical,
		RightTrigger,
		LeftTrigger,
	}
		
	[Serializable]
	public class InputButton
	{
		public KeyCode key;
		public XBoxControllerButtons controllerButton;
		public bool Down { get; protected set; }
		public bool Held { get; protected set; }
		public bool Up { get; protected set; }
		public bool Enabled
		{
			get { return m_Enabled; }
		}

		[SerializeField]
		protected bool m_Enabled = true;
		protected bool m_GettingInput = true;

		//Used only to change the state of a button (Up/Down) only if a FixedUpdate happened between previous frame and this one.
		//Movement happens during FixedUpdate, input could be missed inbetween FizedUpdate
		public bool m_AfterFixedUpdateDown { get; protected set; }
		public bool m_AfterFixedUpdateHeld { get; protected set; }
		public bool m_AfterFixedUpdateUp { get; protected set; }

		protected static readonly Dictionary <int, string> k_ButtonsToName = new Dictionary<int,string> {
			{(int)XBoxControllerButtons.A, "A"},
			{(int)XBoxControllerButtons.B, "B"},
			{(int)XBoxControllerButtons.X, "X"},
			{(int)XBoxControllerButtons.Y, "Y"},
			{(int)XBoxControllerButtons.LeftStick, "Left Stick"},
			{(int)XBoxControllerButtons.RightStick, "Right Stick"},
			{(int)XBoxControllerButtons.View, "View"},
			{(int)XBoxControllerButtons.Menu, "Menu"},
			{(int)XBoxControllerButtons.LeftBumper, "Left Bumper"},
			{(int)XBoxControllerButtons.RightBumper, "Right Bumper"},
		};


		public InputButton(KeyCode key, XBoxControllerButtons controllerButton)
		{			
			this.key = key;
			this.controllerButton = controllerButton;
		}

		public void Get(bool fixedUpdateHappened, InputType inputType)
		{
			if (!m_Enabled) 
			{
				Down = false;
				Held = false;
				Up = false;
				return;
			}

			if (!m_GettingInput)
				return;

			if (inputType == InputType.Controller) 
			{
				if (fixedUpdateHappened) 
				{
					Down = Input.GetButtonDown (k_ButtonsToName [(int)controllerButton]);
					Held = Input.GetButton (k_ButtonsToName [(int)controllerButton]);
					Up = Input.GetButtonUp (k_ButtonsToName [(int)controllerButton]);

					m_AfterFixedUpdateDown = Down;
					m_AfterFixedUpdateHeld = Held;
					m_AfterFixedUpdateUp = Up;
				} 
				else 
				{
					Down = Input.GetButtonDown (k_ButtonsToName [(int)controllerButton])	|| m_AfterFixedUpdateDown;
					Held = Input.GetButton (k_ButtonsToName [(int)controllerButton])		|| m_AfterFixedUpdateHeld;
					Up = Input.GetButtonUp (k_ButtonsToName [(int)controllerButton]) 		|| m_AfterFixedUpdateUp;

					m_AfterFixedUpdateDown = Down;
					m_AfterFixedUpdateHeld = Held;
					m_AfterFixedUpdateUp = Up;
				}
			} 
			else if (inputType == InputType.Keyboard) 
			{
				if (fixedUpdateHappened) 
				{
					Down = Input.GetKeyDown (key);
					Held = Input.GetKey (key);
					Up = Input.GetKeyUp (key);

					m_AfterFixedUpdateDown = Down;
					m_AfterFixedUpdateHeld = Held;
					m_AfterFixedUpdateUp = Up;
				} 
				else 
				{
					Down = Input.GetKeyDown (key)	|| m_AfterFixedUpdateDown;
					Held = Input.GetKey (key)		|| m_AfterFixedUpdateHeld;
					Up = Input.GetKeyUp (key) 		|| m_AfterFixedUpdateUp;

					m_AfterFixedUpdateDown = Down;
					m_AfterFixedUpdateHeld = Held;
					m_AfterFixedUpdateUp = Up;
				}
			}
		}

		public void Enable()
		{
			m_Enabled = true;
		}

		public void Disable ()
		{
			m_Enabled = false;
		}

		public void GainControl()
		{
			m_GettingInput = true;
		}

		public IEnumerator ReleaseControl(bool resetValues)
		{
			m_GettingInput = false;

			if (!resetValues)
				yield break;

			if (Down)
				Up = true;
			Down = false;
			Held = false;

			m_AfterFixedUpdateDown = false;
			m_AfterFixedUpdateHeld = false;
			m_AfterFixedUpdateUp = false;

			yield return null;

			Up = false;
		}
	}

	[Serializable]
	public class InputAxis
	{
		public KeyCode positive;
		public KeyCode negative;
		public XBoxControllerAxes controllerAxis;
		public float Value { get; protected set;}
		public bool ReceivingInput { get; protected set;}
		public bool Enabled
		{	
			get { return m_Enabled; } 
		}

		protected bool m_Enabled = true;
		protected bool m_GettingInput = true;

		protected readonly static Dictionary<int, string> k_AxisToName = new Dictionary<int,string> {
			{(int) XBoxControllerAxes.LeftStickHorizontal, "Left Stick Horizontal"},
			{(int) XBoxControllerAxes.LeftStickVertical, "Left Stick Vertical"},
			{(int) XBoxControllerAxes.DPadHorizontal, "DPad Horizontal"},
			{(int) XBoxControllerAxes.DPadVertical, "DPad Vertical"},
			{(int) XBoxControllerAxes.RightStickHorizontal, "Right Stick Horizontal"},
			{(int) XBoxControllerAxes.RightStickVertical, "Right Stick Vertical"},
			{(int) XBoxControllerAxes.LeftTrigger, "Left Trigger"},
			{(int) XBoxControllerAxes.RightTrigger, "Right Trigger"},
		};

		public InputAxis(KeyCode positive, KeyCode negative, XBoxControllerAxes controllerAxis)
		{
			this.positive = positive;
			this.negative = negative;
			this.controllerAxis = controllerAxis;
		}

		public void Get(InputType inputType)
		{
			if (!m_Enabled)
			{
				Value = 0f;
				return;
			}

			if (!m_GettingInput)
				return;

			bool positiveHeld = false;
			bool negativeHeld = false;

			if (inputType == InputType.Controller)
			{
				float value = Input.GetAxisRaw(k_AxisToName[(int)controllerAxis]);
				positiveHeld = value > Single.Epsilon;
				negativeHeld = value < -Single.Epsilon;
			}
			else if (inputType == InputType.Keyboard)
			{
				positiveHeld = Input.GetKey(positive);
				negativeHeld = Input.GetKey(negative);
			}

			if (positiveHeld == negativeHeld)
				Value = 0f;
			else if (positiveHeld)
				Value = 1f;
			else
				Value = -1f;

			ReceivingInput = positiveHeld || negativeHeld;
		}

		public void Enable ()
		{
			m_Enabled = true;
		}

		public void Disable()
		{
			m_Enabled = false;
		}

		public void GainControl()
		{
			m_GettingInput = true;
		}

		public void ReleaseControl(bool resetValues)
		{
			m_GettingInput = false;
			if (resetValues)
			{
				Value = 0f;
				ReceivingInput = false;
			}
		}
	}
		
	public InputType inputType = InputType.Keyboard;

	bool m_FixedUpdateHappened;

	void Update()
	{
		GetInputs(m_FixedUpdateHappened || Mathf.Approximately(Time.timeScale,0));

		m_FixedUpdateHappened = false;
	}

	void FixedUpdate()
	{
		m_FixedUpdateHappened = true;
	}

	protected abstract void GetInputs(bool fixedUpdateHappened);

	public abstract void GainControl();

	public abstract void ReleaseControl(bool resetValues = true);

	protected void GainControl(InputButton inputButton)
	{
		inputButton.GainControl();
	}

	protected void GainControl(InputAxis inputAxis)
	{
		inputAxis.GainControl();
	}

	protected void ReleaseControl(InputButton inputButton, bool resetValues)
	{
		StartCoroutine(inputButton.ReleaseControl(resetValues));
	}

	protected void ReleaseControl(InputAxis inputAxis, bool resetValues)
	{
		inputAxis.ReleaseControl(resetValues);
	}
		

}
