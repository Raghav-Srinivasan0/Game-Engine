using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using GameEngine2;
using System.Diagnostics;
using Jitter;
using Jitter.Collision;

namespace GameEngine2
{
    // We now have a rotating rectangle but how can we make the view move based on the users input?
    // In this tutorial we will take a look at how you could implement a camera class
    // and start responding to user input.
    // You can move to the camera class to see a lot of the new code added.
    // Otherwise you can move to Load to see how the camera is initialized.

    // In reality, we can't move the camera but we actually move the rectangle.
    // This will explained more in depth in the web version, however it pretty much gives us the same result
    // as if the view itself was moved.
    public class Window : GameWindow
    {

        CollisionSystem collision;
        World world;

        public List<Shader> shaders = new List<Shader>();
        public List<Texture> textures = new List<Texture>();
        private List<CachedSound> sounds = new List<CachedSound>();

        

        private string datapath = "";

        // The view and projection matrices have been removed as we don't need them here anymore.
        // They can now be found in the new camera class.

        // We need an instance of the new camera class so it can manage the view and projection matrix code.
        // We also need a boolean set to true to detect whether or not the mouse has been moved for the first time.
        // Finally, we add the last position of the mouse so we can calculate the mouse offset easily.

        public Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private double _time;

        private Prog main;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            collision = new CollisionSystemSAP();
            world = new World(collision);
            world.Gravity = new Jitter.LinearMath.JVector(0, -0.05f, 0);
            main = new Prog();

            base.OnLoad();

            string[] lines = System.IO.File.ReadAllLines(@"config.cfg");
            datapath = lines[0];
            //Load Shaders

            //index of end
            int end_shader = Array.FindIndex(lines,s => s.Equals("end shader"));

            string temp_vert = "";

            for (int i = 1; i<end_shader; i++)
            {
                if(i%2 == 1)
                {
                    temp_vert = lines[i];
                }
                else
                {
                    Trace.WriteLine(lines[i]);
                    shaders.Add(new Shader(temp_vert, lines[i]));
                }
            }

            // Load textures
            int end_texture = Array.FindIndex(lines, s => s.Equals("end texture"));

            for (int i = end_shader+1; i < end_texture; i++)
            {
                Debug.WriteLine(lines[i]);
                textures.Add(Texture.LoadFromFile(lines[i]));
            }

            int end_audio = Array.FindIndex(lines, s => s.Equals("end audio"));
            for (int i = end_texture + 1; i < end_audio; i++)
            {
                sounds.Add(new CachedSound(lines[i]));
            }

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            main.Load(datapath, world, _camera, shaders, textures);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            main.Render(e);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            world.Step(1.0f / 100.0f, true);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;


            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * Prog.sensitivity;
                _camera.Pitch -= deltaY * Prog.sensitivity; // Reversed since y-coordinates range from bottom to top
            }

            main.Update(e,input,mouse,_camera);
        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}