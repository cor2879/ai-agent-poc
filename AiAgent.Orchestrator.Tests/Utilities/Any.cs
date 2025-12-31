#pragma warning disable CS8602
namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Utilities
{
    public static class Any
    {
        private static readonly int Seed = (int)DateTime.Now.Ticks;
        private static readonly Random Random = new Random(Seed);

        #region String

        private const int MaxString = 3000;

        /// <summary>
        /// Gets a randomized string
        /// </summary>
        /// <returns>A randomized string</returns>
        public static string String()
        {
            var length = Int(1, MaxString);
            return String(length);
        }

        /// <summary>
        /// Gets a randomized string of randomized length
        /// </summary>
        /// <param name="minLen">The minimum length of the string</param>
        /// <param name="maxLen">The maximum length of the string</param>
        /// <returns>A randomized string</returns>
        public static string String(int minLen, int maxLen)
        {
            var length = Int(minLen, maxLen);
            return String(length);
        }

        /// <summary>
        /// Gets a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A randomized string</returns>
        public static string String(int length)
        {
            var buffer = new char[length];
            for (var position = 0; position < length; position++)
            {
                buffer[position] = Ascii();
            }
            return new string(buffer);
        }

        /// <summary>
        /// Gets a random numeric string.
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random numeric string</returns>
        public static string NumericString(int length)
        {
            var buffer = new char[length];

            for (var position = 0; position < length; position++)
            {
                buffer[position] = Any.Numeric();
            }

            return new string(buffer);
        }

        /// <summary>
        /// Gets a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <param name="first">The minimum character value to allow in the string</param>
        /// <param name="last">The maximum character value to allow in the string</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string String(int length, char first, char last)
        {
            if (first > last)
            {
                throw new ArgumentOutOfRangeException();
            }
            var buffer = new char[length];
            for (var position = 0; position < length; position++)
            {
                buffer[position] = Char(first, last);
            }
            return new string(buffer);
        }

        /// <summary>
        /// Generates a random 'name' according to the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns>An alphanumeric string of 'length' characters</returns>
        public static string Name(int length)
        {
            var buffer = new char[length];
            for (var position = 0; position < length; position++)
            {
                if (position == 0)
                {
                    buffer[position] = Alpha();
                }
                else
                {
                    buffer[position] = Alpha_Numeric();
                }
            }
            return new string(buffer);
        }

        #endregion

        #region Char

        private const int SmallestChar = 32;
        private const int LargestChar = 250;

        /// <summary>
        /// Gets a random ASCII compliant char
        /// </summary>
        /// <returns>A random ASCII compliant char</returns>
        public static char Ascii()
        {
            return (char)Int(SmallestChar, LargestChar);
        }

        /// <summary>
        /// Gets a random char
        /// </summary>
        /// <param name="first">The minimum bound of the char</param>
        /// <param name="last">The maximum bound of the char</param>
        /// <returns>A random char</returns>
        public static char Char(char first, char last)
        {
            return (char)Int(first, last);
        }

        /// <summary>
        /// Gets a random char
        /// </summary>
        /// <returns>A random char</returns>
        public static char Char()
        {
            return (char)PositiveInt(255);
        }

        private const string CharsFrom0ToA = @":;<=>?@";
        private const string CharsFromZtoA = @"[\]^`";
        private const string CharsFrom0ToZ = CharsFrom0ToA + CharsFromZtoA;

        /// <summary>
        /// Returns a random alphanumeric char
        /// </summary>
        /// <returns>a random alphanumeric char</returns>
        public static char Alpha_Numeric()
        {
            char value;
            do
            {
                value = Char('0', 'z');
            } while (CharsFrom0ToZ.IndexOf(value) != -1);
            return value;
        }

        /// <summary>
        /// Returns a random Alpha char
        /// </summary>
        /// <returns>A random Alpha char</returns>
        public static char Alpha()
        {
            char value;
            do
            {
                value = Char('A', 'z');
            } while (CharsFromZtoA.IndexOf(value) != -1);
            return value;
        }

        public static char Numeric()
        {
            return (char)Any.Int('0', '9');
        }

        #endregion

        #region Byte

        /// <summary>
        /// Returns a random byte value
        /// </summary>
        /// <returns>a random byte value</returns>
        public static byte Byte()
        {
            return (byte)Int(byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// Returns a random Signed Byte value
        /// </summary>
        /// <returns>A random signed byte value</returns>
        public static sbyte SByte()
        {
            return (sbyte)Int(sbyte.MinValue, sbyte.MinValue);
        }

        #endregion

        /// <summary>
        /// Returns a random Decimal value
        /// </summary>
        /// <returns>A random Decimal value</returns>
        public static decimal Decimal()
        {
            var double0To1 = Random.NextDouble();
            var randomDouble = double0To1 * (double)decimal.MaxValue + (1 - double0To1) * (double)decimal.MinValue;
            return (decimal)randomDouble;
        }

        /// <summary>
        /// Returns a random 16-bit integer
        /// </summary>
        /// <returns>A random 16-bit integer</returns>
        public static short Short()
        {
            return (short)Int(short.MinValue, short.MaxValue);
        }

        /// <summary>
        /// Returns a random 16-bit unsigned integer
        /// </summary>
        /// <returns>A random unsigned 16-bit integer</returns>
        public static ushort UShort()
        {
            return (ushort)Short();
        }

        /// <summary>
        /// Returns a random 32-bit Unsigned integer
        /// </summary>
        /// <returns>A random 32-bit unsigned integer</returns>
        public static uint UInt()
        {
            return (uint)Int();
        }

        /// <summary>
        /// Returns a random 64-bit Unsigned integer
        /// </summary>
        /// <returns>A random 64-bit unsigned integer</returns>
        public static ulong ULong()
        {
            return (ulong)Long();
        }

        /// <summary>
        /// Retuns a random 64-bit integer
        /// </summary>
        /// <returns>A random 64-bit integer</returns>
        public static long Long()
        {
            return Int() + ((long)Int() >> 32);
        }

        #region Int

        /// <summary>
        /// Returns an integer
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>Returns an integer</returns>
        public static int Int(int min = int.MinValue, int max = int.MaxValue)
        {
            var val = Random.Next(min, max);
            return val;
        }

        /// <summary>
        /// Returns a random positive integer
        /// </summary>
        /// <param name="max">The maximum value</param>
        /// <returns>A random positive integer</returns>
        public static int PositiveInt(int max = int.MaxValue)
        {
            return Int(1, max);
        }

        /// <summary>
        /// Returns an integer no less than the specified min value
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <returns>A 32 bit integer no less than the specified min value</returns>
        public static int IntAtLeast(int min)
        {
            return Int(min);
        }

        /// <summary>
        /// Returns an integer value no greater than the specified maximum
        /// </summary>
        /// <param name="max">The max value</param>
        /// <returns>An integer no greater than the specified max value</returns>
        public static int IntAtMost(int max)
        {
            return Int(int.MinValue, max - 1);
        }

        #endregion

        #region Float

        /// <summary>
        /// Returns a floating point value
        /// </summary>
        /// <param name="min">The minimum value to return</param>
        /// <param name="max">The maximum value to return</param>
        /// <returns>A floating point value</returns>
        public static float Float(float min = float.MinValue, float max = float.MaxValue)
        {
            return (float)Double(min, max);
        }

        /// <summary>
        /// Returns a random positive floating point value
        /// </summary>
        /// <returns>A random positive floating point value</returns>
        public static float PositiveFloat()
        {
            return Float(0);
        }

        /// <summary>
        /// Returns a random positive floating point value no greater than the specified maximum
        /// </summary>
        /// <param name="max">The maximum value</param>
        /// <returns>A random positive floating point value</returns>
        public static float PositiveFloat(float max)
        {
            return Float(0, max);
        }

        /// <summary>
        /// Returns a randomly generated Floating Point value
        /// </summary>
        /// <param name="min">The minimum floating point value to return</param>
        /// <returns>A floating point value that is no less than the specified minimum</returns>
        public static float FloatAtLeast(float min)
        {
            return Float(min);
        }

        /// <summary>
        /// Returns a randomly generated Floating Point value
        /// </summary>
        /// <param name="max">The maxium value</param>
        /// <returns>A randomly generated floating point value</returns>
        public static float FloatAtMost(float max)
        {
            return Float(float.MinValue, max);
        }

        #endregion

        #region Double

        /// <summary>
        /// Gets a randomly generated Double precision floating point value.
        /// </summary>
        /// <param name="min">The minimum value to generate</param>
        /// <param name="max">The maximum value to generate</param>
        /// <returns>A randomly generated floating point value</returns>
        public static double Double(double min = double.MinValue, double max = double.MaxValue)
        {
            var double0To1 = Random.NextDouble();
            var randomDouble = double0To1 * max + (1 - double0To1) * min;
            return randomDouble;
        }

        /// <summary>
        /// Gets a randomly generated positive Double precision floating point value.
        /// </summary>
        /// <returns></returns>
        public static double PositiveDouble()
        {
            return Double(0);
        }

        /// <summary>
        /// Gets a randomly generated positive Double precision floating point value.
        /// </summary>
        /// <param name="max">The maximum value</param>
        /// <returns>A randomly generated positive Double precision floating point value.</returns>
        public static double PositiveDouble(double max)
        {
            return Double(0, max);
        }

        #endregion

        #region Arrays

        /// <summary>
        /// An array of positive double precision floating point values
        /// </summary>
        /// <param name="numberOfElements">The number of elements to generate</param>
        /// <returns>An array of positive double precisions floating point values</returns>
        public static double[] PositiveDoubleArray(int numberOfElements)
        {
            return DoubleArray(numberOfElements, 0, double.MaxValue);
        }

        /// <summary>
        /// Gets an array of double precision floating point values
        /// </summary>
        /// <param name="numberOfElements">The number of elements to generate</param>
        /// <param name="min">The minimum value of the elements</param>
        /// <param name="max">The maximum value of the elements</param>
        /// <returns>An array of double precision floating point values</returns>
        public static double[] DoubleArray(int numberOfElements, double min, double max)
        {
            var doubleArray = new double[numberOfElements];
            for (var element = 0; element < doubleArray.Length; element++)
            {
                doubleArray[element] = Double(min, max);
            }
            return doubleArray;
        }

        /// <summary>
        /// Gets an array of randomly generated positive floating point values
        /// </summary>
        /// <param name="numberOfElements">The number of elements to generate</param>
        /// <returns>An array of randomly generated positive floating point values</returns>
        public static float[] PositiveFloatArray(int numberOfElements)
        {
            var positiveFloatArray = new float[numberOfElements];
            for (var element = 0; element < positiveFloatArray.Length; element++)
            {
                positiveFloatArray[element] = PositiveFloat();
            }
            return positiveFloatArray;
        }

        /// <summary>
        /// Gets an array of randomly generated floating point values
        /// </summary>
        /// <param name="numberOfElements">The number of values to generate</param>
        /// <param name="min">The minimum value to generate</param>
        /// <param name="max">The maximum value to generate</param>
        /// <returns>An array of randomly generated floating point values</returns>
        public static float[] RangeFloatArray(int numberOfElements, float min, float max)
        {
            var rangeFloatArray = new float[numberOfElements];
            for (var element = 0; element < rangeFloatArray.Length; element++)
            {
                rangeFloatArray[element] = Float(min, max);
            }
            return rangeFloatArray;
        }

        /// <summary>
        /// Gets an array of randomly generated floating point values
        /// </summary>
        /// <param name="numberOfElements">The number of values to generate</param>
        /// <returns>An array of randomly generated floating point values</returns>
        public static float[] FloatArray(int numberOfElements)
        {
            var floatArray = new float[numberOfElements];
            for (var element = 0; element < floatArray.Length; element++)
            {
                floatArray[element] = Float();
            }
            return floatArray;
        }

        /// <summary>
        /// Gets a random Char array
        /// </summary>
        /// <param name="numberOfElements">The number of elements to add to the array</param>
        /// <returns>An array of thyp char[]</returns>
        public static char[] CharArray(int numberOfElements)
        {
            var charArray = new char[numberOfElements];
            for (var element = 0; element < charArray.Length; element++)
            {
                charArray[element] = Char();
            }
            return charArray;
        }

        #endregion

        #region Bool

        /// <summary>
        /// Gets a random boolean value
        /// </summary>
        /// <returns></returns>
        public static bool Bool()
        {
            return Int(0, 1) == 1;
        }

        /// <summary>
        /// Creates an array of names
        /// </summary>
        /// <param name="numElements">The number of names to generate</param>
        /// <param name="maxLength">The maximum length of the names</param>
        /// <returns>An array of randomly generated names.</returns>
        public static string[] NameArray(int numElements, int maxLength)
        {
            var strings = new string[numElements];
            for (var i = 0; i < numElements; i++)
            {
                strings[i] = Name(maxLength);
            }
            return strings;
        }

        #endregion

        /// <summary>
        /// Gets an enum of type T
        /// </summary>
        /// <typeparam name="T">The Type of the Enum</typeparam>
        /// <returns>A randomly selected enum of type T</returns>
        public static T OneOfEnum<T>()
        {
            var enumerationValues = Enum.GetValues(typeof(T));
            var selectedValue = Int(0, enumerationValues.Length - 1);

            return (T)(enumerationValues.GetValue(selectedValue) ?? null!);
        }

        /// <summary>
        /// Returns any Enum of the specified type except that specified
        /// </summary>
        /// <typeparam name="T">The type of Enum</typeparam>
        /// <param name="undesiredEnumerationValue">The Enumeration value to block</param>
        /// <returns>An enumeration of type T</returns>
        public static T OneOfEnumExcept<T>(T undesiredEnumerationValue)
        {
            T enumerationValue;
            do
            {
                enumerationValue = OneOfEnum<T>();
            } while (enumerationValue.Equals(undesiredEnumerationValue));
            return enumerationValue;
        }

        /// <summary>
        /// Returns a random element from a list of items
        /// </summary>
        /// <typeparam name="T">The type of the Collection</typeparam>
        /// <param name="collection">The Collection</param>
        /// <returns>A randomly selected element from the collection, or a default value if the collection is empty.</returns>
        public static T OneFromCollection<T>(IList<T> collection)
        {
            if (!collection.Any())
            {
#pragma warning disable CS8603
                return default;
#pragma warning restore
            }

            return collection.ElementAt(Any.Int(0, collection.Count - 1));
        }
    }
}
