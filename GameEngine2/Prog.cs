using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GameEngine2;
using Jitter;

public class Prog
{
    private readonly float[] _vertices =
    {
        // Positions          Normals              Texture coords
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

        0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
    };

    private readonly uint[] _indices = {};

    private Lamp lamp;

    public Sprite box;
    public Sprite box2;

    public const float cameraSpeed = 1.5f;
    public const float sensitivity = 0.2f;
    public Prog()
	{
    }
    public void Load(string datapath, World world, Camera _camera, List<Shader> shaders, List<Texture> textures)
    {
        lamp = new Lamp();
        lamp.lampPos = new Vector3(3, 3, 3);
        lamp.lampColor = new Vector3(1.0f, 1.0f, 1.0f);
        lamp.lamp_shader = shaders[1].Copy();
        lamp.lightPower = 10;
        lamp.vertices = _vertices;
        lamp.triangles = _indices;
        lamp.world = world;
        lamp.ambient = new Vector3(0.2f, 0.2f, 0.2f);
        lamp.diffuse = new Vector3(0.5f, 0.5f, 0.5f);
        lamp.specular = new Vector3(1.0f, 1.0f, 1.0f);

        box = new Sprite();
        box.isLamp = false;
        box.lamp = lamp;
        box.vertices = _vertices;
        box.triangles = _indices;
        box.center = Vector3.Zero;
        box.shader = shaders[0].Copy();
        box.objectColor = new Vector3(0.3f, 0.6f, 0.9f);
        box.world = world;
        box.gravity = false;
        box.material_data = new float[] { 1f, 0.5f, 0.31f, 1f, 0.5f, 0.31f, 0.5f, 0.5f, 0.5f, 32f };
        box.texture = textures[0];
        box.texture_unit = TextureUnit.Texture0;

        box2 = new Sprite();
        box2.isLamp = false;
        box2.lamp = lamp;
        box2.objPath = datapath + "test.obj";
        box2.center = Vector3.Zero;
        box2.shader = shaders[0].Copy();
        box2.objectColor = new Vector3(0.9f, 0.6f, 0.3f);
        box2.world = world;
        box2.initialPosition = new Vector3(0, 10, 0);
        box2.material_data = new float[] { 1f, 0.5f, 0.31f, 1f, 0.5f, 0.31f, 0.5f, 0.5f, 0.5f, 32f };
        box2.texture = textures[0];
        box2.texture_unit = TextureUnit.Texture1;

        // We initialize the camera so that it is 3 units back from where the rectangle is.
        // We also give it the proper aspect ratio.
        box.camera = _camera;
        box2.camera = _camera;
        lamp.camera = _camera;

        //Load all assets
        lamp.Load();
        box.Load();
        box2.Load();

        // Play an audio file specified in the config.cfg file
        //AudioPlaybackEngine.Instance.PlaySound(sounds[0]); // Broken cant access field because of protection level
    }

    public void Render(FrameEventArgs e)
    {
        lamp.Draw();
        box.Draw();
        box2.Draw();
    }

    public void Update(FrameEventArgs e, KeyboardState input, MouseState mouse, Camera _camera)
    {
        box.deltaTime = (float)e.Time;
        box2.deltaTime = (float)e.Time;
        lamp.lamp.deltaTime = (float)e.Time;

        if (input.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        if (input.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }
        if (input.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        // Let the lamp move to show differences in light intensity
        lamp.lamp.Move(new Vector3(-0.0001f, 0, 0));
    }
}
