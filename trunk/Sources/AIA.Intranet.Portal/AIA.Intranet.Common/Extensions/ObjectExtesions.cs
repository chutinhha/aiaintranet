using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AIA.Intranet.Common.Helpers;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using System.Reflection.Emit;
using System.Reflection;

namespace AIA.Intranet.Common.Extensions
{
    public static class HashtableExtensions
    {
        public static Object ToAnonymos(this Dictionary<string, object> dictionary)
        {
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();

            //dictionary.Add("Name", "Truong Nguyen");
            //dictionary.Add("Birthday", new DateTime(1984, 4, 20));

            AssemblyName an = new AssemblyName();
            an.Name = "myAssembly";
            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyBuilder ab = currentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = ab.DefineDynamicModule("myModule");


            TypeBuilder tBuild = mb.DefineType("myType", TypeAttributes.Public);
            foreach (var key in dictionary.Keys)
            {

                //tBuild.DefineField(key, dictionary[key].GetType(),FieldAttributes.Public);
                //System.Type[] types = new System.Type[1]
                //types[0] = System.Type.GetType(to.type)
                var fieldBulder = tBuild.DefineField(key.ToLower(),
                                                       dictionary[key].GetType(),
                                                      FieldAttributes.Private);


                var propertyBuilder = tBuild.DefineProperty(key, PropertyAttributes.HasDefault, dictionary[key].GetType(), null);

                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;


                MethodBuilder custNameGetPropMthdBldr = tBuild.DefineMethod("get_" + key, getSetAttr, dictionary[key].GetType(), Type.EmptyTypes);

                ILGenerator custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

                custNameGetIL.Emit(OpCodes.Ldarg_0);
                custNameGetIL.Emit(OpCodes.Ldfld, fieldBulder);
                custNameGetIL.Emit(OpCodes.Ret);

                MethodBuilder custNameSetPropMthdBldr = tBuild.DefineMethod("set_" + key, getSetAttr, null, new Type[] { dictionary[key].GetType() });

                ILGenerator custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

                custNameSetIL.Emit(OpCodes.Ldarg_0);
                custNameSetIL.Emit(OpCodes.Ldarg_1);
                custNameSetIL.Emit(OpCodes.Stfld, fieldBulder);
                custNameSetIL.Emit(OpCodes.Ret);

                // Last, we must map the two methods created above to our PropertyBuilder to  
                // their corresponding behaviors, "get" and "set" respectively. 
                propertyBuilder.SetGetMethod(custNameGetPropMthdBldr);
                propertyBuilder.SetSetMethod(custNameSetPropMthdBldr);



            }
            Type theType = tBuild.CreateType();
            object o = Activator.CreateInstance(theType);

            Type aType = mb.GetType("myType");
            foreach (var item in dictionary)
            {
                o.SetProperty(item.Key, item.Value);
                
            }
            return o;

        }
        public static void SyncProperties(this object destination, object source)
        {
            var type = destination.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.SetProperty);

            foreach (var item in properties)
            {
                var data = source.GetPropertyValue(item.Name);
                destination.SetProperty(item.Name, data);
            }
        }

        public static bool IsCollection(object o)
        {
            return typeof(ICollection).IsAssignableFrom(o.GetType())
                || typeof(ICollection<>).IsAssignableFrom(o.GetType());
        }

        public static void PopulaResources(this object obj)
        {
            if (IsCollection(obj))
            {
                if (obj is IEnumerable)
                {
                    var ctype = obj.GetType();
                    if (ctype == typeof(List<string>))
                    {
                        //TODO - How ugly this code (.)(.)
                        List<string> arr = obj as List<string>;

                        for (int i = 0; i < arr.Count; i++)
                        {
                            string resource = arr[i];
                            resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                            if (resource.StartsWith("$Resources:"))
                            {
                                arr[i] = SPUtility.GetLocalizedString(resource, string.Empty, 1033);
                            }

                        }
                        
                    }
                    else
                    foreach (object o in (obj as IEnumerable))
                    {
                        o.PopulaResources();
                    }
                }
                else
                {
                    // reflect over item
                }
            }
            var type = obj.GetType();
            var properties = type.GetProperties();

            if (obj != null && obj is string)
            {
                string resource = obj.ToString();
                resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                if (resource.StartsWith("$Resources:"))
                {
                    obj = SPUtility.GetLocalizedString(resource, string.Empty, 1033);
                }
            }
            else
            {
                foreach (var item in properties)
                {
                    try
                    {

                        var data = obj.GetPropertyValue(item.Name);
                        if (IsCollection(data))
                        {
                            data.PopulaResources();
                        }
                        if (data != null && data is string)
                        {
                            string resource = data.ToString();
                            resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                            if (resource.StartsWith("$Resources:"))
                            {
                                obj.SetProperty(item.Name, SPUtility.GetLocalizedString(resource, string.Empty, 1033));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            
        }

        public static object GetPropertyValue(this object destination, string name)
        {
            var type = destination.GetType();
            var pi = type.GetProperty(name);
            object result = null;
            if (pi != null)
            {
                result = pi.GetValue(destination, null);
            }
            return result;
        }

        public static void UpdateWith(this Hashtable first, Hashtable second)
        {
            foreach (DictionaryEntry item in second)
            {
                first[item.Key] = item.Value;
            }
        }

        

         public static T FromHash<T>(this Hashtable obj )  
        {
            var type = typeof(T);
            T result =(T)Activator.CreateInstance(typeof(T), new object[] {  });

            foreach (var item in type.GetProperties())
            {
                object value = obj[item.Name];
                //if (item.GetType() == typeof(Boolean))
                //{
                //    bool boolValue = bool.Parse(value.ToString());

                //    item.SetValue(result, boolValue, null);
                //}
                //else
                {
                    item.SetValue(result, Convert.ChangeType(value, item.PropertyType), null);
                }

            }
            return result;
        }
        
    }

    public static class ObjectExtesions
    {
        public static T Clone<T>(this object obj )  
        {
            string xml = SerializationHelper.SerializeToXml<T>((T) obj);
            return SerializationHelper.DeserializeFromXml<T>(xml);
        }

        public static void SetProperty(this object obj, string property, object value)
        {

            var type = obj.GetType();
            var pi = type.GetProperty(property);
            if (pi != null)
            {
                Object objValue = Convert.ChangeType(value, pi.PropertyType);

                pi.SetValue(obj, objValue, null);
            }
        }

        public static Hashtable ToHashtable(this object obj)
        {
            Hashtable hs = new Hashtable();
            var type = obj.GetType();
            var progs = type.GetProperties();
            foreach (var item in progs)
            {
                hs.Add(item.Name, item.GetValue(obj,null));
            }
            return hs;

        }
    }
}
