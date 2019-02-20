using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace YAGOL
{
    class Yagol : GameWindow
    {
        Texture FrontTexture { get; set; }
        Texture BackTexture { get; set; }

        ShaderProgram GolShader { get; set; }
        ShaderProgram CopyShader { get; set; }

        int FrameBufferHandle { get; set; }

        void SwapTextures ()
        {
            var tmp = FrontTexture;
            FrontTexture = BackTexture;
            BackTexture = tmp;
        }

        Texture CreateTexture ()
        {
            Texture tex = new Texture(TextureTarget.Texture2D);

            tex.SetParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            tex.SetParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            tex.SetParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            tex.SetParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.BindTexture(tex.TextureTarget, tex.Handle);
            GL.TexImage2D(
                TextureTarget.Texture2D, 
                0, 
                PixelInternalFormat.Rgba, 
                Width, 
                Height, 
                0,
                PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                (IntPtr)null);

            return tex;
        }

        void SetState(byte[] newState)
        {
            var size = Width * Height;

            var rgbaState = new byte[size * 4];

            for (int i = 0; i < size; i++)
            {
                int j = i * 4;

                rgbaState[j] 
                    = rgbaState[j + 1] 
                    = rgbaState[j + 2]
                    = (newState[i] > 0 ? byte.MaxValue : byte.MinValue);

                rgbaState[j + 3] = byte.MaxValue;
            }

            GL.TextureSubImage2D(FrontTexture.Handle,
                0,
                0,
                0,
                Width,
                Height,
                PixelFormat.Rgba,
                PixelType.Byte,
                rgbaState);
        }

        void RandomizeState()
        {
            var size = Width * Height;

            var newState = new byte[size];
            var random = new Random();

            for (int i = 0; i < size; i++)
                newState[i] = (byte)random.Next(0, 1);

            SetState(newState);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FrontTexture = CreateTexture();
            BackTexture = CreateTexture();

            GolShader = new ShaderProgram("Quad_Vert.glsl", "Gol_State_Fragment.glsl");
            CopyShader = new ShaderProgram("Quad_Vert.glsl", "Gol_Copy_Fragment.glsl");

            RandomizeState();

            int handle;
            GL.CreateFramebuffers(1, out handle);
            FrameBufferHandle = handle;
        }

        protected override void OnUnload(EventArgs e)
        {
            FrontTexture.Dispose();
            BackTexture.Dispose();

            GolShader.Dispose();
            CopyShader.Dispose();

            GL.DeleteFramebuffer(FrameBufferHandle);

            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Width, Height);
            GL.BindTexture(TextureTarget.Texture2D, FrontTexture.Handle);

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferHandle);
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                BackTexture.Handle, 0);
            GL.Viewport(0, 0, Width, Height);
            GL.BindTexture(TextureTarget.Texture2D, FrontTexture.Handle);

            SwapTextures();

            base.OnUpdateFrame(e);
        }
    }
}
