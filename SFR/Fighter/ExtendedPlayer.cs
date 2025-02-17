﻿using System;
using System.Collections.Generic;
using SFD;
using SFD.Sounds;
using SFDGameScriptInterface;
using SFR.Fighter.Jetpacks;
using SFR.Sync.Generic;

namespace SFR.Fighter;

/// <summary>
///     Since we need to save additional data into the player instance
///     we use this file to "extend" the player class.
/// </summary>
internal sealed class ExtendedPlayer : IEquatable<Player>
{
    internal static readonly List<ExtendedPlayer> ExtendedPlayers = new();
    internal readonly Player Player;
    internal readonly TimeSequence Time = new();
    internal bool Afraid = false;
    internal bool AfraidCheck = false;
    internal GenericJetpack GenericJetpack;
    internal JetpackType JetpackType = JetpackType.None;
    internal bool PrepareJetpack = false;

    internal ExtendedPlayer(Player player) => Player = player;

    internal bool AdrenalineBoost
    {
        get => Time.AdrenalineBoost > 0f;
        set => Time.AdrenalineBoost = value ? TimeSequence.AdrenalineBoostTime : 0f;
    }

    public bool Equals(Player other) => other?.ObjectID == Player.ObjectID;

    // TODO: Change other methods instead of using modifiers, like strength boost & speed boost do
    internal void ApplyAdrenalineBoost()
    {
        var modifiers = new PlayerModifiers(true)
        {
            SprintSpeedModifier = 1.3f,
            RunSpeedModifier = 1.3f,
            SizeModifier = 1.05f,
            MeleeForceModifier = 1.2f,
            CurrentHealth = Player.Health.CurrentValue
        };
        Player.SetModifiers(modifiers);
        AdrenalineBoost = true;
        GenericData.SendGenericDataToClients(new GenericData(DataType.ExtraClientStates, new SyncFlag[] { }, Player.ObjectID, GetStates()));
    }

    internal object[] GetStates()
    {
        object[] states = new object[6];
        states[0] = AdrenalineBoost;
        states[1] = PrepareJetpack;
        states[2] = Afraid;
        states[3] = AfraidCheck;
        states[4] = (int)JetpackType;
        states[5] = GenericJetpack?.Fuel?.CurrentValue ?? 100f;

        return states;
    }

    // TODO: Change other methods instead of using modifiers, like strength boost & speed boost do
    internal void DisableAdrenalineBoost()
    {
        SoundHandler.PlaySound("StrengthBoostStop", Player.Position, Player.GameWorld);
        var modifiers = new PlayerModifiers(true)
        {
            CurrentHealth = Player.Health.CurrentValue
        };
        Player.SetModifiers(modifiers);
        AdrenalineBoost = false;
        GenericData.SendGenericDataToClients(new GenericData(DataType.ExtraClientStates, new SyncFlag[] { }, Player.ObjectID, GetStates()));
    }

    internal class TimeSequence
    {
        internal const float AdrenalineBoostTime = 16000f;
        internal float AdrenalineBoost;
    }
}