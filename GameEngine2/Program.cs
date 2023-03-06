using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using static OpenTK.Graphics.OpenGL.GL;

namespace GameEngine2 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static int width = 1280, height = 1280;
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = "Game",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            // To create a new window, create a class that extends GameWindow, then call Run() on it.
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}