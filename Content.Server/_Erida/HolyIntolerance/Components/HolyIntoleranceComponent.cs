using Robust.Shared.Prototypes;

namespace Content.Server._Erida.HolyIntolerance.Components;

[RegisterComponent]
public sealed partial class HolyIntoleranceComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan UpdateCooldown = TimeSpan.FromSeconds(1);

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    [DataField]
    public int Range = 3;

    [DataField]
    public int FireStacks = 1;
}
