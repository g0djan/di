using System;

namespace TagsCloudContainer
{
    class RegistringImplemetation
    {
        public string Name { get; }
        public Type Implementation { get; }

        public RegistringImplemetation(string name, Type implementation)
        {
            Name = name;
            Implementation = implementation;
        }
    }
}