// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;

namespace Microsoft.Xna.Framework
{
    public partial class GraphicsDeviceManager
    {
        partial void PlatformInitialize(PresentationParameters presentationParameters)
        {
            var backBufferFormat = _game.graphicsDeviceManager.PreferredBackBufferFormat;
            var surfaceFormat = backBufferFormat.GetColorFormat();
            var depthStencilFormat = _game.graphicsDeviceManager.PreferredDepthStencilFormat;

#if LINUX_GLES
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.ContextProfileMask, (int)Sdl.GL.Profile.ES);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.ContextMajorVersion, 2);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.ContextMinorVersion, 0);
#else // LINUX_GLES
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.ContextMajorVersion, 2);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.ContextMinorVersion, 1);
#endif

            // TODO Need to get this data from the Presentation Parameters
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.RedSize, surfaceFormat.R);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.GreenSize, surfaceFormat.G);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.BlueSize, surfaceFormat.B);
            Sdl.GL.SetAttribute(Sdl.GL.Attribute.AlphaSize, surfaceFormat.A);

            if (backBufferFormat == SurfaceFormat.ColorSRgb || backBufferFormat == SurfaceFormat.Bgr32SRgb || backBufferFormat == SurfaceFormat.Bgra32SRgb)
            {
                Sdl.GL.SetAttribute(Sdl.GL.Attribute.FramebufferSRGBCapable, 1);
            }

            switch (depthStencilFormat)
            {
                case DepthFormat.None:
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.DepthSize, 0);
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.StencilSize, 0);
                    break;
                case DepthFormat.Depth16:
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.DepthSize, 16);
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.StencilSize, 0);
                    break;
                case DepthFormat.Depth24:
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.DepthSize, 24);
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.StencilSize, 0);
                    break;
                case DepthFormat.Depth24Stencil8:
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.DepthSize, 24);
                    Sdl.GL.SetAttribute(Sdl.GL.Attribute.StencilSize, 8);
                    break;
            }

            Sdl.GL.SetAttribute(Sdl.GL.Attribute.DoubleBuffer, 1);

            if (presentationParameters.MultiSampleCount > 0)
            {
                Sdl.GL.SetAttribute(Sdl.GL.Attribute.MultiSampleBuffers, 1);
                Sdl.GL.SetAttribute(Sdl.GL.Attribute.MultiSampleSamples, presentationParameters.MultiSampleCount);
            }

            int clientWidth = presentationParameters.BackBufferWidth;
            int clientHeight = presentationParameters.BackBufferHeight;

            bool fullScreen = presentationParameters.IsFullScreen;
            bool hardwareFullScreen = presentationParameters.HardwareModeSwitch;

            SdlGameWindow window = (SdlGameWindow)SdlGameWindow.Instance;
            window.CreateWindow(clientWidth, clientHeight, fullScreen, hardwareFullScreen);

            presentationParameters.DeviceWindowHandle = window.Handle;
        }
    }
}
