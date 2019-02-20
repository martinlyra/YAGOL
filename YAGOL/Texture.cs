using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAGOL
{
    class Texture : IDisposable
    {
        public int Handle { get; private set; }
        public TextureTarget TextureTarget { get; private set; }

        public Texture(TextureTarget textureTarget)
        {
            TextureTarget = textureTarget;

            int o;
            GL.CreateTextures(textureTarget, 1, out o);
            Handle = o;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget, Handle);
        }

        public void SetParameter(TextureParameterName parameterName, params int[] value)
        {
            GL.TextureParameterI(Handle, parameterName, value);
        }

        public void Dispose()
        {
            if (Handle != 0)
                GL.DeleteTexture(Handle);
        }
    }
}
