// --------------------------------------
//  Unity Foundation
//  MonoBehaviourExt.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Linq;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// extension class for monobehaviour.
    /// </summary>
    public static class MonoBehaviourExt
    {
        #region find parent

        /// <summary>
        /// max number of hierarchy checks 32
        /// </summary>
        private const int MaxParentLevels = 32;

        /// <summary>
        /// Finds highest level transform from source in hierarchy.
        /// </summary>
        /// <param name="gameObject">source</param>
        /// <returns></returns>
        public static Transform GetTransformRoot(this MonoBehaviour gameObject)
        {
            var t = gameObject.transform;

            for (int i = 0; i < MaxParentLevels; i++)
            {
                var tran = t.transform;

                if (tran.parent == null)
                {
                    return tran;
                }

                t = t.parent;
            }

            return null;
        }
        #endregion

        #region find in parent
        /// <summary>
        ///  Looks for component at or above source in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">source</param>
        /// <param name="ignoreSelf">exclude self. If ignore instance if found locally.</param>
        /// <returns></returns>
        public static T GetComponenetInParent<T>(this MonoBehaviour gameObject, bool ignoreSelf) where T : Component
        {
            var t = gameObject.transform;

            if (ignoreSelf)
                t = t.parent;

            for (int i = 0; i < MaxParentLevels; i++)
            {
                if (t == null)
                    return null;
                
                var found = t.GetComponent<T>();
                
                if (found != null)
                {
                    return found;
                }

                if (t.parent == null)
                {
                    return null;
                }

                t = t.parent;

            }

            return null;
        }
        /// <summary>
        ///  Looks for component at or above source in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetComponenetInParent<T>(this MonoBehaviour gameObject) where T : Component
        {
            var t = gameObject.transform;

            for (int i = 0; i < MaxParentLevels; i++)
            {
                var found = t.GetComponent<T>();

                if (found != null)
                {
                    return found;
                }

                if (t.parent == null)
                {
                    return null;
                }

                t = t.parent;

            }

            return null;
        }

        /// <summary>
        ///  Looks for component at or above source in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">source</param>
        /// <returns></returns>
        public static T GetComponenetInParent<T>(this GameObject gameObject) where T : Component
        {
            var t = gameObject.transform;

            for (int i = 0; i < MaxParentLevels; i++)
            {
                var found = t.GetComponent<T>();

                if (found != null)
                {
                    return found;
                }

                if (t.parent == null)
                {
                    return null;
                }

                t = t.parent;

            }

            return null;
        }
        #endregion

        #region find by Name

        /// <summary>
        /// Looks for a components with the matching name at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject">source</param>
        /// <param name="name">name to look for</param>
        /// <returns></returns>
        public static GameObject[] FindChildrenByName(this MonoBehaviour gameObject, string name)
        {
            return gameObject.GetComponentsInChildren<Transform>().Where(o => o.name == name).Select(o => o.gameObject).ToArray();
        }

        /// <summary>
        /// Looks for a components with the matching name at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T">cast type as</typeparam>
        /// <param name="gameObject">source</param>
        /// <param name="name">name to look for</param>
        /// <returns></returns>
        public static T[] FindChildrenByName<T>(this MonoBehaviour gameObject, string name) where T : Component
        {
            return gameObject.GetComponentsInChildren<Transform>().Where(o => o.name == name).Select(o => o.GetComponent<T>()).ToArray();
        }

        /// <summary> 
        /// Looks for a component with the matching name at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindChildByName(this MonoBehaviour gameObject, string name)
        {
            return FindChildrenByName(gameObject, name).SingleOrDefault();
        }

        /// <summary> 
        /// Looks for a component with the matching name at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T">cast type as</typeparam>
        /// <param name="gameObject">source</param>
        /// <param name="name">name to look for</param>
        /// <returns></returns>
        public static T FindChildByName<T>(this MonoBehaviour gameObject, string name) where T : Component
        {
            return FindChildrenByName<T>(gameObject, name).SingleOrDefault();
        }

        #endregion

        #region find by Tag
        /// <summary>
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static GameObject[] FindChildrenWithTag(this MonoBehaviour gameObject, string tag, bool includeInactive)
        {
            return gameObject.GetComponentsInChildren<Transform>(includeInactive).Where(o => o.CompareTag(tag)).Select(o => o.gameObject).ToArray();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static T[] FindChildrenWithTag<T>(this MonoBehaviour gameObject, string tag, bool includeInactive) where T : Component
        {
            return gameObject.GetComponentsInChildren<Transform>(includeInactive).Where(o => o.CompareTag(tag)).Select(o => o.GetComponent<T>()).ToArray();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static GameObject FindChildWithTag(this MonoBehaviour gameObject, string tag, bool includeInactive)
        {
            return FindChildrenWithTag(gameObject, tag, includeInactive).SingleOrDefault();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static T FindChildWithTag<T>(this MonoBehaviour gameObject, string tag, bool includeInactive) where T : Component
        {
            return FindChildrenWithTag<T>(gameObject, tag, includeInactive).SingleOrDefault();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject[] FindChildrenWithTag(this MonoBehaviour gameObject, string tag)
        {
            return gameObject.GetComponentsInChildren<Transform>().Where(o => o.CompareTag(tag)).Select(o => o.gameObject).ToArray();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static T[] FindChildrenWithTag<T>(this MonoBehaviour gameObject, string tag) where T : Component
        {
            return gameObject.GetComponentsInChildren<Transform>().Where(o => o.CompareTag(tag)).Select(o => o.GetComponent<T>()).ToArray();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject FindChildWithTag(this MonoBehaviour gameObject, string tag)
        {
            return FindChildrenWithTag(gameObject, tag).SingleOrDefault();
        }

        /// <summary> 
        /// Looks for component with matching tag at source and below in hierarchy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static T FindChildWithTag<T>(this MonoBehaviour gameObject, string tag) where T : Component
        {
            return FindChildrenWithTag<T>(gameObject, tag).SingleOrDefault();
        }
        #endregion

        #region InstantiateChild

        /// <summary>
        /// Instantiates the prefab as a child with a matching local transform.
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="prefab"></param>
        public static GameObject InstantiateChild(this MonoBehaviour mono, GameObject prefab)
        {
            return InstantiateChild(mono, prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Instantiates the prefab as a child
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="prefab"></param>
        /// <param name="localposition"></param>
        /// <param name="localrotation"></param>
        public static GameObject InstantiateChild(this MonoBehaviour mono, GameObject prefab, Vector3 localposition, Quaternion localrotation)
        {
            var inst = UnityEngine.Object.Instantiate(prefab) as GameObject;

            if (inst != null)
            {
                inst.transform.parent = mono.transform;
                inst.transform.localPosition = localposition;
                inst.transform.localRotation = localrotation;

            }

            return inst;
        }
        #endregion

        #region interfaces

        /// <summary>
        /// GetComponents for Interfaces. Returns all monobehaviours with the interface
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfaces<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            var mObjs = gObj.GetComponents<MonoBehaviour>();
            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
        }

        /// <summary>
        /// GetComponent for Interfaces. Returns monobehaviour with the interface
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterface<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            return gObj.GetInterfaces<T>().FirstOrDefault();
        }

        /// <summary>
        /// GetComponentsInChildren for Interfaces. Returns all monobehaviours in children with the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>    
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterfaceInChildren<T>(this MonoBehaviour gObj)
        {
            if (!typeof(T).IsInterface)
                throw new SystemException("Specified type is not an interface!");
            return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
        }

        /// <summary>
        /// GetComponentsInChildren for Interfaces. Returns all monobehaviours in children with the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfacesInChildren<T>(this MonoBehaviour gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>(true);
            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();

        }

        #endregion
    }
}