#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of $projectname$.
//
//    $projectname$ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    $projectname$ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with $projectname$.  If not, see <http://www.gnu.org/licenses/>.
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