using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;

namespace Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesRepository expensesRepository;
    private readonly IMapper mapper;

    public GetAllExpensesUseCase(
        IExpensesRepository expensesRepository,
        IMapper mapper)
    {
        this.expensesRepository = expensesRepository;
        this.mapper = mapper;
    }

    public async Task<ExpensesResponse> Execute()
    {
       var result = await expensesRepository.GetAllAsync();
       
       return mapper.Map<ExpensesResponse>(result);
    }
}