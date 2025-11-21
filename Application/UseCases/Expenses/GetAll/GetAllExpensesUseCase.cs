using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;

namespace Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    private readonly IMapper mapper;

    public GetAllExpensesUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<ExpensesResponse> Execute()
    {
       var result = await repository.GetAllAsync();
       
       return mapper.Map<ExpensesResponse>(result);
    }
}