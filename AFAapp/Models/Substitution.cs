namespace AFAapp.Models
{
    // Simple class to represent a substitution during a computation.
    public class Substitution
    {
        // Properties and Fields.

        public char letter { get; }
        public string state { get; }
        public string formula { get; }

        // Constructors.

        // Object from a letter consumed, the state substituted and
        // the substitution that replaced the state.
        public Substitution(char letter, string state, string formula)
        {
            this.letter = letter;
            this.state = state;
            this.formula = formula;
        }

        // Methods.

        // Prints substitutions to the console. Testing purposes.
        public override string ToString()
        {
            return $"({letter},{state},{formula})";
        }
    }
}
