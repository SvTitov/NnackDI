using System;
using System.Collections.Generic;
using System.Linq;

namespace NnackDI
{
	public static class Container 
	{
        private static Dictionary<Type, object> _commonDictionary = new Dictionary<Type, object>();
        private static Dictionary<Type, (string, object)[]> _paramsDictionary = new Dictionary<Type, (string, object)[]>();

        public static void RegistrationType<TType, RType>()
        {
            if (_commonDictionary.ContainsKey(typeof(TType)))
                return;

            _commonDictionary.Add(typeof(TType), typeof(RType));
        }

        public static void RegistrationInstance<TType>(TType instance)
        {
            if (_commonDictionary.ContainsKey(typeof(TType)))
                return;

            _commonDictionary.Add(typeof(TType), instance);
        }

        public static void RegistrationWithParameters<TType,RType>(params (string, object)[] values)
        {
            if (_commonDictionary.ContainsKey(typeof(TType)))
                return;

            _commonDictionary.Add(typeof(TType), typeof(RType));
            _paramsDictionary.Add(typeof(TType), values);
        }

        /// <summary>
        /// Resolve class by the most bigger constructor parameters count
        /// </summary>
        /// <typeparam name="TType">Resolved type</typeparam>
        /// <returns>Instance of TType</returns>
        public static TType ResolveType<TType> ()
        {
            var typeObject = _commonDictionary[typeof(TType)] as Type;
            var constructors = typeObject.GetConstructors();
            var mostBiggerConstructor = constructors.OrderByDescending(item => item.GetParameters().Length).First();

            List<Type> constrTypes = new List<Type>();
            List<object> values = new List<object>();
            bool isHaveAddedParams = _paramsDictionary.Where(item => item.Key == typeof(TType)).Any();
            foreach (var parameter in mostBiggerConstructor.GetParameters())
            {
                if (!isHaveAddedParams)
                    constrTypes.Add(parameter.ParameterType);
                else
                {
                    var paramsList = _paramsDictionary.First(item => item.Key == typeof(TType)).Value;
                    var value = paramsList.First(item => item.Item1 == parameter.Name).Item2;

                    values.Add(value);
                }
            }

            var result = (TType)mostBiggerConstructor
                .Invoke(isHaveAddedParams ? values.ToArray() : constrTypes.Select(item => typeof(Container).GetMethod("ResolveType").MakeGenericMethod(item).Invoke(item, null))
                .ToArray());
            return result;
        }

        public static TType ResolveInstance<TType>()
        {
            var instance = _commonDictionary[typeof(TType)];
            return (TType)instance;
        }
	}
}