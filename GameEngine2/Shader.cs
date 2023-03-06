using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

public class Shader
{
	public int Handle;
    private bool disposedValue = false;
    private readonly Dictionary<string, int> _uniformLocations;
    private string vertPath;
    private string fragPath;

    public Shader(string vertexPath, string fragmentPath)
	{
        vertPath = vertexPath; fragPath = fragmentPath;

        int VertexShader;
        int FragmentShader;
        // Get all the shader and fragment data from the files
        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        // Create a vertex shader and a fragment shader from the data
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        // Compile the shader data and check for errors

        GL.CompileShader(VertexShader);

        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success1);
        if (success1 == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }

        // Convert the vertex shader and fragment shader into a working shader program called Handle

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success2);
        if (success2 == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        _uniformLocations = new Dictionary<string, int>();

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(Handle, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }

        // Cleanup and destroy useless files
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    // Get location of shader dynamically
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }
    public void Use()
    {
        GL.UseProgram(Handle);
    }

    // Destroy the shader
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }
    ~Shader()
    {
        GL.DeleteProgram(Handle);
    }
    private static void CompileShader(int shader)
    {
        // Try to compile the shader
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    private static void LinkProgram(int program)
    {
        // We link the program
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    // Uniform setters
    // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
    // You use VBOs for vertex-related data, and uniforms for almost everything else.

    // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
    //     1. Bind the program you want to set the uniform on
    //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
    //     3. Use the appropriate GL.Uniform* function to set the uniform.

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        var uniformLocation = GL.GetUniformLocation(Handle, name);
        GL.UseProgram(Handle);
        GL.Uniform1(uniformLocation, data);
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        var uniformLocation = GL.GetUniformLocation(Handle, name);
        GL.UseProgram(Handle);
        GL.Uniform1(uniformLocation, data);
    }

    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        var uniformLocation = GL.GetUniformLocation(Handle, name);
        GL.UseProgram(Handle);
        GL.UniformMatrix4(uniformLocation, true, ref data);
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        var uniformLocation = GL.GetUniformLocation(Handle, name);
        GL.UseProgram(Handle);
        GL.Uniform3(uniformLocation, data);
    }
    public Shader Copy()
    {
        return new Shader(vertPath, fragPath);
    }
}
