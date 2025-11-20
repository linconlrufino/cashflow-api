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
        CreateMap<RegisterExpenseRequest, Expense>();
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, RegisteredExpenseResponse>();
        
        
        CreateMap<Expense, ShortExpenseResponse>();        
        CreateMap<Expense, ExpenseResponse>();
        CreateMap<IEnumerable<Expense>, ExpensesResponse>()
            .ForMember(
                dest => dest.Expenses,
                opt => opt.MapFrom(src => src));
    }   
}