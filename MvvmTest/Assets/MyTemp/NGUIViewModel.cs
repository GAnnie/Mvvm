using UnityEngine;
using System.Collections;
using Foundation.Core;
using System.Linq;

namespace NGUIDataBinding.Examples
{
	public class ScrollViewItem : ObservableObject
	{
		private string _name;
		public string Name{
			get{ return _name; }	
			set{
				if(_name == value)
					return;
				_name = value;
				NotifyProperty("Name",value);
			}
		}
		private Color _color;
		public Color Color {
			get { return _color; }
			set {
				if (_color == value)
					return;
				_color = value;
				NotifyProperty ("Color", value);
			}
		}
		
		[SerializeField]
		private bool _isSelected;
		public bool IsSelected {
			get { return _isSelected; }
			set {
				if (_isSelected == value)
					return;
				_isSelected = value;
				NotifyProperty ("IsSelected", value);
			}
		}
		
		public event System.Action<ScrollViewItem> OnSelect;
		
		public ScrollViewItem(string name,Color c,System.Action<ScrollViewItem> onselect)
		{
			_name = name;
			_color = c;
			OnSelect = onselect;
		}
		
		public void OnSelectItem()
		{
			IsSelected = true;
			if(OnSelect != null)
				OnSelect(this);
		}
		
		
	}
	
	public class NGUIViewModel : ObservableBehaviour
	{
		[SerializeField]
		private string _label1;

		public string Label1 {
			get { return _label1; }
			set {
				if (_label1 == value)
					return;
				_label1 = value;
				NotifyProperty ("Label1", value);
			}
		}

		[SerializeField]
		private string _myText;

		public string MyText {
			get { return _myText; }
			set {
				if (_myText == value)
					return;
				_myText = value;
				NotifyProperty ("MyText", value);
			}
		}
        
		[SerializeField]
		private bool _allToggle;

		public bool AllToggle {
			get { return _allToggle; }
			set {
				if (_allToggle == value)
					return;
				_allToggle = value;
				NotifyProperty ("AllToggle", value);
			}
		}
        
		[SerializeField]
		private int _myNumber;

		public int MyNumber {
			get { return _myNumber; }
			set {
				if (_myNumber == value)
					return;
				_myNumber = value;
				NotifyProperty ("MyNumber", value);
			}
		}

		[SerializeField]
		private float _slider1 = 1;

		public float Slider1 {
			get { return _slider1; }
			set {
				if (_slider1 == value)
					return;
				_slider1 = value;
				NotifyProperty ("Slider1", _slider1);

				_slider2 = 1 - value;
				NotifyProperty ("Slider2", _slider2);
			}
		}

		[SerializeField]
		private float _slider2;

		public float Slider2 {
			get { return _slider2; }
			set {
				if (_slider2 == value)
					return;
				_slider2 = value;
				NotifyProperty ("Slider2", _slider2);

				_slider1 = 1 - value;
				NotifyProperty ("Slider1", _slider1);
			}
		}
        
		[SerializeField]
		private Texture2D _myTexture1;
		public Texture2D MyTexture1 {
			get { return _myTexture1; }
			set {
				if (_myTexture1 == value)
					return;
				_myTexture1 = value;
				NotifyProperty ("MyTexture1", value);
			}
		}

		[SerializeField]
		private Texture2D _myTexture2;

		public Texture2D MyTexture2 {
			get { return _myTexture2; }
			set {
				if (_myTexture2 == value)
					return;
				_myTexture2 = value;
				NotifyProperty ("MyTexture2", value);
			}
		}
		
		public UIAtlas uiAtlas;
		[SerializeField]
		private string _spriteName;
		public string SpriteName{
			get{ return _spriteName; }	
			set{
				if(_spriteName == value)
					return;
				_spriteName = value;
				NotifyProperty("SpriteName",value);
			}
		}

		[SerializeField]
		private Color _myColor;

		public Color MyColor {
			get { return _myColor; }
			set {
				if (_myColor == value)
					return;
				_myColor = value;
				NotifyProperty ("MyColor", value);
			}
		}
	
		public ObservableCollection<ScrollViewItem> Items = new ObservableCollection<ScrollViewItem> ();
		
		protected override void Awake ()
		{
			base.Awake ();
			
			Items.Add (new ScrollViewItem("red",Color.red,Select));
			Items.Add (new ScrollViewItem("green",Color.green,Select));
			Items.Add (new ScrollViewItem("yellow",Color.yellow,Select));
			Items.Add (new ScrollViewItem("blue",Color.blue,Select));
		}
		
		public void Select (ScrollViewItem item)
		{
			foreach (var testItem in Items) {
				testItem.IsSelected = item == testItem;
			}
		}

		public void AddItem ()
		{
			Items.Add (new ScrollViewItem("magenta",Color.magenta,Select));
		}
		
		public void UpdateItem ()
		{
			ScrollViewItem item = Items.Random();
			item.Color = new Color(Random.value,Random.value,Random.value);
			item.Name = StringHelper.RandomString(5);
			item.IsSelected = Random.value >0.5f ? true:false;
		}
		
		public void RemoveItem ()
		{
			ScrollViewItem item = Items.Random();
			Items.Remove(item);
		}

		public IEnumerator ChangeLabelWithCoroutine ()
		{
			yield return 1;

			MyText = StringHelper.RandomString (10);
		}

		public void ChangeImage ()
		{
			MyColor = MyColor == Color.red ? Color.blue : Color.red;
			var arg = MyTexture1;
			MyTexture1 = MyTexture2;
			MyTexture2 = arg;
			
			UISpriteData spriteData = uiAtlas.spriteList.Random();
			SpriteName = spriteData.name;
		}

//		public void ChangeLanguage ()
//		{
//			var languages = LocalizationService.Instance.GetLanguages ();
//
//			LocalizationService.Instance.Language = languages.Where (o => o != LocalizationService.Instance.Language).Random ();
//		}
		
		public void SubmitText()
		{
			if(!string.IsNullOrEmpty(MyText))		
			{
				Label1 = MyText;
			}
			else
				Debug.LogError("MyText is null");
		}
		
		void OnGUI()
		{
			if(GUILayout.Button("UpdateMyText"))	
				MyText = "HelloWorld";
		}
	}
}
