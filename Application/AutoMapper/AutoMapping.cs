using AutoMapper;
using Communication.Enums;
using Communication.Requests.Expense;
using Communication.Requests.User;
using Communication.Responses;
using Communication.Responses.User;
using Domain.Entities;

namespace Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Password,
                config => config.Ignore());

        CreateMap<ExpenseRequest, Expense>()
            .ForMember(dest => dest.Tags, 
                config => config.MapFrom(
                    src => src.Tags.Distinct()));
                
        CreateMap<TagType, Tag>()
            .ForMember(dest => dest.TagType, 
                config => config.MapFrom(src => src));
        
        CreateMap<UpdateUserRequest, User>();
    }

    private void EntityToResponse()
    {
        CreateMap<User, UserProfileResponse>();
        
        CreateMap<Expense, RegisteredExpenseResponse>();
        CreateMap<Expense, ShortExpenseResponse>();        
        CreateMap<Expense, ExpenseResponse>();
        CreateMap<IEnumerable<Expense>, ExpensesResponse>()
            .ForMember(
                dest => dest.Expenses,
                config => config.MapFrom(src => src));
    }   
}