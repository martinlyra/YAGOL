using OpenTK.Graphics.OpenGL;

namespace YAGOL
{
    class ShaderAttributeInfo
    {
        public ShaderProgram Program { get; private set; }
        public string Name { get; private set; }
        public ActiveAttribType Type { get; private set; }
        public int Size { get; private set; }
        public int Location { get; private set; }

        public ShaderAttributeInfo(ShaderProgram program, string name, ActiveAttribType type, int size, int location)
        {
            Program = program;
            Name = name;
            Type = type;
            Location = location;
        }
    }
}