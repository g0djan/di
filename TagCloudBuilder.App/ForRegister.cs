using System;

namespace TagCloudBuilder.App
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