// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.Views;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;

namespace Microsoft.Xna.Framework.Input
{
    public partial class MouseCursor
    {
        private PointerIcon pointerIcon;

        public MouseCursor(PointerIcon systemIcon)
        {
            this.pointerIcon = systemIcon;
        }

        public PointerIcon Icon
        {
            get
            {
                return pointerIcon;
            }
        }

        private static void PlatformInitalize()
        {
#pragma warning disable XA0001 // Find issues with Android API usage
#pragma warning disable CA1416 // Find issues with Android API usage
            if (PlatformInfo.IsAndroidVersionAtLeast(24))
            {
                Arrow = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Arrow));
                IBeam = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Text));
                Wait = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Wait));
                Crosshair = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Crosshair));
                WaitArrow = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Wait));
                SizeNWSE = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.TopLeftDiagonalDoubleArrow));
                SizeNESW = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.TopRightDiagonalDoubleArrow));
                SizeWE = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.HorizontalDoubleArrow));
                SizeNS = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.VerticalDoubleArrow));
                SizeAll = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Default));
                No = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.NoDrop));
                Hand = new MouseCursor(PointerIcon.GetSystemIcon(Game.Activity, PointerIconType.Hand));
            }
            else
            {
                Arrow = new MouseCursor(IntPtr.Zero);
                IBeam = new MouseCursor(IntPtr.Zero);
                Wait = new MouseCursor(IntPtr.Zero);
                Crosshair = new MouseCursor(IntPtr.Zero);
                WaitArrow = new MouseCursor(IntPtr.Zero);
                SizeNWSE = new MouseCursor(IntPtr.Zero);
                SizeNESW = new MouseCursor(IntPtr.Zero);
                SizeWE = new MouseCursor(IntPtr.Zero);
                SizeNS = new MouseCursor(IntPtr.Zero);
                SizeAll = new MouseCursor(IntPtr.Zero);
                No = new MouseCursor(IntPtr.Zero);
                Hand = new MouseCursor(IntPtr.Zero);
            }
#pragma warning restore XA0001 // Find issues with Android API usage
#pragma warning restore CA1416 // Find issues with Android API usage
        }

        private static MouseCursor PlatformFromTexture2D(Texture2D texture, int originx, int originy)
        {
            return new MouseCursor(IntPtr.Zero);
        }

        private void PlatformDispose()
        {
        }
    }
}
