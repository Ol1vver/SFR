using System;
using Microsoft.Xna.Framework;
using SFD;
using SFD.Effects;
using SFD.Materials;
using SFD.Objects;
using SFD.Sounds;
using SFD.Weapons;

namespace SFR.Weapons.Makeshift;

internal sealed class Boot : MWeapon
{
	internal Boot()
	{
		MWeaponProperties weaponProperties = new(106, "Boot", 6f, 8f, "MeleeSwing", "MeleeHitBlunt", "HIT_B", "MeleeBlock", "HIT", "MeleeDraw", "Boot00", false, WeaponCategory.Melee, true)
		{
			MeleeWeaponType = MeleeWeaponTypeEnum.OneHanded,
			WeaponMaterial = MaterialDatabase.Get("cloth"),
			DurabilityLossOnHitObjects = 20f,
			DurabilityLossOnHitPlayers = 40f,
			DurabilityLossOnHitBlockingPlayers = 20f,
			ThrownDurabilityLossOnHitPlayers = 50f,
			ThrownDurabilityLossOnHitBlockingPlayers = 20f,
			DeflectionDuringBlock =
			{
				DeflectType = DeflectBulletType.Absorb,
				DurabilityLoss = 60f
			},
			DeflectionOnAttack =
			{
				DeflectType = DeflectBulletType.Absorb,
				DurabilityLoss = 60f
			}
		};

		MWeaponVisuals weaponVisuals = new();
		weaponVisuals.SetModelTexture("Boot00");
		weaponVisuals.SetDrawnTexture("Boot00");
		weaponVisuals.SetSheathedTexture(string.Empty);
		weaponVisuals.AnimBlockUpper = "UpperBlockMelee";
		weaponVisuals.AnimMeleeAttack1 = "UpperMelee1H1";
		weaponVisuals.AnimMeleeAttack2 = "UpperMelee1H2";
		weaponVisuals.AnimMeleeAttack3 = "UpperMelee1H3";
		weaponVisuals.AnimFullJumpAttack = "FullJumpAttackMelee";
		weaponVisuals.AnimDraw = "UpperDrawMelee";
		weaponVisuals.AnimCrouchUpper = "UpperCrouchMelee";
		weaponVisuals.AnimIdleUpper = "UpperIdleMelee";
		weaponVisuals.AnimJumpKickUpper = "UpperJumpKickMelee";
		weaponVisuals.AnimJumpUpper = "UpperJumpMelee";
		weaponVisuals.AnimJumpUpperFalling = "UpperJumpFallingMelee";
		weaponVisuals.AnimKickUpper = "UpperKickMelee";
		weaponVisuals.AnimStaggerUpper = "UpperStagger";
		weaponVisuals.AnimRunUpper = "UpperRunMelee";
		weaponVisuals.AnimWalkUpper = "UpperWalkMelee";
		weaponVisuals.AnimFullLand = "FullLandMelee";
		weaponVisuals.AnimToggleThrowingMode = "UpperToggleThrowing";
		weaponProperties.VisualText = "Boot";

		SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
	}

	private Boot(MWeaponProperties weaponProperties, MWeaponVisuals weaponVisuals)
	{
		SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
	}

	public override void Destroyed(Player ownerPlayer)
	{
		SoundHandler.PlaySound("DestroySmall", ownerPlayer.GameWorld);
		EffectHandler.PlayEffect("DestroyWood", ownerPlayer.Position, ownerPlayer.GameWorld);
	}

	public override void OnThrowWeaponItem(Player player, ObjectWeaponItem thrownWeaponItem)
	{
		if (player.LastDirectionX > 0)
		{
			thrownWeaponItem.Body.SetTransform(thrownWeaponItem.Body.Position, thrownWeaponItem.Body.Rotation - 1.57079637f);
		}
		else
		{
			thrownWeaponItem.Body.SetTransform(thrownWeaponItem.Body.Position, thrownWeaponItem.Body.Rotation + 1.57079637f);
		}

		var linearVelocity = thrownWeaponItem.Body.GetLinearVelocity();
		thrownWeaponItem.Body.SetLinearVelocity(linearVelocity);
		var unitX = Vector2.UnitX;
		SFDMath.ProjectUonV(ref linearVelocity, ref unitX, out var x);
		float num = 2f * (x.CalcSafeLength() / linearVelocity.CalcSafeLength());
		thrownWeaponItem.Body.SetAngularVelocity(-(float)Math.Sign(linearVelocity.X) * num);
		base.OnThrowWeaponItem(player, thrownWeaponItem);
	}

	public override MWeapon Copy() => new Boot(Properties, Visuals)
	{
		Durability =
		{
			CurrentValue = Durability.CurrentValue
		}
	};
}