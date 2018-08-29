using System;
using System.Collections.Generic;

namespace TextAdv
{
    public class Spell
    {
        public string name;
        public int cost;
        public int damage;
        public string effect;
        public bool target;
        public Spell(string spellName, int spellDamage, bool spellTarget = true)
        {
            name = spellName;
            damage = spellDamage;
            target = spellTarget;
            cost = damage * 2;
        }
        public Spell(string spellName, string spellEffect, bool spellTarget = true)
        {
            name = spellName;
            effect = spellEffect;
            target = spellTarget;
            cost = 50;
        }
        public Spell(string spellName, int spellDamage, string spellEffect, bool spellTarget = true)
        {
            name = spellName;
            damage = spellDamage;
            effect = spellEffect;
            target = spellTarget;
            cost = damage * 2 + 50;
        }

        public void Cast(Human caster)  // Make it so casting the spell does not pass a target (the cast has a target already). if the spell has no target, cast on self
        {
            if(this.target == false)
            {
                CastSelf(caster);
                return;
            }
            Human toAttack = caster.target;
            int spellDamage = damage * caster.intelligence;
            string msg = "";
            caster.mana -= this.cost;
            toAttack.health -= spellDamage;
            if(spellDamage > 0)
            {
                msg += $"{caster.name} cast {this.name} on {toAttack.name} dealing {spellDamage} damage.";
            }
            if(toAttack.health <= 0)
            {
                msg += $" {toAttack.name} has died. You gained {toAttack.level * 5} experience.";
                caster.experience += toAttack.level * 5;
                caster.target = null;
                Messages.msgs.Add(msg);
            }
            else
            {
                if(this.effect == "Burn")
                {
                    toAttack.status = "Burning";
                    msg += $" {toAttack.name} is now burning.";
                }
                Messages.msgs.Add(msg);
                toAttack.Attack(false);
            }
        }

        public void CastSelf(Human caster)
        {
            caster.mana -= this.cost;
            if(effect == null)
            {
                int toHeal = damage * caster.intelligence;
                caster.health = (caster.health + toHeal > caster.maxHealth) ? caster.maxHealth : caster.health + toHeal;
                Messages.msgs.Add($"{caster.name} cast {this.name}.");
            }
            if(caster.target != null)
            {
                caster.target.Attack(false);
            }
        }
    }

}