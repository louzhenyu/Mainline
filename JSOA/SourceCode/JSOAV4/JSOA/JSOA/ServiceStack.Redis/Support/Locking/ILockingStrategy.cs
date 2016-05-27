using System;

namespace JSOA.Redis.Support.Locking
{
    /// <summary>
    /// Locking strategy interface
    /// </summary>
	public interface ILockingStrategy
	{
		IDisposable ReadLock();

		IDisposable WriteLock();
	}
}