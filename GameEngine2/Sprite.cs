using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using ObjParser;

namespace GameEngine2 // Note: actual namespace depends on the project name.
{
    public class Sprite
    {
        public Vector3 lightColor { get; set; }
        public Vector3 objectColor { get; set; }
        public float[] vertices { get; set; }
        public Vector3 center { get; set; }
        public uint[] triangles { get; set; }
        public Shader shader { get; set; }
        public Camera camera { get; set; }
        public bool isLamp { get; set; }
        private Matrix4 movement;
        public string objPath = "";
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        public Sprite() 
        {}
        public void Move(Vector3 offset)
        {
            center = center + offset;
            movement = movement * Matrix4.CreateTranslation(offset.X, offset.Y, offset.Z);
            //Trace.WriteLine(vertices[vertices.Length - 4].ToString() + ", " + vertices[vertices.Length - 3].ToString() + ", " + vertices[vertices.Length - 2].ToString());
        }
        public void RotateX(float angle)
        {
            movement = movement * Matrix4.CreateRotationX(angle);
        }
        public void RotateY(float angle)
        {
            movement = movement * Matrix4.CreateRotationY(angle);
        }
        public void RotateZ(float angle)
        {
            movement = movement * Matrix4.CreateRotationZ(angle);
        }
        public void Scale(float scale)
        {
            movement = movement * Matrix4.CreateScale(scale);
        }
        public void Load()
        {
            if (objPath != "")
            {
                List<float> vertices_list = new List<float>();
                List<float> texture_list = new List<float>();
                List<uint> triangle_list = new List<uint>();

                Obj objParser = new Obj();
                objParser.LoadObj(objPath);
                objParser.VertexList.ForEach(v => { vertices_list.Add((float)v.X); vertices_list.Add((float)v.Y); vertices_list.Add((float)v.Z); });
                objParser.TextureList.ForEach(t => { texture_list.Add((float)t.X); texture_list.Add((float)t.Y); });
                for (int i = 0; i < objParser.FaceList.Count; i++)
                {
                    for (int j = 0; j < objParser.FaceList[i].VertexIndexList.Length; j++)
                    {
                        triangle_list.Add((uint)objParser.FaceList[i].VertexIndexList[j] - 1);
                    }
                }
                triangles = triangle_list.ToArray();
                List<float> vertices_list_temp = new List<float>();
                for (int i = 0; i < vertices_list.Count / 3; i++)
                {
                    vertices_list_temp.Add(vertices_list[i * 3]);
                    vertices_list_temp.Add(vertices_list[i * 3 + 1]);
                    vertices_list_temp.Add(vertices_list[i * 3 + 2]);
                    vertices_list_temp.Add(texture_list[i]);
                    vertices_list_temp.Add(texture_list[i + 1]);
                }
                vertices = vertices_list_temp.ToArray();
            }

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, triangles.Length * sizeof(uint), triangles, BufferUsageHint.StaticDraw);

            shader.Use();

            var vertexLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            movement = Matrix4.Identity;

            if(!isLamp)
            {
                shader.SetVector3("lightColor", lightColor);
                shader.SetVector3("objectColor", objectColor);
            }
            
        }
        public void Draw()
        {
            // Send the vertices for a square to the GPU through the buffer

            /*
            StaticDraw: the data will most likely not change at all or very rarely.
            DynamicDraw: the data is likely to change a lot.
            StreamDraw: the data will change every time it is drawn.
            */

            GL.BindVertexArray(_vertexArrayObject);
            shader.Use();

            var model = Matrix4.Identity*movement;
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, triangles.Length, DrawElementsType.UnsignedInt, 0);
        }
        public void Unload()
        {
            // Dispose of the shader after the program is done
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            //Matrix4 projection = Matrix4.CreateOrthographic(window_width, window_height, 0, 100);
            //GL.UniformMatrix4(proj_loc, false, ref projection);

            GL.DeleteProgram(shader.Handle);
        }
    }
}
