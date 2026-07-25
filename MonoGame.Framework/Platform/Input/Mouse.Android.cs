// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.Views;

namespace Microsoft.Xna.Framework.Input
{
    public static partial class Mouse
    {
        private static long _lastClick = 0;
        private static byte _clicks;

        private static IntPtr PlatformGetWindowHandle()
        {
            return IntPtr.Zero;
        }

        private static void PlatformSetWindowHandle(IntPtr windowHandle)
        {
        }

        private static MouseState PlatformGetState(GameWindow window)
        {
            return window.MouseState;
        }

        private static void PlatformSetPosition(int x, int y)
        {
            PrimaryWindow.MouseState.X = x;
            PrimaryWindow.MouseState.Y = y;
        }

        /// <summary>
        /// Sets the <see cref="MouseCursor">MouseCursor</see>.
        /// </summary>
        /// <remarks>
        /// This method does not set a custom cursor as it is not currently supported on the IOS platform.
        /// </remarks>
        /// <param name="cursor">The <see cref="MouseCursor">MouseCursor</see> for the system to use</param>
        public static void PlatformSetCursor(MouseCursor cursor)
        {
            PointerIcon icon = cursor.Icon;
            if (icon == null)
                return;

            View view = ((AndroidGameWindow)Game.Instance.Window).GameView;
            view.PointerIcon = icon;
        }

        internal static void OnMouseMove(MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();
            PrimaryWindow.MouseState.X = (int)x;
            PrimaryWindow.MouseState.Y = (int)y;
        }

        internal static bool OnButtonPressed(KeyEvent e)
        {
            // Android APIs is awful
            // We need to handle secondary click as OnKeyDown with Keycode.Back
            if (e.KeyCode != Keycode.Back)
                return false;

            if (e.DownTime - _lastClick < 500)
                _clicks++;
            else
                _clicks = 1;

            _lastClick = e.DownTime;

            PrimaryWindow.MouseState.RightButton = ButtonState.Pressed;
            PrimaryWindow.MouseState.ClickCount = _clicks;
            return true;
        }

        internal static bool OnButtonReleased(KeyEvent e)
        {
            if (e.KeyCode != Keycode.Back)
                return false;

            PrimaryWindow.MouseState.RightButton = ButtonState.Released;
            PrimaryWindow.MouseState.ClickCount = _clicks;
            return true;
        }

        internal static bool OnMouseEvent(MotionEvent e)
        {
            MotionEventButtonState state;

            switch (e.Action)
            {
                case MotionEventActions.Scroll:
                    PrimaryWindow.MouseState.ScrollWheelValue += (int)(e.GetAxisValue(Axis.Vscroll) * 120f);
                    PrimaryWindow.MouseState.HorizontalScrollWheelValue += (int)(e.GetAxisValue(Axis.Hscroll) * 120f);
                    return true;

                case MotionEventActions.HoverMove:
                    OnMouseMove(e);
                    return true;

                case MotionEventActions.ButtonPress:
                    if (e.DownTime - _lastClick < 500)
                        _clicks++;
                    else
                        _clicks = 1;

                    _lastClick = e.DownTime;

                    state = e.ButtonState;
                    PrimaryWindow.MouseState.LeftButton = (state & MotionEventButtonState.Primary) == MotionEventButtonState.Primary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.RightButton = (state & MotionEventButtonState.Secondary) == MotionEventButtonState.Secondary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.MiddleButton = (state & MotionEventButtonState.Tertiary) == MotionEventButtonState.Tertiary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.ClickCount = _clicks;
                    return true;

                case MotionEventActions.ButtonRelease:
                    state = e.ButtonState;
                    PrimaryWindow.MouseState.LeftButton = (state & MotionEventButtonState.Primary) == MotionEventButtonState.Primary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.RightButton = (state & MotionEventButtonState.Secondary) == MotionEventButtonState.Secondary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.MiddleButton = (state & MotionEventButtonState.Tertiary) == MotionEventButtonState.Tertiary ? ButtonState.Pressed : ButtonState.Released;
                    PrimaryWindow.MouseState.ClickCount = _clicks;
                    return true;
            }

            return false;
        }
    }
}
