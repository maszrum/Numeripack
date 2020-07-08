namespace Numeripack.Tests.StateMachine
{
	internal enum ValidEnumOne
	{
		First = 1,
		Second = 2,
		Third = 4,
		Fourth = 8,
		Fifth = 16
	}

	internal enum ValidEnumTwo
	{
		First = 16,
		Second = 32,
		Third = 4,
		Fourth = 8,
		Fifth = 64
	}

	internal enum InvalidEnumOne
	{
		First,
		Second,
		Third,
		Fourth,
		Fifth
	}

	internal enum InvalidEnumTwo
	{
		First = -1,
		Second = 0,
		Third = 1,
		Fourth = 2,
		Fifth = 4
	}

	internal enum ProcessState
	{
		Standby = 1,
		Active = 2,
		Suspect = 4,
		Failed = 8
	}
}
