using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace YAGOL
{
    class SubShader : IDisposable
    {
        public int Handle { get; private set; }
        public ShaderType Type { get; private set; }

        public int CompileStatus { get; private set; }
        public string Info { get; private set; }

        public SubShader(ShaderType type, string source)
        {
            string info;

            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, source);
            GL.CompileShader(Handle);
            GL.GetShaderInfoLog(Handle, out info);

            CompileStatus = GetParameter(ShaderParameter.CompileStatus);
            Info = info;

            if (CompileStatus != 1)
                throw new ApplicationException(info);
        }

        public int GetParameter(ShaderParameter parameter)
        {
            int o;
            GL.GetShader(Handle, parameter, out o);
            return o;
        }

        public void Dispose()
        {
            if (Handle != 0)
                GL.DeleteShader(Handle);
        }
    }
}
