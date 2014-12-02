// --------------------------------------
//  Unity Foundation
//  ExportAttribute.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Decorates ScriptableObject or CLR Services with a default ctor.
    /// //
    /// Loads the service into memory on application initialization. 
    /// For ScriptableObject, assign the Resource File Name. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class InjectorService : Attribute
    {
        /// <summary>
        /// aborts automatic loading into memory
        /// </summary>
        public bool AbortLoad { get; private set; }

        /// <summary>
        /// Location / File name of the ScriptableObject resource
        /// </summary>
        public string ResourceName { get; private set; }


        public InjectorService()
        {

        }

        /// <summary>
        /// With resource name. IE:
        /// MyService or /Services/MyService
        /// </summary>
        /// <param name="resourceName"></param>
        public InjectorService(string resourceName)
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// With resource name. IE:
        /// MyService or /Services/MyService
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="abortLoad"></param>
        public InjectorService(string resourceName, bool abortLoad)
        {
            ResourceName = resourceName;
            AbortLoad = abortLoad;
        }
        
        /// <summary>
        /// Has the LoadScriptableObjects been called ?
        /// </summary>
        public static bool IsLoaded { get; private set; }

        /// <summary>
        /// Editor creation is running
        /// </summary>
        public static bool IsLoading { get; set; }

        /// <summary>
        /// All Object Types that are considered services
        /// </summary>
        public static Type[] GetServiceTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(o => o.HasAttribute<InjectorService>())).ToArray();
        }

        /// <summary>
        /// Loads all services into memory
        /// </summary>
        public static void LoadServices()
        {
            if (IsLoaded)
            {
                return;
            }

            if (IsLoading)
            {
                return;
            }

            IsLoaded = true;
            var types = GetServiceTypes();

            foreach (var type in types)
            {
                //Already Loaded
                if(Injector.GetFirst(type) != null)
                    continue;

                if (typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    var deco = type.GetAttribute<InjectorService>();

                    if (deco.AbortLoad)
                        continue;

                    // note Object has the responsibility of exporting itself to the injector
                    var resource = Resources.Load(deco.ResourceName);

                    if (resource == null)
                    {
                        Debug.LogWarning("Resource " + deco.ResourceName + " is not found");
                        Debug.LogWarning("Run Window/Foundation Create AppServices");
                    }
                    else
                    {
                        Injector.AddExport(resource);
                    }
                }
                else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                {
                    Debug.LogError(string.Format("Service {0} should not inherit from UnityEngine.Object", type));
                }
                else
                {
                    // CLR
                    try
                    {
                        // check for singleton variable
                        var type1 = type;
                        var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(o=> o.PropertyType == type1).ToArray();
                        if (props.Any())
                        {
                            // call it
                            var resource = props.First().GetValue(null, null);
                            Injector.AddExport(resource);
                        }
                        else
                        {

                            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public).Where(o => o.FieldType == type1).ToArray();
                            if (fields.Any())
                            {
                                // call it
                                var resource = fields.First().GetValue(null);
                                Injector.AddExport(resource);
                            }
                            else
                            {
                                Debug.LogWarning(string.Format("Service {0} should have a Singleton Instance Property", type));
                                // note Object has the responsibility of exporting itself to the injector
                                var resource = Activator.CreateInstance(type);
                                Injector.AddExport(resource);
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to create instance of " + type);
                        Debug.LogError(ex);
                    }
                }
            }
        }
    }
}