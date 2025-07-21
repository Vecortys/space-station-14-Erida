using Robust.Shared.Prototypes;
using Content.Shared.Chat.Prototypes;

namespace Content.Server._Erida.FaolHunger.Components;

[RegisterComponent]
public sealed partial class FaolHungerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan Interval = TimeSpan.FromSeconds(1);


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FaolStock = 105.0f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FaolLoss = 0.02f;

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

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<FaolThreshold, string> MoodEffects = new()
    {
        { FaolThreshold.Overfed, "FaolHungerOverfed" },
        { FaolThreshold.Okay, "FaolHungerOkay" },
        { FaolThreshold.Peckish, "FaolHungerPeckish" },
        { FaolThreshold.Starving, "FaolHungerStarving" },
        { FaolThreshold.Deadly, "FaolHungerDeadly" },
        { FaolThreshold.Dead, "FaolHungerDead" }
    };

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public FaolThreshold CurrentThreshold = FaolThreshold.Okay;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DamageByDead = 1.8f;
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
