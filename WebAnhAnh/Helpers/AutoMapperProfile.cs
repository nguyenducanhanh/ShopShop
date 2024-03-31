using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterRepository, Customer>();

		}
	}
}
