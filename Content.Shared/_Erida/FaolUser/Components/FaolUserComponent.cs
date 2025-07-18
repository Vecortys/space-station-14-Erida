using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._Erida.FaolUser.Components;

[RegisterComponent]
public sealed partial class FaolUserComponent : Component
{
    public EntityUid? GetFaolAction = null;

    [DataField]
    public EntityUid Target;
}

public sealed partial class GetFaolEvent : EntityTargetActionEvent { }

[Serializable, NetSerializable]
public sealed partial class GetFaolDoAfterEvent : SimpleDoAfterEvent { }
