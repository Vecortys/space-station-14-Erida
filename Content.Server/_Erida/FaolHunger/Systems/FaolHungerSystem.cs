using Content.Server._Erida.FaolHunger.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Jittering;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using System.Diagnostics;

namespace Content.Server._Erida.Strangle.Systems;

public sealed class FaolHungerSystem : EntitySystem
{

    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedJitteringSystem _jitter = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FaolHungerComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, FaolHungerComponent component, MapInitEvent args)
    {
        component.FaolLoss = _random.NextFloat(0.008f, 0.025f);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var curTime = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<FaolHungerComponent>();
        while (query.MoveNext(out var uid, out var faolHungerComponent))
        {
            if (faolHungerComponent.NextUpdate > curTime)
                continue;

            UpdateFaolStock(uid, faolHungerComponent);
            UpdateFaolThreshold(uid, faolHungerComponent);

            if (faolHungerComponent.CurrentThreshold == FaolThreshold.Dead)
            {
                _damage.TryChangeDamage(uid, new DamageSpecifier(_prototypeManager.Index<DamageTypePrototype>("Asphyxiation"), faolHungerComponent.DamageByDeadly), true, false);
                _jitter.DoJitter(uid, TimeSpan.FromSeconds(2), true, amplitude: 1, frequency: 7);
            }
            else
                faolHungerComponent.FaolStock -= faolHungerComponent.FaolLoss * faolHungerComponent.HungerModificator;

            faolHungerComponent.NextUpdate = curTime + faolHungerComponent.Interval;
        }
    }

    private void UpdateFaolStock(EntityUid uid, FaolHungerComponent component)
    {
        if (_solutionContainer.TryGetInjectableSolution(uid, out var targetSoln, out var targetSolution))
        {
            if (targetSolution.ContainsReagent("Faol", null) && component.CurrentThreshold != FaolThreshold.Overfed)
            {
                component.FaolStock += 0.5f;
            }
        }
    }

    private void UpdateFaolThreshold(EntityUid uid, FaolHungerComponent component)
    {
        var result = FaolThreshold.Dead;

        foreach (var threshold in component.Thresholds)
        {
            if (threshold.Value <= component.FaolStock)
            {
                result = threshold.Key;
                break;
            }
        }
        if (component.CurrentThreshold != result)
        {
            component.CurrentThreshold = result;
            OnThresholdUpdate(uid, component);
        }
    }

    private void OnThresholdUpdate(EntityUid uid, FaolHungerComponent component)
    {
        switch (component.CurrentThreshold)
        {
            case FaolThreshold.Deadly:
                component.HungerModificator = 0.7f;
                _jitter.DoJitter(uid, TimeSpan.FromSeconds(5), true, amplitude: 2, frequency: 6);
                break;
            case FaolThreshold.Starving:
                component.HungerModificator = 0.8f;
                break;
            case FaolThreshold.Peckish:
                component.HungerModificator = 0.9f;
                break;
            case FaolThreshold.Okay:
                component.HungerModificator = 1.0f;
                break;
            case FaolThreshold.Overfed:
                component.HungerModificator = 1.2f;
                break;
        }
    }
}
