using WebAnhAnh.Repository;

namespace WebAnhAnh.Services
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
		VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
	}
}
