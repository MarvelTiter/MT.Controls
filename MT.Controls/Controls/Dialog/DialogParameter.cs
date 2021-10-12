using System;
using System.Collections;
using System.Collections.Generic;

namespace MT.Controls
{
    public interface IDialogParameter
    {
        void Add(object value);
        void Add(string key, object value);
        T GetValue<T>(string key);
        T GetValue<T>();
    }
    public class DialogParameter : Dictionary<string, object>, IDialogParameter
    {
        const string DefaultKey = "DEFAULT_KEY";
        public void Add(object value)
        {
            if (ContainsKey(DefaultKey))
            {
                this[DefaultKey] = value;
            }
            else
            {
                Add(DefaultKey, value);
            }
        }
        public T GetValue<T>()
        {
            return GetValue<T>(DefaultKey);
        }
        public T GetValue<T>(string key)
        {
            if (TryGetValue(key, out object val))
            {
                return (T)val;
            }
            return default;
        }

       
    }
}
