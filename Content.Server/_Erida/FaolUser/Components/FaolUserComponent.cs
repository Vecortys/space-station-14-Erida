namespace Content.Server._Erida.FaolUser.Components;

[RegisterComponent]
public sealed partial class FaolUserComponent : Component
{
    public EntityUid? GetFaolAction = null;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid Target;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float UnitPerGetFaol = 20.0f;
}
