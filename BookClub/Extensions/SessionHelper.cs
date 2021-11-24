using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Extensions
{
	public static class SessionHelper
	{
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static T GetObjectFromJsonAndDestroy<T>(this ISession session, string key)
        {
            var res = GetObjectFromJson<T>(session, key);
            session.Remove(key);
            return res;
        }
    }
}
