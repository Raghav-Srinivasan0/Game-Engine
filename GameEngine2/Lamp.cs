using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using GameEngine2;
using System.Diagnostics;

namespace GameEngine2
{
    public class Lamp
    {
        public Sprite lamp;
        public Shader lamp_shader { get; set; }
        public Camera camera { get; set; }
        public string lampObj { get; set; }
        public Vector3 lampPos { get; set; }
        public Vector3 lampColor { get; set; }
        public Lamp() { }
        public void Load()
        {
            lamp = new Sprite();
            lamp.shader = lamp_shader;
            lamp.camera = camera;
            lamp.objPath = lampObj;
            lamp.center = Vector3.Zero;
            lamp.isLamp = true;
            lamp.Load();
            lamp.Move(lampPos);
        }
        public void Draw()
        {
            lamp.Draw();
        }
    }
}