namespace Hukiry
{
    /// <summary>
    /// 普通类使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
	{
		private static T _instance = (default(T) == null) ? new T() : default(T);

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new T();
				}
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

	}

}


