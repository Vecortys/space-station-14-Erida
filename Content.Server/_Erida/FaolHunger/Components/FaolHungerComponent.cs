using Robust.Shared.Prototypes;
using Content.Shared.Chat.Prototypes;

namespace Content.Server._Erida.FaolHunger.Components;

[RegisterComponent]
public sealed partial class FaolHungerComponent : Component
{
    [DataField]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan Interval = TimeSpan.FromSeconds(1);


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FaolStock = 100.0f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FaolLoss = 0.04f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float HungerModificator = 1.0f;


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<FaolThreshold, float> Thresholds = new()
    {
        { FaolThreshold.Overfed, 150.0f },
        { FaolThreshold.Okay, 100.0f },
        { FaolThreshold.Peckish, 40.0f },
        { FaolThreshold.Starving, 20.0f },
        { FaolThreshold.Deadly, 1.0f },
        { FaolThreshold.Dead, 0.0f }
    };

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public FaolThreshold CurrentThreshold = FaolThreshold.Okay;


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DamageByDeadly = 1.8f;
}

public enum FaolThreshold : byte
{
    Overfed = 1 << 4,
    Okay = 1 << 3,
    Peckish = 1 << 2,
    Starving = 1 << 1,
    Deadly = 1 << 0,
    Dead = 0
}
