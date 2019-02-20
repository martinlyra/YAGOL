using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace YAGOL
{
    class ShaderProgram : IDisposable
    {
        public int Handle { get; set; }

        public SubShader Vertex { get; private set; }
        public SubShader Fragment { get; private set; }

        public ShaderProgram(string vertexSourceName, string fragmentSourceName)
        {
            Vertex = new SubShader(ShaderType.VertexShader, 
                new StreamReader(vertexSourceName).ReadToEnd());
            Fragment = new SubShader(ShaderType.FragmentShader, 
                new StreamReader(fragmentSourceName).ReadToEnd());

            Handle = GL.CreateProgram();

            Attach(Vertex);
            Attach(Fragment);

            GL.LinkProgram(Handle);
            Use();
        }

        void Attach(SubShader shader)
        {
            GL.AttachShader(Handle, shader.Handle);
        }


        public void Use()
        {
            GL.UseProgram(Handle); 
        }

        public void Dispose()
        {
            if (Handle != 0)
                GL.DeleteProgram(Handle);
            Vertex.Dispose();
            Fragment.Dispose();
        }
    }
}
