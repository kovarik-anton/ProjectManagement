using AutoMapper;
using ProjectManagement.Data.Models;

namespace ProjectManagement.ViewModels
{
    public class Mapper: Profile
    {
        public static void Init()
        {
            AutoMapper.Mapper.Initialize(a => {
                a.AddProfile<Mapper>();
            });
        }

        public Mapper()
        {
            CreateMap<Project, ProjectViewModel>()
                .ReverseMap();

            CreateMap<Solution, SolutionViewModel>()
                .ReverseMap();
        }
    }
}