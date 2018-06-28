using System;
using System.Collections.Generic;
using System.Linq;

namespace NnackDI
{
	public static class Container 
	{
        private static Dictionary<Type, object> _commonDictionary = new Dictionary<Type, object>();

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
            foreach (var parameter in mostBiggerConstructor.GetParameters())
            {
                constrTypes.Add(parameter.ParameterType);
            }

            var ss = constrTypes.Select(item => typeof(Container).GetMethod("ResolveType").MakeGenericMethod(item).Invoke(this, null)).ToArray();
            var result = (TType)mostBiggerConstructor.Invoke(constrTypes.Select(item => typeof(Container).GetMethod("ResolveType").MakeGenericMethod(item).Invoke(this, null)).ToArray());

            return result;
        }

        public static TType ResolveInstance<TType>()
        {
            var instance = _commonDictionary[typeof(TType)];
            return (TType)instance;
        }
	}
}