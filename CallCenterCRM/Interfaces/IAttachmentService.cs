namespace CallCenterCRM.Interfaces
{
	public interface IAttachmentService
	{
		public Task<int> UploadFileToStorage(IFormFile AttachmentId);
	}
}
