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

        static public void Register(string channel, Action<object> callback)
        {
            if (!mediatorDictionary.ContainsKey(channel))
            {
                //if new channel simply add it
                var list = new List<Action<object>>();
                list.Add(callback);
                mediatorDictionary.Add(channel, list);
            }
            else
            {
                bool found = false;
                //if channel with method call already exists don't add it again
                foreach (var item in mediatorDictionary[channel])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    mediatorDictionary[channel].Add(callback);
            }
        }

        static public void Unregister(string channel, Action<object> callback)
        {
            if (mediatorDictionary.ContainsKey(channel))
                mediatorDictionary[channel].Remove(callback);
        }

        static public void NotifyColleagues(string channel, object args)
        {
            if (mediatorDictionary.ContainsKey(channel))
                foreach (var callback in mediatorDictionary[channel].ToArray())
                    callback(args);
        }

        static public void ClearMediator()
        {
            mediatorDictionary = new Dictionary<string, List<Action<object>>>();
        }
    }
}