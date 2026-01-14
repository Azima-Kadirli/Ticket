using System.Threading.Tasks;

namespace Ticket.Helper
{
    public static class ExtensionMethods
    {
        public static async Task<string> FileUpload(this IFormFile file, string filePath)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(filePath,uniqueFileName);
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return uniqueFileName;
        }

        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
