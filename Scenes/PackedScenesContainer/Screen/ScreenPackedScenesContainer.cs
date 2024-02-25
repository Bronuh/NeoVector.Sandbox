using Godot;
using KludgeBox;

public partial class ScreenPackedScenesContainer : Node
{
    [Export] [NotNull] public PackedScene MainMenu { get; private set; }
    [Export] [NotNull] public PackedScene ProfileMenu { get; private set; }
    [Export] [NotNull] public PackedScene Hud { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }
}