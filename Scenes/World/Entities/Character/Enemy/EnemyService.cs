﻿using KludgeBox;
using KludgeBox.Events;
using KludgeBox.Events.Global;

namespace NeoVector;

[GameService]
public class EnemyService
{
    [EventListener]
    public void OnEnemyReady(EnemyReadyEvent e)
    {
        var enemy = e.Enemy;

        enemy.PrimaryCd.Duration = 1;
        enemy.PrimaryCd.Ready += () =>
        {
            if (CanSeePlayer(enemy))
            {
                EventBus.Publish(new EnemyAttackEvent(enemy));
            }
        };
    }
    
    private bool CanSeePlayer(Enemy enemy)
    {
        var collider = enemy.RayCast.GetCollider();
        return collider is Player;
    }
}