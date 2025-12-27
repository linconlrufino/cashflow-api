using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;

namespace Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    private readonly IMapper mapper;
    private readonly ILoggedUser loggedUser;

    public GetAllExpensesUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.loggedUser = loggedUser;
    }

    public async Task<ExpensesResponse> Execute()
    {
        var loggedUserInfo = await loggedUser.Get(); 
        var result = await repository.GetAllAsync(loggedUserInfo.Id);
        return mapper.Map<ExpensesResponse>(result);
    }
}