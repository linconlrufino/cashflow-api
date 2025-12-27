using AutoMapper;
using Communication.Requests;
using Communication.Responses;
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
        CreateMap<ExpenseRequest, Expense>();
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, RegisteredExpenseResponse>();
        
        CreateMap<Expense, ShortExpenseResponse>();        
        CreateMap<Expense, ExpenseResponse>();
        CreateMap<IEnumerable<Expense>, ExpensesResponse>()
            .ForMember(
                dest => dest.Expenses,
                config => config.MapFrom(src => src));
    }   
}