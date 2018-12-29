namespace Yudiz.Gaming.Engine
{
	[System.Serializable]
	public enum MenuExecutation
	{
		SelectionTap = 0,
		PitPoint = 1,
		Both = 2
	}

	[System.Serializable]
	public class Tags
	{
		public const string UNTAGGED = "Untagged";
		public const string RESPAWN = "Respawn";
		public const string FINISH = "Finish";
		public const string EDITOR_ONLY = "EditorOnly";
		public const string DEFAULT_CAM = "MainCamera";
		public const string PLAYER = "Player";
		public const string GAME_CONTROLLERR = "GameController";
		public const string GUI_CAM = "GUICamera";
		public const string CONTROLS = "Controls";
		public const string TWITTER = "twitter";
		public const string FACEBOOK = "facebook";
		public const string MAIL = "mail";
		public const string BACK = "back";
		public const string SHARECAM = "Sharecam";
	}

	[System.Serializable]
	public enum RotaionDirection
	{
		Forward = 0,
		Reverse = 1
	}

	[System.Serializable]
	public enum MessageBoxStyle
	{
		YesNo,
		Ok,
		OkCancel,
		YesOkNo
	}

	[System.Serializable]
	public enum MessageResult
	{
		Yes,
		No,
		Ok,
		Cancel
	}

	[System.Serializable]
	public enum MessageBoxType
	{
		Information,
		Warning,
		Error
	}

	[System.Serializable]
	public enum SearchOption
	{
		Equal = 0,
		NotEqual = 1,
		StartWith = 2,
		EndWith = 3,
		Contians = 4,
		NotContains = 5,
		NotStartWith = 6,
		NotEndWith = 7,
		Greater = 8,
		Less = 9,
		GreaterOrEqual = 10,
		LessOrEqual = 11,
		IN = 12
	}

	[System.Serializable]
	public enum ButtonType
	{
		Ray,
		Rect
	}
}