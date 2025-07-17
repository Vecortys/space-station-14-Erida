using Content.Server._Vecortys.Strangle.Components;
using Content.Server._Vecortys.Choke.Components;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.CombatMode;
using Content.Shared.Popups;
using System.Diagnostics;

namespace Content.Shared._Vecortys.Strangle.Systems;

public sealed class StrangleSystem : EntitySystem
{

    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedCombatModeSystem _combat = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StrangleComponent, PullStartedMessage>(HandlePullStarted);
        SubscribeLocalEvent<StrangleComponent, PullStoppedMessage>(HandlePullStopped);
    }

    private void HandlePullStarted(EntityUid uid, StrangleComponent component, PullStartedMessage args)
    {

        if (_combat.IsInCombatMode(args.PullerUid))
        {
            if (uid == args.PullerUid)
                return;
            if (_entityManager.TryGetComponent<MetaDataComponent>(args.PullerUid, out var puller_meta) && _entityManager.TryGetComponent<MetaDataComponent>(uid, out var pulled_meta))
            {
                component.Source = args.PullerUid;
                _entityManager.AddComponent<ChokeComponent>(uid);
                _popup.PopupEntity(puller_meta.EntityName + " сжимают шею " + pulled_meta.EntityName, args.PullerUid, PopupType.MediumCaution);
            }
        }
    }

    private void HandlePullStopped(EntityUid uid, StrangleComponent component, PullStoppedMessage args)
    {
        if (uid == args.PullerUid)
            return;

        _entityManager.RemoveComponent<ChokeComponent>(uid);
    }
}
