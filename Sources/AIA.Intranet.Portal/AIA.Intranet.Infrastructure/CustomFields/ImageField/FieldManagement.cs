using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.SharePoint;
using System.Reflection;
using System.ComponentModel;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    /// <summary>
    /// Static method used to set/get/save custom field properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class FieldManagement<T>
    {

        /// <summary>
        /// Static dictionary used to store properties defined in the FieldEditor
        /// </summary>
        private static Dictionary<int, T> DicProperties = new Dictionary<int, T>();

        /// <summary>
        /// Add a custom property to an XDocument if this property does not exist, otherwise, change the value of it
        /// </summary>
        /// <param name="schemaXml">The XML</param>
        /// <param name="properyName">Property name</param>
        /// <param name="value">Property value</param>
        private static void AddCustomProperties(XDocument schemaXml, string properyName, string value)
        {
            // Get the attribute
            XAttribute attribute = schemaXml.Root.Attribute(properyName);

            if (value != null)
            {
                // If the attribute does not exist in schemaXml, add it
                if (attribute != null)
                {
                    schemaXml.Root.Attribute(properyName).Value = value;
                }
                // If the attribute exist in schemaXml, change its value
                else
                {
                    schemaXml.Root.Add(new XAttribute(properyName, value));
                }
            }
            else
            {
                // If the attribute does not exist in schemaXml, remove it
                if (attribute != null)
                {
                    schemaXml.Root.Attribute(properyName).Remove();
                }
            }
        }

        /// <summary>
        /// Change the field schema to save the custom properties to the field
        /// </summary>
        /// <param name="field">Field to edit</param>
        public static void SetFieldProperties(SPField field)
        {
            
            int key = SPContext.Current.GetHashCode();

            if (DicProperties.ContainsKey(key))
            {
                // Get the field schema
                XDocument schemaXml = XDocument.Parse(field.SchemaXml);
                T properties = DicProperties[key];

                // If the property is a value type or a string, save directly the custom property
                if (typeof(T).IsValueType || typeof(T) == typeof(string))
                {
                    AddCustomProperties(schemaXml, "CustomProperty", properties.ToString());
                }
                else
                {
                    // Get object properties
                    var objectProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    // Add each properties as a field attribute
                    foreach (var property in objectProperties)
                    {

                        string value = null;

                        // Get the property value
                        try { value = property.GetValue(properties, null).ToString(); }
                        catch { }

                        // Add the property as a field attribute
                        AddCustomProperties(schemaXml, property.Name, value);
                    }

                }

                DicProperties.Remove(key);

                // Update the field
                field.SchemaXml = schemaXml.ToString();
                field.Update();
            }

        }

        /// <summary>
        /// Get the custom properties of the field and make a T object with them
        /// </summary>
        /// <param name="field">Field containing custom properties</param>
        /// <returns>An object with properties values</returns>
        public static T GetFieldProperties(SPField field)
        {
            // If the field is not null
            if (field != null)
            {
                // Get the schema of the field
                XDocument schemaXml = XDocument.Parse(field.SchemaXml);

                // If the property is a value type or a string, return directly the custom property
                if (typeof(T).IsValueType || typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(schemaXml.Root.Attribute("CustomProperty").Value, typeof(T));
                }
                else
                {
                    // Create a T instance
                    T properties = Activator.CreateInstance<T>();

                    // Get all public properties of object
                    var objectProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // For each property, retrieves the attribute value
                    foreach (var property in objectProperties)
                    {
                        try
                        {
                            // Handle Nullable type
                            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter nullableConverter = new NullableConverter(property.PropertyType);
                                property.SetValue(properties, Convert.ChangeType(schemaXml.Root.Attribute(property.Name).Value, nullableConverter.UnderlyingType), null);
                            }
                            else
                            {
                                property.SetValue(properties, Convert.ChangeType(schemaXml.Root.Attribute(property.Name).Value, property.PropertyType), null);
                            }
                        }
                        catch { }
                    }

                    // The object that stores properties values
                    return properties;

                }
            }

            return default(T);

        }

        /// <summary>
        /// Save properties of the field
        /// </summary>
        /// <param name="properties">Properties to save</param>
        public static void SaveProperties(T properties)
        {
            DicProperties.Add(SPContext.Current.GetHashCode(), properties);
        }
 
    }
}
