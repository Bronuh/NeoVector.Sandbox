using Godot;

namespace AbroDraft.Scripts.Screen;

public partial class ProfileButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			/*var playerMenu = Root.Instance.PackedScenes.Screen.ProfileMenu.Instantiate<Control>();
		var menu = Root.Instance.Game.MenuContainer.GetCurrentStoredNode<Menu>();
		menu.changeMenu(playerMenu);*/ //TODO 
		};
	}

	public override void _Process(double delta)
	{
	}
}