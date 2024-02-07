using Godot;

public class PlayerMovementService
{
    public PlayerMovementService()
    {
        Root.Instance.EventBus.Subscribe<PlayerPhysicsProcessEvent>(OnPlayerPhysicsProcessEvent);
    }
    
    public void OnPlayerPhysicsProcessEvent(PlayerPhysicsProcessEvent playerPhysicsProcessEvent) {
        MoveByKeyboard(playerPhysicsProcessEvent.Player, playerPhysicsProcessEvent.Delta);
    }
    
    private void MoveByKeyboard(Player player, double delta)
    {
        var movementInput = GetInput();
        player.MoveAndCollide(movementInput * player.MovementSpeed * delta);
    }
    
    private Vector2 GetInput()
    {
        return Input.GetVector(Keys.Left, Keys.Right, Keys.Up, Keys.Down);
    }
    
}