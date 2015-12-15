using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace VoterManager.Helpers
{
    /// <summary>
    /// Строитель запросов в адресной строке
    /// </summary>
    [Serializable]
    public class CustomQueryStringBuilder : NameValueCollection
    {
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        private readonly string _parameterName;

        public CustomQueryStringBuilder(NameValueCollection collection, string parameterName)
            : base(collection)
        {
            _parameterName = parameterName;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            if (base.AllKeys.Count() != 0) result.Append("?");
            foreach (string key in base.AllKeys)
            {
                string[] values = base.GetValues(key);
                if (values != null && values.Count() != 0)
                {
                    result.Append(key + "=" + values[0] + "&");
                }
            }
            string resultString = result.ToString();
            return resultString.EndsWith("&") ? resultString.Substring(0, resultString.Length - 1) : resultString;
        }

        public string GetQueryStringForParameter(string parameterValue)
        {
            if (base[_parameterName] != null)
                base[_parameterName] = parameterValue;
            else
                base.Add(_parameterName, parameterValue);

            return ToString();
        }
    }
}