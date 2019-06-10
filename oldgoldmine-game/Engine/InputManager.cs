﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace oldgoldmine_game.Engine
{

    public static class InputManager
    {
        private static Point mousePosition = Point.Zero;
        private static Vector2 mouseMovement = Vector2.Zero;
        private static bool mouseLeftClickSingle = false;
        private static bool mouseLeftClickHold = false;
        private static bool leftKeyHold = false;
        private static bool rightKeyHold = false;
        private static bool downKeyHold = false;
        private static bool leftKeyReleased = false;
        private static bool rightKeyReleased = false;
        private static bool downKeyReleased = false;
        private static bool jumpWasPressed = false;
        private static bool jumpPressed = false;
        private static bool pauseWasPressed = false;
        private static bool pausePressed = false;
        private static bool debugKeyWasPressed = false;
        private static bool debugKeyPressed = false;

        public static Vector2 CurrentFrameMouseMovement { get { return mouseMovement; } }
        public static Point MousePosition { get { return mousePosition; } }
        public static float MouseMovementX { get { return mouseMovement.X; } }
        public static float MouseMovementY { get { return -mouseMovement.Y; } }
        public static bool MouseSingleLeftClick { get { return mouseLeftClickSingle; } }
        public static bool MouseHoldLeftClick { get { return mouseLeftClickHold; } }
        public static bool PauseKeyPressed { get { return pausePressed; } }
        public static bool DebugKeyPressed { get { return debugKeyPressed; } }

        public static bool LeftKeyHold { get { return leftKeyHold; } }
        public static bool RightKeyHold { get { return rightKeyHold; } }
        public static bool DownKeyHold { get { return downKeyHold; } }
        public static bool LeftKeyReleased { get { return leftKeyReleased; } }
        public static bool RightKeyReleased { get { return rightKeyReleased; } }
        public static bool DownKeyReleased { get { return downKeyReleased; } }
        public static bool JumpKeyPressed { get { return jumpPressed; } }


        public static void UpdateFrameInput()
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            mouseMovement = (mouseState.Position - mousePosition).ToVector2();
            mousePosition = mouseState.Position;
            mouseLeftClickSingle = !mouseLeftClickHold && mouseState.LeftButton == ButtonState.Pressed;
            mouseLeftClickHold = mouseState.LeftButton == ButtonState.Pressed;

            leftKeyReleased = leftKeyHold && (keyboardState.IsKeyUp(Keys.A) || gamepadState.IsButtonUp(Buttons.LeftThumbstickLeft));
            rightKeyReleased = rightKeyHold && (keyboardState.IsKeyUp(Keys.D) || gamepadState.IsButtonUp(Buttons.LeftThumbstickRight));
            downKeyReleased = downKeyHold && (keyboardState.IsKeyUp(Keys.S) || gamepadState.IsButtonUp(Buttons.LeftThumbstickDown));
            leftKeyHold = (keyboardState.IsKeyDown(Keys.A) || gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft));
            rightKeyHold = (keyboardState.IsKeyDown(Keys.D) || gamepadState.IsButtonDown(Buttons.LeftThumbstickRight));
            downKeyHold = (keyboardState.IsKeyDown(Keys.S) || gamepadState.IsButtonDown(Buttons.LeftThumbstickDown));
            jumpPressed = !jumpWasPressed && (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.A));
            jumpWasPressed = (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.A));

            pausePressed = !pauseWasPressed && (keyboardState.IsKeyDown(Keys.Escape) || gamepadState.IsButtonDown(Buttons.Start));
            pauseWasPressed = (keyboardState.IsKeyDown(Keys.Escape) || gamepadState.IsButtonDown(Buttons.Start));
            debugKeyPressed = !debugKeyWasPressed && (keyboardState.IsKeyDown(Keys.F) || gamepadState.IsButtonDown(Buttons.Back));
            debugKeyWasPressed = (keyboardState.IsKeyDown(Keys.F) || gamepadState.IsButtonDown(Buttons.Back));
        }

    }
}