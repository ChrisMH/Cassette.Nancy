using System.IO.IsolatedStorage;

namespace Cassette.Nancy
{
  /// <summary>
  /// Provides the isolated storage used by Cassette.
  /// Storage is only created on demand.
  /// </summary>
  static class IsolatedStorageContainer
  {
    static readonly DisposableLazy<IsolatedStorageFile> lazyStorage = new DisposableLazy<IsolatedStorageFile>(() => CreateIsolatedStorage());
   
    static IsolatedStorageFile CreateIsolatedStorage()
    {
      return IsolatedStorageFile.GetMachineStoreForAssembly();
    }

    public static IsolatedStorageFile IsolatedStorageFile
    {
      get
      {
        return lazyStorage.Value;
      }
    }

    public static void Dispose()
    {
      lazyStorage.Dispose();
    }
  }
}