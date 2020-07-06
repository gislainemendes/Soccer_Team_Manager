using System;

namespace Codenation.Challenge
{
    public class Player
    {
        public long id { get; set; }
        public long teamId { get; set; }
        public string name { get; set; }
        public DateTime birthDate { get; set; }
        public int skillLevel { get; set; }
        public decimal salary { get; set; }
        public bool isCaptain = false;

        public Player()
        {

        }

        public Player(long id)
        {
            this.id = id;
        }
        public Player(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            this.id = id;
            this.teamId = teamId;
            this.name = name;
            this.birthDate = birthDate;
            this.skillLevel = skillLevel;
            this.salary = salary;

        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Player player = (Player)obj;
                return (this.id == player.id);
            }
        }


        public override int GetHashCode()
        {
            return (int)id;
        }


    }
}
