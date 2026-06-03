using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLibrary.Helpers
{
    public static class FileUtilities
    {
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream? stream = null;
            try
            {
                // Attempt to open the file in read/write mode
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // The file is locked by another process
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            // the file is not locked
            return false;
        }

        public static string GetLockedFileDetails(FileInfo file) 
        {
            string modifiedBy = file.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            string lockedFile = $"{file.Name} is locked by: {modifiedBy}, last modified: {file.LastWriteTime}";
            return lockedFile;
        }
    }
}
