using Godot;
using System;

public partial class MenuContainer : Control, INodeContainer<Control>
{
	public Control CurrentStoredNode { get; set; }
	
	public void ChangeStoredNode(Control newStoredNode)
	{
		CurrentStoredNode?.QueueFree();
		CurrentStoredNode = newStoredNode;
		(this as Node)?.AddChild(newStoredNode);
	}

	public void ClearStoredNode()
	{
		CurrentStoredNode?.QueueFree();
	}
}
