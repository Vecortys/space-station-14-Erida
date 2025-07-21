using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

public sealed partial class GetFaolEvent : EntityTargetActionEvent { }

[Serializable, NetSerializable]
public sealed partial class GetFaolDoAfterEvent : SimpleDoAfterEvent { }
