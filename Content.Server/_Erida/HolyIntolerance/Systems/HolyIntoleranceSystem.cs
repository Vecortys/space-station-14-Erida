using System.Diagnostics;
using Content.Server._Erida.HolyIntolerance.Components;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Bible.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._Erida.HolyIntolerance.Systems;

public sealed class HolyIntolerance : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly FlammableSystem _flammableSystem = default!;


    public override void Initialize()
    {
        base.Initialize();

    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<HolyIntoleranceComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.NextUpdate > _timing.CurTime)
                continue;

            UpdateState((uid, component));

            component.NextUpdate += component.UpdateCooldown;
        }
    }

    private void UpdateState(Entity<HolyIntoleranceComponent> vampire)
    {
        var component = vampire.Comp;
        if (!_entityManager.TryGetComponent<TransformComponent>(vampire, out var vampireTransform))
            return;

        var query = EntityQueryEnumerator<BibleComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var bible, out var transformComponent))
        {

            if (!vampireTransform.Coordinates.TryDistance(_entityManager, transformComponent.Coordinates, out var distance) || distance > component.Range)
                continue;

            if (_entityManager.TryGetComponent<FlammableComponent>(vampire, out var flammableComponent))
            {
                flammableComponent.FireStacks += component.FireStacks;
                _flammableSystem.Ignite(vampire, uid);
            }
        }
    }
}
