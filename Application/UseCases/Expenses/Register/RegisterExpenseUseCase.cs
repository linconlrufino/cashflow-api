using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Expenses;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository expensesRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public RegisterExpenseUseCase(IExpensesRepository expensesRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.expensesRepository = expensesRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<RegisteredExpenseResponse> Execute(RegisterExpenseRequest request)
    {
        Validate(request);
        
        var entity = mapper.Map<Expense>(request);

        await expensesRepository.AddAsync(entity);
        await unitOfWork.Commit();
        
        return mapper.Map<RegisteredExpenseResponse>(entity);
    }

    private static void Validate(RegisterExpenseRequest request)
    {
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;
        
        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}