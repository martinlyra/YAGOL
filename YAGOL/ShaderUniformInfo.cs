using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAGOL
{
    class ShaderUniformInfo
    {
        public ShaderProgram Program { get; private set; }
        public string Name { get; private set; }
        public ActiveUniformType Type { get; private set; }
        public int Size { get; private set; }
        public int Location { get; private set; }

        public ShaderUniformInfo(ShaderProgram program, string name, ActiveUniformType type, int size, int location)
        {
            Program = program;
            Name = name;
            Type = type;
            Location = location;
        }
    }
}
