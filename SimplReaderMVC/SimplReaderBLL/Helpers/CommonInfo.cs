using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace SimplReaderBLL.Helpers
{
    public static class CommonInfo
    {
        /// <summary>
        /// Size of the object reference.
        /// </summary>
        public static readonly int ObjectReferenceSizeBytes = 4;
        /// <summary>
        /// Size of the object memory overhead.
        /// </summary>
        public static readonly int ObjectHeaderOverhead = 8;
        /// <summary>
        /// Size of the string object in memory.
        /// </summary>
        public static readonly int StringObjectSize = 18;

        /// <summary>
        /// Document collection default directory.
        /// </summary>
        public static string CollectionsDirectory = @"d:\nerc\collections\";
        /// <summary>
        /// Resource files default directory.
        /// </summary>
        public static string ResourceDirectory = @"d:\nerc\resursi\";
        /// <summary>
        /// Executing application path.
        /// </summary>
        public static string AppPath;

        /// <summary>
        /// Flags for object inspection.
        /// </summary>
        public static readonly BindingFlags FlagsInspectObject;

        static CommonInfo()
        {
            ObjectReferenceSizeBytes = Marshal.SizeOf(typeof(IntPtr));
            if (ObjectReferenceSizeBytes == 8)	// 64 bit
            {
                ObjectHeaderOverhead = 16;
                StringObjectSize = 28;			// actually it is 30 because of the first_char member, but it is excluded from the char array.
            }
            else								//32 bit
            {
                ObjectHeaderOverhead = 8;
                StringObjectSize = 16;			// actually it is 18 because of the first_char member, but it is excluded from the char array.
            }

            FlagsInspectObject = BindingFlags.Default;
            FlagsInspectObject |= BindingFlags.DeclaredOnly;
            FlagsInspectObject |= BindingFlags.Instance;
            FlagsInspectObject |= BindingFlags.Public;
            FlagsInspectObject |= BindingFlags.NonPublic;
            FlagsInspectObject |= BindingFlags.GetField;
            var a = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            AppPath = Path.GetDirectoryName(a.Location);
        }

        /// <summary>
        /// Gets the size of the given value type.
        /// </summary>
        /// <param name="t">Type to get the size for.</param>
        /// <returns>Size of the given type.</returns>
        public static int GetValueTypeSize(Type t)
        {
            if (t == typeof(Boolean))
                return sizeof(Boolean);
            else if (t == typeof(Byte))
                return sizeof(Byte);
            else if (t == typeof(SByte))
                return sizeof(SByte);
            else if (t == typeof(Int16))
                return sizeof(Int16);
            else if (t == typeof(UInt16))
                return sizeof(UInt16);
            else if (t == typeof(Int32))
                return sizeof(Int32);
            else if (t == typeof(UInt32))
                return sizeof(UInt32);
            else if (t == typeof(Int64))
                return sizeof(Int64);
            else if (t == typeof(UInt64))
                return sizeof(UInt64);
            else if (t == typeof(Single))
                return sizeof(Single);
            else if (t == typeof(Double))
                return sizeof(Double);
            else if (t == typeof(Char))
                return sizeof(Char);
            //else if (t == typeof(IntPtr))
            //    return sizeof(IntPtr);
            //else if (t == typeof(UIntPtr))
            //    return sizeof(UIntPtr);
            else if (t == typeof(Decimal))
                return sizeof(Decimal);
            else if (t == typeof(DateTime))
                return SizeOf<DateTime>();
            //return Marshal.SizeOf(typeof(DateTime));
            else if (t == typeof(TimeSpan))
                return SizeOf<TimeSpan>();
            //return Marshal.SizeOf(typeof(TimeSpan));
				//else if (t == typeof(Color))
				//	 return SizeOf<Color>();
            //return Marshal.SizeOf(typeof(Color));
            else if (t.IsEnum)
                return sizeof(int);
            else
                //return Marshal.SizeOf(t);
                return 0;
        }
        public static int SizeOf<T>()
        {
            try
            {
                return System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            }
            catch (ArgumentException)
            {
                return System.Runtime.InteropServices.Marshal.SizeOf(new TypeSizeProxy<T>());
            }
        }

        /// <summary>
        /// Iterates the given type members and fields.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <returns>Type members and fields.</returns>
        public static IEnumerable<FieldInfo> IterateTypeFields(Type t)
        {
            while (t != typeof(object))
            {
                var fields = t.GetFields(FlagsInspectObject);
                foreach (var f in fields)
                    yield return f;
                t = t.BaseType;
            }
        }
        static long GetObjectSizeInternal(object o, ObjectReferenceCollection objects)
        {
            if (o == null)
                return 0;
            var t = o.GetType();
            if (t.IsValueType)
            {
                var n = GetValueTypeSize(t);
                if (n > 0)
                    return n;
                long sum = 0;
                var fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.GetField);
                if (fields.Length == 1 && fields[0].FieldType == t)
                    return Marshal.SizeOf(t);
                foreach (var f in fields)
                    sum += GetObjectSizeInternal(f.GetValue(o), objects);
                return sum;
            }
            else if (!objects.TryAdd(o))		// if the object reference is already accounted for then skip it
                return 0;
            else if (t.IsPointer)
                return CommonInfo.ObjectReferenceSizeBytes;
            else if (t.IsArray)
            {
                var array = (Array)o;
                long sum = GetObjectTypeSize(o);
                if (array.Length == 0)
                    return sum;
                var t2 = t.GetElementType();
                // check if the array holds reference values and add reference pointer
                int n = 0;
                if (!t2.IsValueType)
                    sum += array.Length * ObjectReferenceSizeBytes;
                else
                    n = GetValueTypeSize(t2);
                if (n > 0)							// array is of generic type and holds primitive value types
                    sum += array.Length * n;
                else								// it is not a primitive type
                {
                    var fields = IterateTypeFields(t2).ToList();
                    if (fields.All(x => x.FieldType.IsPrimitive))			// if all the elements of the struct are primitive types
                        sum += array.Length * fields.Sum(x => GetValueTypeSize(x.FieldType));
                    else													// iterate through the array element by element
                        foreach (var arrayObject in array)
                            sum += GetObjectSizeInternal(arrayObject, objects);
                }
                return sum;
            }
            else if (o is Delegate)
                return ObjectHeaderOverhead + ObjectReferenceSizeBytes;
            else if (o is string)
                return StringObjectSize + ((string)o).Length * 2;
            else
            {
                long sum = ObjectHeaderOverhead;
                foreach (var f in IterateTypeFields(t))
                {
                    if (!f.FieldType.IsValueType)
                        sum += ObjectReferenceSizeBytes;
                    var o2 = f.GetValue(o);
                    sum += GetObjectSizeInternal(o2, objects);
                }
                return sum;
            }
        }

        /// <summary>
        /// Gets the memory size of the given object.
        /// </summary>
        /// <param name="o">Object to inspect.</param>
        /// <returns>Memory size of the given object.</returns>
        public static long GetObjectSize(object o)
        {
            return GetObjectSizeInternal(o, new ObjectReferenceCollection());
        }

        /// <summary>
        /// Gets the memory size of the type of the given object.
        /// </summary>
        /// <param name="o">Object whose type to inspect.</param>
        /// <returns>Memory size of type of the given object.</returns>
        public static int GetObjectTypeSize(object o)
        {
            return GetTypeSize(o.GetType());
        }
        /// <summary>
        /// Gets the estimated memory size of the given type.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <returns>Estimated memory size of the given type.</returns>
        public static int GetTypeSize(Type t)
        {
            return GetTypeSize(t, true);
        }
        /// <summary>
        /// Gets the estimated memory size of the given type.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <param name="first">Indication if the given type is the top level type for inspection.</param>
        /// <returns>Estimated memory size of the given type.</returns>
        public static int GetTypeSize(Type t, bool first)
        {
            if (t == null)
                return 0;
            if (t.IsValueType)
            {
                int n = GetValueTypeSize(t);
                if (n > 0)
                    return n;
                var sum = 0;
                var fields = IterateTypeFields(t).ToList();
                if (fields.Count == 1 && fields[0].FieldType == t)
                    return Marshal.SizeOf(t);
                foreach (var f in fields)
                    sum += GetTypeSize(f.FieldType, false);
                return sum;
            }
            else if (!first)
                return ObjectReferenceSizeBytes;
            else if (t == typeof(string))
                return StringObjectSize;
            else
            {
                var sum = ObjectHeaderOverhead;
                if (t.IsArray)
                    sum += ObjectReferenceSizeBytes + sizeof(int);
                else
                {
                    foreach (var f in IterateTypeFields(t))
                        sum += GetTypeSize(f.FieldType, false);
                }
                return sum;
            }
        }
        /// <summary>
        /// Checks if the given type contains only primitive types.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <returns>True if the given type contains only primitive types, otherwise false.</returns>
        public static bool TypeContainsOnlyPrimitives(Type t)
        {
            return IterateTypeFields(t).All(x => x.FieldType.IsPrimitive);
        }
        /// <summary>
        /// Checks if the given type contains only value types.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <returns>True if the given type contains only value types, otherwise false.</returns>
        public static bool TypeContainsOnlyValueTypes(Type t)
        {
            return IterateTypeFields(t).All(x => x.FieldType.IsValueType);
        }
        /// <summary>
        /// Checks if the given type contains only value or string types.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <returns>True if the given type contains only value or string types, otherwise false.</returns>
        public static bool TypeContainsOnlyValueOrStringTypes(Type t)
        {
            return IterateTypeFields(t).All(x => x.FieldType.IsValueType || x.FieldType == typeof(string));
        }
        /// <summary>
        /// Checks if the given type contains only value or string types.
        /// </summary>
        /// <param name="t">Type to inspect.</param>
        /// <param name="containsStringTypes">Output parameter indicating if the given type contains a string type member.</param>
        /// <returns>True if the given type contains only value or string types, otherwise false.</returns>
        public static bool TypeContainsOnlyValueOrStringTypes(Type t, out bool containsStringTypes)
        {
            containsStringTypes = false;
            foreach (var x in IterateTypeFields(t))
                if (x.FieldType.IsValueType)
                    continue;
                else if (x.FieldType == typeof(string))
                    containsStringTypes = true;
                else
                    return false;
            return true;
        }
        /// <summary>
        /// Checks if the given object is of type that inherits from a given base type.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <param name="baseType">Base type to compare.</param>
        /// <returns>True if the given object is of type that inherits from a given base type, otherwise false.</returns>
        public static bool IsOfType(object obj, Type baseType)
        {
            if (obj == null)
                return false;
            var t = obj.GetType();
            do
            {
                if (t == baseType)
                    return true;
                t = t.BaseType;
            } while (t.BaseType != t);
            return false;
        }

        /// <summary>
        /// Prints the sizes of basic types to the console.
        /// </summary>
        public static void PrintBasicTypeSizes()
        {
            Console.WriteLine("Sizeof bool: {0}", CommonInfo.GetTypeSize(typeof(bool)));
            Console.WriteLine("Sizeof short: {0}", CommonInfo.GetTypeSize(typeof(short)));
            Console.WriteLine("Sizeof int: {0}", CommonInfo.GetTypeSize(typeof(int)));
            Console.WriteLine("Sizeof long: {0}", CommonInfo.GetTypeSize(typeof(long)));
            Console.WriteLine("Sizeof char: {0}", CommonInfo.GetTypeSize(typeof(char)));
            Console.WriteLine("Sizeof string: {0} bytes", CommonInfo.GetTypeSize(typeof(string)));
            Console.WriteLine("Sizeof Array: {0} bytes", CommonInfo.GetTypeSize(typeof(int[])));
            Console.WriteLine("Sizeof Dictionary<K,T>: {0} bytes", CommonInfo.GetTypeSize(typeof(Dictionary<int, int>), true));
            Console.WriteLine("Sizeof HashSet<T>: {0} bytes", CommonInfo.GetTypeSize(typeof(HashSet<int>)));
            Console.WriteLine("Sizeof LinkedList<T>: {0} bytes", CommonInfo.GetTypeSize(typeof(LinkedList<int>)));
            Console.WriteLine("Sizeof List<T>: {0} bytes", CommonInfo.GetTypeSize(typeof(List<int>)));
            Console.WriteLine("Sizeof Queue<T>: {0} bytes", CommonInfo.GetTypeSize(typeof(Queue<int>)));
            Console.WriteLine("Sizeof SortedDictionary<K,T>: {0} bytes", CommonInfo.GetTypeSize(typeof(SortedDictionary<int, int>)));
            Console.WriteLine("Sizeof SortedList<K,T>: {0} bytes", CommonInfo.GetTypeSize(typeof(SortedList<int, int>)));
            Console.WriteLine("Sizeof Stack<T>: {0} bytes", CommonInfo.GetTypeSize(typeof(Stack<int>)));
        }

        /// <summary>
        /// Sets the current thread and proces priority to highest value and the processor affinity to a single processor.
        /// </summary>
        public static void SetSingleThreadTestPriority()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            if (Environment.ProcessorCount > 1)
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1 << (Environment.ProcessorCount - 1));
        }

        /// <summary>
        /// Finds the specified collection file in the current or the default collection directory.
        /// <remarks>The file is first searched for in the current working directory and then in the default collection directory.</remarks>
        /// </summary>
        /// <param name="file">File to find.</param>
        /// <returns>The full path of the found file, or the same string as input if the file is not found or the input filename contains a fullpath.</returns>
        public static string FindCollectionFile(string file)
        {
            return FindFile(file, CollectionsDirectory);
        }
        /// <summary>
        /// Finds the specified resource file in the current or the default resource directory.
        /// <remarks>The file is first searched for in the current working directory and then in the default resource directory.</remarks>
        /// </summary>
        /// <param name="file">File to find.</param>
        /// <returns>The full path of the found file, or the same string as input if the file is not found or the input filename contains a fullpath.</returns>
        public static string FindResourceFile(string file)
        {
            return FindFile(file, ResourceDirectory);
        }
        static string FindFile(string file, string referenceDir)
        {
            if (Path.IsPathRooted(file))
                return file;
            var f = new FileInfo(file);
            if (f.Exists)
                return f.FullName;
            f = new FileInfo(Path.Combine(AppPath, file));
            if (f.Exists)
                return f.FullName;
            if (Directory.Exists(referenceDir))
            {
                f = new FileInfo(Path.Combine(referenceDir, file));
                if (f.Exists)
                    return f.FullName;
                var files = Directory.GetFiles(referenceDir, Path.GetFileName(file), SearchOption.AllDirectories);
                if (files.Length > 0)
                    return files[0];
            }
            return file;
        }

        public struct TypeSizeProxy<T>
        {
            public T PublicField;
        }
    }

    [DebuggerDisplay("Count: {list.Count}")]
    public class ObjectReferenceCollection
    {
        List<object> list;

        public ObjectReferenceCollection()
        {
            list = new List<object>();
        }

        public int Count { get { return list.Count; } }

        public bool Contains(object o)
        {
            return list.Exists(x => object.ReferenceEquals(x, o));
        }
        public void Add(object o)
        {
            list.Add(o);
        }
        public bool TryAdd(object o)
        {
            if (list.Exists(x => object.ReferenceEquals(x, o)))
                return false;
            list.Add(o);
            return true;
        }
        public void Clear()
        {
            list.Clear();
        }

        public override string ToString()
        {
            return string.Format("Count: {0}", list.Count);
        }
    }
}
