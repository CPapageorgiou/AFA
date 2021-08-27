using System;
namespace AFAapp.Models
{
    public class Substitution
    {
        public char letter { get; set; }
        public string state { get; set; }
        public string formula { get; set; }
     

        public Substitution(char letter, string state, string formula)
        {
            this.letter = letter;
            this.state = state;
            this.formula = formula;
        }
    }
}
