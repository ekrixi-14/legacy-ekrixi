using System.Linq;
using Content.Server._FTL.AutomatedShip.Components;
using Content.Server._FTL.ShipTracker;
using Content.Server._FTL.Weapons;
using Robust.Shared.Random;

namespace Content.Server._FTL.AutomatedShip.Systems;

public sealed partial class AutomatedShipSystem
{
    private List<EntityUid> GetWeaponsOnGrid(EntityUid gridUid)
    {
        var weapons = new List<EntityUid>();
        var query = EntityQueryEnumerator<FTLWeaponComponent, TransformComponent>();
        while (query.MoveNext(out var entity, out var weapon, out var xform))
        {
            if (xform.GridUid == gridUid && weapon.CanBeUsed)
            {
                weapons.Add(entity);
            }
        }

        return weapons;
    }

    private void PerformCombat(
        EntityUid entity,
        ActiveAutomatedShipComponent activeComponent,
        AutomatedShipComponent aiComponent,
        TransformComponent transformComponent,
        ShipTrackerComponent aiTrackerComponent,
        EntityUid mainShip
        )
    {
        if (activeComponent.TimeSinceLastAttack < aiComponent.AttackRepetition)
            return;

        var weapon = _random.Pick(GetWeaponsOnGrid(entity));

       if (!TryComp<FTLWeaponComponent>(weapon, out var weaponComponent) ||  !TryFindRandomTile(mainShip, out _, out var coordinates))
           return;

       activeComponent.TimeSinceLastAttack = 0;
       _weaponTargetingSystem.TryFireWeapon(weapon, weaponComponent, mainShip, coordinates, null);
    }
}
