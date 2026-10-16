// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using GameController;
using System.Collections.Generic;

#if !NET
using ControllerIndexType = System.Int32;
#else
using ControllerIndexType = GameController.GCControllerPlayerIndex;
#endif

namespace Microsoft.Xna.Framework.Input
{
    static partial class GamePad
    {
        private static int PlatformGetMaxNumberOfGamePads()
        {
            return 4;
        }

        static bool IndexIsUsed(GCControllerPlayerIndex index)
        {
            foreach (var ctrl in GCController.Controllers)
            {
                if (ctrl.PlayerIndex == (ControllerIndexType)index)
                    return true;
            }

            return false;
        }

        static void AssignIndex(GCControllerPlayerIndex index)
        {
            if (IndexIsUsed(index))
                return;
            foreach (var controller in GCController.Controllers)
            {
                if (controller.PlayerIndex == (ControllerIndexType)index)
                    break;
                if (controller.PlayerIndex == (ControllerIndexType)GCControllerPlayerIndex.Unset)
                {
                    controller.PlayerIndex = (ControllerIndexType)index;
                    break;
                }
            }
        }

        private static GamePadCapabilities PlatformGetCapabilities(int index)
        {
            var ind = (GCControllerPlayerIndex)index;

            AssignIndex(ind);

            foreach (var controller in GCController.Controllers)
            {
                if (controller == null)
                    continue;
                if (controller.PlayerIndex == (ControllerIndexType)ind)
                    return GetCapabilities(controller);
            }
            return new GamePadCapabilities { IsConnected = false };
        }

        private static GamePadCapabilities GetCapabilities(GCController controller)
        {
            //All iOS controllers have these basics
            var capabilities = new GamePadCapabilities()
            {
                IsConnected = true,
                GamePadType = GamePadType.GamePad,
            };
            if (controller.ExtendedGamepad != null)
            {
                capabilities.IsConnected = true;
                capabilities.HasAButton = true;
                capabilities.HasBButton = true;
                capabilities.HasXButton = true;
                capabilities.HasYButton = true;
                capabilities.HasBackButton = true;
                capabilities.HasStartButton = true;
                capabilities.HasDPadUpButton = true;
                capabilities.HasDPadDownButton = true;
                capabilities.HasDPadLeftButton = true;
                capabilities.HasDPadRightButton = true;
                capabilities.HasLeftShoulderButton = true;
                capabilities.HasRightShoulderButton = true;
                capabilities.HasLeftTrigger = true;
                capabilities.HasRightTrigger = true;
                capabilities.HasLeftXThumbStick = true;
                capabilities.HasLeftYThumbStick = true;
                capabilities.HasLeftStickButton = true;
                capabilities.HasRightXThumbStick = true;
                capabilities.HasRightYThumbStick = true;
                capabilities.HasRightStickButton = true;
            }
            else if (controller.Gamepad != null)
            {
                capabilities.IsConnected = true;
                capabilities.HasAButton = true;
                capabilities.HasBButton = true;
                capabilities.HasXButton = true;
                capabilities.HasYButton = true;
                capabilities.HasDPadUpButton = true;
                capabilities.HasDPadDownButton = true;
                capabilities.HasDPadLeftButton = true;
                capabilities.HasDPadRightButton = true;
                capabilities.HasLeftShoulderButton = true;
                capabilities.HasRightShoulderButton = true;
            }
            return capabilities;
        }

