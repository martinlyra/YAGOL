using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YAGOL
{
    class ShaderProgram : IDisposable
    {
        public int Handle { get; set; }

        public SubShader Vertex { get; private set; }
        public SubShader Fragment { get; private set; }

        public List<ShaderAttributeInfo> Attributes { get; private set; }
        public List<ShaderUniformInfo> Uniforms { get; private set; }

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

            PostProcessing();
        }

        void PostProcessing()
        {
            Attributes = new List<ShaderAttributeInfo>();
            Uniforms = new List<ShaderUniformInfo>();

            GetParameter(GetProgramParameterName.ActiveUniforms, out int uniformCount);

            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(
                    Handle,
                    i,
                    byte.MaxValue,
                    out int length,
                    out int size,
                    out ActiveUniformType type,
                    out string name);

                Uniforms.Add(new ShaderUniformInfo(this, name, type, size, i));
            }

            GetParameter(GetProgramParameterName.ActiveAttributes, out int attributeCount);

            for (int i = 0; i < attributeCount; i++)
            {
                GL.GetActiveAttrib(
                    Handle,
                    i,
                    byte.MaxValue,
                    out int length,
                    out int size,
                    out ActiveAttribType type,
                    out string name);

                Attributes.Add(new ShaderAttributeInfo(this, name, type, size, i));
            }
        }

        void Attach(SubShader shader)
        {
            GL.AttachShader(Handle, shader.Handle);
        }

        public void GetParameter(GetProgramParameterName parameterName, out int output)
        {
            GL.GetProgram(Handle, parameterName, out output);
        }

        public void SetAttribute (string name, params double[] value)
        {
            var attribute = Attributes.Find((attrib) => attrib.Name == name);

            if (attribute != null)
            {
                switch (attribute.Type)
                {
                    case ActiveAttribType.Double:
                    case ActiveAttribType.Float:
                    case ActiveAttribType.Int:
                    case ActiveAttribType.UnsignedInt:
                        GL.VertexAttrib1(attribute.Location, value[0]); break;
                    case ActiveAttribType.DoubleVec2:
                    case ActiveAttribType.FloatVec2:
                    case ActiveAttribType.IntVec2:
                    case ActiveAttribType.UnsignedIntVec2:
                        GL.VertexAttrib2(attribute.Location, value[0], value[1]); break;
                    case ActiveAttribType.DoubleVec3:
                    case ActiveAttribType.FloatVec3:
                    case ActiveAttribType.IntVec3:
                    case ActiveAttribType.UnsignedIntVec3:
                        GL.VertexAttrib3(attribute.Location, value[0], value[1], value[2]); break;
                    case ActiveAttribType.DoubleVec4:
                    case ActiveAttribType.FloatVec4:
                    case ActiveAttribType.IntVec4:
                    case ActiveAttribType.UnsignedIntVec4:
                        GL.VertexAttrib4(attribute.Location, value[0], value[1], value[2], value[3]); break;
                    default:
                        break;
                }
            }
        }

        public void SetUniform (string name, params double[] value)
        {
            var uniform = Uniforms.Find((unfrm) => unfrm.Name == name);

            if (uniform != null)
            {
                switch (uniform.Type)
                {
                    case ActiveUniformType.Double:
                    case ActiveUniformType.Float:
                    case ActiveUniformType.Int:
                    case ActiveUniformType.UnsignedInt:
                        GL.VertexAttrib1(uniform.Location, value[0]); break;
                    case ActiveUniformType.DoubleVec2:
                    case ActiveUniformType.FloatVec2:
                    case ActiveUniformType.IntVec2:
                    case ActiveUniformType.UnsignedIntVec2:
                        GL.VertexAttrib2(uniform.Location, value[0], value[1]); break;
                    case ActiveUniformType.DoubleVec3:
                    case ActiveUniformType.FloatVec3:
                    case ActiveUniformType.IntVec3:
                    case ActiveUniformType.UnsignedIntVec3:
                        GL.VertexAttrib3(uniform.Location, value[0], value[1], value[2]); break;
                    case ActiveUniformType.DoubleVec4:
                    case ActiveUniformType.FloatVec4:
                    case ActiveUniformType.IntVec4:
                    case ActiveUniformType.UnsignedIntVec4:
                        GL.VertexAttrib4(uniform.Location, value[0], value[1], value[2], value[3]); break;
                    default:
                        break;
                }
            }
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
