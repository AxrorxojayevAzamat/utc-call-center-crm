

using CallCenterCRM.Data;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Models;

namespace CallCenterCRM.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly CallcentercrmContext _context;

        public AttachmentService(CallcentercrmContext context)
        {
            _context = context;
        }
        public int UploadFileToStorage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                string filePath = "", hashName = Convert.ToString(Guid.NewGuid()).Substring(0, 8);

                try
                {
                    var filePathStream = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "uploads", $"{DateTime.Now.Year}", $"{DateTime.Now.Month}", $"{DateTime.Now.Day}", hashName);
                    filePath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{hashName}/{fileName}";
                    Directory.CreateDirectory(filePathStream);
                    using (var fs = new FileStream($"{filePathStream}/{fileName}", FileMode.OpenOrCreate))
                    {
                        file.CopyTo(fs);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                var objFiles = new Attachment()
                {
                    OriginName = fileName,
                    HashName = hashName,
                    Path = filePath,
                    Extension = fileExtension,
                };

                _context.Attachments.Add(objFiles);
                _context.SaveChanges();

                int Id = objFiles.Id;

                return Id;
            }
            return -1;
        }
    }
}