        private static GamePadState PlatformGetState(int index, GamePadDeadZone leftDeadZoneMode, GamePadDeadZone rightDeadZoneMode)
        {
            var ind = (GCControllerPlayerIndex)index;

            Buttons buttons = 0;
            bool connected = false;
            ButtonState Up = ButtonState.Released;
            ButtonState Down = ButtonState.Released;
            ButtonState Left = ButtonState.Released;
            ButtonState Right = ButtonState.Released;

            Vector2 leftThumbStickPosition = Vector2.Zero;
            Vector2 rightThumbStickPosition = Vector2.Zero;

            float leftTriggerValue = 0;
            float rightTriggerValue = 0;

            AssignIndex(ind);

            for (int i = 0; i < GCController.Controllers.Length; i++)
            {
                GCController controller = GCController.Controllers[i];
                if (controller == null)
                    continue;

                if (controller.PlayerIndex != (ControllerIndexType)ind)
                    continue;

                // validate controller has a valid input profile before reporting as connected
                if (controller.ExtendedGamepad == null && controller.Gamepad == null)
                    continue;

                connected = true;

                GCExtendedGamepad extendedGamepad = controller.ExtendedGamepad;
                if (extendedGamepad != null)
                {
                    if (extendedGamepad.ButtonA.IsPressed)
                        buttons |= Buttons.A;
                    if (extendedGamepad.ButtonB.IsPressed)
                        buttons |= Buttons.B;
                    if (extendedGamepad.ButtonX.IsPressed)
                        buttons |= Buttons.X;
                    if (extendedGamepad.ButtonY.IsPressed)
                        buttons |= Buttons.Y;

                    if (extendedGamepad.LeftShoulder.IsPressed)
                        buttons |= Buttons.LeftShoulder;
                    if (extendedGamepad.RightShoulder.IsPressed)
                        buttons |= Buttons.RightShoulder;

                    if (extendedGamepad.LeftTrigger.IsPressed)
                        buttons |= Buttons.LeftTrigger;
                    if (extendedGamepad.RightTrigger.IsPressed)
                        buttons |= Buttons.RightTrigger;

                    if (extendedGamepad.ButtonMenu != null
                    && extendedGamepad.ButtonMenu.IsPressed)
                    {
                        buttons |= Buttons.Start;
                    }
                        
                    if (extendedGamepad.ButtonOptions?.IsPressed == true)
                    {
                        buttons |= Buttons.Back;
                    }

                    if (extendedGamepad.DPad.Up.IsPressed)
                    {
                        Up = ButtonState.Pressed;
                        buttons |= Buttons.DPadUp;
                    }
                    if (extendedGamepad.DPad.Down.IsPressed)
                    {
                        Down = ButtonState.Pressed;
                        buttons |= Buttons.DPadDown;
                    }
                    if (extendedGamepad.DPad.Left.IsPressed)
                    {
                        Left = ButtonState.Pressed;
                        buttons |= Buttons.DPadLeft;
                    }
                    if (extendedGamepad.DPad.Right.IsPressed)
                    {
                        Right = ButtonState.Pressed;
                        buttons |= Buttons.DPadRight;
                    }

                    if (extendedGamepad.LeftThumbstickButton != null
                    && extendedGamepad.LeftThumbstickButton.IsPressed)
                    {
                        buttons |= Buttons.LeftStick;
                    }

                    if (extendedGamepad.RightThumbstickButton != null
                    && extendedGamepad.RightThumbstickButton.IsPressed)
                    {
                        buttons |= Buttons.RightStick;
                    }

                    leftThumbStickPosition.X = extendedGamepad.LeftThumbstick.XAxis.Value;
                    leftThumbStickPosition.Y = extendedGamepad.LeftThumbstick.YAxis.Value;
                    rightThumbStickPosition.X = extendedGamepad.RightThumbstick.XAxis.Value;
                    rightThumbStickPosition.Y = extendedGamepad.RightThumbstick.YAxis.Value;
                    leftTriggerValue = extendedGamepad.LeftTrigger.Value;
                    rightTriggerValue = extendedGamepad.RightTrigger.Value;
                }
                else if (controller.Gamepad != null)
                {
                    GCGamepad gamepad = controller.Gamepad;
                    if (gamepad.ButtonA.IsPressed)
                        buttons |= Buttons.A;
                    if (gamepad.ButtonB.IsPressed)
                        buttons |= Buttons.B;
                    if (gamepad.ButtonX.IsPressed)
                        buttons |= Buttons.X;
                    if (gamepad.ButtonY.IsPressed)
                        buttons |= Buttons.Y;

                    if (gamepad.LeftShoulder.IsPressed)
                        buttons |= Buttons.LeftShoulder;
                    if (gamepad.RightShoulder.IsPressed)
                        buttons |= Buttons.RightShoulder;

                    if (gamepad.DPad.Up.IsPressed)
                    {
                        Up = ButtonState.Pressed;
                        buttons |= Buttons.DPadUp;
                    }
                    if (gamepad.DPad.Down.IsPressed)
                    {
                        Down = ButtonState.Pressed;
                        buttons |= Buttons.DPadDown;
                    }
                    if (gamepad.DPad.Left.IsPressed)
                    {
                        Left = ButtonState.Pressed;
                        buttons |= Buttons.DPadLeft;
                    }
                    if (gamepad.DPad.Right.IsPressed)
                    {
                        Right = ButtonState.Pressed;
                        buttons |= Buttons.DPadRight;
                    }
                }
            }
            var state = new GamePadState(
                new GamePadThumbSticks(leftThumbStickPosition, rightThumbStickPosition, leftDeadZoneMode, rightDeadZoneMode),
                new GamePadTriggers(leftTriggerValue, rightTriggerValue),
                new GamePadButtons(buttons),
                new GamePadDPad(Up, Down, Left, Right));
            state.IsConnected = connected;
            return state;
        }

        private static bool PlatformSetVibration(int index, float leftMotor, float rightMotor, float leftTrigger, float rightTrigger)
        {
            return false;
        }
    }
}
