using System;

namespace Assets.Scripts.NPCs
{
    public interface ICreature
    {
        void movement();
        void attack();
        void takeDamage(float damageTaken);        
        void death();
    }
}
