using Robust.Shared.Prototypes;
using Content.Shared.Chat.Prototypes;

namespace Content.Server._Vecortys.Choke.Components;

[RegisterComponent]
public sealed partial class ChokeComponent : Component
{
    [DataField]
    public ProtoId<EmotePrototype> GaspEmote = "Gasp";

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    [DataField("interval"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Interval = TimeSpan.FromSeconds(2);

    [DataField("gaspChance"), ViewVariables(VVAccess.ReadOnly)]
    public int GaspChance = 25;

    [DataField("damage"), ViewVariables(VVAccess.ReadWrite)]
    public int Damage = 15;

}
