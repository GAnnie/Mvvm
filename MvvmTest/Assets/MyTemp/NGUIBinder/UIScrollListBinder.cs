using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Core.Databinding
{
	/// <summary>
	/// Presents an array, observable collection or other IEnumerable visually.
	/// </summary>
	[AddComponentMenu("Foundation/Databinding/UIScrollListBinder")]
	public class UIScrollListBinder : BindingBase
	{
		/// <summary>
        /// Item Prefab
        /// </summary>
        public GameObject Prefab;

        /// <summary>
        /// Shown when loading
        /// </summary>
        public GameObject LoadingMask;

        /// <summary>
        /// True if this is a static list.
        /// The list will only update once. 
        /// </summary>
        public bool OneTime;

        [HideInInspector]
        public BindingInfo SourceBinding = new BindingInfo { BindingName = "DataSource" };

        protected IObservableCollection DataList;
        protected List<BindingContext> Children = new List<BindingContext>();
        protected bool IsLoaded;
        protected bool IsInit;
        protected int Index = 0;
		protected GameObject CacheGo;
		protected UIGrid Grid;
		protected UITable Table;

        void Awake()
        {
			CacheGo = this.gameObject;
			Grid = this.GetComponent<UIGrid>();
			Table = this.GetComponent<UITable>();

            if (Prefab)
            {
                if (Prefab.GetComponent<BindingContext>() == null)
                {
                    Debug.LogError("template item must have an Root.");
                    enabled = false;
                }
            }

            if (LoadingMask)
                LoadingMask.SetActive(false);

            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            SourceBinding.Action = UpdateSource;
            SourceBinding.Filters = BindingFilter.Properties;
            SourceBinding.FilterTypes = new[] { typeof(IEnumerable) };
        }

        protected void UpdateSource(object value)
        {
            if (OneTime && IsLoaded)
                return;

            Bind(value);
        }

        /// <summary>
        /// Bind the ObservableCollection
        /// </summary>
        /// <param name="data"></param>
        public void Bind(object data)
        {
			//绑定前先执行清空操作
            if (DataList != null)
            {
                DataList.OnObjectAdd -= OnAdd;
                DataList.OnClear -= OnClear;
                DataList.OnObjectRemove -= OnRemove;
            }

            DataList = null;
            OnClear();
            StopAllCoroutines();

            if (data is IObservableCollection)
            {
                DataList = data as IObservableCollection;

                StartCoroutine(AddAsync(DataList.GetObjects()));

                DataList.OnObjectAdd += OnAdd;
                DataList.OnClear += OnClear;
                DataList.OnObjectRemove += OnRemove;

                IsLoaded = true;
            }
            else if (data is IEnumerable)
            {
				//普通集合类型没有添加任何事件监听，所以一旦绑定了
                var a = data as IEnumerable;

                StartCoroutine(AddAsync(a.Cast<object>()));
                IsLoaded = true;
            }
        }
		
		#region IObservableCollection Event Handler
        void OnClear()
        {
            foreach (var item in Children)
            {
                item.DataInstance = null;

                Recycle(item.gameObject);
            }

            Children.Clear();

            IsLoaded = false;
        }

        void OnRemove(object obj)
        {
            var view = Children.FirstOrDefault(o => o.DataInstance == obj);

            if (view != null)
            {
                view.DataInstance = null;

                Children.Remove(view);

                Recycle(view.gameObject);
				
				Resposition();
            }
        }

        void OnAdd(object obj)
        {
            var view = NGUITools.AddChild(CacheGo,Prefab);

            var root = view.GetComponent<BindingContext>();

            root.DataInstance = obj;

            view.name = "_Item " + Index++;

            Children.Add(root);
			
			Resposition();
        }
		#endregion
		
		/// <summary>
		/// 选择异步实例化prefab，是为了避免同步实例化大量prefab时造成的顿卡
		/// </summary>
        IEnumerator AddAsync(IEnumerable<object> objects)
        {
            if (LoadingMask)
                LoadingMask.SetActive(true);

            foreach (var obj in objects)
            {
                yield return 1;
                OnAdd(obj);
            }

            if (LoadingMask)
                LoadingMask.SetActive(false);

            yield return 1;
        }
		
		/// <summary>
		/// 这一步可以做缓冲池处理，来避免大量销毁与实例化的操作
		/// </summary>
        void Recycle(GameObject instance)
        {
            Destroy(instance);
        }
		
		void Resposition()
		{
			if(Grid)
				Grid.repositionNow = true;
			
			if(Table)
				Table.repositionNow = true;
		}
	}
}
