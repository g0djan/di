using System;

namespace TagsCloudContainer
{
    class ForRegister
    {
        public string Name { get; }
        public Type Implementation { get; }

        public ForRegister(string name, Type implementation)
        {
            Name = name;
            Implementation = implementation;
        }
    }
}