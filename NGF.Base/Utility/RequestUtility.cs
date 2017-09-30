using System;
using System.Collections.Specialized;
using System.Web;

namespace NGF.Base.Utility
{
    public class RequestUtility
    {
        public static T GetHeader<T>(string key)
        {
            string value = GetHeader(key);

            return (!string.IsNullOrWhiteSpace(value)
                    ? ConvertValue<T>(value)
                    : default(T));
        }

        public static T GetQueryString<T>(string queryStringName)
        {
            string queryStringValue = GetQueryString(queryStringName);

            return (!string.IsNullOrWhiteSpace(queryStringValue)
                    ? ConvertValue<T>(queryStringValue)
                    : default(T));
        }

        public static T GetQueryString<T>(Uri uri, string queryStringName)
        {
            string queryStringValue = GetQueryString(uri, queryStringName);

            return (!string.IsNullOrWhiteSpace(queryStringValue)
                   ? ConvertValue<T>(queryStringValue)
                   : default(T));
        }

        private static string GetHeader(string key)
        {
            string value = HttpContext.Current.Request.Headers.Get(key);

            return
                (!string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : string.Empty);
        }

        private static string GetQueryString(string queryStringName)
        {
            string queryStringValue = HttpContext.Current.Request.QueryString[queryStringName];

            return
                (!string.IsNullOrWhiteSpace(queryStringValue)
                   && (queryStringValue.ToLower().Trim() != "undefined")
                ? queryStringValue.Trim()
                : string.Empty);
        }

        private static string GetQueryString(Uri uri, string queryStringName)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);
            string queryStringValue = queryString[queryStringName];

            return
                (!string.IsNullOrWhiteSpace(queryStringValue)
                   && (queryStringValue.ToLower().Trim() != "undefined")
                ? queryStringValue.Trim()
                : string.Empty);
        }

        private static TResult ConvertValue<TResult>(object value)
        {
            if (Convert.IsDBNull(value) || value == null) return default(TResult);
            object obj = ConvertValue(typeof(TResult), value);
            if (obj == null) return default(TResult);
            return (TResult)obj;
        }

        private static object ConvertValue(Type type, object value)
        {
            if (Convert.IsDBNull(value) || value == null) return null;
            Type type2 = value.GetType();
            if (type == type2) return value;
            if ((type == typeof(Guid) || type == typeof(Guid?)) && type2 == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(value.ToString())) return null;
                return new Guid(value.ToString());
            }
            if ((type == typeof(DateTime) || type == typeof(DateTime?)) && type2 == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(value.ToString())) return null;
                return Convert.ToDateTime(value);
            }
            if (type.IsEnum)
            {
                try
                {
                    return Enum.Parse(type, value.ToString(), true);
                }
                catch
                {
                    return Enum.ToObject(type, value);
                }
            }
            if (type == typeof(bool) || type == typeof(bool?))
            {
                bool tempbool = false;
                if (bool.TryParse(value.ToString(), out tempbool)) return tempbool;
                if (string.IsNullOrWhiteSpace(value.ToString())) return false;
                return true;
            }
            if (type.IsGenericType) type = type.GetGenericArguments()[0];
            return Convert.ChangeType(value, type);
        }
    }
}
