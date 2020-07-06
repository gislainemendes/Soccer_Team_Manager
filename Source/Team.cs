using Codenation.Challenge.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codenation.Challenge
{
    public class Team 
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public string mainShirtColor { get; set; }
        public string secondaryShirtColor { get; set; }
        

        public Team()
        {
        }

        public Team(long id)
        {
            this.id = id;
        }

        public Team(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            this.id = id;
            this.name = name;
            this.createDate = createDate;
            this.mainShirtColor = mainShirtColor;
            this.secondaryShirtColor = secondaryShirtColor;
        }

        public override string ToString()
        {
            return "time: " + this.name;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Team time = (Team) obj;
                return (this.id == time.id);
            }
        }

        public override int GetHashCode()
        {
            return (int) id;
        }

    }
}
