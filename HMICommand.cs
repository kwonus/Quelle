using System;
using System.Collections.Generic;
using System.Linq;

namespace QuelleHMI
{
    public class HMICommand
    {
		public HMIStatement statement { get; private set; }
		public HMIClause explicitClause
        {
			get
			{
				return this.statement != null ? statement.explicitClause : null;
			}
        }
		public string command { get; private set; }
		public List<string> errors { get; private set; }
		public List<string> warnings { get; private set; }

		public void Notify(string level, string message)
        {
			if (level != null && level.Equals("warning", StringComparison.InvariantCultureIgnoreCase))
            {
				warnings.Add(message.Trim());
			}
            else
            {
				errors.Add(message.Trim());
			}
		}
		public HMICommand(String command)
        {
			this.command = command.Trim();
			this.warnings = new List<string>();
			this.errors = new List<string>();

			if (command != null)
			{
				this.statement = new HMIStatement(this, command.Trim());
			}
        }

		public bool HasMacro()
		{
			if (this.explicitClause != null && this.explicitClause.verb == Verbs.Define.SAVE)
				return true;

			return false;
		}
		public Verbs.Define GetMacroDefinition()
		{
			if (this.explicitClause != null && this.explicitClause.verb == Verbs.Define.SAVE)
				return (Verbs.Define)this.explicitClause;

			return null;
		}
		public Verbs.Print GetPrintClause()
		{
			if (this.explicitClause != null && this.explicitClause.verb == Verbs.Print.VERB)
				return (Verbs.Print)this.explicitClause;

			return null;
		}
		public static HMIConfigurationDefault configuration = new HMIConfigurationDefault();
		public bool Search()
        {
			return false;	// This will call into cloud driver; search proivers are unimplemented in this ference implementation
        }
	}
}
