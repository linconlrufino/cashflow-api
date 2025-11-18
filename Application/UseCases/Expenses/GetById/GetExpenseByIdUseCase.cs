using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;

namespace Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    IExpensesRepository expensesRepository;
    IMapper mapper;

    public GetExpenseByIdUseCase(
        IExpensesRepository expensesRepository,
        IMapper mapper)
    {
        this.expensesRepository = expensesRepository;
        this.mapper = mapper;
    }
    
    public async Task<ShortExpenseResponse> Execute(int expenseId)
    {
        var response = await expensesRepository.GetByIdAsync(expenseId);
        return mapper.Map<ShortExpenseResponse>(response);
    }
}