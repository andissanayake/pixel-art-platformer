using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelAdventure.Game
{
    /// <summary>
    /// Bindings for the whole session. The player, sound player and end screen read this instead of the devices.
    /// Move: A/D or arrows, gamepad left stick. Jump: W, Up, Space, gamepad South.
    /// Run: Left Shift, gamepad West. Mute: M, gamepad Select.
    /// Restart: R, gamepad Start or South, or a click/tap.
    /// </summary>
    public sealed class GameInput : MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        public static event Action MutePressed;
        public static event Action RestartPressed;

        public InputAction Move { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Run { get; private set; }

        InputAction m_Mute;
        InputAction m_Restart;

        void Awake()
        {
            Instance = this;

            Move = new InputAction("Move", InputActionType.Value, expectedControlType: "Axis");
            Move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
            Move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");
            Move.AddBinding("<Gamepad>/leftStick/x");

            Jump = new InputAction("Jump", InputActionType.Button);
            Jump.AddBinding("<Keyboard>/w");
            Jump.AddBinding("<Keyboard>/upArrow");
            Jump.AddBinding("<Keyboard>/space");
            Jump.AddBinding("<Gamepad>/buttonSouth");

            Run = new InputAction("Run", InputActionType.Button);
            Run.AddBinding("<Keyboard>/leftShift");
            Run.AddBinding("<Gamepad>/buttonWest");

            m_Mute = new InputAction("Mute", InputActionType.Button);
            m_Mute.AddBinding("<Keyboard>/m");
            m_Mute.AddBinding("<Gamepad>/select");

            m_Restart = new InputAction("Restart", InputActionType.Button);
            m_Restart.AddBinding("<Keyboard>/r");
            m_Restart.AddBinding("<Gamepad>/start");
            m_Restart.AddBinding("<Gamepad>/buttonSouth");
            m_Restart.AddBinding("<Pointer>/press");
        }

        void OnEnable()
        {
            Move.Enable();
            Jump.Enable();
            Run.Enable();
            m_Mute.Enable();
            m_Restart.Enable();
        }

        void OnDisable()
        {
            Move.Disable();
            Jump.Disable();
            Run.Disable();
            m_Mute.Disable();
            m_Restart.Disable();
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
            Move.Dispose();
            Jump.Dispose();
            Run.Dispose();
            m_Mute.Dispose();
            m_Restart.Dispose();
        }

        void Update()
        {
            if (m_Mute.WasPressedThisFrame()) MutePressed?.Invoke();
            if (m_Restart.WasPressedThisFrame()) RestartPressed?.Invoke();
        }
    }
}
