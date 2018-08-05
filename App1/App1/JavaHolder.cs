using System;

namespace App1
{
    class JavaHolder:Java.Lang.Object
    {
        public readonly object Instance;

        public JavaHolder(object instance)
        {
            Instance = instance;
        }
    }
}