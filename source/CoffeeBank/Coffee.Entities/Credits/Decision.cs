using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Entities
{
    public class Decision: IUpdateable<Decision>
    {
        [Key]
        public long Id { get; set; }
        
        public string Authority { get; set; }

        public DateTime DecisionTime { get; set; }

        public bool? Verdict { get; set; }

        public void Update(Decision other) {
            this.Authority = other.Authority;
            this.DecisionTime = other.DecisionTime;
            this.Verdict = Verdict;
        }
    }
}
