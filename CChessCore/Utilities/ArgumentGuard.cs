#region Header
////////////////////////////////////////////////////////////////////////////// 
//The MIT License (MIT)

//Copyright (c) 2013 Dirk Bretthauer

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;

using System.Globalization;

namespace CChessCore
{
    /// <summary>Guard to check null object - typically used for the parameters check</summary>
    public static class ArgumentGuard {
        /// <summary>Verify if object is null - throws an exception</summary>
        /// <param name="sourceObject"></param>
        /// <param name="objectName"></param>
        public static void NotNull(object sourceObject, string objectName) {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(objectName));
            if (sourceObject == null)
                throw new ArgumentNullException(string.IsNullOrEmpty(objectName) ? "<unknown object name>" : objectName);
        }

        /// <summary>Verify if enumerable is null or empty</summary>
        /// <typeparam name="T">enumerable type</typeparam>
        /// <param name="enumerable">source</param>
        /// <param name="objectName">name of the object for diagnostic</param>
        public static void NotNullOrEmptyEnumerable<T>(IEnumerable<T> enumerable, string objectName) {
            NotNull(enumerable, objectName);
            if (!enumerable.GetEnumerator().MoveNext())
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture,"Enumerable parameter object {0} should not be empty", objectName));
        }

        /// <summary>Verify if enumerable is null or empty or have more than one entry</summary>
        /// <typeparam name="T">enumerable type</typeparam>
        /// <param name="enumerable">source</param>
        /// <param name="objectName">name of the object for diagnostic</param>
        public static void NonSingleElementEnumerable<T>(IEnumerable<T> enumerable, string objectName) {
            NotNullOrEmptyEnumerable(enumerable, objectName);
            IEnumerator<T> enumerator = enumerable.GetEnumerator();
            enumerator.MoveNext();
            // ... we have the first element there - if we have at least one more - broken
            if (enumerator.MoveNext())
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture,"Enumerable {0} should contain exactly one element.", objectName));
        }

        /// <summary>Verify if the object from expected type</summary>
        /// <param name="sourceObject">object to verify</param>
        /// <param name="objectName">name of the object for diagnostic</param>
        /// <param name="expectedType">expected type</param>
        public static void NotNullAndType<T>(object sourceObject, string objectName) {
            NotNull(sourceObject, objectName);
            if (!typeof(T).IsAssignableFrom(sourceObject.GetType()))
                throw new InvalidCastException(string.Format(CultureInfo.CurrentUICulture,"Invalid object type {0}. Expected type {1} but was {2}",
                                                             objectName, typeof(T), sourceObject.GetType()));
        }

        /// <summary>Verify if string argument neither null nor empty</summary>
        /// <param name="source">string to verify</param>
        /// <param name="objectName">name of the parameter</param>
        public static void NotNullOrEmpty(string source, string objectName) {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(objectName));
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(string.IsNullOrEmpty(objectName) ? "<unknown object name>" : objectName);
        }

        /// <summary>Check if source value is in range</summary>
        /// <param name="source">value</param>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <param name="name">parameter name</param>
        public static void VerifyRange(uint source, uint min, uint max, string name) {
            if (source < min || source > max)
                throw new ArgumentOutOfRangeException(name, source, string.Format(CultureInfo.CurrentUICulture,"Must be in range [{0}, {1}].", min, max));
        }
        /// <summary>Check if source value is in range</summary>
        /// <param name="source">value</param>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <param name="name">parameter name</param>
        public static void VerifyRange(int source, int min, int max, string name)
        {
            if (source < min || source > max)
                throw new ArgumentOutOfRangeException(name, source, string.Format(CultureInfo.CurrentUICulture,"Must be in range [{0}, {1}].", min, max));
        }
        /// <summary>Check if source value is in range</summary>
        /// <param name="source">value</param>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <param name="name">parameter name</param>
        public static void VerifyRange(double source, double min, double max, string name) {
            if (source < min || source > max)
                throw new ArgumentOutOfRangeException(name, source, string.Format(CultureInfo.CurrentUICulture,"Must be in range [{0}, {1}].", min, max));
        }
        public static void VerifyDateTimeKindIsNotUnspecified(DateTime time, string parameterName){
            if(time.Kind == DateTimeKind.Unspecified)
                throw new ArgumentException("The DateTime object is unspecified", parameterName);
        }
    }
}