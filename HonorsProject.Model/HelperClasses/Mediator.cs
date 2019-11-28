using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.HelperClasses
{
    public class Mediator
    {
        private static IDictionary<string, List<Action<object>>> mediatorDictionary = new Dictionary<string, List<Action<object>>>();

        static public void Register(string token, Action<object> callback)
        {
            if (!mediatorDictionary.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                mediatorDictionary.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in mediatorDictionary[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    mediatorDictionary[token].Add(callback);
            }
        }

        static public void Unregister(string token, Action<object> callback)
        {
            if (mediatorDictionary.ContainsKey(token))
                mediatorDictionary[token].Remove(callback);
        }

        static public void NotifyColleagues(string token, object args)
        {
            if (mediatorDictionary.ContainsKey(token))
                foreach (var callback in mediatorDictionary[token].ToArray())
                    callback(args);
        }
    }
}