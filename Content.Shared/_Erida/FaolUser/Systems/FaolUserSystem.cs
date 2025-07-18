using Content.Shared.Popups;
using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Content.Shared._Erida.FaolUser.Components;
using Robust.Shared.Prototypes;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared.Backmen.Mood;

namespace Content.Shared._Erida.FaolUser.Systems;

public sealed class FaolUserSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private SharedSolutionContainerSystem _solutionContainer = default!;

    [ValidatePrototypeId<EntityPrototype>] private const string GetFaolAction = "ActionGetFaol";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FaolUserComponent, GetFaolEvent>(OnGetFaol);
        SubscribeLocalEvent<FaolUserComponent, ComponentStartup>(OnGetFaolStartup);
        SubscribeLocalEvent<FaolUserComponent, GetFaolDoAfterEvent>(OnGetFaolDoAfterEvent);
    }

    private void OnGetFaolStartup(EntityUid uid, FaolUserComponent component, ComponentStartup args)
    {
        _actionsSystem.AddAction(uid, ref component.GetFaolAction, GetFaolAction);
    }

    private void OnGetFaol(EntityUid uid, FaolUserComponent comp, ref GetFaolEvent args)
    {
        if (args.Handled)
            return;
        args.Handled = true;
        comp.Target = args.Target;

        var dargs = new DoAfterArgs(EntityManager, args.Performer, 5.5f, new GetFaolDoAfterEvent(), args.Performer, target: args.Target)
        {
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnMove = true
        };
        _doAfter.TryStartDoAfter(dargs);
        if (_entityManager.TryGetComponent<MetaDataComponent>(args.Target, out var targetMeta) &&
        _entityManager.TryGetComponent<MetaDataComponent>(uid, out var userMeta))
            _popup.PopupEntity($"Хвост {userMeta.EntityName} захватывает {targetMeta.EntityName}!", args.Target, PopupType.MediumCaution);
    }

    private void OnGetFaolDoAfterEvent(EntityUid uid, FaolUserComponent component, GetFaolDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;
        args.Handled = true;

        if (_solutionContainer.TryGetInjectableSolution(uid, out var targetSoln, out var targetSolution))
        {
            _solutionContainer.AddSolution(targetSoln.Value, new Solution("Faol", FixedPoint2.New(20)));
            RaiseLocalEvent(component.Target, new MoodEffectEvent("EmotionReduce"));
        }

    }
}
