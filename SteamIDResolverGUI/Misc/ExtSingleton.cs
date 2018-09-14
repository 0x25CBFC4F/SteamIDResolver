using System;
using System.Collections.Generic;

namespace SteamIDResolverGUI.Misc
{
    public class ExtSingleton
    {
        private static readonly Dictionary<Type, object> ObjectDictionary = new Dictionary<Type, object>();

        public static T GetInstance<T>()
        {
            var type = typeof(T);

            if (!ObjectDictionary.ContainsKey(type))
            {
                ObjectDictionary.Add(type, Activator.CreateInstance(type));
            }

            return (T)ObjectDictionary[type];
        }

        public static T CreateInstance<T>(params object[] arguments)
        {
            var type = typeof(T);

            if (!ObjectDictionary.ContainsKey(type))
            {
                ObjectDictionary.Add(type, Activator.CreateInstance(type, arguments));
            }

            return (T)ObjectDictionary[type];
        }
    }
}