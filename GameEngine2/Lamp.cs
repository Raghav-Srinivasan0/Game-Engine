using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using GameEngine2;
using System.Diagnostics;
using Jitter;

namespace GameEngine2
{
    public class Lamp
    {
        public Sprite lamp;
        public Shader lamp_shader { get; set; }
        public Camera camera { get; set; }
        public string lampObj { get; set; }
        public float[] vertices { get; set; }
        public uint[] triangles { get; set; }
        public Vector3 lampPos { get; set; }
        public Vector3 lampColor { get; set; }
        public float lightPower { get; set; }
        public World world { get; set; }
        public Vector3 ambient { get; set; }
        public Vector3 diffuse { get; set; }
        public Vector3 specular { get; set; }
        public Lamp() { }
        public void Load()
        {
            lamp = new Sprite();
            lamp.shader = lamp_shader;
            lamp.camera = camera;
            if (triangles == null || vertices == null)
            {
                lamp.objPath = lampObj;
            }
            lamp.center = Vector3.Zero;
            lamp.isLamp = true;
            lamp.triangles = triangles;
            lamp.vertices = vertices;
            lamp.world = world;
            lamp.Load();
            lamp.Move(lampPos);
        }
        public void Draw()
        {
            lamp.Draw();
        }
    }
}