using Content.Server._Vecortys.Choke.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Random;
using Content.Server.Chat.Systems;
using Content.Server._Vecortys.Strangle.Components;
using Content.Shared.CombatMode;

namespace Content.Server._Vecortys.Choke.Systems;

public sealed class ChokeSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly SharedCombatModeSystem _combat = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
    }


    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var curTime = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<ChokeComponent>();
        while (query.MoveNext(out var uid, out var chokeComponent))
        {
            if (chokeComponent.NextUpdate > curTime)
                continue;

            if (_entManager.TryGetComponent<StrangleComponent>(uid, out var strangle))
            {
                if (!_combat.IsInCombatMode(strangle.Source))
                    _entManager.RemoveComponent<ChokeComponent>(uid);
            }

            if (chokeComponent.NextUpdate != TimeSpan.Zero) // Not First Tick
                _damage.TryChangeDamage(uid, new DamageSpecifier(_prototypeManager.Index<DamageGroupPrototype>("Airloss"), chokeComponent.Damage), true, false);

            var time = curTime + chokeComponent.Interval;
            chokeComponent.NextUpdate = time;

            if (_random.Next(1, 100) <= chokeComponent.GaspChance)
            {
                _chatSystem.TryEmoteWithoutChat(uid, chokeComponent.GaspEmote);
                chokeComponent.GaspChance = 25;
            }
            else
                chokeComponent.GaspChance += 10;

        }
    }
}
