using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    private readonly IMapper mapper;

    public GetExpenseByIdUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }
    
    public async Task<ExpenseResponse> Execute(long id)
    {
        var result = await repository.GetByIdAsync(id);
        if (result is null)
            throw new NotFoundException(ResourcesErrorMessages.EXPENSE_NOT_FOUND);
        
        return mapper.Map<ExpenseResponse>(result);
    }
}